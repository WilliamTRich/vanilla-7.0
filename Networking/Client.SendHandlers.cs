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
            ref var spanRef = ref MemoryMarshal.GetReference(_send.PacketBytes.AsSpan());
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
}
