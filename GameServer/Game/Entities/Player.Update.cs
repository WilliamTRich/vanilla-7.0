using Common;
using RotMG.Networking;
using System;
using System.Collections.Generic;

namespace RotMG.Game.Entities;

public partial class Player
{
    public int TickId { get; private set; }
    public int TickTime { get; private set; }

    public const int SightRadius = 15;
    private const float StartAngle = 0;
    private const float EndAngle = (float)(2 * Math.PI);
    private const float RayStepSize = .05f;
    private const float AngleStepSize = 2.30f / (SightRadius * 2);

    private static readonly IntPoint[] SurroundingPoints =
    [
        new(1, 0),
        new(1, 1),
        new(0, 1),
        new(-1, 1),
        new(-1, 0),
        new(-1, -1),
        new(0, -1),
        new(1, -1)
    ];

    private static HashSet<IntPoint> SightCircle;
    public static void InitSightCircle()
    {
        SightCircle = [];
        for (var x = -SightRadius; x <= SightRadius; x++)
            for (var y = -SightRadius; y <= SightRadius; y++)
                if (x * x + y * y <= SightRadius * SightRadius)
                    SightCircle.Add(new IntPoint(x, y));
    }

    private static HashSet<IntPoint>[] SightRays;
    public static void InitSightRays()
    {
        var sightRays = new List<HashSet<IntPoint>>();

        var currentAngle = StartAngle;
        while (currentAngle < EndAngle)
        {
            var ray = new HashSet<IntPoint>();
            var dist = RayStepSize;
            while (dist < SightRadius + 25)
            {
                var point = new IntPoint(
                    (int)(dist * Math.Cos(currentAngle)),
                    (int)(dist * Math.Sin(currentAngle)));

                if (SightCircle.Contains(point))
                    ray.Add(point);
                dist += RayStepSize;
            }
            sightRays.Add(ray);
            currentAngle += AngleStepSize;
        }

        SightRays = sightRays.ToArray();
    }

    public int[,] TileUpdates;
    public Dictionary<int, int> EntityUpdates;
    public HashSet<Entity> Entities;
    public HashSet<IntPoint> CalculatedSightCircle;

    private readonly List<ObjectStatus> _statuses = new(128);

    public void SendNewTick()
    {
        TickId++;
        TickTime = Manager.TickDelta;

        _statuses.Clear();
        foreach (var en in Entities)
            if (EntityUpdates[en.Id] != en.UpdateCount)
            {
                _statuses.Add(en.GetObjectStatus(true));
                EntityUpdates[en.Id] = en.UpdateCount;
            }

        PrivateSVs.Clear();

        AwaitMove(TickId);
        Client.SendNewTick(TickId,TickTime, _statuses);
    }

    private readonly List<ObjectDrop> _drops = new(128);
    private readonly List<ObjectDefinition> _adds = new(128);
    private readonly List<TileData> _tiles = new(768);
    private readonly HashSet<int> _droppedIds = new(128);
    public void SendUpdate()
    {
        var nUpdate = ShouldCalculateSightCircle();
        HashSet<IntPoint> sight = GetSight(nUpdate);

        //Reset lists
        ResetUpdate();
        //Add tiles
        CheckTilesToAdd(nUpdate, sight);

        //Add entities
        CheckEntitiesToAdd(sight);

        //Remove entities and statics (as they end up in the same Entities dictionary
        CheckEntitiesToRemove(sight);

        if (_tiles.Count == 0 && _adds.Count == 0 && _drops.Count == 0)
            return;

        AwaitUpdateAck(Manager.TickWatch.ElapsedMilliseconds);
        Client.SendUpdate(_tiles, _adds, _drops);

        FameStats.TilesUncovered += _tiles.Count;
    }

    private HashSet<IntPoint> GetSight(bool nUpdate) {
        return Parent.BlockSight == 0 ? SightCircle :
                nUpdate ? CalculateSightCircle() : CalculatedSightCircle;
    }

    private void ResetUpdate()
    {
        _tiles.Clear();
        _adds.Clear();
        _drops.Clear();
        _droppedIds.Clear();
    }

    private void CheckTilesToAdd(bool nUpdate, HashSet<IntPoint> sight)
    {
        if (nUpdate)
        {
            //Get tiles
            foreach (var p in sight)
            {
                var x = p.X + (int)Position.X;
                var y = p.Y + (int)Position.Y;
                var tile = Parent.GetTile(x, y);

                if (tile == null || TileUpdates[x, y] == tile.UpdateCount)
                    continue;

                _tiles.Add(new TileData
                {
                    TileType = tile.Type,
                    X = (short)x,
                    Y = (short)y
                });

                TileUpdates[x, y] = tile.UpdateCount;
            }

            //Add statics
            foreach (var p in SightCircle)
            {
                var x = p.X + (int)Position.X;
                var y = p.Y + (int)Position.Y;

                var tile = Parent.GetTile(x, y);
                if (tile == null || tile.StaticObject == null)
                    continue;

                if (TileUpdates[x, y] == tile.UpdateCount)
                {
                    if (Entities.Add(tile.StaticObject))
                    {
                        _adds.Add(tile.StaticObject.GetObjectDefinition());
                        EntityUpdates.Add(tile.StaticObject.Id, tile.StaticObject.UpdateCount);
                    }
                }
            }
        }
    }

