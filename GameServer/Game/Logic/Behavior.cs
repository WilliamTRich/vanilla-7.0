using Common;

namespace RotMG.Game.Logic;

public abstract class Behavior : IBehavior
{
    public readonly int Id;

    public Behavior()
    {
        Id = ++BehaviorDb.NextId;
    }

    public virtual void Enter(Entity host) { }
    /// <returns>true if behavior complete</returns>
    public virtual bool Tick(Entity host) => true;
    public virtual void Exit(Entity host) { }
    public virtual void Death(Entity host) { }

    public static ushort GetObjectType(string id)
    {
        if (!Resources.Id2Object.TryGetValue(id, out var desc))
        {
#if DEBUG
            SLog.Warn( $"Object type '{id}' not found. Using Pirate.");
#endif
            desc = Resources.Id2Object["Pirate"];
        }
        return desc.Type;
    }
}