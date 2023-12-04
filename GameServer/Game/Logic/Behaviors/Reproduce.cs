using System.Collections.Generic;
using System.Linq;
using Common;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors;

public sealed class Reproduce : Behavior
{
    public readonly float DensityRadius;
    public readonly int DensityMax;
    public readonly ushort? Children;
    public readonly int CoolDown;
    public readonly int CooldownVariance;
    public readonly Region Region;
    public readonly float RegionRange;
    public List<IntPoint> ReproduceRegions; 

    public Reproduce(string children = null,  float densityRadius = 10, int densityMax = 5, int cooldown = 60000, int cooldownVariance = 0, Region region = Region.None, float regionRange = 10)
    {
        Children = children == null ? null : (ushort?)GetObjectType(children);
        DensityRadius = densityRadius;
        DensityMax = densityMax;
        CoolDown = cooldown;
        CooldownVariance = cooldownVariance;
        Region = region;
        RegionRange = regionRange;
    }
    public override void Enter(Entity host)
    {
        host.StateCooldown[Id] = CoolDown;
        if (Region == Region.None)
            return;

        ReproduceRegions = host.Parent.GetAllRegion(Region);
    }

    public override bool Tick(Entity host)
    {
        host.StateCooldown[Id] -= Settings.MillisecondsPerTick;
        if (host.StateCooldown[Id] <= 0)
        {
            var count = host.CountEntities(DensityRadius, Children ?? host.Type);

            if (count < DensityMax)
            {
                var target = host.Position;

                if (ReproduceRegions != null && ReproduceRegions.Count > 0)
                {
                    var regions = ReproduceRegions
                        .Where(point => point.Distance(host) <= RegionRange)
                        .ToArray();
                    var tile = regions[MathUtils.Next(regions.Length)];
                    target = new Vector2()
                    {
                        X = tile.X,
                        Y = tile.Y
                    };
                }

                if (host.Parent.IsUnblocked(target, true))
                {
                    var entity = Entity.Resolve(Children ?? host.Type);
                    host.Parent.AddEntity(entity, target);
                    host.StateCooldown[Id] = CoolDown.NextCooldown(CooldownVariance);
                    return true;
                }
            }
            host.StateCooldown[Id] = CoolDown.NextCooldown(CooldownVariance);
        }

        return false;
    }

    public override void Exit(Entity host)
    {
        host.StateCooldown.Remove(Id);
    }
}