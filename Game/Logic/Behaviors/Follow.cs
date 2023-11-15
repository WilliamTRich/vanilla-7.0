using RotMG.Common;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class Follow : Behavior
    {
        private enum State
        {
            NoTarget,
            HasTarget,
            Resting
        }
        
        private class FollowState
        {
            public State State;
            public int RemainingTime;
        }
        
        public readonly float Speed;
        public readonly float AcquireRange;
        public readonly float Range;
        public readonly int Duration;
        public readonly int Cooldown;
        public readonly int CooldownVariance;
        public readonly bool FollowEnemy;
        public Follow(float speed, float acquireRange = 10, float range = 6, int duration = 0, int cooldown = 0, int cooldownVariance = 0, bool followEnemy = false)
        {
            Speed = speed;
            AcquireRange = acquireRange;
            Range = range;
            Duration = duration;
            Cooldown = cooldown;
            if (cooldown == 0 && duration != 0)
            {
                Cooldown = 1000;
            }
            CooldownVariance = cooldownVariance;
            FollowEnemy = followEnemy;
        }
        public override void Enter(Entity host)
        {
            host.StateObject[Id] = new FollowState();
        }

        public override bool Tick(Entity host)
        {
            var s = (FollowState)host.StateObject[Id];

            s.RemainingTime -= Settings.MillisecondsPerTick;
            if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
                return false;
            //always check for null returns
            var target = FollowEnemy ? host.GetNearestEnemy(AcquireRange) : host.GetNearestPlayer(AcquireRange);
            if(target == null)
            {
                return false;
            }
            Vector2 vect;
            switch (s.State)
            {
                case State.NoTarget:
                    if (target != null && s.RemainingTime <= 0)
                    {
                        s.State = State.HasTarget;
                        if (Duration > 0)
                            s.RemainingTime = Duration;
                        goto case State.HasTarget;
                    }
                    break;
                case State.HasTarget:
                    if (target == null)
                    {
                        s.State = State.NoTarget;
                        s.RemainingTime = 0;
                        break;
                    }
                    else if (s.RemainingTime <= 0 && Duration > 0)
                    {
                        s.State = State.NoTarget;
                        s.RemainingTime = Cooldown.NextCooldown(CooldownVariance);
                        break;
                    }

                    vect = target.Position - host.Position;
                    if (vect.Length() > Range)
                    {
                        vect.X -= MathUtils.NextInt(-1, 2);
                        vect.Y -= MathUtils.NextInt(-1, 2);
                        vect.Normalize();
                        var dist = host.GetSpeed(Speed) * Settings.SecondsPerTick;
                        host.ValidateAndMove(host.Position + vect * dist);
                        host.StateObject[Id] = s;
                        return true;
                    }
                    else
                    {
                        s.State = State.Resting;
                        s.RemainingTime = 0;
                    }
                    break;
                case State.Resting:
                    if (target == null)
                    {
                        s.State = State.NoTarget;
                        if (Duration > 0)
                            s.RemainingTime = Duration;
                        break;
                    }

                    vect = target.Position - host.Position;
                    if (vect.Length() > Range + 1)
                    {
                        s.State = State.HasTarget;
                        s.RemainingTime = Duration;
                        goto case State.HasTarget;
                    }
                    break;
            }
            
            host.StateObject[Id] = s;
            return false;
        }

        public override void Exit(Entity host)
        {
            host.StateObject.Remove(Id);
        }
    }
}