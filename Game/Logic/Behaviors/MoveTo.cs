using RotMG.Common;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class MoveTo : Behavior
    {
        private readonly float Speed;
        private readonly Vector2 Destination;
        
        public MoveTo(float speed, int x, int y)
        {
            Speed = speed;
            Destination = new Vector2(x, y);
        }
        public override bool Tick(Entity host)
        {
            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            var path = Destination - host.Position;
            var dist = host.GetSpeed(Speed) * Settings.SecondsPerTick;
            if (path.Length() <= dist)
            {
                host.ValidateAndMove(Destination);
                return true;
            }
            
            path.Normalize();
            host.ValidateAndMove(host.Position + path * dist);
            return false;
        }
    }
}