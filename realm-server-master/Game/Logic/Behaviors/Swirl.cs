using System;
using RotMG.Common;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class Swirl : Behavior
    {
        private class SwirlState
        {
            public Vector2 Center;
            public bool Acquired;
            public int RemainingTime;
        }
        public readonly float Speed;
        public readonly float AcquireRange;
        public readonly float Radius;
        public readonly bool Targeted;

        public Swirl(float speed = 1, float radius = 8, float acquireRange = 10, bool targeted = false)
        {
            Speed = speed;
            Radius = radius;
            AcquireRange = acquireRange;
            Targeted = targeted;
        }
        public Swirl(float speed = 1, float radius = 8, float acquireRange = 10)
        {
            Speed = speed;
            Radius = radius;
            AcquireRange = acquireRange;
            Targeted = false;
        }
        public Swirl(float speed = 1, float radius = 8)
        {
            Speed = speed;
            Radius = radius;
            AcquireRange = 10f;
            Targeted = false;
        }
        public Swirl(float speed = 1, bool targeted = false)
        {
            Speed = speed;
            Radius = 8f;
            AcquireRange = 10f;
            Targeted = targeted;
        }
        public Swirl(float speed = 1)
        {
            Speed = speed;
            Radius = 8f;
            AcquireRange = 10f;
            Targeted = false;
        }
        public Swirl()
        {
            Speed = 1f;
            Radius = 8f;
            AcquireRange = 10f;
            Targeted = false;
        }
        public override void Enter(Entity host)
        {
            host.StateObject[Id] = new SwirlState()
            {
                Acquired = !Targeted,
                Center = Targeted ? Vector2.Zero : host.Position
            };
        }
        
        public override bool Tick(Entity host)
        { 
            var s = (SwirlState)host.StateObject[Id];

            s.RemainingTime -= Settings.MillisecondsPerTick;
            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;

            var period = (int)(1000 * Radius / host.GetSpeed(Speed) * (2 * MathF.PI));
            if (!s.Acquired && s.RemainingTime <= 0 && Targeted)
            {
                var entity = host.GetNearestPlayer(AcquireRange);
                if (entity != null && (int)entity.Position.X != (int)host.Position.X && (int)entity.Position.Y != (int)host.Position.Y)
                { 
                    //find circle which pass through host and player pos
                    var l = entity.Position.Distance(host);
                    var hx = (host.Position.X + entity.Position.X) / 2;
                    var hy = (host.Position.Y + entity.Position.Y) / 2;
                    var c = MathF.Sqrt(Math.Abs(Radius * Radius - l * l) / 4);
                    s.Center = new Vector2(
                        hx + c * (host.Position.Y - entity.Position.Y) / l,
                        hy + c * (entity.Position.X - host.Position.X) / l);

                    s.RemainingTime = period;
                    s.Acquired = true;
                }
                else
                    s.Acquired = false;
            }
            else if (s.RemainingTime <= 0 || (s.RemainingTime - period > 200 && host.GetNearestEntity(2) != null))
            {
                if (Targeted)
                {
                    s.Acquired = false;
                    var entity = host.GetNearestPlayer(AcquireRange);
                    if (entity != null)
                        s.RemainingTime = 0;
                    else
                        s.RemainingTime = 5000;
                }
                else
                    s.RemainingTime = 5000;
            }

            float angle;
            if ((int)host.Position.Y == (int)s.Center.Y && (int)host.Position.X == (int)s.Center.X)//small offset
                angle = MathF.Atan2(host.Position.Y - s.Center.Y + (MathUtils.NextFloat() * 2 - 1), host.Position.X - s.Center.X + (MathUtils.NextFloat() * 2 - 1));
            else
                angle = MathF.Atan2(host.Position.Y - s.Center.Y, host.Position.X - s.Center.X);

            var spd = host.GetSpeed(Speed) * (s.Acquired ? 1 : 0.2f);
            var angularSpd = spd / Radius;
            angle += angularSpd * Settings.SecondsPerTick;

            var x = s.Center.X + MathF.Cos(angle) * Radius;
            var y = s.Center.Y + MathF.Sin(angle) * Radius;
            var vect = new Vector2(x, y) - host.Position;
            vect.Normalize();
            vect *= spd * Settings.SecondsPerTick;

            host.ValidateAndMove(host.Position + vect);

            host.StateObject[Id] = s;
            return true;
        }

        public override void Exit(Entity host)
        {
            host.StateObject.Remove(Id);
        }
    }
}