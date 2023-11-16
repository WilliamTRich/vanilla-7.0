using RotMG.Common;
using RotMG.Game;
using RotMG.Game.Entities;
using SimpleLog;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using static RotMG.Networking.GameServer;

namespace RotMG.Networking
{
    public enum ProtocolState
    {
        Handshaked, //Indicates that the client has initialized a connection and is now waiting for Hello packet.
        Awaiting, //Received Hello and is now waiting for a Load/Create packet to put the player in game.
        Connected, //Indicates that the client is now fully initialized and is in game.
        Disconnected //Packets received will no longer be processed and the server will disconnect the client.
    }

    public sealed partial class Client
    {
        public const int LENGTH_PREFIX = 2;

        public ProtocolState State;
        public int Id;
        public int TargetWorldId;
        public string IP;

        public AccountModel Account;
        public CharacterModel Character;
        public Player Player;
        public wRandom Random;
        public bool Active; //Used in escape to stop incoming packets (so you don't die)
        public int DCTime;

        private Socket _socket;
        private Queue<byte[]> _pending;
        private SendState _send;
        private ReceiveState _receive;

        public CancellationTokenSource TokenSource;

        public Client(SendState send, ReceiveState receive)
        {
            _pending = new Queue<byte[]>();
            _send = send;
            _receive = receive;
            TokenSource = new();
        }

        public void Disconnect() //Disconnects, clears all individual client data and pushes the instance back to the server queue.
        {
            TokenSource.Cancel();

            if (State == ProtocolState.Disconnected)
            {
#if DEBUG   
                SLog.Error("Already dcd");
#endif      
                return;
            }
#if DEBUG
            try
            {
                SLog.Debug($"Disconnecting client from <{_socket.RemoteEndPoint}>");
            }
            catch (Exception ex) 
            {
                SLog.Error(ex.ToString());
            }
#endif
            //Save what's needed
            if (Account != null)
            {
                Account.Connected = false;
                Account.LastSeen = Database.UnixTime();
                Account.Save();
                Manager.AccountIdToClientId.Remove(Account.Id);

                if (Player != null && Player.Parent != null)
                {
                    Player.TradeDone(Player.TradeResult.Canceled);
                    Player.SaveToCharacter();
                    Player.Parent.RemoveEntity(Player);
                    if (!Character.Dead) //Already saved during death.
                    {
                        Database.SaveCharacter(Character);
                    }
                }
            }

            //Shutdown socket
            State = ProtocolState.Disconnected;

            try
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
            }
#if DEBUG
            catch (Exception ex)
            {
                SLog.Error( ex);
            }
#endif
#if RELEASE
            catch 
            {

            }
#endif

            //Clear data 
            Active = false;
            _send.Reset();
            _receive.Reset();
            _pending.Clear();
            Account = null;
            Player = null;
            Character = null;
            Random = null;
            TargetWorldId = -1;

            //Push back client to queue
            Manager.RemoveClient(this);
            GameServer.AddBack(this);
        }

        public void BeginHandling(Socket socket, string ip)
        {
            _socket = socket;
            //_socket.Blocking = false;

            State = ProtocolState.Handshaked;
            IP = ip;
            Active = true;
            DCTime = -1;

            Manager.AddClient(this);
            Receive();
        }
        private async void TrySend(int len)
        {
            if (!_socket.Connected)
                return;

            try
            {
                SLog.Info($"Sending packet {(S2CPacketId)_send.PacketBytes.AsSpan()[LENGTH_PREFIX]} {len}");
                BinaryPrimitives.WriteUInt16LittleEndian(_send.PacketBytes.AsSpan(), (ushort)(len - LENGTH_PREFIX));
                _ = await _socket.SendAsync(_send.PacketBytes[..len]);
            }
            catch (Exception e)
            {
                Disconnect();
                if (e is not SocketException se || se.SocketErrorCode != SocketError.ConnectionReset &&
                    se.SocketErrorCode != SocketError.Shutdown)
                    SLog.Error($"{Account?.Name ?? "[unconnected]"} ({IP}): {e}");
            }
        }
        private async void Receive()
        {
            try
            {
                while (_socket.Connected)
                {
                    var len = await _socket.ReceiveAsync(_receive.PacketBytes.AsMemory(), TokenSource.Token);

                    if (len == 0)
                    {
                        Disconnect();
                        break;
                    }

                    if (len > 0)
                        ProcessPacket(len);
                }
            }
            catch (Exception e)
            {
                Disconnect();
                if (e is not SocketException se || se.SocketErrorCode != SocketError.ConnectionReset &&
                    se.SocketErrorCode != SocketError.Shutdown)
                    SLog.Error($"Could not receive data from {Account?.Name ?? "[unconnected]"} ({IP}): {e}");
            }
        }

