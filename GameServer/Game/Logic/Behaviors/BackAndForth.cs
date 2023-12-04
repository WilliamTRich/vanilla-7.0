using Common;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors;

public sealed class BackAndForth : Behavior
{
    public readonly float Speed;
    public readonly float Distance;
    
    public BackAndForth(float speed, int distance = 5)
    {
        Speed = speed;
        Distance = distance;
    }
    public override void Enter(Entity host)
    {
        host.StateObject[Id] = Distance;
    }

    public override bool Tick(Entity host)
    {
        if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed)) 
            return false;

        var dist = (float)host.StateObject[Id];

        var moveDist = host.GetSpeed(Speed) * Settings.SecondsPerTick;
        if (dist > 0)
        {
            host.ValidateAndMove(new Vector2(host.Position.X + moveDist, host.Position.Y));
            dist -= moveDist;
            if (dist <= 0)
                dist = -Distance;
        }
        else
        {
            host.ValidateAndMove(new Vector2(host.Position.X - moveDist, host.Position.Y));
            dist += moveDist;
            if (dist >= 0)
                dist = Distance;
        }

        host.StateObject[Id] = dist;
        return true;
    }
    
    public override void Exit(Entity host)
    {
        host.StateObject.Remove(Id);
    }
}