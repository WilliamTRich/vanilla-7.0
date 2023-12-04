using System.Linq;
using Common;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors;

public sealed class SpawnGroup : Behavior
{ 
    private class SpawnState
    {
        public int CurrentNumber;
        public int RemainingTime;
    }
    
    public readonly int MaxChildren;
    public readonly int InitialSpawn;
    public readonly int Cooldown;
    public readonly int CooldownVariance;
    public readonly ushort[] Children;
    public readonly float Radius;

    public SpawnGroup(string group, int maxChildren = 5, float initialSpawn = 0.5f, int cooldown = 0,
        int cooldownVariance = 0, float radius = 0)
    {
        Children = Resources.Id2Object.Values
            .Where(x => x.Group == group)
            .Select(x => x.Type).ToArray();
        MaxChildren = maxChildren;
        InitialSpawn = (int)(maxChildren * initialSpawn);
        Cooldown = cooldown;
        CooldownVariance = cooldownVariance;
        Radius = radius;
    }
    public override void Enter(Entity host)
    {   
        host.StateObject[Id] = new SpawnState()
        {
            CurrentNumber = InitialSpawn,
            RemainingTime = Cooldown.NextCooldown(CooldownVariance)
        };
        
        for (var i = 0; i < InitialSpawn; i++)
        {

            var x = host.Position.X + MathUtils.NextFloat() * Radius;
            var y = host.Position.Y + MathUtils.NextFloat() * Radius;
            var entity = Entity.Resolve(Children[MathUtils.Next(Children.Length)]);
            host.Parent.AddEntity(entity, new Vector2(x, y));
        }
    }

    public override bool Tick(Entity host)
    {
        var spawn = (SpawnState)host.StateObject[Id];

        spawn.RemainingTime -= Settings.MillisecondsPerTick;
        if (spawn.RemainingTime <= 0 && spawn.CurrentNumber < MaxChildren)
        {
            var entity = Entity.Resolve(Children[MathUtils.Next(Children.Length)]);
            host.Parent.AddEntity(entity, host.Position);
            spawn.RemainingTime = Cooldown.NextCooldown(CooldownVariance);
            spawn.CurrentNumber++;
            return true;
        }

        return false;
    }

    public override void Exit(Entity host)
    {
        host.StateObject.Remove(Id);
    }
}