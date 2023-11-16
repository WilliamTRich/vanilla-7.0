using RotMG.Common;
using RotMG.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using RotMG.Utils;
using SimpleLog;

namespace RotMG.Networking
{
    public enum SocketEventState
    {
        Awaiting, //Can start sending/receiving
        InProgress, //Currently sending/receiving
    }

    public class ReceiveState
    {
        public int PacketLength;
        public readonly byte[] PacketBytes;
        public SocketEventState State;

        public ReceiveState()
        {
            PacketBytes = new byte[GameServer.BufferSize];
            PacketLength = GameServer.PrefixLength;
        }

        public byte[] GetPacketBody()
        {
            var packetBody = new byte[PacketLength - GameServer.PrefixLength];
            Array.Copy(PacketBytes, GameServer.PrefixLength, packetBody, 0, packetBody.Length);
            return packetBody;
        }

        public int GetPacketId()
        {
            return PacketBytes[4];
        }

        public void Reset()
        {
            State = SocketEventState.Awaiting;
            PacketLength = 0;
        }
    }

    public class SendState
    {
        public int BytesWritten;
        public int PacketLength;
        public byte[] PacketBytes;
        public SocketEventState State;

        public readonly byte[] Data;

        public SendState() {
            Data = new byte[GameServer.BufferSize];
        }

        public void Reset()
        {
            State = SocketEventState.Awaiting;
            PacketLength = 0;
            BytesWritten = 0;
            PacketBytes = null;
        }
    }

    public static partial class GameServer
    {
        public const int BufferSize = ushort.MaxValue;
        public const int PrefixLength = 5;
        public const int PrefixLengthWithId = PrefixLength - 1;
        public const int AddBackMinDelay = 10000;
        public const byte MaxClientsPerIp = 4;

        private static bool _terminating;
        private static Socket _listener;
        private static ConcurrentQueue<Client> _clients;
        private static ConcurrentQueue<Client> _addBack;
        private static Dictionary<string, int> _connected;

        public static void Init()
        {
            var endpoint = new IPEndPoint(IPAddress.Any, Settings.Ports[1]);
            _listener = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(endpoint);

            _connected = new Dictionary<string, int>();
            _addBack = new ConcurrentQueue<Client>();
            _clients = new ConcurrentQueue<Client>();
            for (var i = 0; i < Settings.MaxClients; i++)
                _clients.Enqueue(new Client(new SendState(), new ReceiveState()));
        }

        public static void Stop()
        {
            _terminating = true;
            Thread.Sleep(200);
        }

        public static async void Start()
        {
            _listener.Listen((int)(Settings.MaxClients * 2f));
            SLog.Info( $"Started GameServer listening at <{_listener.LocalEndPoint}>");

            while (!_terminating)
            {
                try
                {
                    Socket skt;
                    skt = await _listener.AcceptAsync(CancellationToken.None);
                    if (skt == null)
                        continue;

                    //Wait for a client to connect and validate the connection.
                    //var skt = _listener.Accept();

                    var queueBack = new List<Client>();
                    while (_addBack.TryDequeue(out var add))
                    {
                        if (add.IP != null)
                        {
                            _connected[add.IP]--;
                            if (_connected[add.IP] == 0)
                                _connected.Remove(add.IP);
                            add.IP = null;
                        }

                        if (!(Manager.TotalTimeUnsynced - add.DCTime > AddBackMinDelay))
                            queueBack.Add(add);
                        else
                            _clients.Enqueue(add);
                    }

                    foreach (var q in queueBack)
                        _addBack.Enqueue(q);

#if DEBUG
                    if (skt == null || !skt.Connected)
                    {
                        SLog.Warn( "<Socket connection aborted>");
                        continue;
                    }
#endif

#if DEBUG
                    SLog.Debug( $"Client connected from <{skt.RemoteEndPoint}>");
#endif

                    if (!_clients.TryDequeue(out Client client))
                    {
#if DEBUG
                        SLog.Warn($"No pooled client available, aborted connection from <{skt.RemoteEndPoint}>");
#endif
                        skt.Disconnect(false);
                        continue;
                    }

                    var ip = skt.RemoteEndPoint.ToString().Split(':')[0];
                    if (!_connected.TryGetValue(ip, out int value))
                        _connected[ip] = 1;
                    else
                    {
                        if (value == MaxClientsPerIp)
                        {
#if DEBUG
                            SLog.Warn( $"Too many clients connected, disconnecting <{skt.RemoteEndPoint}>");
#endif
                            skt.Disconnect(false);
                            continue;
                        }
                        _connected[ip] = ++value;
                    }

                    client.BeginHandling(skt, ip);
                    //Program.PushWork(() =>
                    //{
                    //});

                    Thread.Sleep(10);
                }
#if DEBUG
                catch (Exception ex)
                {
                    SLog.Error( ex.ToString());
                }
#endif
#if RELEASE
                catch
                {

                }
#endif
            }
        }

        public static void AddBack(Client client)
        {
            client.TokenSource = new();
            client.DCTime = Manager.TotalTimeUnsynced;
            _addBack.Enqueue(client);
        }
    }
}
