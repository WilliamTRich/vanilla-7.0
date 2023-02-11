using RotMG.Game.Entities;
using RotMG.Game.Worlds;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RotMG.Common;

namespace RotMG.Game.Logic.Commands
{
    public static class UnrankedCommands
    {
        [Command("Commands")]
        public static void Commands(Player player)
        {
            var sb = new StringBuilder("Available commands: ");

            List<string> commands = CommandManager.Commands
                .Where(x => !x.Value.Item3.RequiresPerms && x.Value.Item3.ListCommand)
                .Select(x => x.Key)
                .ToList();

            if(player.Client.Account.Ranked)
            {
                commands.AddRange(CommandManager.Commands
                .Where(x => x.Value.Item3.RequiresPerms && x.Value.Item3.ListCommand)
                .Select(x => x.Key)
                .ToArray()
                );
            }

            for (var i = 0; i < commands.Count; i++)
            {
                if (i != 0) sb.Append(", ");
                sb.Append(commands[i]);
            }

            player.SendInfo(sb.ToString());
        }

        [Command("Trade")]
        public static void Trade(Player player, string name = "")
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                if (player.PotentialPartner == null)
                {
                    player.SendError("No pending trades");
                    return;
                }

                player.TradeRequest(player.PotentialPartner.Name);
                return;
            }

            var partner = Manager.GetPlayer(name.ToLower());
            if (partner == null)
            {
                player.SendError($"Player {name} not found");
                return;
            }

