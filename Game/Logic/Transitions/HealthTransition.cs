namespace RotMG.Game.Logic.Transitions
{
    public class HealthTransition : Transition
    {
        public float Threshold;

        public HealthTransition(float threshold, params string[] targetStates) : base(targetStates)
        {
            Threshold = threshold;
        }

        public override bool Tick(Entity host)
        {
            var hp = host.GetHealthPercentage();
            return hp <= Threshold;
        }
    }
}
