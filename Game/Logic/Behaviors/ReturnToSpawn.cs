using RotMG.Common;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class ReturnToSpawn : Behavior
    {
        public readonly float Speed;
        public readonly float ReturnWithinRadius;
        
        public ReturnToSpawn(float speed, float returnWithinRadius = 1)
        {
            Speed = speed;
            ReturnWithinRadius = returnWithinRadius;
        }
        public override bool Tick(Entity host)
        {
            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            var spawn = host.SpawnPoint;
            var vect = spawn - host.Position;
            if (vect.Length() > ReturnWithinRadius)
            {
                vect.Normalize();
                vect *= host.GetSpeed(Speed) * Settings.SecondsPerTick;
                host.ValidateAndMove(host.Position + vect);
                return true;
            }

            return false;
        }
    }
}