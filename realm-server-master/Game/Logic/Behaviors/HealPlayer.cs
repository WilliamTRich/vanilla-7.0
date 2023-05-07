using System.Linq;
using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class HealPlayer : Behavior
    {
        public readonly int Amount;
        public readonly float Radius;
        public readonly int Cooldown;
        public readonly int CooldownVariance;
        public readonly bool MPHeal;
        public HealPlayer(float radius = 5, int amount = 0, int cooldown = 1000, int cooldownVariance = 0, bool isMP = false)
        {
            Amount = amount;
            Radius = radius;
            Cooldown = cooldown;
            CooldownVariance = cooldownVariance;
            MPHeal = isMP;
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

                foreach (var en in host.GetNearestPlayers(Radius, true))
                {
                    if (en.Parent == null || !(en is Player player) || player.HasConditionEffect(ConditionEffectIndex.Sick))
                        continue;
                    
                    
                    var newHp = !MPHeal ? player.MaxHp : player.MaxMP;

                    var newHealth = (int) Amount + (!MPHeal ? player.MaxHp : player.MaxMP);
                    if (newHp > newHealth)
                        newHp = newHealth;
                    

                    if (newHp != (!MPHeal ? player.Hp : player.MP))
                    {
                        var increase = newHp - (!MPHeal ? player.Hp : player.MP);
                        if (!MPHeal) player.Hp = newHp;
                        else player.MP = newHp;
                        var heal = GameServer.ShowEffect(ShowEffectIndex.Heal, player.Id, 0xffffffff, player.Position);
                        var line = GameServer.ShowEffect(ShowEffectIndex.Line, host.Id, 0xffffffff, player.Position);
                        var text = GameServer.Notification(player.Id, "+" + increase, 0xff00ff00);
                        foreach (var e in host.Parent.PlayerChunks.HitTest(host.Position, Player.SightRadius))
                        {
                            if (!(e is Player p)) continue;

                            if (p.Client.Account.Effects)
                            {
                                p.Client.Send(heal);
                                p.Client.Send(line);
                            }
                            if (p.Client.Account.Notifications)
                                p.Client.Send(text);
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