using Common;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors;

public sealed class Protect : Behavior
{
    private enum ProtectState
    {
        DontKnowWhere,
        Protecting,
        Protected,
    }

    public readonly float Speed;
    public readonly ushort Protectee;
    public readonly float AcquireRange;
    public readonly float ProtectionRange;
    public readonly float ReprotectRange;
    
    public Protect(float speed, string protectee, float acquireRange = 10, float protectionRange = 2, float reprotectRange = 1)
    {
        Speed = speed;
        Protectee = GetObjectType(protectee);
        AcquireRange = acquireRange;
        ProtectionRange = protectionRange;
        ReprotectRange = reprotectRange;
    }
    
    
    public override void Enter(Entity host)
    {
        host.StateObject[Id] = ProtectState.DontKnowWhere;
    }
    
    public override bool Tick(Entity host)
    {
        if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
            return false;

        var entity = host.GetNearestEntity(AcquireRange, Protectee);
        Vector2 vect;
        switch (host.StateObject[Id])
        {
            case ProtectState.DontKnowWhere:
                if (entity != null)
                {
                    host.StateObject[Id] = ProtectState.Protecting;
                    goto case ProtectState.Protecting;
                }
                break;
            case ProtectState.Protecting:
                if (entity == null)
                {
                    host.StateObject[Id] = ProtectState.DontKnowWhere;
                    break;
                }

                vect = entity.Position - host.Position;
                if (vect.Length() > ReprotectRange)
                {
                    vect.Normalize();
                    var dist = host.GetSpeed(Speed) * Settings.SecondsPerTick;
                    host.ValidateAndMove(host.Position + vect * dist);
                    return true;
                }
                else 
                    host.StateObject[Id] = ProtectState.Protected;
                break;
            case ProtectState.Protected:
                if (entity == null)
                {
                    host.StateObject[Id] = ProtectState.DontKnowWhere;
                    break;
                }
                vect = entity.Position - host.Position;
                if (vect.Length() > ProtectionRange)
                {
                    host.StateObject[Id] = ProtectState.Protecting;
                    goto case ProtectState.Protecting;
                }
                break;
        }

        return false;
    }

    public override void Exit(Entity host)
    {
        host.StateObject.Remove(Id);
    }
}