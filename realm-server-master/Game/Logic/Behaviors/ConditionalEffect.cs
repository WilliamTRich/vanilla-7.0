using RotMG.Common;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class ConditionalEffect : Behavior
    {
        public readonly ConditionEffectIndex Effect;
        public readonly int Duration;
        public readonly bool Perm;

        public ConditionalEffect(ConditionEffectIndex effect, bool perm = false, int duration = -1)
        {
            Effect = effect;
            Duration = duration;
            Perm = perm;
        }
        public override void Enter(Entity host)
        {
            host.ApplyConditionEffect(Effect, Duration);
        }

        public override void Exit(Entity host)
        {
            if (!Perm) 
                host.RemoveConditionEffect(Effect);
        }
    }
}