        private void ProcessPacket(int len)
        {
            var ptr = 0;
            ref var spanRef = ref MemoryMarshal.GetReference(_receive.PacketBytes.AsSpan());
            while (ptr < len)
            {
                var packetLen = PacketUtils.ReadUShort(ref ptr, ref spanRef, len);
                var nextPacketPtr = ptr + packetLen - 2;
                var packetId = (C2SPacketId)PacketUtils.ReadByte(ref ptr, ref spanRef, nextPacketPtr);

                SLog.Info("Packet received {0}", packetId);

                switch (packetId)
                {
                    //case C2SPacketId.AcceptTrade:
                    //    ProcessAcceptTrade(PacketUtils.ReadBoolArray(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadBoolArray(ref ptr, ref spanRef, nextPacketPtr));
                    //
                    //    break;
                    //case C2SPacketId.AoeAck:
                    //    ProcessAoeAck(PacketUtils.ReadLong(ref ptr, ref spanRef, nextPacketPtr), PacketUtils.ReadFloat(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadFloat(ref ptr, ref spanRef, nextPacketPtr));
                    //
                    //    break;
                    //case C2SPacketId.Buy:
                    //    ProcessBuy(PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr));
                    //    break;
                    //case C2SPacketId.CancelTrade:
                    //    ProcessCancelTrade();
                    //    break;
                    //case C2SPacketId.ChangeGuildRank:
                    //    ProcessChangeGuildRank(PacketUtils.ReadString(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr));
                    //
                    //    break;
                    //case C2SPacketId.ChangeTrade:
                    //    ProcessChangeTrade(PacketUtils.ReadBoolArray(ref ptr, ref spanRef, nextPacketPtr));
                    //    break;
                    //case C2SPacketId.ChooseName:
                    //    ProcessChooseName(PacketUtils.ReadString(ref ptr, ref spanRef, nextPacketPtr));
                    //    break;
                    //case C2SPacketId.CreateGuild:
                    //    ProcessCreateGuild(PacketUtils.ReadString(ref ptr, ref spanRef, nextPacketPtr));
                    //    break;
                    //case C2SPacketId.EditAccountList:
                    //    ProcessEditAccountList(PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadBool(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr));
                    //
                    //    break;
                    //case C2SPacketId.EnemyHit:
                    //    ProcessEnemyHit(PacketUtils.ReadLong(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadByte(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr), ReadBool(ref ptr, ref spanRef, nextPacketPtr));
                    //
                    //    break;
                    //case C2SPacketId.Escape:
                    //    ProcessEscape();
                    //    break;
                    //case C2SPacketId.GroundDamage:
                    //    ProcessGroundDamage(PacketUtils.ReadLong(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadFloat(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadFloat(ref ptr, ref spanRef, nextPacketPtr));
                    //
                    //    break;
                    //case C2SPacketId.GuildInvite:
                    //    ProcessGuildInvite(PacketUtils.ReadString(ref ptr, ref spanRef, nextPacketPtr));
                    //    break;
                    //case C2SPacketId.GuildRemove:
                    //    ProcessGuildRemove(PacketUtils.ReadString(ref ptr, ref spanRef, nextPacketPtr));
                    //    break;
                    case C2SPacketId.Hello:
                        {
                            var buildVer = PacketUtils.ReadString(ref ptr, ref spanRef, nextPacketPtr);
                            var gameId = PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr);
                            var guid = PacketUtils.ReadString(ref ptr, ref spanRef, nextPacketPtr);
                            var pwd = PacketUtils.ReadString(ref ptr, ref spanRef, nextPacketPtr);
                            var chrId = PacketUtils.ReadShort(ref ptr, ref spanRef, nextPacketPtr);
                            var createChar = PacketUtils.ReadBool(ref ptr, ref spanRef, nextPacketPtr);
                            var charType = (ushort)(createChar ? (ushort)PacketUtils.ReadShort(ref ptr, ref spanRef, nextPacketPtr) : 0);
                            var skinType = (ushort)(createChar ? (ushort)PacketUtils.ReadShort(ref ptr, ref spanRef, nextPacketPtr) : 0);
                            ProcessHello(buildVer, gameId, guid, pwd, chrId, createChar, charType, skinType);
                            break;
                        }
                    //case C2SPacketId.InvDrop:
                    //    ProcessInvDrop(PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadByte(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr));
                    //
                    //    break;
                    //case C2SPacketId.InvSwap:
                    //    ProcessInvSwap(PacketUtils.ReadLong(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadFloat(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadFloat(ref ptr, ref spanRef, nextPacketPtr), PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadByte(ref ptr, ref spanRef, nextPacketPtr), PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr), PacketUtils.ReadByte(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr));
                    //
                    //    break;
                    //case C2SPacketId.JoinGuild:
                    //    ProcessJoinGuild(PacketUtils.ReadString(ref ptr, ref spanRef, nextPacketPtr));
                    //    break;
                    //case C2SPacketId.Move:
                    //    {
                    //        var tickId = PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr);
                    //        var time = PacketUtils.ReadLong(ref ptr, ref spanRef, nextPacketPtr);
                    //        var x = PacketUtils.ReadFloat(ref ptr, ref spanRef, nextPacketPtr);
                    //        var y = PacketUtils.ReadFloat(ref ptr, ref spanRef, nextPacketPtr);
                    //        ProcessMove(tickId, time, x, y, PacketUtils.ReadMoveRecordArray(ref ptr, ref spanRef, nextPacketPtr));
                    //        break;
                    //    }
                    //case C2SPacketId.OtherHit:
                    //    ProcessOtherHit(PacketUtils.ReadLong(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadByte(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr), PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr));
                    //
                    //    break;
                    //case C2SPacketId.PlayerHit:
                    //    ProcessPlayerHit(PacketUtils.ReadByte(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr));
                    //
                    //    break;
                    //case C2SPacketId.PlayerShoot:
                    //    ProcessPlayerShoot(PacketUtils.ReadLong(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadByte(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadUShort(ref ptr, ref spanRef, nextPacketPtr), PacketUtils.ReadFloat(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadFloat(ref ptr, ref spanRef, nextPacketPtr), PacketUtils.ReadFloat(ref ptr, ref spanRef, nextPacketPtr));
                    //
                    //    break;
                    //case C2SPacketId.PlayerText:
                    //    ProcessPlayerText(PacketUtils.ReadString(ref ptr, ref spanRef, nextPacketPtr));
                    //    break;
                    //case C2SPacketId.Pong:
                    //    ProcessPong(PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadLong(ref ptr, ref spanRef, nextPacketPtr));
                    //
                    //    break;
                    //case C2SPacketId.RequestTrade:
                    //    ProcessRequestTrade(PacketUtils.ReadString(ref ptr, ref spanRef, nextPacketPtr));
                    //    break;
                    //case C2SPacketId.Reskin:
                    //    ProcessReskin((ushort)PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr));
                    //    break;
                    //case C2SPacketId.ShootAck:
                    //    ProcessShootAck(PacketUtils.ReadLong(ref ptr, ref spanRef, nextPacketPtr));
                    //    break;
                    //case C2SPacketId.SquareHit:
                    //    ProcessSquareHit(PacketUtils.ReadLong(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadByte(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr));
                    //
                    //    break;
                    //case C2SPacketId.Teleport:
                    //    ProcessTeleport(PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr));
                    //    break;
                    //case C2SPacketId.UpdateAck:
                    //    ProcessUpdateAck();
                    //    break;
                    //case C2SPacketId.UseItem:
                    //    ProcessUseItem(PacketUtils.ReadLong(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadByte(ref ptr, ref spanRef, nextPacketPtr),
                    //        (ushort)PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadFloat(ref ptr, ref spanRef, nextPacketPtr), PacketUtils.ReadFloat(ref ptr, ref spanRef, nextPacketPtr),
                    //        PacketUtils.ReadByte(ref ptr, ref spanRef, nextPacketPtr));
                    //
                    //    break;
                    //case C2SPacketId.UsePortal:
                    //    ProcessUsePortal(PacketUtils.ReadInt(ref ptr, ref spanRef, nextPacketPtr));
                    //    break;
                    default:
                        SLog.Warn($"Unhandled packet '.{packetId}'.");
                        break;
                }

                ptr = nextPacketPtr;

            }
        }

        public void Send(byte[] packet)
        {
            _pending.Enqueue(packet);
        }
    }

    public class wRandom
    {
        private uint _seed;

        public wRandom(uint seed)
        {
            _seed = seed;
        }

        public void Drop(int count)
        {
            for (var i = 0; i < count; i++)
                Gen();
        }

        public uint NextIntRange(uint min, uint max)
        {
            return min == max ? min : min + Gen() % (max - min);
        }

        private uint Gen()
        {
            var lb = 16807 * (_seed & 0xFFFF);
            var hb = 16807 * (_seed >> 16);
            lb = lb + ((hb & 32767) << 16);
            lb = lb + (hb >> 15);
            if (lb > 2147483647)
            {
                lb = lb - 2147483647;
            }
            return _seed = lb;
        }
    }
}
