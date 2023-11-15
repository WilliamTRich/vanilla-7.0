using System;
using System.Collections.Generic;
using System.IO;
using RotMG.Common;
using RotMG.Networking;
using System.Linq;
using System.Text.RegularExpressions;
using RotMG.Game.SetPieces;
using RotMG.Game.Worlds;
using RotMG.Utils;
using RotMG.Game.Logic.Commands;
using SimpleLog;

namespace RotMG.Game.Entities
{
    public partial class Player
    {
        private const int ChatCooldownMS = 200;

        public int LastChatTime;

        private readonly string[] _unrankedCommands =
        {
            "commands", "g", "guild", "tell", "allyshots", "allydamage", "effects", "sounds", "vault", "realm",
            "notifications", "online", "who", "server", "pos", "loc", "where", "find", "fame", "famestats", "stats",
            "trade", "currentsong", "song", "glands"
        };

        private readonly string[] _rankedCommands =
        {
            "announce", "announcement", "legendary", "roll", "disconnect", "dcAll", "dc", "songs", "changesong",
            "terminate", "stop", "gimme", "give", "gift", "closerealm", "rank", "create", "spawn", "killall",
            "setpiece", "max", "tq", "god", "eff", "effect", "ban", "unban", "mute", "unmute"
        };

        public void SendInfo(string text) => Client.Send(GameServer.Text("", 0, -1, 0, "", text));
        public void SendError(string text) => Client.Send(GameServer.Text("*Error*", 0, -1, 0, "", text));
        public void SendHelp(string text) => Client.Send(GameServer.Text("*Help*", 0, -1, 0, "", text));
        public void SendClientText(string text) => Client.Send(GameServer.Text("*Client*", 0, -1, 0, "", text));

        private bool PassFilter(string text)
        {
            return true;
        }
        
        public void Chat(string text)
        {
            if (text.Length <= 0 || text.Length > 128)
            {
#if DEBUG
                SLog.Error( "Text too short or too long");
#endif
                Client.Disconnect();
                return;
            }

            var validText = Regex.Replace(text, @"[^a-zA-Z0-9`!@#$%^&* ()_+|\-=\\{}\[\]:"";'<>?,./]", "");
            if (validText.Length <= 0)
            {
                SendError("Invalid text.");
                return;
            }

            if (LastChatTime + ChatCooldownMS > Manager.TotalTimeUnsynced)
            {
                SendError("Message sent too soon after previous one.");
                return;
            }

            //Not implemented
            /*if (!PassFilter(validText))
            {
                Client.Disconnect();
                return;
            }*/

            LastChatTime = Manager.TotalTimeUnsynced;

            if (validText[0] == '/')
            {
                CommandManager.Execute(this, validText);
                return;
            }

            if (Client.Account.Muted)
            {
                SendError("You are muted");
                return;
            }

            Parent.ChatReceived(this, validText); //Player Text Chat Transitions for things like Thessal

            var name = Client.Account.Ranked ? "@" + Name : Name;
            var packet = GameServer.Text(name, Id, NumStars, 5, "", validText);

            foreach (var player in Parent.Players.Values)
                if (!player.Client.Account.IgnoredIds.Contains(AccountId))
                    player.Client.Send(packet);
        }
    }
}
