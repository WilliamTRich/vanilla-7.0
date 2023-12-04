using System;
using Common;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors;

public sealed class Orbit : Behavior
{
    //State storage: orbit state
    private class OrbitState
    {
        public float Speed;
        public float Radius;
        public int Direction;
    }

    public readonly float Speed;
    public readonly float AcquireRange;
    public readonly float Radius;
    public readonly ushort? Target;
    public readonly float SpeedVariance;
    public readonly float RadiusVariance;
    public readonly bool? OrbitClockwise;

    
    public Orbit(float speed, float radius, float acquireRange = 10,
        string target = null, float? speedVariance = null, float? radiusVariance = null,
        bool? orbitClockwise = false)
    {
        Speed = speed;
        Radius =  radius;
        AcquireRange = acquireRange;
        Target = target == null ? null : (ushort?)GetObjectType(target);
        SpeedVariance = (float)(speedVariance ?? 0);
        RadiusVariance = (float)(radiusVariance ?? 0);
        OrbitClockwise = orbitClockwise;
    }
    public override void Enter(Entity host)
    {
        int orbitDir;
        if (OrbitClockwise == null)
            orbitDir = MathUtils.Next(2) == 1 ? 1 : -1;
        else
            orbitDir = (bool)OrbitClockwise ? 1 : -1;

        host.StateObject[Id] = new OrbitState()
        {
            Speed = Speed + SpeedVariance * (MathUtils.NextFloat() * 2 - 1),
            Radius = Radius + RadiusVariance * (MathUtils.NextFloat() * 2 - 1),
            Direction = orbitDir
        };
    }
    public override bool Tick(Entity host)
    {
        var s = (OrbitState)host.StateObject[Id];

        if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
            return false;

        var entity = host.GetNearestEntity(AcquireRange, Target);

        if (entity != null)
        {

            float angle;
            var hostPos = host.Position;
            var entityPos = entity.Position;
            if (hostPos.Y == entityPos.Y && hostPos.X == entityPos.X)//small offset
                angle = MathF.Atan2(hostPos.Y - entityPos.Y + (MathUtils.NextFloat() * 2 - 1), hostPos.X - entityPos.X + (MathUtils.NextFloat() * 2 - 1));
            else
                angle = MathF.Atan2(hostPos.Y - entityPos.Y, hostPos.X - entityPos.X);
            var angularSpd = s.Direction * host.GetSpeed(s.Speed) / s.Radius;
            angle += angularSpd * Settings.SecondsPerTick;

            var x = entityPos.X + MathF.Cos(angle) * s.Radius;
            var y = entityPos.Y + MathF.Sin(angle) * s.Radius;
            var vect = new Vector2(x, y) - host.Position;
            vect.Normalize();
            vect *= host.GetSpeed(s.Speed) * Settings.SecondsPerTick;

            host.ValidateAndMove(hostPos + vect);
            return true;
        }


        host.StateObject[Id] = s;
        return false;
    }

    public override void Exit(Entity host)
    {
        host.StateObject.Remove(Id);
    }
}