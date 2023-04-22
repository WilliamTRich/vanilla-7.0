using System.Collections.Generic;
using RotMG.Common;
using RotMG.Utils;

namespace RotMG.Game.Worlds
{
    public sealed class OceanTrench : World
    {
        public Dictionary<IntPoint, Entity> _vents;

        public OceanTrench(Map map, WorldDesc desc) : base(map, desc)
        {
            _vents = new Dictionary<IntPoint, Entity>();
            foreach (var entity in Entities.Values)
            {
                if (entity.Desc.Id == "Ocean Vent")
                    _vents[entity.Position.ToIntPoint()] = entity;
            }
        }

        public override void Tick()
        {
            base.Tick();

            foreach (var player in Players.Values)
            {
                if (player.HasConditionEffect(ConditionEffectIndex.Invincible))
                    continue;

                if (_vents.ContainsKey(player.Position.ToIntPoint()))
                {
                    if (player.Oxygen < 100)
                        player.Oxygen += (int)(75 * Settings.SecondsPerTick);
                    if (player.Oxygen > 100)
                        player.Oxygen = 100;
                }
                else
                {
                    if (player.Oxygen == 0)
                        player.Hp -= (int)(100 * Settings.SecondsPerTick);
                    else
                        player.Oxygen -= (int)(10 * Settings.SecondsPerTick);

                    if (player.Oxygen < 0)
                        player.Oxygen = 0;

                    if (player.Hp <= 0)
                        player.Death(" Lmao he drowned");
                }
            }
        }

        public void addVentToWorld(Entity newVent)
        {
            if (this == null)
                return;

            _vents[new IntPoint { X = (int)newVent.Position.X, Y = (int)newVent.Position.Y }] = newVent;
        }
    }
}