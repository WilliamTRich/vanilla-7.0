using Common;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors;

public sealed class StayAbove : Behavior
{
    public readonly float Speed;
    public readonly int Altitude;

    public StayAbove(float speed, int altitude)
    {
        Speed = speed;
        Altitude = altitude;
    }
    public override bool Tick(Entity host)
    {
        if (host.HasConditionEffect(ConditionEffectIndex.Paralyzed))
            return false;

        if (!(host.Parent.Map is WMap))
            return false;
        
        var tile = host.Parent.Map.Tiles[(int)host.Position.X, (int)host.Position.Y];
        if (tile.Elevation != 0 && tile.Elevation < Altitude)
        {
            var map = host.Parent.Map;
            var vect = new Vector2(map.Width / 2f - host.Position.X, map.Height / 2f - host.Position.Y);
            vect.Normalize();
            var dist = host.GetSpeed(Speed) * Settings.SecondsPerTick;
            host.ValidateAndMove(host.Position + vect * dist);
            return true;
        }

        return false;
    }
}