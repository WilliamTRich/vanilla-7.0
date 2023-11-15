using RotMG.Utils;

namespace RotMG.Game.Logic.Transitions
{
    public class NoPlayerWithinTransition : Transition
    {
        public readonly float Radius;
        public readonly bool SeeInvis;
        
        public NoPlayerWithinTransition(float radius, bool seeInvis = false, params string[] targetStates) : base(targetStates)
        {
            Radius = radius;
            SeeInvis = seeInvis;
        }

        public override bool Tick(Entity host)
        {
            return host.GetNearestPlayer(Radius, SeeInvis) == null;
        }
    }
}