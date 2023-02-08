using RotMG.Common;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class StayCloseToSpawn : Behavior
    {
        public readonly float Speed;
        public readonly int Range;
        
        public StayCloseToSpawn(float speed, int range = 5)
        {
            Speed = speed;
            Range = range;
        }
        public override void Enter(Entity host)
        {
            host.StateObject[Id] = new Vector2(host.Position.X, host.Position.Y);
        }

        public override bool Tick(Entity host)
        {
            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            var vect = (Vector2) host.StateObject[Id];
            if ((vect - host.Position).Length() > Range)
            {
                vect -= host.Position;
                vect.Normalize();
                var dist = host.GetSpeed(Speed) * Settings.SecondsPerTick;
                host.ValidateAndMove(host.Position + vect * dist);
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