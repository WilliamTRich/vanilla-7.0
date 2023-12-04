using RotMG.Utils;

namespace RotMG.Game.Logic.Transitions;

public class EntityWithinTransition : Transition
{
    public readonly ushort Target;
    public readonly float Radius;

    public EntityWithinTransition(string target, float radius, params string[] targetStates) : base(targetStates)
    {
        Target = Behavior.GetObjectType(target);
        Radius = radius;
    }

    public override bool Tick(Entity host)
    {
        return host.GetNearestEntity(Radius, Target) != null;
    }
}