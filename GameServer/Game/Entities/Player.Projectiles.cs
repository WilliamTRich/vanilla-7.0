﻿using Common;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RotMG.Game.Entities;

public class AoeAck
{
    public int Damage;
    public ConditionEffectDesc[] Effects;
    public Vector2 Position;
    public string Hitter;
    public float Radius;
    public int Time;
}

public struct ProjectileAck
{
    public static ProjectileAck Undefined = new ProjectileAck
    {
        Projectile = null,
        Time = -1
    };

    public Projectile Projectile;
    public int Time;

    public override bool Equals(object obj)
    {
#if DEBUG
        if (obj is null)
            throw new Exception("Undefined object");
#endif
        return (obj as ProjectileAck?).Value.Projectile.Id == Projectile.Id;
    }

    public override int GetHashCode()
    {
        return Projectile.Id;
    }
}

public partial class Player
{
    private const int TimeUntilAckTimeout = 2000;
    private const int TickProjectilesDelay = 100;
    private const float RateOfFireThreshold = 1.1f;
    private const float EnemyHitTrackPrecision = 8;
    private const int EnemyHitHistoryBacktrack = 2;
    public const float EnemyHitRangeAllowance = 1.7f;

    public Queue<List<Projectile>> AwaitingProjectiles;
    public Dictionary<int, ProjectileAck> AckedProjectiles;

    public Queue<AoeAck> AwaitingAoes; //Doesn't really belong here... But Player.Aoe.cs???

    public Dictionary<int, Projectile> ShotProjectiles;
    public int NextAEProjectileId = int.MinValue; //Goes up positively from bottom (Server sided projectiles)
    public int NextProjectileId; //Goes down negatively (Client sided projectiles)
    public int ShotTime;
    public int ShotDuration;

    public void TickProjectiles()
    {
        if (Manager.TotalTime % TickProjectilesDelay != 0)
            return;

        foreach (var aoe in AwaitingAoes)
        {
            if (ClientTime - aoe.Time > TimeUntilAckTimeout + Constants.MaxLatencyMS)
            {
#if DEBUG
                SLog.Error( "Aoe ack timed out");
#endif
                Client.Disconnect();
                return;
            }
        }

        foreach (var apList in AwaitingProjectiles)
        {
            foreach (var ap in apList)
            {
                if (ClientTime - ap.Time > TimeUntilAckTimeout + Constants.MaxLatencyMS)
                {
#if DEBUG
                    SLog.Error( "Proj ack timed out");
#endif
                    Client.Disconnect();
                    return;
                }
            }
        }
    }

    public int GetClientDamage(int min, int max, bool useMult = false)
    {
        var mult = useMult ? GetAttackMultiplier() : 1.0;
        return (int)(Client.Random.NextIntRange((uint)min, (uint)max) * mult);
    }

    public int GetNextDamageSeeded(int min, int max, int data)
    {
        var dmgMod = ItemDesc.GetStat(data, ItemData.Damage, ItemDesc.DamageMultiplier);
        var minDmg = min + (int)(min * dmgMod);
        var maxDmg = max + (int)(max * dmgMod);
        return (int)Client.Random.NextIntRange((uint)minDmg, (uint)maxDmg);
    }

    public int GetNextDamage(int min, int max, int data)
    {
        var dmgMod = ItemDesc.GetStat(data, ItemData.Damage, ItemDesc.DamageMultiplier);
        var minDmg = min + (int)(min * dmgMod);
        var maxDmg = max + (int)(max * dmgMod);
        return MathUtils.NextInt(minDmg, maxDmg);
    }

