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
using System.Threading.Tasks;
using static RotMG.Networking.GameServer;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RotMG.Networking;

public sealed partial class Client
{
    public enum C2SPacketId : byte //Client 2 Server 
    {
        Unknown = 0,
        AcceptTrade = 1,
        AoeAck = 2,
        Buy = 3,
        CancelTrade = 4,
        ChangeGuildRank = 5,
        ChangeTrade = 6,
        ChooseName = 8,
        CreateGuild = 10,
        EditAccountList = 11,
        EnemyHit = 12,
        Escape = 13,
        GroundDamage = 15,
        GuildInvite = 16,
        GuildRemove = 17,
        Hello = 18,
        InvDrop = 19,
        InvSwap = 20,
        JoinGuild = 21,
        Move = 23,
        OtherHit = 24,
        PlayerHit = 25,
        PlayerShoot = 26,
        PlayerText = 27,
        Pong = 28,
        RequestTrade = 29,
        Reskin = 30,
        ShootAck = 32,
        SquareHit = 33,
        Teleport = 34,
        UpdateAck = 35,
        UseItem = 36,
        UsePortal = 37
    }
    private void ProcessHello(string buildVer, int gameId, string email, string pwd, short charId, bool createChar,
        ushort charType, ushort skinType)
    {
        if (State != ProtocolState.Handshaked) {
            SLog.Info("Hello received but state {0} is not handshaked", State);
            return;
        }

        var acc = Database.Verify(email, pwd, IP);
        if (acc == null) {
            SLog.Info("Invalid account, disconnecting {0}", IP);
            SendFailure(0, "Invalid account.");
            Manager.AddTimedAction(1000, () => { Disconnect("Invalid account."); });
            return;
        }

        if (acc.Banned) {
            SLog.Info("Banned, disconnecting {0}", IP);
            SendFailure(0, "Banned.");
            Manager.AddTimedAction(1000, () => { Disconnect("Banned."); });
            return;
        }

        if (!acc.Ranked && Settings.AdminOnly) {
            SLog.Info("Admin Only, disconnecting {0}", IP);
            SendFailure(0, "Admin Only.");
            Manager.AddTimedAction(1000, () => { Disconnect("Admin only."); });
            return;
        }

        //if (!acc.Ranked && gameId == Manager.EditorId)
        //{
        //    client.Send(Failure(0, "Not ranked."));
        //    Manager.AddTimedAction(1000, client.Disconnect);
        //}

        Manager.GetClient(acc.Id)?.Disconnect();

        if (Database.IsAccountInUse(acc)) {
            SLog.Info("Account in use! disconnecting {0}", IP);
            SendFailure(0, "Account in use!");
            Manager.AddTimedAction(1000, () => { Disconnect("Account in use."); });
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

        if (world == null) {
            SLog.Info("Invalid world! disconnecting {0}", IP);
            SendFailure(0, "Invalid world!");
            Manager.AddTimedAction(1000, () => { Disconnect("Invalid world."); });
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
            SendFailure(0, "Failed to create character.");
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
        SendCreateSuccess(world.AddEntity(Player, world.GetSpawnRegion().ToVector2()), Character.Id);
    }

    private void Load(int charId)
    {
        SLog.Debug("Loading character {0}", charId);
        var character = Database.LoadCharacter(Account, charId);
        if (character == null || character.Dead) {
            SendFailure(0, "Failed to load character.");
            Disconnect();
            return;
        }

        var world = Manager.GetWorld(TargetWorldId, this);
        Character = character;
        Player = new Player(this);
        State = ProtocolState.Connected;
        SendCreateSuccess(world.AddEntity(Player, world.GetSpawnRegion().ToVector2()), Character.Id);
    }

    private void ProcessMove(int tickId, long time, float x, float y, MoveRecord[] records)
    {
        if (Player == null)
            return;//seperated logic so i can breakpoint
        if (Player.Parent == null)
            return;

        if (x == -1 && y == -1) // will cause bounds error might look into removing this type of interaction
            return;

        if (!Player.Parent.InBounds((int)x,(int)y))
        {
            SLog.Warn("Player went out of map bounds | x:{0} y:{1}", x, y);
            //Disconnect($"Player went out of map bounds: {x}, {y}");
            return;
        }

        Player.MoveReceived(tickId, time);
        Player.Parent.MoveEntity(Player, new Vector2(x, y));

        //TODO Add Tile visibility
        //if ((int)x != (int)Player.PreviousPosition.X || (int)y != (int)Player.PreviousPosition.Y)
        //    Player.UpdateVisibility();
        
        if (Player.IsNoClipping() /* || !Player.VisibleTiles.Contains(new IntPoint((int)x, (int)y)) */) // replaces Player.Tiles check
            Disconnect("Invalid position");
    }

    private void ProcessPong(int serial, long pongTime) {
        Player?.Pong(serial, pongTime);
    }
    private void ProcessUpdateAck() {
        Player?.UpdateAckReceived();
    }
    private void ProcessPlayerText(string text) {
        Player?.Chat(text);
    }

    private void ProcessUsePortal(int objId) {
        if (Player?.Parent == null)
            return;

        Player?.UsePortal(objId);
    }

    private void ProcessAoeAck(long time, float x, float y) {
        Player.TryAckAoe((int)time, new Vector2(x, y));
    }

    private void ProcessEnemyHit(long time, byte bulletId, int targetId, bool kill) {
        Player.TryHitEnemy((int)time, bulletId, targetId, kill);
    }

    private void ProcessEscape() {
        if (Player == null || Player.Parent == null)
            return;

        PrepareReconnect("Nexus", -1);
    }

    public void PrepareReconnect(string name, int id) {
        if (Account == null) {
            Disconnect("Tried to reconnect an client with a null account...");
            return;
        }

        Reconnecting = true;
        SLog.Trace("Reconnecting client ({0}) @ {1} to {2}...", Account.Name, IP, name);
        Reconnect(id);
        Reconnecting = false;
    }

    private void ProcessInvDrop(int objId, byte slotId, int objType) {
        Player.DropItem(objId, slotId);
        
        //if (Player?.Owner == null || Player.tradeTarget != null)
        //    return;
        //
        //const ushort normBag = 0x0500;
        //const ushort soulBag = 0x0503;
        //
        //IContainer con;
        //
        //// container isn't always the player's inventory, it's given by the SlotObject's ObjectId
        //if (objId != Player.Id) {
        //    if (Player.Parent.GetEntity(objId) is Player) {
        //        Player.Client.SendInventoryResult(1);
        //        return;
        //    }
        //
        //    con = Player.Owner.GetEntity(objId) as IContainer;
        //}
        //else
        //{
        //    con = Player;
        //}
        //
        //
        //if (objId == Player.Id && Player.Stacks.Any(stack => stack.Slot == slotId)) {
        //    Player.Client.SendInventoryResult(1);
        //    return; // don't allow dropping of stacked items
        //}
        //
        //if (con?.Inventory[slotId] == null) {
        //    //give proper error
        //    Player.Client.SendInventoryResult(1);
        //    return;
        //}
        //
        //var item = con.Inventory[slotId];
        //con.Inventory[slotId] = null;
        //
        //// create new container for item to be placed in
        //Container container;
        //if (item.Soulbound || Player.Client.Account.Admin) {
        //    container = new Container(Player.Manager, soulBag, 1000 * 60, true);
        //    container.BagOwners = new[] { Player.AccountId };
        //}
        //else
        //{
        //    container = new Container(Player.Manager, normBag, 1000 * 60, true);
        //}
        //
        //// init container
        //container.Inventory[0] = item;
        //container.Move(Player.X + (float)((InvRand.NextDouble() * 2 - 1) * 0.5), Player.Y + (float)((InvRand.NextDouble() * 2 - 1) * 0.5));
        //
        //container.SetDefaultSize(75);
        //Player.Owner.EnterWorld(container);
        //
        //// send success
        //Player.Client.SendInventoryResult(0);
    }

    private void ProcessInvSwap(long time, float x, float y, int objId1, byte slotId1, int itemType1, int objId2,
    byte slotId2, int itemType2)
    {
        Player.SwapItem(
            new SlotData() { ObjectId = objId1, SlotId = slotId1 },
            new SlotData() { ObjectId = objId2, SlotId = slotId2 }
            );
        //var a = Player.Owner.GetEntity(objId1);
        //var b = Player.Owner.GetEntity(objId2);
        //
        //if (Player?.Owner == null)
        //    return;
        //
        //if (!ValidateEntities(Player, a, b) || Player.tradeTarget != null)
        //{
        //    a.ForceUpdate(slotId1);
        //    b.ForceUpdate(slotId2);
        //    SendInventoryResult(1);
        //    return;
        //}
        //
        //var conA = (IContainer)a;
        //var conB = (IContainer)b;
        //
        //// check if stacking operation
        //if (b == Player)
        //    foreach (var stack in Player.Stacks)
        //        if (stack.Slot == slotId2)
        //        {
        //            var stackTrans = conA.Inventory.CreateTransaction();
        //            var item = stack.Put(stackTrans[slotId1]);
        //            if (item == null) // success
        //            {
        //                stackTrans[slotId1] = null;
        //                Inventory.Execute(stackTrans);
        //                SendInventoryResult(1);
        //                return;
        //            }
        //        }
        //
        //// not stacking operation, continue on with normal swap
        //
        //// validate slot types
        //if (!ValidateSlotSwap(Player, conA, conB, slotId1, slotId2))
        //{
        //    a.ForceUpdate(slotId1);
        //    b.ForceUpdate(slotId2);
        //    SendInventoryResult(1);
        //    return;
        //}
        //
        //// setup swap
        //var queue = new Queue<Action>();
        //var conATrans = conA.Inventory.CreateTransaction();
        //var conBTrans = conB.Inventory.CreateTransaction();
        //var itemA = conATrans[slotId1];
        //var itemB = conBTrans[slotId2];
        //conBTrans[slotId2] = itemA;
        //conATrans[slotId1] = itemB;
        //
        //// validate that soulbound items are not placed in public bags (includes any swaped item from admins)
        //if (!ValidateItemSwap(Player, a, itemB))
        //{
        //    queue.Enqueue(() => DropInSoulboundBag(Player, itemB));
        //    conATrans[slotId1] = null;
        //}
        //
        //if (!ValidateItemSwap(Player, b, itemA))
        //{
        //    queue.Enqueue(() => DropInSoulboundBag(Player, itemA));
        //    conBTrans[slotId2] = null;
        //}
        //
        //// swap items
        //if (Inventory.Execute(conATrans, conBTrans))
        //{
        //    while (queue.Count > 0)
        //        queue.Dequeue()();
        //
        //    SendInventoryResult(0);
        //    return;
        //}
        //
        //a.ForceUpdate(slotId1);
        //b.ForceUpdate(slotId2);
        //SendInventoryResult(1);
    }

    private void ProcessOtherHit(long time, byte bulletId, int ownerId, int targetId)
    {
    }

    private void ProcessPlayerHit(byte bulletId, int objId) {
        if (Player?.Parent == null)
            return;

        Player?.TryHit(bulletId, objId);
    }

    private void ProcessPlayerShoot(long time, byte bulletId, ushort objType, float x, float y, float angle)
    {
        if (Player.Inventory[1] == objType) {
            // we dont handle ability on player shoot
            Player.Client.Random.Drop(1); // this is needed as we doShoot client side which uses the random gen
            return;
        }

        if (!Resources.Type2Item.TryGetValue((ushort)Player.Inventory[0], out var item)) {
            Player.Client.Random.Drop(1);
            return;
        }

        var prjDesc = item.Projectile; //Assume only one

        // validate
        var result = Player.ValidatePlayerShoot(item, time);
        if (result != Player.PlayerShootStatus.OK) {
            Player.Client.Random.Drop(1);
            return;
        }

        // create projectile and show other players
        var prj = Player.PlayerShootProjectile(
            bulletId, prjDesc, item.Type,
            time, new Vector2 { X = x, Y = y }, angle);

        //Player.Parent.AddProjectile(prj);

        foreach (var en in Player.Parent.PlayerChunks.HitTest(Player.Position, Player.SightRadius))
            if (en is Player player && player.Client.Account.AllyShots && !player.Equals(Player))
                player.Client.SendAllyShoot(bulletId, Player.Id, objType, angle);

        //foreach (var otherPlayer in Player.Parent.Players.Values)
        //    if (otherPlayer.Id != Player.Id && otherPlayer.DistSqr(Player) < Player.RADIUS_SQR)
        //        otherPlayer.Client.SendAllyShoot(bulletId, Player.Id, objType, angle);

        Player.FameStats.Shots++;
    }

    private void ProcessShootAck(long time) {
        //Player.ShootAckReceived(time);
    }

    private void ProcessSquareHit(long time, byte bulletId, int objId) {
        Player.TryHitSquare((int)time, bulletId);
    }

    private void ProcessTeleport(int objId) {
        if (Player == null || Player.Parent == null)
            return;

        var ent = Player.Parent.GetEntity(objId);
        if (ent == null)
            return;

        Player.Teleport((int)Manager.TickWatch.ElapsedMilliseconds, ent.Position);
    }

    private void ProcessUseItem(long time, int objId, byte slotId, int objType, float x, float y, byte useType) {
        if (Player == null || Player.Parent == null)
            return;

        Player.TryUseItem((int)time, 
            new SlotData() { ObjectId = objId, SlotId = slotId }, 
            new Vector2 { X = x, Y = y });
    }
}
