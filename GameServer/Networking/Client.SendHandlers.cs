using Common;
using Common.Networking;
using RotMG.Game;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using static RotMG.Networking.GameServer;

namespace RotMG.Networking;

public sealed partial class Client
{
    private object SendLock = new();

    public enum S2CPacketId : byte //Server 2 Client
    {
        Unknown = 0,
        AccountList = 1,
        AllyShoot = 2,
        Aoe = 3,
        BuyResult = 4,
        ClientStat = 5,
        CreateSuccess = 6,
        Damage = 7,
        Death = 8,
        EnemyShoot = 9,
        Failure = 10,
        File = 11,
        GlobalNotification = 12,
        GoTo = 13,
        GuildResult = 14,
        InvResult = 15,
        InvitedToGuild = 16,
        MapInfo = 17,
        NameResult = 18,
        NewTick = 19,
        Notification = 20,
        Pic = 21,
        Ping = 22,
        PlaySound = 23,
        QuestObjId = 24,
        Reconnect = 25,
        ServerPlayerShoot = 26,
        ShowEffect = 27,
        Text = 28,
        TradeAccepted = 29,
        TradeChanged = 30,
        TradeDone = 31,
        TradeRequested = 32,
        TradeStart = 33,
        Update = 34
    }
    public void SendMapInfo(int width, int height, string idName,
        string displayName, uint seed, int difficulty,
        int background, bool allowTeleport, bool showDisplays,
        int bgLightColor, float bgLightIntensity,
        float dayLightIntensity, float nightLightIntensity, long totalElapsedMicroSeconds) {
        lock (SendLock) {
            var ptr = LENGTH_PREFIX;
            ref var spanRef = ref MemoryMarshal.GetReference(_send.Data.AsSpan());
            PacketUtils.WriteByte(ref ptr, ref spanRef, (byte)S2CPacketId.MapInfo);

            PacketUtils.WriteInt(ref ptr, ref spanRef, width);
            PacketUtils.WriteInt(ref ptr, ref spanRef, height);
            PacketUtils.WriteString(ref ptr, ref spanRef, idName);
            PacketUtils.WriteString(ref ptr, ref spanRef, displayName);

            PacketUtils.WriteUInt(ref ptr, ref spanRef, seed);
            PacketUtils.WriteInt(ref ptr, ref spanRef, difficulty);
            PacketUtils.WriteInt(ref ptr, ref spanRef, background);

            PacketUtils.WriteBool(ref ptr, ref spanRef, allowTeleport);
            PacketUtils.WriteBool(ref ptr, ref spanRef, showDisplays);

            PacketUtils.WriteInt(ref ptr, ref spanRef, bgLightColor);
            PacketUtils.WriteFloat(ref ptr, ref spanRef, bgLightIntensity);
            PacketUtils.WriteBool(ref ptr, ref spanRef, dayLightIntensity != 0.0);
            if (dayLightIntensity != 0.0) {
                PacketUtils.WriteFloat(ref ptr, ref spanRef, dayLightIntensity);
                PacketUtils.WriteFloat(ref ptr, ref spanRef, nightLightIntensity);
                PacketUtils.WriteLong(ref ptr, ref spanRef, totalElapsedMicroSeconds);
            }

            TrySend(ptr);
        }
    }

    public void SendFailure(int errorCode, string description) {
        lock (SendLock) {
            var ptr = LENGTH_PREFIX;
            ref var spanRef = ref MemoryMarshal.GetReference(_send.Data.AsSpan());
            PacketUtils.WriteByte(ref ptr, ref spanRef, (byte)S2CPacketId.Failure);

            PacketUtils.WriteInt(ref ptr, ref spanRef, errorCode);
            PacketUtils.WriteString(ref ptr, ref spanRef, description);

            TrySend(ptr);
        }
    }

    public void SendCreateSuccess(int objectId, int charId) {
        lock (SendLock) {
            var ptr = LENGTH_PREFIX;
            ref var spanRef = ref MemoryMarshal.GetReference(_send.Data.AsSpan());
            PacketUtils.WriteByte(ref ptr, ref spanRef, (byte)S2CPacketId.CreateSuccess);

            PacketUtils.WriteInt(ref ptr, ref spanRef, objectId);
            PacketUtils.WriteInt(ref ptr, ref spanRef, charId);

            TrySend(ptr);
        }
    }

    public void SendUpdate(List<TileData> tiles, List<ObjectDefinition> newObjs, List<ObjectDrop> drops) {
        lock (SendLock) {
            var ptr = LENGTH_PREFIX;
            ref var spanRef = ref MemoryMarshal.GetReference(_send.Data.AsSpan());
            PacketUtils.WriteByte(ref ptr, ref spanRef, (byte)S2CPacketId.Update);

            PacketUtils.WriteUShort(ref ptr, ref spanRef, (ushort)tiles.Count);
            foreach (var tile in tiles) {
                PacketUtils.WriteShort(ref ptr, ref spanRef, tile.X);
                PacketUtils.WriteShort(ref ptr, ref spanRef, tile.Y);
                PacketUtils.WriteUShort(ref ptr, ref spanRef, tile.TileType);
            }

            PacketUtils.WriteUShort(ref ptr, ref spanRef, (ushort)drops.Count);
            foreach (var drop in drops)
                PacketUtils.WriteInt(ref ptr, ref spanRef, drop.Id);

            PacketUtils.WriteUShort(ref ptr, ref spanRef, (ushort)newObjs.Count);
            foreach (var newObj in newObjs) {
                PacketUtils.WriteUShort(ref ptr, ref spanRef, newObj.ObjectType);
                PacketUtils.WriteInt(ref ptr, ref spanRef, newObj.ObjectStatus.Id);
                PacketUtils.WriteFloat(ref ptr, ref spanRef, newObj.ObjectStatus.Position.X);
                PacketUtils.WriteFloat(ref ptr, ref spanRef, newObj.ObjectStatus.Position.Y);
                PacketUtils.WriteUShort(ref ptr, ref spanRef, (ushort)newObj.ObjectStatus.Stats.Count);
                var lengthPtr = ptr;
                PacketUtils.WriteUShort(ref ptr, ref spanRef, 0);
                foreach (var stat in newObj.ObjectStatus.Stats) WriteStat(ref ptr, ref spanRef, stat.Key, stat.Value);
                PacketUtils.WriteUShort(ref lengthPtr, ref spanRef, (ushort)(ptr - lengthPtr - 2));
            }

            TrySend(ptr);
        }
    }

