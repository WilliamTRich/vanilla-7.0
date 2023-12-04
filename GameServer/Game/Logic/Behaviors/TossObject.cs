using System;
using System.Linq;
using Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors;

public sealed class TossObject : Behavior
{
    public readonly float Range;
    public readonly float? Angle;
    public readonly int Cooldown;
    public readonly int CooldownVariance;
    public readonly int CooldownOffset;
    public readonly bool TossInvis;
    public readonly float Probability;
    public readonly ushort[] Children;
    public readonly float? MinRange;
    public readonly float? MaxRange;
    public readonly float? MinAngle;
    public readonly float? MaxAngle;
    public readonly float? DensityRange;
    public readonly int? MaxDensity;
    public readonly string Group;
    public readonly uint Color;
    /// <summary>
    /// Throw a object with a effect
    /// </summary>
    /// <param name="child">Whats the child name that we spawn?</param>
    /// <param name="range">How far can we throw?</param>
    /// <param name="angle">Angle we throw at?</param>
    /// <param name="cooldown">How often do we throw? in MS</param>
    /// <param name="cooldownVariance">Variance between each cooldown</param>
    /// <param name="cooldownOffset">Offset from the first throw</param>
    /// <param name="tossInvis">Should the toss be invisible</param>
    /// <param name="probability">chance we throw</param>
    /// <param name="group"></param>
    /// <param name="minAngle"></param>
    /// <param name="maxAngle"></param>
    /// <param name="minRange"></param>
    /// <param name="maxRange"></param>
    /// <param name="densityRange"></param>
    /// <param name="maxDensity"></param>
    public TossObject(string child, float range = 5, float? angle = null,
        int cooldown = 1000, int cooldownVariance = 0, int cooldownOffset = 0,
        bool tossInvis = false, float probability = 1, string group = null,
        float? minAngle = null, float? maxAngle = null,
        float? minRange = null, float? maxRange = null,
        float? densityRange = null, int? maxDensity = null,
        uint color = 0xffffbf00)
    {
        if (group == null)
            Children = new [] { GetObjectType(child) };
        else
            Children = Resources.Id2Object.Values
            .Where(x => x.Group == group)
            .Select(x => x.Type).ToArray();
        
        Range = range;
        Angle = angle * MathUtils.ToRadians;
        Cooldown = cooldown;
        CooldownVariance = cooldownVariance;
        CooldownOffset = cooldownOffset;
        TossInvis = tossInvis;
        Probability = probability;
        MinRange = minRange;
        MaxRange = maxRange;
        MinAngle = minAngle * MathUtils.ToRadians;
        MaxAngle = maxAngle * MathUtils.ToRadians;
        DensityRange = densityRange;
        MaxDensity = maxDensity;
        Group = group;
        Color = color;
    }
   
    public override void Enter(Entity host)
    {
        host.StateCooldown[Id] = CooldownOffset;
    }

    public override bool Tick(Entity host)
    {
        host.StateCooldown[Id] -= Settings.MillisecondsPerTick;
        if (host.StateCooldown[Id] <= 0)
        {
            if (host.HasConditionEffect(ConditionEffectIndex.Stunned))
                return false;

            if (!MathUtils.Chance(Probability))
            {
                host.StateCooldown[Id] = Cooldown.NextCooldown(CooldownVariance);
                return false;
            }

            var player = host.GetNearestPlayer(Range);
            if (player != null || Angle != null)
            {
                if (DensityRange != null && MaxDensity != null)
                {
                    int cnt;
                    if (Children.Length > 1)
                        cnt = host.CountEntities(DensityRange.Value, Group);
                    else
                    {
                        cnt = host.CountEntities(DensityRange.Value, Children[0]);
                    }

                    if (cnt >= MaxDensity)
                    {
                        host.StateCooldown[Id] = Cooldown.NextCooldown(CooldownVariance);
                        return false;
                    }
                }

                var r = Range;
                if (MinRange != null && MaxRange != null)
                    r = MathUtils.NextFloat(MinRange.Value, MaxRange.Value);

                var a = Angle;
                if (Angle == null && MinAngle != null && MaxAngle != null)
                    a = MathUtils.NextFloat(MinAngle.Value, MaxAngle.Value);

                Vector2 target;
                if (a != null)
                    target = new Vector2()
                    {
                        X = host.Position.X + r*MathF.Cos(a.Value),
                        Y = host.Position.Y + r*MathF.Sin(a.Value),
                    };
                else
                    target = new Vector2()
                    {
                        X = player.Position.X,
                        Y = player.Position.Y,
                    };

                var eff = GameServer.ShowEffect(ShowEffectIndex.Throw, host.Id, Color, target);

                if (!TossInvis)
                {
                    foreach (var p in host.Parent.PlayerChunks.HitTest(target, Player.SightRadius).OfType<Player>())
                        p.Client.Send(eff);
                }

                var world = host.Parent;
                Manager.AddTimedAction(1500, () =>
                {
                    if (world is null)
                        return;

                    if (!world.IsUnblocked(target, true))
                        return;

                    var entity = Entity.Resolve(Children[MathUtils.Next(Children.Length)]);
                    if (entity.Desc.Static)
                        world.UpdateStatic((int)target.X, (int)target.Y, entity.Type);
                    else
                        world.AddEntity(entity, target);
                });
                host.StateCooldown[Id] = Cooldown.NextCooldown(CooldownVariance);
                return true;
            }
        }
        return false;
    }

    public override void Exit(Entity host)
    {
        host.StateCooldown.Remove(Id);
    }
}