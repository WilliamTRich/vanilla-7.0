using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors;

public sealed class DropPortalOnDeath : Behavior
{
    public readonly ushort Target;
    public readonly float Chance;
    public readonly int? Lifetime;

    public DropPortalOnDeath(string target, float chance = 1, int? lifetime = -1)
    {
        Target = GetObjectType(target);
        Chance = chance;
        Lifetime = lifetime;
    }
    public override void Death(Entity host)
    {
        if (MathUtils.Chance(Chance))
        {
            var entity = Entity.Resolve(Target);
            if (Lifetime != -1)
                entity.Lifetime = Lifetime;
            
            host.Parent.AddEntity(entity, host.Position);
        }
    }
}