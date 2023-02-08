using System;
using RotMG.Common;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class MoveLine : Behavior
    {
        public readonly float Speed;
        public readonly float Direction;

        public MoveLine(float speed, float direction = 0)
        {
            Speed = speed;
            Direction = direction * MathUtils.ToRadians;
        }
        public override bool Tick(Entity host)
        {
            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;
            
            var vect = new Vector2(MathF.Cos(Direction), MathF.Sin(Direction));
            var dist = host.GetSpeed(Speed) * Settings.SecondsPerTick;
            host.ValidateAndMove(host.Position + vect * dist);
            return true;
        }
    }
}