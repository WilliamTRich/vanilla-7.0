using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class Spawn : Behavior
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
        public readonly ushort Children;
        public readonly bool GivesNoXp;

        public Spawn(string children, int maxChildren = 5, float initialSpawn = 0.5f, int cooldown = 0,
            int cooldownVariance = 0, bool givesNoXp = true)
        {
            Children = GetObjectType(children);
            MaxChildren = maxChildren;
            InitialSpawn = (int)(maxChildren * initialSpawn);
            Cooldown = cooldown;
            CooldownVariance = cooldownVariance;
            GivesNoXp = givesNoXp;
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
                var entity = Entity.Resolve(Children);
                host.Parent.AddEntity(entity, host.Position);
            }
        }

        public override bool Tick(Entity host)
        {
            var spawn = (SpawnState)host.StateObject[Id];

            spawn.RemainingTime -= Settings.MillisecondsPerTick;
            if (spawn.RemainingTime <= 0 && spawn.CurrentNumber < MaxChildren)
            {
                var entity = Entity.Resolve(Children);
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
}