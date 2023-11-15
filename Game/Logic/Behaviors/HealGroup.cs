using System.Linq;
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class HealGroup : Behavior
    { 
        public readonly float Radius;
        public readonly string Group;
        public readonly int Cooldown;
        public readonly int CooldownVariance;
        public readonly int Amount;

        public HealGroup(float radius, string name = null, int amount = 0, int cooldown = 1000,
            int cooldownVariance = 0)
        {
            Radius = radius;
            Group = name;
            Cooldown = cooldown;
            CooldownVariance = cooldownVariance;
            Amount = amount;
        }
        public override void Enter(Entity host)
        {
            host.StateCooldown[Id] = 0;
        }

        public override bool Tick(Entity host)
        {
            host.StateCooldown[Id] -= Settings.MillisecondsPerTick;
            if (host.StateCooldown[Id] <= 0)
            {
                if (Amount <= 0) return false;

                if (host.HasConditionEffect(ConditionEffectIndex.Stunned))
                    return false;

                foreach (var entity in host.GetNearestEntitiesByGroup(Radius, Group))
                {
                    var newHp = entity.MaxHp;

                    var newHealth = (int) Amount + entity.Hp;
                    if (newHp > newHealth)
                        newHp = newHealth;
                    

                    if (newHp != entity.Hp)
                    {
                        var increase = newHp - entity.Hp;
                        entity.Hp = newHp;
                        var heal = GameServer.ShowEffect(ShowEffectIndex.Heal, entity.Id, 0xffffffff, entity.Position);
                        var line = GameServer.ShowEffect(ShowEffectIndex.Line, host.Id, 0xffffffff, entity.Position);
                        var text = GameServer.Notification(entity.Id, "+" + increase, 0xff00ff00);
                        foreach (var ent in host.Parent.PlayerChunks.HitTest(host.Position, Player.SightRadius))
                        {
                            if (!(ent is Player player)) continue;

                            if (player.Client.Account.Effects)
                            {
                                player.Client.Send(heal);
                                player.Client.Send(line);
                            }
                            if (player.Client.Account.Notifications)
                                player.Client.Send(text);
                        }
                    }
                }

                host.StateCooldown[Id] = Cooldown.NextCooldown(CooldownVariance);
                return true;
            }
            
            return false;
        }
        
        public override void Exit(Entity host)
        {
            host.StateCooldown.Remove(Id);
        }
    }
}