            player.TradeRequest(partner.Name);
        }

        [Command("Guild", aliases: "g")]
        public static void Guild(Player player, string text)
        {
            if (text.Length == 0)
                return;


            if (player.Client.Account.GuildName == null)
            {
                player.SendError("Not in a guild");
                return;
            }

            if (player.Client.Account.Muted)
            {
                player.SendError("You are muted");
                return;
            }

            foreach (var client in Manager.Clients.Values)
            {
                //if receiever Parent id is the same as sender parent id then send player id to show speech bubble or -1 to not
                var guild = GameServer.Text(player.Name, client.Player.Parent.Id == player.Parent.Id ? player.Id : -1, player.NumStars, 5, "*Guild*", text);
                if (client.Player != null && client.Account.GuildName != null && client.Account.GuildName == player.Client.Account.GuildName)
                    client.Send(guild);

            }
        }
        [Command("Tell", aliases: "t")]
        public static void Tell(Player player, string name, string text)
        {
            if (text.Length == 0 || string.IsNullOrEmpty(name) || string.Equals(name, player.Client.Account.Name,
                StringComparison.CurrentCultureIgnoreCase))
                return;

            if (player.Client.Account.Muted)
            {
                player.SendError("You are muted");
                return;
            }

            var recipient = Manager.GetPlayer(name.ToLower());
            if (recipient == null)//no finding hidden admins
            {
                player.SendError("Player not online");
                return;
            }
            //same as /g
            var tell = GameServer.Text(player.Name, recipient.Parent.Id == player.Parent.Id ? player.Id : -1, player.NumStars, 5, recipient.Name, text);
            if (recipient.Client.Account.IgnoredIds.Contains(player.AccountId))
            {
                player.Client.Send(tell);
                return;
            }
            else
            {
                player.Client.Send(tell);
                recipient.Client.Send(tell);
            }
        }

        [Command("Allyshots")]
        public static void Shots(Player player)
        {
            player.Client.Account.AllyShots = !player.Client.Account.AllyShots;
            player.SendInfo($"Ally shots set to {player.Client.Account.AllyShots}");
        }

        [Command("Allydamage")]
        public static void Damage(Player player)
        {
            player.Client.Account.AllyDamage = !player.Client.Account.AllyDamage;
            player.SendInfo($"Ally Damage set to {player.Client.Account.AllyDamage}");
        }

        [Command("Effects")]
        public static void Effects(Player player)
        {
            player.Client.Account.Effects = !player.Client.Account.Effects;
            player.SendInfo($"Ally Effects set to {player.Client.Account.Effects}");
        }

        [Command("Sounds")]
        public static void Sounds(Player player)
        {
            player.Client.Account.Sounds = !player.Client.Account.Sounds;
            player.SendInfo($"Ally sounds set to {player.Client.Account.Sounds}");
        }

        [Command("Notifications")]
        public static void Notifications(Player player)
        {
            player.Client.Account.Notifications = !player.Client.Account.Notifications;
            player.SendInfo($"Ally Notifications set to {player.Client.Account.Notifications}");
        }

        [Command("Who")]
        public static void Who(Player player)
        {
            var players = player.Parent.Players.Values.ToArray();

            player.SendInfo($"Players in current area ({players.Length}): " +
                            $"{string.Join(", ", players.Select(p => p.Name))}");
        }
        [Command("Online")]
        public static void Online(Player player)
        {
            var players = Manager.Clients.Values
                .Where(x => x.Player != null)
                .ToArray();

            player.SendInfo($"Players online ({players.Length}): " +
                            $"{string.Join(", ", players.Select(k => k.Player.Name))}");
        }

        [Command("Pos", false, true, "loc", "server")]
        public static void Pos(Player player)
        {
            player.SendInfo(player.ToString());
        }
        [Command("Where", aliases: "find")]
        public static void Where(Player player, string name)
        {
            Player findTarget = null;
            try
            {
                findTarget = Manager.GetPlayer(name.ToLower());
            }
            catch (Exception e)
            {
#if DEBUG
                Program.Print(PrintType.Warn, "[Command] Where Exception: " + e.Message);
#endif
            }
            if (findTarget == null)
            { player.SendError($"Couldn't find {name}"); return; }
            if (findTarget != null) { player.SendError($"Couldn't find {name}"); return; }
            else { player.SendInfo(findTarget.ToString()); return; }
        }

        [Command("Vault")]
        public static void Vault(Player player)
        {
            player.Client.Send(GameServer.Reconnect(Manager.VaultId));
        }

        [Command("Realm")]
        public static void Realm(Player player)
        {
            var realmIds = new List<int>(Manager.Realms.Keys);
            Realm realm = null;
            while (realmIds.Count > 0 && realm == null)
            {
                var id = realmIds[MathUtils.Next(realmIds.Count)];
                realm = Manager.GetWorld(id, player.Client) as Realm;
                if (realm is null || realm.Closed)
                {
                    realm = null;
                    realmIds.Remove(id);
                }
            }

            if (realm == null)
            {
                player.SendInfo("No available realms");
                return;
            }

            player.Client.Send(GameServer.Reconnect(realm.Id));
        }

        [Command("deathfame", false, true, "famestats", "stats")]
        public static void DeathFame(Player player)
        {
            player.SaveToCharacter();
            var fameStats = Common.Database.CalculateStats(player.Client.Account, player.Client.Character);
            player.SendInfo($"Active: {player.FameStats.MinutesActive} minutes");
            player.SendInfo($"Shots: {player.FameStats.Shots}");
            player.SendInfo(
                $"Accuracy: {(int)((float)player.FameStats.ShotsThatDamage / player.FameStats.Shots * 100f)}% ({player.FameStats.ShotsThatDamage}/{player.FameStats.Shots})");
            player.SendInfo($"Abilities Used: {player.FameStats.AbilitiesUsed}");
            player.SendInfo($"Tiles Seen: {player.FameStats.TilesUncovered}");
            player.SendInfo(
                $"Monster Kills: {player.FameStats.MonsterKills} ({player.FameStats.MonsterAssists} Assists, {(int)((float)player.FameStats.MonsterKills / (player.FameStats.MonsterKills + player.FameStats.MonsterAssists) * 100f)}% Final Blows)");
            player.SendInfo(
                $"God Kills: {player.FameStats.GodKills} ({(int)((float)player.FameStats.GodKills / player.FameStats.MonsterKills * 100f)}%) ({player.FameStats.GodKills}/{player.FameStats.MonsterKills})");
            player.SendInfo(
                $"Oryx Kills: {player.FameStats.OryxKills} ({(int)((float)player.FameStats.OryxKills / player.FameStats.MonsterKills * 100f)}%) ({player.FameStats.OryxKills}/{player.FameStats.MonsterKills})");
            player.SendInfo(
                $"Cube Kills: {player.FameStats.CubeKills} ({(int)((float)player.FameStats.CubeKills / player.FameStats.MonsterKills * 100f)}%) ({player.FameStats.CubeKills}/{player.FameStats.MonsterKills})");
            player.SendInfo($"Damage Taken: {player.FameStats.DamageTaken}");
            player.SendInfo($"Damage Dealt: {player.FameStats.DamageDealt}");
            player.SendInfo($"Teleports: {player.FameStats.Teleports}");
            player.SendInfo($"Potions Drank: {player.FameStats.PotionsDrank}");
            player.SendInfo($"Quests Completed: {player.FameStats.QuestsCompleted}");
            player.SendInfo($"Pirate Caves Completed: {player.FameStats.PirateCavesCompleted}");
            player.SendInfo($"Spider Dens Completed: {player.FameStats.SpiderDensCompleted}");
            player.SendInfo($"Snake Pits Completed: {player.FameStats.SnakePitsCompleted}");
            player.SendInfo($"Sprite Worlds Completed: {player.FameStats.SpriteWorldsCompleted}");
            player.SendInfo($"Undead Lairs Completed: {player.FameStats.UndeadLairsCompleted}");
            player.SendInfo($"Abyss Of Demons Completed: {player.FameStats.AbyssOfDemonsCompleted}");
            player.SendInfo($"Tombs Completed: {player.FameStats.TombsCompleted}");
            player.SendInfo($"Escapes: {player.FameStats.Escapes}");
            player.SendInfo($"Near Death Escapes: {player.FameStats.NearDeathEscapes}");
            player.SendInfo($"Party Member Level Ups: {player.FameStats.LevelUpAssists}");
            foreach (var bonus in fameStats.Bonuses)
                player.SendHelp($"{bonus.Name}: +{bonus.Fame}");
            player.SendInfo($"Base Fame: {fameStats.BaseFame}");
            player.SendInfo($"Total Fame: {fameStats.TotalFame}");
        }
        [Command("LeftToMax")]
        public static void LeftToMax(Player player)
        {
            var desc = ((PlayerDesc)player.Desc);
            player.SendInfo($"Life: {desc.Stats[0].MaxValue - player.Stats[0]}");
            player.SendInfo($"Mana: {desc.Stats[1].MaxValue - player.Stats[1]}");
            

            player.SendInfo($"Attack: {desc.Stats[2].MaxValue - player.Stats[2]}");
            player.SendInfo($"Defense: {desc.Stats[3].MaxValue - player.Stats[3]}");
            player.SendInfo($"Speed: {desc.Stats[4].MaxValue - player.Stats[4]}");
            player.SendInfo($"Dexterity: {desc.Stats[5].MaxValue - player.Stats[5]}");
            player.SendInfo($"Vitality: {desc.Stats[6].MaxValue - player.Stats[6]}");
            player.SendInfo($"Wisdom: {desc.Stats[7].MaxValue - player.Stats[7]}");
        }
        [Command("Glands", false, true, "gl", "gland")]
        public static void Glands(Player player)
        {
            if (!(player.Parent is Realm))
            {
                player.SendError("Must be in realm");
                return;
            }
            player.ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 3000);
            player.ApplyConditionEffect(ConditionEffectIndex.Stunned, 1500);
            player.Teleport(player.ClientTime, new Vector2(1250.5f, 1000.5f));
        }
        [Command("tp", aliases: "teleport")]
        public static void Teleport(Player player, string name)
        {

            if (!player.Parent.AllowTeleport)
            {
                player.SendError("This world doesn't allow teleporting");
                return;
            }
            if (!Common.Database.AccountExists(name, out var otherAccount))
            {
                player.SendError("Player not found");
                return;
            }

            if(Manager.TotalTime < player.NextTeleportTime)
            {
                player.SendError("Can't teleport this soon");
                return;
            }

            var otherClientId = Manager.AccountIdToClientId[otherAccount.Id];

            if (Manager.Clients.TryGetValue(otherClientId, out var otherClient))
            {
                if ((otherClient.Player.Parent.Id == player.Parent.Id))
                {

                    player.Teleport(player.ClientTime, (otherClient.Player.Position));
                    return;

                }
                player.SendError("Player not found");
            }
        }
        [Command("gkick")]
        public static void GuildKick(Player player, string name)
        {
            if (player.Client.Account.Name == name)
            {
                if (!Common.Database.RemoveFromGuild(player.Client.Account))
                {
                    player.Client.Send(GameServer.GuildResult(false, "Guild not found"));
                    return;
                }

                player.Client.Player.GuildName = "";
                player.Client.Player.GuildRank = 0;
                return;
            }

            if (!Common.Database.AccountExists(name, out var otherAccount))
            {
                player.Client.Send(GameServer.GuildResult(false, "Player not found"));
                return;
            }

            if (player.Client.Account.GuildRank >= 20 &&
                player.Client.Account.GuildName == otherAccount.GuildName &&
                player.Client.Account.GuildRank > otherAccount.GuildRank)
            {
                if (!Common.Database.RemoveFromGuild(otherAccount))
                {
                    player.Client.Send(GameServer.GuildResult(false, "Guild not found"));
                    return;
                }

                var otherClientId = Manager.AccountIdToClientId[otherAccount.Id];
                if (Manager.Clients.TryGetValue(otherClientId, out var otherClient))
                {
                    otherClient.Player.GuildRank = 0;
                    otherClient.Player.GuildName = "";
                }

                player.Client.Send(GameServer.GuildResult(true, "Success!"));
                return;
            }

            player.Client.Send(GameServer.GuildResult(false, "Insufficient privileges"));
        }

        [Command("ginvite", aliases: "invite")]
        public static void GuildInvite(Player player, string name)
        {
            if (player.Client.Account.GuildRank < 20)
            {
                player.SendError("Insufficient privileges");
                return;
            }

            var acc = new AccountModel(Common.Database.IdFromUsername(name));

            if (string.IsNullOrEmpty(acc.GuildName))
            {
                try
                {
                    var plr = Manager.GetPlayer(name.ToLower());
                    plr.GuildInvite = player.Client.Account.GuildName;
                    plr.Client.Send(GameServer.InvitedToGuild(player.Client.Account.Name, player.Client.Account.GuildName));
                }
                catch (Exception e)
                {
                    player.SendError("User not found:" + e);
                }

            }
            else
            {
                player.SendError($"{name} is already in a guild.");
            }

        }

        [Command("gwho")]
        public static void GuildWho(Player player)
        {
            if (player.GuildName == null && player.GuildName.Length > 0)
            {
                player.SendError("You are not in a guild");
                return;
            }
            player.SendInfo($"Guild members online ({Manager.Clients.Values.Count(k => k.Player != null && k.Player.GuildName != null && k.Player.GuildName == player.GuildName)}): " +
                            $"{string.Join(", ", Manager.Clients.Values.Where(k => k.Player != null && k.Player.GuildName != null && k.Player.GuildName == player.GuildName).Select(k => k.Player.Name))}");
        }

        [Command("ghall")]
        public static void GHall(Player player)
        {
            if (player.GuildName == null && player.GuildName.Length > 0)
            {
                player.SendError("You are not in a guild");
                return;
            }

            player.Client.Send(GameServer.Reconnect(Manager.GuildId));
        }

        [Command("uptime")]
        public static void UpTime(Player player)
        {
            var t = TimeSpan.FromMilliseconds(Manager.TotalTimeUnsynced);

            var answer = $"{t.Days:D2}d:{t.Hours:D2}h:{t.Minutes:D2}m:{t.Seconds:D2}s";

            player.SendInfo("The server has been up for " + answer + ".");
        }

        [Command("Lock")]
        public static void Lock(Player player, string name)
        {
            if (name.ToLower().Equals(player.Name.ToLower(), StringComparison.InvariantCultureIgnoreCase))
            {
                player.SendError("Can't lock yourself");
                return;
            }

            if (!Common.Database.AccountExists(name, out var otherAccount))
            {
                player.SendError("Player not found");
                return;
            }

            if (player.Client.Account.LockedIds.Contains(otherAccount.Id))
            {
                player.SendInfo("Player already locked");
                return;
            }

            player.Client.Account.LockedIds.Add(otherAccount.Id);
            player.Client.Account.Save();
            player.Client.Send(GameServer.AccountList(0, player.Client.Account.LockedIds));
        }

        [Command("Unlock")]
        public static void UnLock(Player player, string name)
        {
            if (name.Equals(player.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                player.SendError("You are no longer locking yourself. Nice!");
                return;
            }

            if (!Common.Database.AccountExists(name, out var otherAccount))
            {
                player.SendError("Player not found");
                return;
            }

            if (!player.Client.Account.LockedIds.Contains(otherAccount.Id))
            {
                player.SendInfo("Player already unlocked");
                return;
            }

            player.Client.Account.LockedIds.Remove(otherAccount.Id);
            player.Client.Send(GameServer.AccountList(0, player.Client.Account.LockedIds));
        }

        [Command("Ignore")]
        public static void Ignore(Player player, string name)
        {
            if (name.Equals(player.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                player.SendError("Can't ignore yourself");
                return;
            }

            if (!Common.Database.AccountExists(name, out var otherAccount))
            {
                player.SendError("Player not found");
                return;
            }

            if (player.Client.Account.IgnoredIds.Contains(otherAccount.Id))
            {
                player.SendInfo("Player already ignored");
                return;
            }

            player.Client.Account.IgnoredIds.Add(otherAccount.Id);
            player.Client.Send(GameServer.AccountList(1, player.Client.Account.IgnoredIds));
        }

        [Command("unignore")]
        public static void UnIgnore(Player player, string name)
        {
            if (name.Equals(player.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                player.SendError("You are no longer ignoring yourself. Good job");
                return;
            }

            if (!Common.Database.AccountExists(name, out var otherAccount))
            {
                player.SendError("Player not found");
                return;
            }

            if (!player.Client.Account.IgnoredIds.Contains(otherAccount.Id))
            {
                player.SendInfo("Player already unignored");
                return;
            }

            player.Client.Account.IgnoredIds.Remove(otherAccount.Id);
            player.Client.Send(GameServer.AccountList(1, player.Client.Account.IgnoredIds));
        }
    }
}
