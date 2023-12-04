using System.Collections.Generic;
using System.Linq;
using Common;
using RotMG.Game.Entities;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors;

public sealed class ReproduceChildren : Behavior
{
    private class SpawnState
    {
        public List<Enemy> LivingChildren;
        public int RemainingTime;
    }
    
    public readonly int MaxChildren;
    public readonly int InitialSpawn;
    public readonly int Cooldown;
    public readonly int CooldownVariance;
    public readonly ushort[] Children;

    public ReproduceChildren(int maxChildren = 5, float initialSpawn = 0.5f, int cooldown = 0, int cooldownVariance = 0,
        params string[] children)
    {
        Children = children.Select(GetObjectType).ToArray();
        MaxChildren = maxChildren;
        InitialSpawn = (int)(maxChildren * initialSpawn);
        Cooldown = cooldown;
        CooldownVariance = cooldownVariance;
    }
    public override void Enter(Entity host)
    {   
        host.StateObject[Id] = new SpawnState()
        {
            LivingChildren = new List<Enemy>(),
            RemainingTime = Cooldown.NextCooldown(CooldownVariance)
        };
        
        for (var i = 0; i < InitialSpawn; i++)
        {
            var entity = Entity.Resolve(Children[MathUtils.Next(Children.Length)]);
            var enemyEntity = entity as Enemy;
            (host.StateObject[Id] as SpawnState).LivingChildren.Add(enemyEntity);
            host.Parent.AddEntity(entity, host.Position);
        }
    }

    public override bool Tick(Entity host)
    {
        var spawn = (SpawnState)host.StateObject[Id];

        spawn.RemainingTime -= Settings.MillisecondsPerTick;
        
        foreach (var child in spawn.LivingChildren.Where(child => child.Hp< 0).ToArray())
            spawn.LivingChildren.Remove(child);

        if (spawn.RemainingTime <= 0 && spawn.LivingChildren.Count < MaxChildren)
        {
            var entity = Entity.Resolve(Children[MathUtils.Next(Children.Length)]);

            var enemyEntity = entity as Enemy;
            host.Parent.AddEntity(entity, host.Position);
            spawn.RemainingTime = Cooldown.NextCooldown(CooldownVariance);
            spawn.LivingChildren.Add(enemyEntity);
            return true;
        }

        return false;
    }

    public override void Exit(Entity host)
    {
        host.StateObject.Remove(Id);
    }
}