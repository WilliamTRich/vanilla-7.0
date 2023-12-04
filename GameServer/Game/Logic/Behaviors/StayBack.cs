using Common;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors;

public sealed class StayBack : Behavior
{
    public readonly float Speed;
    public readonly float Distance;
    public readonly ushort? Entity;

    public StayBack(float speed, float distance = 8, string entity = null)
    {
        Speed = speed;
        Distance = distance;
        Entity = entity == null ? (ushort?)null : GetObjectType(entity);
    }
    public override bool Tick(Entity host)
    {
        if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
            return false;

        var entity = Entity != null ? host.GetNearestEntity(Distance, Entity) : host.GetNearestPlayer(Distance);

        if (entity != null)
        {
            var vect = entity.Position - host.Position;
            vect.Normalize();
            var dist = host.GetSpeed(Speed) * Settings.SecondsPerTick;
            host.ValidateAndMove(host.Position - vect * dist);
            return true;
        }

        return false;
    }
}