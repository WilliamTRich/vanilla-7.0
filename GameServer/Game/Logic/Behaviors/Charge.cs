using Common;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors;

public sealed class Charge : Behavior
{ 
    private class ChargeState
    {
        public Vector2 Direction;
        public int RemainingTime;
    }

    public readonly float Speed;
    public readonly float Range;
    public readonly int Cooldown;
    public readonly int CooldownVariance;

    public Charge(float speed = 4, float range = 10, int cooldown = 2000, int cooldownVariance = 0)
    {
        Speed = speed;
        Range = range;
        Cooldown = cooldown;
        CooldownVariance = cooldownVariance;
    }
    public override void Enter(Entity host)
    {
        host.StateObject[Id] = new ChargeState();
    }
    
    public override bool Tick(Entity host)
    {
        var state = (ChargeState)host.StateObject[Id];
        state.RemainingTime -= Settings.MillisecondsPerTick;
        if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed)) 
            return false;
        
        if (state.RemainingTime <= 0)
        {
            if (state.Direction == Vector2.Zero)
            {
                var player = host.GetNearestPlayer(Range);
                if (player != null && (int)player.Position.X != (int)host.Position.X && (int)player.Position.Y != (int)host.Position.Y)
                {
                    state.Direction = player.Position - host.Position;
                    var d = state.Direction.Length();
                    state.Direction.Normalize();
                    state.RemainingTime = (int)(d / host.GetSpeed(Speed) * 1000);
                }
            }
            else
            {
                state.Direction = Vector2.Zero;
                state.RemainingTime = Cooldown.NextCooldown(CooldownVariance);
            }
        }

        if (state.Direction != Vector2.Zero)
        {
            var dist = host.GetSpeed(Speed) * Settings.SecondsPerTick;
            host.ValidateAndMove(host.Position + state.Direction * dist);
        }
        
        host.StateObject[Id] = state;
        return state.Direction != Vector2.Zero;
    }
    
    public override void Exit(Entity host)
    {
        host.StateObject.Remove(Id);
    }
}