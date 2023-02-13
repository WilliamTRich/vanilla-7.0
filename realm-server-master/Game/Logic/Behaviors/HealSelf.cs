using System.Linq;
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class HealSelf : Behavior
    {
        public readonly int Cooldown;
        public readonly int CooldownVariance;
        public readonly int Amount;

        public HealSelf(int amount = 0, int cooldown = 1000,
            int cooldownVariance = 0)
        {
            Amount = amount;
            Cooldown = cooldown;
            CooldownVariance = cooldownVariance;
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
                
                var newHp = host.MaxHp;
                var newHealth = (int) Amount + host.Hp;
                if (newHp > newHealth)
                    newHp = newHealth;


                if (newHp != host.Hp)
                {
                    var increase = newHp - host.Hp;
                    host.Hp = newHp;
                    var heal = GameServer.ShowEffect(ShowEffectIndex.Heal, host.Id, 0xffffffff, host.Position);
                    var text = GameServer.Notification(host.Id, "+" + increase, 0xff00ff00);
                    foreach (var player in host.Parent.PlayerChunks.HitTest(host.Position, Player.SightRadius).OfType<Player>())
                    {
                        if (player.Client.Account.Effects)
                        {
                            player.Client.Send(heal);
                        }
                        if (player.Client.Account.Notifications)
                            player.Client.Send(text);
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