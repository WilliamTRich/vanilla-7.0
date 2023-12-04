using System.Collections.Generic;
using System.Linq;
using RotMG.Utils;

namespace RotMG.Game.Logic;

public abstract class Transition : IBehavior
{
    public readonly int Id;

    public Transition(params string[] targetStates)
    {
        StringTargetStates = targetStates.Select(x => x.ToLower()).ToArray();
        Id = ++BehaviorDb.NextId;
    }

    public string[] StringTargetStates; //Only used for parsing.
    public readonly List<int> TargetStates = new List<int>();

    public int GetTargetState() => TargetStates[MathUtils.Next(TargetStates.Count)];

    public virtual void Enter(Entity host) { }
    public virtual bool Tick(Entity host) => false;
    public virtual void Exit(Entity host) { }
}