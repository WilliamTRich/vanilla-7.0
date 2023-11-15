using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Worlds;
using RotMG.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Game.Logic.Commands
{
    public static class RankedCommands
    {
        private static ObjectDesc GetObjectDesc(string name)
        {
            if (Resources.Id2Object.TryGetValue(name, out var item))
                return item;

            var items = Resources.Id2Object
                .Where(i => i.Key.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                .Select(i => i.Value)
                .ToArray();
            /* The Resource Id2Object doesnt account if the name is lower case,
               so if the length of any of the items in the array is the same as what was inputted
               assume that it was a case sensitive issue and return that item itself.
             */
            foreach (var i in items)
            {
                if (name.Length == i.DisplayId.Length)
                    return i;
            }

            return items.Length == 0 ? null : items[0];
        }

        private static ItemDesc GetItemDesc(string name)
        {
            if (Resources.Id2Item.TryGetValue(name, out var item))
                return item;

            var items = Resources.Id2Item
                .Where(i => i.Key.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                .Select(i => i.Value)
                .ToArray();
            return items.Length == 0 ? null : items[0];
        }
        [Command("Give", true, aliases: "Gimmie")]
        public static void Give(Player player, string name)
        {

            var item = GetItemDesc(name);

            if (item == null)
            {
                player.SendError($"Item <{name}> not found in GameData");
                return;
            }

            if (!player.Client.Account.Ranked)
            {
                player.SendError($"Unknown Command");
                return;
            }
            
            if (player.GiveItem(item.Type))
                player.SendInfo("Success");
            else player.SendError("No inventory slots");
        }
        [Command("Max", true)]
        public static void Max(Player player)
        {

            for (var i = 0; i < player.Stats.Length; i++)
            {
                player.Stats[i] = ((PlayerDesc)player.Desc).Stats[i].MaxValue;
            }

            Level20(player);
            player.UpdateStats();
            player.SendInfo("Maxed");
        }

        [Command("Level20", true, aliases: "l20")]
        public static void Level20(Player player)
        {
            if (player.Level < 20)
            {
                player.GainEXP(40000);
                player.Level = 20;
            }
        }

        [Command("Tq", true)]
        public static void Tq(Player player)
        {
            if (player.Quest == null)
            {
                player.SendError("No quest to teleport to");
                return;
            }

            player.ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 3000);
            player.ApplyConditionEffect(ConditionEffectIndex.Stunned, 1500);
            player.Teleport(player.ClientTime, player.Quest.Position);
        }

        [Command("Effect", true, aliases: "eff")]
        public static void Effect(Player player, string effect)
        {
            ConditionEffectIndex eff;
            if (int.TryParse(effect, out var effInt))
            {
                eff = (ConditionEffectIndex)effInt;
            }
            else if (!Enum.TryParse(effect, true, out eff))
            {
                player.SendError("Invalid effect");
                return;
            }

            if (eff == ConditionEffectIndex.Nothing)
                return;
            if (player.HasConditionEffect(eff))
            {
                player.RemoveConditionEffect(eff);
                player.SaveToCharacter();
                player.SendInfo("Removed condition effect " + eff);
            }
            else
            {
                player.ApplyConditionEffect(eff, -1);
                player.SaveToCharacter();
                player.SendInfo("Applied condition effect " + eff);
            }
        }


        [Command("Kick", true)]
        public static void Kick(Player player, string name)
        {
            var other = Manager.GetPlayer(name.ToLower());
            if (other == null)
            {
                player.SendError("Player not found");
                return;
            }

            other.Client.Disconnect();
            player.SendInfo($"{name} disconnected");
        }

        [Command("Summon", true)]
        public static void Summon(Player player, string name)
        {
            var other = player.Parent.Players.Values.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (other == null || !other.Parent.Equals(player.Parent))
            {
                player.SendError("Player not found");
                return;
            }


            other.EntityTeleport(other.ClientTime, player.Id, true);
            other.SendHelp($"You have been summoned by {player.Name}");
            player.SendInfo("Player summoned");
        }

        [Command("Summonall", true)]
        public static void SummonAll(Player player)
        {
            foreach (var other in player.Parent.Players.Values)
            {
                other.EntityTeleport(other.ClientTime, player.Id, true);
                other.SendHelp($"You have been summoned by {player.Name}");
            }
            player.SendInfo("All players summoned");

        }

        [Command("Quake", true)]
        public static void Quake(Player player, string worldName = null)
        {
            if (worldName is null)
            {
                var worlds = Resources.Worlds.Values
                    .Where(x => !x.IsTemplate)
                    .Select(x => x.Name);
                player.SendInfo($"Valid world names: {string.Join(", ", worlds)}");
                return;
            }

            if (player.Parent is Nexus)
            {
                player.SendError("Can not use /quake in nexus");
                return;
            }

            if (player.Parent is GuildHall)
            {
                player.SendError("Can not use /quake in guild hall");
                return;
            }

            var worldDesc = Resources.Worlds.Values
                .Where(x => !x.IsTemplate)
                .FirstOrDefault(x => x.Name.Equals(worldName, StringComparison.InvariantCultureIgnoreCase));

            if (worldDesc == null)
            {
                player.SendError("Invalid world");
                return;
            }

            World world;
            if (worldDesc.Persist)
                world = Manager.GetWorld(worldDesc.Id, player.Client);
            else
                world = Manager.AddWorld(worldDesc);

            player.Parent.QuakeToWorld(world);
        }

        [Command("Bring", true)]
        public static void Bring(Player player, string name)
        {
            var other = Manager.GetPlayer(name.ToLower());
            if (other is null)
            {
                player.SendError("Player not found");
                return;
            }

            int plrCount = 0;
            foreach (var e in player.Parent.Players.Values)
            {
                plrCount++;
            }

            if (player.Parent is Vault)
            {
                player.SendError("You can't summon players to your own vault!");
                return;
            }

            other.Client.Send(GameServer.Reconnect(player.Parent.Id));
        }

        [Command("Visit", true)]
        public static void Visit(Player player, string name)
        {
            var other = Manager.GetPlayer(name.ToLower());
            if (other is null)
            {
                player.SendError("Player not found");
                return;
            }
            int plrCount = 0;
            foreach (var e in player.Parent.Players.Values)
            {
                plrCount++;
            }

            player.Client.Send(GameServer.Reconnect(other.Parent.Id));
        }

        [Command("Spawn", true)]
        public static void Spawn(Player player, int count, string entity)
        {
            var objDesc = GetObjectDesc(entity);
            if (objDesc == null)
            {
                player.SendError($"Entity <{entity}> not found in Game Data");
                return;
            }

            if (objDesc.Player || objDesc.Static)
            {
                player.SendError("Can't spawn this entity");
                return;
            }

            if (count <= 0)
            {
                player.SendError($"Really? {count} {player.Name}? I'll get right on that...");
                return;
            }

            player.SendInfo($"Spawning <{count}> <{objDesc.DisplayId}> in 2 seconds");
            var plrCount = 0;
            var notification = GameServer.Notification(player.Id,
                $"Spawning <{count}> <{objDesc.DisplayId}> in 2 seconds", 0xFF0000);
            foreach (var en in player.Parent.PlayerChunks.HitTest(player.Position, 15))
            {
                if (en is Player plr)
                    plr.Client.Send(notification);
            }

            foreach (var e in player.Parent.Players.Values)
            {
                plrCount++;
            }

            var pos = player.Position;
            Manager.AddTimedAction(2000, () =>
            {
                for (var i = 0; i < count; i++)
                {
                    var entity = Entity.Resolve(objDesc.Type);
                    player.Parent?.AddEntity(entity, pos);
                }
            });
        }

        [Command("Killall", true)]
        public static void KillAll(Player player, string name = "")
        {
            var count = 0;
            foreach (var entity in player.Parent.Entities.Values.ToArray())
            {
                if (entity is Enemy enemy && (string.IsNullOrWhiteSpace(name) ||
                                              entity.Desc.Id.Contains(name,
                                                  StringComparison.InvariantCultureIgnoreCase)))
                {
                    enemy.Death(player);
                    count++;
                }
            }

            player.SendInfo($"Killed {count} entities");
        }

        [Command("CloseRealm", true)]
        public static void CloseRealm(Player player)
        {
            if (!(player.Parent is Realm realm))
            {
                player.SendError("Must be in a realm to close it");
                return;
            }

            realm.Close();
        }
    }
}