    public void TryHitEnemy(int time, int bulletId, int targetId, bool kill)
    {
        if (!ValidTime(time))
        {
#if DEBUG
            SLog.Error("Invalid time for enemy hit");
#endif
            Client.Disconnect();
            return;
        }

        if (!ShotProjectiles.TryGetValue(bulletId, out var p)) {
#if DEBUG
            SLog.Error("Tried to hit enemy with undefined projectile");

#endif
            return;
        }

        var target = Parent.GetEntity(targetId);
        if (target == null || !target.Desc.Enemy) {
#if DEBUG
            SLog.Error("Invalid enemy target");
#endif
            return;
        }
        var elapsed = time - p.Time;
        var steps = (int)Math.Ceiling(p.Desc.Speed / 100f * (elapsed * EnemyHitTrackPrecision / 1000f));
        var timeStep = (float)elapsed / steps;

        for (var k = 0; k <= steps; k++) {
            var pos = p.PositionAt(k * timeStep);
            //Try hit enemy
            if (k == steps) {
                if (target.Desc.Static) {
                    if (pos.Distance(target.Position) <= EnemyHitRangeAllowance && p.CanHit(target)) {
                        target.HitByProjectile(p);
                        if (!p.Desc.MultiHit)
                            ShotProjectiles.Remove(p.Id);
                        return;
                    }
                }
                else {
                    for (var j = 0; j <= EnemyHitHistoryBacktrack; j++) {
                        if (pos.Distance(target.TryGetHistory(j)) <= EnemyHitRangeAllowance && p.CanHit(target)) {
                            target.HitByProjectile(p);
                            if (!p.Desc.MultiHit)
                                ShotProjectiles.Remove(p.Id);
                            return;
                        }
                    }
                }
#if DEBUG
            //Console.WriteLine(pos);
            //Console.WriteLine(target);
            SLog.Error("Enemy hit aborted, too far away from projectile");
#endif
            }
            else //Check collisions to make sure player isn't shooting through walls etc
            {
                var tile = Parent.GetTileF(pos.X, pos.Y);

                if (tile == null || tile.Type == 255 || tile.StaticObject != null && 
                    !tile.StaticObject.Desc.Enemy && (tile.StaticObject.Desc.EnemyOccupySquare || 
                    !p.Desc.PassesCover && tile.StaticObject.Desc.OccupySquare))
                {
#if DEBUG
                    SLog.Error( "Shot projectile hit wall, removed");
#endif
                    ShotProjectiles.Remove(bulletId);
                    return;
                }
            }
        }
    }
    internal Projectile PlayerShootProjectile(
        byte id, ProjectileDesc desc, ushort objType,
        long startTime, Vector2 position, float angle)
    {
        var dmg = GetClientDamage(desc.MinDamage, desc.MaxDamage);
        return CreateProjectile(desc, objType, dmg,
            C2STime(startTime), position, angle);
    }
    public Projectile CreateProjectile(ProjectileDesc desc, ushort container, int dmg, long time, Vector2 pos, float angle)
    {
        var ret = new Projectile(this, desc, NextProjectileId++, (int)time, angle, pos, dmg); //Assume only one

        ShotProjectiles.Add(ret.Id, ret);
        return ret;
    }
    //public void TryShoot(int time, Vector2 pos, float attackAngle, bool ability, int numShots)
    //{
    //    if (!ValidTime(time))
    //    {
//#i//f DEBUG
    //        SLog.Error( "Invalid time for player shoot");
//#e//ndif
    //        Client.Disconnect();
    //        return;
    //    }
    //
    //    if (AwaitingGoto.Count > 0)
    //    {
    //        Client.Random.Drop(numShots);
    //        return;
    //    }
    //
    //    if (!ValidMove(time, pos))
    //    {
//#i//f DEBUG
    //        SLog.Error( "Invalid move for player shoot");
//#e//ndif
    //        Client.Disconnect();
    //        return;
    //    }
    //
    //    var startId = NextProjectileId;
    //    NextProjectileId -= numShots;
    //
    //    var desc = ability ? GetItem(1) : GetItem(0);
    //    if (desc == null)
    //    {
//#i//f DEBUG
    //        SLog.Error( "Undefined item descriptor");
//#e//ndif
    //        Client.Random.Drop(numShots);
    //        return;
    //    }
    //
    //
    //    if (numShots != desc.NumProjectiles)
    //    {
//#i//f DEBUG
    //        SLog.Error( "Manipulated num shots");
//#e//ndif
    //        Client.Random.Drop(numShots);
    //        return;
    //    }
    //
    //    if (HasConditionEffect(ConditionEffectIndex.Stunned))
    //    {
//#i//f DEBUG
    //        SLog.Error( "Stunned...");
//#e//ndif
    //        Client.Random.Drop(numShots);
    //        return;
    //    }
    //
    //    if (ability)
    //    {
    //        if (ShootAEs.TryDequeue(out var aeItemType))
    //        {
    //            if (aeItemType != desc.Type)
    //            {
    //                Client.Random.Drop(numShots);
    //                return;
    //            }
    //
    //            var arcGap = desc.ArcGap * MathUtils.ToRadians;
    //            var totalArc = arcGap * (numShots - 1);
    //            var angle = attackAngle - totalArc / 2f;
    //            for (var i = 0; i < numShots; i++)
    //            {
    //                var damage = (int)(GetNextDamageSeeded(desc.Projectile.MinDamage, desc.Projectile.MaxDamage, ItemDatas[1]) * GetAttackMultiplier());
    //                var projectile = new Projectile(this, desc.Projectile, startId - i, time, angle + arcGap * i, pos, damage);
    //                ShotProjectiles.Add(projectile.Id, projectile);
    //            }
    //
    //            var packet = GameServer.AllyShoot(Id, desc.Type, attackAngle);
    //            foreach (var en in Parent.PlayerChunks.HitTest(Position, SightRadius))
    //                if (en is Player player && player.Client.Account.AllyShots && !player.Equals(this))
    //                    player.Client.Send(packet);
    //
    //            FameStats.Shots += numShots;
    //        }
    //        else
    //        {
//#i//f DEBUG
    //            SLog.Error( "Invalid ShootAE");
//#e//ndif
    //            Client.Random.Drop(numShots);
    //        }
    //    }
    //    else
    //    {
    //        if (time > ShotTime + ShotDuration)
    //        {
    //            var arcGap = desc.ArcGap * MathUtils.ToRadians;
    //            var totalArc = arcGap * (numShots - 1);
    //            var angle = attackAngle - totalArc / 2f;
    //            for (var i = 0; i < numShots; i++)
    //            {
    //                var damage = (int)(GetNextDamageSeeded(desc.Projectile.MinDamage, desc.Projectile.MaxDamage, ItemDatas[0]) * GetAttackMultiplier());
    //                var projectile = new Projectile(this, desc.Projectile, startId - i, time, angle + arcGap * i, pos, damage);
    //                ShotProjectiles.Add(projectile.Id, projectile);
    //            }
    //
    //            var packet = GameServer.AllyShoot(Id, desc.Type, attackAngle);
    //            foreach (var en in Parent.PlayerChunks.HitTest(Position, SightRadius))
    //                if (en is Player player && player.Client.Account.AllyShots && !player.Equals(this))
    //                    player.Client.Send(packet);
    //
    //            FameStats.Shots += numShots;
    //            var rateOfFireMod = ItemDesc.GetStat(ItemDatas[0], ItemData.RateOfFire, ItemDesc.RateOfFireMultiplier);
    //            var rateOfFire = desc.RateOfFire;
    //            rateOfFire *= 1 + rateOfFireMod;
    //            ShotDuration = (int)(1f / GetAttackFrequency() * (1f / rateOfFire) * (1f / RateOfFireThreshold));
    //            ShotTime = time;
    //        }
    //
    //        else
    //        {
//#i//f DEBUG
    //            SLog.Error( "Shot too early, ignored");
//#e//ndif
    //            Client.Random.Drop(numShots);
    //        }
    //    }
    //}