    private void CheckEntitiesToRemove(HashSet<IntPoint> sight)
    {
        foreach (var en in Entities)
        {
            var point = new IntPoint
            {
                X = (int)en.Position.X - (int)Position.X,
                Y = (int)en.Position.Y - (int)Position.Y
            };

            if (en.Desc.Static)
            {
                if (en.Parent == null || !SightCircle.Contains(point))
                {
                    _drops.Add(en.GetObjectDrop());
                    _droppedIds.Add(en.Id);
                    EntityUpdates.Remove(en.Id);
                }
            }
            else
            {
                if (en.Parent == null || !sight.Contains(point) && !en.Desc.Player && en != Quest)
                {
                    _drops.Add(en.GetObjectDrop());
                    _droppedIds.Add(en.Id);
                    EntityUpdates.Remove(en.Id);
                }
            }
        }

        Entities.RemoveWhere(k => _droppedIds.Contains(k.Id));
    }

    private void CheckEntitiesToAdd(HashSet<IntPoint> sight)
    {
        foreach (var en in Parent.PlayerChunks.HitTest(Position, SightRadius))
        {
            if (Entities.Add(en))
            {
                _adds.Add(en.GetObjectDefinition());
                EntityUpdates.Add(en.Id, en.UpdateCount);
            }
        }

        //Add players
        foreach (var player in Parent.Players.Values)
        {
            if (Entities.Add(player))
            {
                _adds.Add(player.GetObjectDefinition());
                EntityUpdates.Add(player.Id, player.UpdateCount);
            }
        }

        foreach (var en in Parent.EntityChunks.HitTest(Position, SightRadius))
        {
            var point = new IntPoint
            {
                X = (int)en.Position.X - (int)Position.X,
                Y = (int)en.Position.Y - (int)Position.Y
            };

            if (en is Container)
                if ((en as Container).OwnerId != -1 && (en as Container).OwnerId != AccountId)
                    continue;

            if (sight.Contains(point) && Entities.Add(en))
            {
                _adds.Add(en.GetObjectDefinition());
                EntityUpdates.Add(en.Id, en.UpdateCount);
            }
        }

        //Add quest
        if (Quest != null && Entities.Add(Quest))
        {
            _adds.Add(Quest.GetObjectDefinition());
            EntityUpdates.Add(Quest.Id, Quest.UpdateCount);
        }
    }

    IntPoint _p; int _w;
    private bool ShouldCalculateSightCircle()
    {
        var pos = Position.ToIntPoint();
        if (_p != pos || Parent.UpdateCount != _w)
        {
            _p = pos;
            _w = Parent.UpdateCount;
            return true;
        }
        return false;
    }

    private HashSet<IntPoint> CalculateSightCircle()
    {
        CalculatedSightCircle.Clear();

        if (Parent.BlockSight == 1) //Line casting
        {
            foreach (var ray in SightRays)
                foreach (var p in ray)
                {
                    if (Parent.BlocksSight(p.X + (int)Position.X, p.Y + (int)Position.Y))
                        break;

                    CalculatedSightCircle.Add(p);
                    foreach (var s in SurroundingPoints)
                    {
                        var sp = new IntPoint(p.X + s.X, p.Y + s.Y);
                        if (SightCircle.Contains(sp))
                            CalculatedSightCircle.Add(sp);
                    }
                }
        }

        if (Parent.BlockSight == 2) //Path
        {
            var scan = new Stack<IntPoint>();
            var scanned = new HashSet<int>();

            scan.Push(Position.ToIntPoint());
            while (scan.Count != 0)
            {
                var current = scan.Pop();
                if (CalculatedSightCircle.Add(new IntPoint(current.X - (int)Position.X, current.Y - (int)Position.Y)))
                    foreach (var s in SurroundingPoints)
                    {
                        var p = new IntPoint(current.X + s.X, current.Y + s.Y);
                        var c = new IntPoint(p.X - (int)Position.X, p.Y - (int)Position.Y);
                        if (scanned.Contains(c.GetHashCode()) || !SightCircle.Contains(c))
                            continue;

                        scanned.Add(c.GetHashCode());
                        if (Parent.BlocksSight(p.X, p.Y))
                        {
                            CalculatedSightCircle.Add(c);
                            continue;
                        }

                        scan.Push(p);
                    }
            }
        }

        return CalculatedSightCircle;
    }
}
