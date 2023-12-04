using RotMG.Utils;

namespace RotMG.Game.Logic.Transitions;

public class EntitiesNotWithinTransition : Transition
{
    public readonly ushort[] Targets;
    public readonly float Radius;
    
    public EntitiesNotWithinTransition(float radius, string[] targetStates, string[] targets) : base(targetStates)
    {
        Targets = new ushort[targets.Length];
        for (var i = 0; i < targets.Length; i++)
        {
            Targets[i] = Behavior.GetObjectType(targets[i]);
        }
        Radius = radius;
    }

    public EntitiesNotWithinTransition(int radius, string targetState, params string[] targets) : base(targetState)
    {
        Targets = new ushort[targets.Length];
        for (var i = 0; i < targets.Length; i++)
        {
            Targets[i] = Behavior.GetObjectType(targets[i]);
        }
        Radius = radius;
    }

    public override bool Tick(Entity host)
    {
        foreach (var target in Targets)
        {
            if (host.GetNearestEntity(Radius, target) != null)
                return false;
        }

        return true;
    }
}