    public void AwaitAoe(AoeAck aoe)
    {
        AwaitingAoes.Enqueue(aoe);
    }

    public bool CheckProjectiles(int time)
    {
        foreach (var p in ShotProjectiles.ToArray())
        {
            var elapsed = time - p.Value.Time;
            if (elapsed > p.Value.Desc.LifetimeMS)
            {
                ShotProjectiles.Remove(p.Key);
                continue;
            }
        }
        foreach (var p in AckedProjectiles.ToArray()) 
        {
            var elapsed = time - p.Value.Time;
            if (elapsed > p.Value.Projectile.Desc.LifetimeMS)
            {
                AckedProjectiles.Remove(p.Key);
                continue;
            }

            var pos = p.Value.Projectile.PositionAt(elapsed);
            var dx = Math.Abs(Position.X - pos.X);
            var dy = Math.Abs(Position.Y - pos.Y);
            if (dx <= 0.4f && dy <= 0.4f)
            {
                if (p.Value.Projectile.CanHit(this))
                {
                    if (HitByProjectile(p.Value.Projectile))
                    {
#if DEBUG
                        //SLog.Error( "Died cause of server collision");
#endif
                        return true;
                    }
                    AckedProjectiles.Remove(p.Key);
#if DEBUG
                    //SLog.Error( "Collided on server");
#endif
                }
#if DEBUG
                else
                {
                    //SLog.Error( "In range but can't hit...?");
                }
#endif
            }
        }
        return false;
    }

