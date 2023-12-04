using RotMG.Networking;
using System.Linq;
using System.Collections.Concurrent;
using Common;

namespace RotMG.Game.Entities;

public partial class Player
{
    private const int PingPeriod = 3000;
    public const int DcThresold = 12000;
    private readonly ConcurrentQueue<long> _clientTimeLog = new();
    private readonly ConcurrentQueue<int> _move = new();
    private readonly ConcurrentQueue<long> _serverTimeLog = new();

    private readonly ConcurrentQueue<long> _shootAckTimeout = new();
    private readonly ConcurrentQueue<long> _updateAckTimeout = new();

    private int _cnt;

    private long _latSum;

    private long _pingTime = -1;
    private long _pongTime = -1;

    private long _sum;

    public long LastClientTime = -1;
    public long LastServerTime = -1;
    public long TimeMap { get; private set; }
    public int Latency { get; private set; }

    private bool KeepAlive()
    {
        if (_pingTime == -1)
        {
            _pingTime = Manager.TickWatch.ElapsedMilliseconds - PingPeriod;
            _pongTime = Manager.TickWatch.ElapsedMilliseconds;
        }

        // check for disconnect timeout
        if (Manager.TickWatch.ElapsedMilliseconds - _pongTime > DcThresold)
        {
            Client.Disconnect("Connection timeout. (KeepAlive)");
            return false;
        }

        // check for shootack timeout
        if (_shootAckTimeout.TryPeek(out var timeout))
            if (Manager.TickWatch.ElapsedMilliseconds > timeout)
            {
                Client.Disconnect("Connection timeout. (ShootAck)");
                return false;
            }

        // check for updateack timeout
        if (_updateAckTimeout.TryPeek(out timeout))
            if (Manager.TickWatch.ElapsedMilliseconds > timeout)
            {
                Client.Disconnect("Connection timeout. (UpdateAck)");
                return false;
            }

        if (Manager.TickWatch.ElapsedMilliseconds - _pingTime < PingPeriod)
            return true;

        // send ping
        _pingTime = Manager.TickWatch.ElapsedMilliseconds;
        Client.SendPing((int)Manager.TickWatch.ElapsedMilliseconds);
        return UpdateOnPing();
    }

    public void Pong(int serial, long pongTime)
    {
        _cnt++;

        _sum += Manager.TickWatch.ElapsedMilliseconds - pongTime;
        TimeMap = _sum / _cnt;

        _latSum += (Manager.TickWatch.ElapsedMilliseconds - serial) / 2;
        Latency = (int)_latSum / _cnt;

        _pongTime = Manager.TickWatch.ElapsedMilliseconds;
    }

    private bool UpdateOnPing()
    {
        // save character
        //if (Parent is not Test) {
        SaveToCharacter();
        //}

        return true;
    }

    public long C2STime(long clientTime)
    {
        return clientTime + TimeMap;
    }

    public long S2CTime(long serverTime)
    {
        return serverTime - TimeMap;
    }

    public void AwaitShootAck(long serverTime)
    {
        _shootAckTimeout.Enqueue(serverTime + DcThresold);
    }

    public void ShootAckReceived() {
        if (!_shootAckTimeout.TryDequeue(out _)) Client.Disconnect("One too many ShootAcks");
    }

    public void AwaitUpdateAck(long serverTime) {
        _updateAckTimeout.Enqueue(serverTime + DcThresold);
    }

    public void UpdateAckReceived()
    {
        if (!_updateAckTimeout.TryDequeue(out _))
            Client.Disconnect("One too many UpdateAcks");
    }

    public void AwaitMove(int tickId)
    {
        _move.Enqueue(tickId);
    }

    public void MoveReceived(int moveTickId, long moveTime)
    {
        if (!_move.TryDequeue(out var tickId))
        {
            Client.Disconnect("One too many MovePackets");
            return;
        }

        if (tickId != moveTickId)
        {
            Client.Disconnect("[NewTick -> Move] TickIds don't match");
            return;
        }

        if (moveTickId > TickId)
        {
            Client.Disconnect("[NewTick -> Move] Invalid tickId");
            return;
        }

        var lastClientTime = LastClientTime;
        var lastServerTime = LastServerTime;
        LastClientTime = moveTime;
        LastServerTime = Manager.TickWatch.ElapsedMilliseconds;

        if (lastClientTime == -1)
            return;

        _clientTimeLog.Enqueue(moveTime - lastClientTime);
        _serverTimeLog.Enqueue((int)(Manager.TickWatch.ElapsedMilliseconds - lastServerTime));

        if (_clientTimeLog.Count < 30)
            return;

        if (_clientTimeLog.Count > 30)
        {
            _clientTimeLog.TryDequeue(out _);
            _serverTimeLog.TryDequeue(out _);
        }

        // calculate average
        var clientDeltaAvg = _clientTimeLog.Sum() / _clientTimeLog.Count;
        var serverDeltaAvg = _serverTimeLog.Sum() / _serverTimeLog.Count;
        var dx = clientDeltaAvg > serverDeltaAvg
            ? clientDeltaAvg - serverDeltaAvg
            : serverDeltaAvg - clientDeltaAvg;

        //safe to remove
#if DEBUG
        if (dx > 15)
            SLog.Debug(
                $"TickId: {tickId}, Client Delta: {_clientTimeLog.Sum() / _clientTimeLog.Count}, Server Delta: {_serverTimeLog.Sum() / _serverTimeLog.Count}");
#endif
    }
}
