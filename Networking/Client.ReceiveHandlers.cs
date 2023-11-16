using RotMG.Common;
using RotMG.Game;
using RotMG.Game.Entities;
using RotMG.Utils;
using SimpleLog;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using static RotMG.Networking.GameServer;

namespace RotMG.Networking;

public sealed partial class Client
{
    private void ProcessHello(string buildVer, int gameId, string email, string pwd, short charId, bool createChar,
        ushort charType, ushort skinType)
    {
        if (State != ProtocolState.Handshaked) {
            SLog.Info("Hello received but state {0} is not handshaked", State);
            return;
        }

        var acc = Database.Verify(email, pwd, IP);
        if (acc == null)
        {
            Send(Failure(0, "Invalid account."));
            Manager.AddTimedAction(1000, Disconnect);
            return;
        }

        if (acc.Banned)
        {
            Send(Failure(0, "Banned."));
            Manager.AddTimedAction(1000, Disconnect);
            return;
        }

        if (!acc.Ranked && Settings.AdminOnly)
        {
            Send(Failure(0, "Admin Only."));
            Manager.AddTimedAction(1000, Disconnect);
            return;
        }

        //if (!acc.Ranked && gameId == Manager.EditorId)
        //{
        //    client.Send(Failure(0, "Not ranked."));
        //    Manager.AddTimedAction(1000, client.Disconnect);
        //}

        Manager.GetClient(acc.Id)?.Disconnect();

        if (Database.IsAccountInUse(acc))
        {
            Send(Failure(0, "Account in use!"));
            Manager.AddTimedAction(1000, Disconnect);
            return;
        }

        Account = acc;
        Account.Connected = true;
        Account.LastSeen = Database.UnixTime();
        Account.Save();
        TargetWorldId = gameId;

        Manager.AccountIdToClientId[Account.Id] = Id;
        var world = Manager.GetWorld(gameId, this);


#if DEBUG
        //if (client.TargetWorldId == Manager.EditorId)
        //{
        //    SLog.Debug( "Loading editor world");
        //    var map = new JSMap(Encoding.UTF8.GetString(mapJson));
        //    world = new World(map, Resources.Worlds["Dreamland"]);
        //    client.TargetWorldId = Manager.AddWorld(world);
        //}
#endif

        if (world == null)
        {
            Send(Failure(0, "Invalid world!"));
            Manager.AddTimedAction(1000, Disconnect);
            return;
        }

        var seed = (uint)MathUtils.NextInt(1, int.MaxValue - 1);
        Random = new wRandom(seed);

        SendMapInfo(world.Width, world.Height, world.Name, world.DisplayName, 
            seed, 0, world.Background, world.AllowTeleport, world.ShowDisplays, 
            0, 0, 0, 0, //Lights
            (long)Manager.TickWatch.Elapsed.TotalMicroseconds);

        State = ProtocolState.Awaiting; //Allow the processing of Load/Create.


        if (createChar) {
            Create(charType, skinType);
            return;
        }
        
        Load(charId);
    }
    private void Create(ushort classType, ushort skinType)
    {
        SLog.Debug("Creating new character {0}, {1}", classType, skinType);
        var character = Database.CreateCharacter(Account, classType, skinType);
        if (character == null) {
            Send(Failure(0, "Failed to create character."));
            Disconnect();
            return;
        }
        var targetWorld = TargetWorldId;
        //If this is their first character
        if (Account.NextCharId == 1 && !Account.VisitedTutorial) {
            targetWorld = Manager.TutorialId;
        }

        SLog.Debug($"Connecting player to {targetWorld} | {TargetWorldId} | {Account.NextCharId} {Account.VisitedTutorial}");

        var world = Manager.GetWorld(targetWorld, this);
        Character = character;
        Player = new Player(this);
        State = ProtocolState.Connected;
        Send(CreateSuccess(world.AddEntity(Player, world.GetSpawnRegion().ToVector2()), Character.Id));
    }

    private void Load(int charId)
    {
        SLog.Debug("Loading character {0}", charId);
        var character = Database.LoadCharacter(Account, charId);
        if (character == null || character.Dead) {
            Send(Failure(0, "Failed to load character."));
            Disconnect();
            return;
        }

        var world = Manager.GetWorld(TargetWorldId, this);
        Character = character;
        Player = new Player(this);
        State = ProtocolState.Connected;
        Send(CreateSuccess(world.AddEntity(Player, world.GetSpawnRegion().ToVector2()), Character.Id));
    }
}