    public void TryHit(int bulletId, int objId)
    {
        //Causes dead enemies bullets to do 0 dmg
        //var shooter = Parent.GetEntity(objId);
        //if (shooter == null)
        //    return;

        if (AckedProjectiles.TryGetValue(bulletId, out var v))
        {
            if (v.Projectile.CanHit(this))
            {
                HitByProjectile(v.Projectile);
                AckedProjectiles.Remove(bulletId);
            }
        }
#if DEBUG
        else
        {
            SLog.Error( "Tried to hit with undefined projectile");
        }
#endif
    }

    public override bool HitByProjectile(Projectile projectile)
    {
        return Damage(Resources.Type2Object[projectile.Desc.ContainerType].DisplayId,
               projectile.Damage, 
               projectile.Desc.Effects, 
               projectile.Desc.ArmorPiercing);
    }

    public void AwaitProjectiles(List<Projectile> projectiles)
    {
        AwaitingProjectiles.Enqueue(projectiles);
    }

    public void TryHitSquare(int time, int bulletId)
    {
        if (!ValidTime(time))
        {
#if DEBUG
            SLog.Error( "HitSquare invalid time");
#endif
            Client.Disconnect();
            return;
        }

        if (AckedProjectiles.TryGetValue(bulletId, out var ac))
        {
            var pos = ac.Projectile.PositionAt(time - ac.Time);
            var tile = Parent.GetTileF(pos.X, pos.Y);

            if (tile == null || tile.Type == 255 || TileUpdates[(int)pos.X, (int)pos.Y] != Parent.Tiles[(int)pos.X, (int)pos.Y].UpdateCount ||
                tile.StaticObject != null && (tile.StaticObject.Desc.EnemyOccupySquare || !ac.Projectile.Desc.PassesCover && tile.StaticObject.Desc.OccupySquare))
                AckedProjectiles.Remove(bulletId);
#if DEBUG
            else
            {
                SLog.Error( "Manipualted SquareHit?");
            }
#endif
        }
#if DEBUG
        else
        {
            SLog.Error( "Tried to hit square with undefined projectile");
        }
#endif
    }

    public void TryAckAoe(int time, Vector2 pos)
    {
        if (!ValidTime(time))
        {
#if DEBUG
            SLog.Error( "AoeAck invalid time");
#endif
            Client.Disconnect();
            return;
        }

        if (AwaitingAoes.TryDequeue(out var aoe))
        {
            if (!ValidMove(time, pos) && AwaitingGoto.Count == 0)
            {
#if DEBUG
                SLog.Error( "INVALID MOVE FOR AOEACK!");
#endif
                Client.Disconnect();
                return;
            }

            if (pos.Distance(aoe.Position) < aoe.Radius && !HasConditionEffect(ConditionEffectIndex.Invincible))
            {
                Damage(aoe.Hitter, aoe.Damage, aoe.Effects, false);
            }
        }
        else
        {
#if DEBUG
            SLog.Error( "AoeAck desync");
#endif
            Client.Disconnect();
        }
    }

    public void TryShootAck(int time)
    {
        if (!ValidTime(time))
        {
#if DEBUG
            SLog.Error( "ShootAck invalid time");
#endif
            Client.Disconnect();
            return;
        }

        if (AwaitingProjectiles.TryDequeue(out var projectiles))
        {
            foreach (var p in projectiles)
            {
                if (p.Owner.Equals(this))
                {
                    p.Time = time;
                    ShotProjectiles[p.Id] = p;
                }
                else
                {
#if DEBUG
                    if (AckedProjectiles.ContainsKey(p.Id))
                    {
                        SLog.Warn("Duplicate ack key");
                    }
#endif
                    var ack = new ProjectileAck { Projectile = p, Time = time };
                    AckedProjectiles[p.Id] = ack;
                }
            }
        }
        else
        {
#if DEBUG
            SLog.Error( "ShootAck desync");
#endif
            Client.Disconnect();
        }
    }
}
