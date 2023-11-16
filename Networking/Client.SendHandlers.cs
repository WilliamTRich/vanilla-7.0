using RotMG.Common;
using RotMG.Game;
using RotMG.Game.Entities;
using RotMG.Utils;
using SimpleLog;
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
    public void SendMapInfo(
        int width,
        int height,
        string idName,
        string displayName,
        uint seed,
        int difficulty,
        int background,
        bool allowTeleport,
        bool showDisplays,
        int bgLightColor,
        float bgLightIntensity,
        float dayLightIntensity,
        float nightLightIntensity,
        long totalElapsedMicroSeconds)
    {
        lock (SendLock)
        {
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

    public void SendFailure(int errorCode, string description)
    {
        lock (SendLock)
        {
            var ptr = LENGTH_PREFIX;
            ref var spanRef = ref MemoryMarshal.GetReference(_send.Data.AsSpan());
            PacketUtils.WriteByte(ref ptr, ref spanRef, (byte)S2CPacketId.Failure);

            PacketUtils.WriteInt(ref ptr, ref spanRef, errorCode);
            PacketUtils.WriteString(ref ptr, ref spanRef, description);

            TrySend(ptr);
        }
    }

    public void SendCreateSuccess(int objectId, int charId)
    {
        lock (SendLock)
        {
            var ptr = LENGTH_PREFIX;
            ref var spanRef = ref MemoryMarshal.GetReference(_send.Data.AsSpan());
            PacketUtils.WriteByte(ref ptr, ref spanRef, (byte)S2CPacketId.CreateSuccess);

            PacketUtils.WriteInt(ref ptr, ref spanRef, objectId);
            PacketUtils.WriteInt(ref ptr, ref spanRef, charId);

            TrySend(ptr);
        }
    }
}