    public static void WriteStat(ref int ptr, ref byte spanRef, StatType stat, object val) {
        PacketUtils.WriteByte(ref ptr, ref spanRef, (byte)stat);
        switch (val) {
            //hack
            case Currency value:
                PacketUtils.WriteByte(ref ptr, ref spanRef, (byte)value);
                break;
            case byte value:
                PacketUtils.WriteByte(ref ptr, ref spanRef, value);
                break;
            case bool value:
                PacketUtils.WriteBool(ref ptr, ref spanRef, value);
                break;
            case short value:
                PacketUtils.WriteShort(ref ptr, ref spanRef, value);
                break;
            case ushort value:
                PacketUtils.WriteUShort(ref ptr, ref spanRef, value);
                break;
            case int value:
                PacketUtils.WriteInt(ref ptr, ref spanRef, value);
                break;
            case uint value:
                PacketUtils.WriteUInt(ref ptr, ref spanRef, value);
                break;
            case string value:
                PacketUtils.WriteString(ref ptr, ref spanRef, value);
                break;
            case long value:
                PacketUtils.WriteLong(ref ptr, ref spanRef, value);
                break;
            case ulong value:
                PacketUtils.WriteULong(ref ptr, ref spanRef, value);
                break;
            default:
                SLog.Error($"Unhandled stat {stat}, value: {val}");
                break;
        }
    }

    public void SendNewTick(int tickId, int tickTime, List<ObjectStatus> statuses) {
        lock (SendLock) {
            var ptr = LENGTH_PREFIX;
            ref var spanRef = ref MemoryMarshal.GetReference(_send.Data.AsSpan());
            PacketUtils.WriteByte(ref ptr, ref spanRef, (byte)S2CPacketId.NewTick);

            PacketUtils.WriteInt(ref ptr, ref spanRef, tickId);
            PacketUtils.WriteInt(ref ptr, ref spanRef, tickTime);
            PacketUtils.WriteUShort(ref ptr, ref spanRef, (ushort)statuses.Count);
            foreach (var status in statuses)
            {
                PacketUtils.WriteInt(ref ptr, ref spanRef, status.Id);
                PacketUtils.WriteFloat(ref ptr, ref spanRef, status.Position.X);
                PacketUtils.WriteFloat(ref ptr, ref spanRef, status.Position.Y);
                PacketUtils.WriteUShort(ref ptr, ref spanRef, (ushort)status.Stats.Count);
                var lengthPtr = ptr;
                PacketUtils.WriteUShort(ref ptr, ref spanRef, 0);
                foreach (var stat in status.Stats) WriteStat(ref ptr, ref spanRef, stat.Key, stat.Value);
                PacketUtils.WriteUShort(ref lengthPtr, ref spanRef, (ushort)(ptr - lengthPtr - 2));
            }

            TrySend(ptr);
        }
    }

    public void SendPing(int serial) {
        lock (SendLock) {
            var ptr = LENGTH_PREFIX;
            ref var spanRef = ref MemoryMarshal.GetReference(_send.Data.AsSpan());
            PacketUtils.WriteByte(ref ptr, ref spanRef, (byte)S2CPacketId.Ping);
            PacketUtils.WriteInt(ref ptr, ref spanRef, serial);

            TrySend(ptr);
        }
    }

    public void SendInventoryResult(byte result) {
        lock (SendLock) {
            var ptr = LENGTH_PREFIX;
            ref var spanRef = ref MemoryMarshal.GetReference(_send.Data.AsSpan());
            PacketUtils.WriteByte(ref ptr, ref spanRef, (byte)S2CPacketId.InvResult);

            PacketUtils.WriteByte(ref ptr, ref spanRef, result);

            TrySend(ptr);
        }
    }

    public void SendAllyShoot(byte bulletId, int ownerId, ushort containerType, float angle) {
        lock (SendLock) {
            var ptr = LENGTH_PREFIX;
            ref var spanRef = ref MemoryMarshal.GetReference(_send.Data.AsSpan());
            PacketUtils.WriteByte(ref ptr, ref spanRef, (byte)S2CPacketId.AllyShoot);

            PacketUtils.WriteByte(ref ptr, ref spanRef, bulletId);
            PacketUtils.WriteInt(ref ptr, ref spanRef, ownerId);
            PacketUtils.WriteUShort(ref ptr, ref spanRef, containerType);
            PacketUtils.WriteFloat(ref ptr, ref spanRef, angle);

            TrySend(ptr);
        }
    }
}
