using Common;
using RotMG.Networking;
using RotMG.Utils;
using System;
using System.Linq;

namespace RotMG.Game.Entities;

public partial class Player
{
    private const float ContainerMinimumDistance = 2f;

    private const int MaxSlotsWithoutBackpack = 12;
    private const int MaxSlotsWithBackpack = MaxSlotsWithoutBackpack + 8;

    private const byte HealthPotionSlotId = 254;
    private const byte MagicPotionSlotId = 255;

    public const int HealthPotionItemType = 2594;
    public const int MagicPotionItemType = 2595;

    private static byte[] InvalidInvSwap = GameServer.InvResult(1);
    private static byte[] ValidInvSwap = GameServer.InvResult(0);

    public int[] Inventory { get; set; }
    public int[] ItemDatas { get; set; }

    public void InitInventory(CharacterModel character)
    {
        Inventory = character.Inventory.ToArray();
        ItemDatas = character.ItemDatas.ToArray();
        UpdateInventory();
    }

    public void RecalculateEquipBonuses()
    {
        for (var i = 0; i < 8; i++)
            Boosts[i] = 0;

        for (var i = 0; i < 4; i++)
        {
            if (Inventory[i] == -1)
                continue;

            var item = Resources.Type2Item[(ushort)Inventory[i]];
            foreach (var s in item.StatBoosts)
                Boosts[s.Key] += s.Value;

            var data = ItemDatas[i];
            if (data == -1)
                continue;

            Boosts[0] += (int)ItemDesc.GetStat(data, ItemData.MaxHP, 5);
            Boosts[1] += (int)ItemDesc.GetStat(data, ItemData.MaxMP, 5);
            Boosts[2] += (int)ItemDesc.GetStat(data, ItemData.Attack, 1);
            Boosts[3] += (int)ItemDesc.GetStat(data, ItemData.Defense, 1);
            Boosts[4] += (int)ItemDesc.GetStat(data, ItemData.Speed, 1);
            Boosts[5] += (int)ItemDesc.GetStat(data, ItemData.Dexterity, 1);
            Boosts[6] += (int)ItemDesc.GetStat(data, ItemData.Vitality, 1);
            Boosts[7] += (int)ItemDesc.GetStat(data, ItemData.Wisdom, 1);
        }

        UpdateStats();
    }

    public ItemDesc GetItem(int index)
    {
#if DEBUG
        if (index < 0 || index > (HasBackpack ? MaxSlotsWithBackpack : MaxSlotsWithoutBackpack))
            throw new Exception("GetItem index out of bounds");
#endif
        if (Inventory[index] == -1)
            return null;
        return Resources.Type2Item[(ushort)Inventory[index]];
    }

    public bool GiveItem(ushort type)
    {
        var slot = GetFreeInventorySlot();
        if (slot == -1)
            return false;
        Inventory[slot] = type;
        UpdateInventorySlot(slot);
        return true;
    }

    public int GetFreeInventorySlot()
    {
        var maxSlots = HasBackpack ? MaxSlotsWithBackpack : MaxSlotsWithoutBackpack;
        for (var i = 4; i < maxSlots; i++)
            if (Inventory[i] == -1)
                return i;
        return -1;
    }

    public int GetTotalFreeInventorySlots()
    {
        var count = 0;
        for (var i = 4; i < MaxSlotsWithoutBackpack; i++)
            if (Inventory[i] == -1)
                count++;
        return count;
    }

    public void DropItem(int objId, byte slot)
    {
        UpdateInventorySlot(slot);

        if (!ValidSlot(slot))
        {
            Client.SendInventoryResult(1);
#if DEBUG
            SLog.Error("Invalid slot");
#endif
            return;
        }

        int item = Inventory[slot];
        int data = ItemDatas[slot];

        if (item == -1)
        {
            Client.SendInventoryResult(1);
#if DEBUG
            SLog.Error("Nothing to drop");
#endif
            return;
        }
        Inventory[slot] = -1;
        ItemDatas[slot] = -1;

        UpdateInventorySlot(slot);

        var container = new Container(Container.PurpleBag, AccountId, 120000);
        container.Inventory[0] = item;
        container.ItemDatas[0] = data;
        container.UpdateInventorySlot(0);

        RecalculateEquipBonuses();
        Parent.AddEntity(container, Position + MathUtils.Position(.2f, .2f));
        Client.SendInventoryResult(0);
    }

    public void SwapItem(SlotData slot1, SlotData slot2)
    {
        var en1 = Parent.GetEntity(slot1.ObjectId);
        var en2 = Parent.GetEntity(slot2.ObjectId);

        (en1 as IContainer)?.UpdateInventorySlot(slot1.SlotId);
        (en2 as IContainer)?.UpdateInventorySlot(slot2.SlotId);
        
        //Undefined entities
        if (en1 == null || en2 == null)
        {
#if DEBUG
            SLog.Error( "Undefined entities");
#endif
            Client.SendInventoryResult(1);
            return;
        }
        
        //Entities which are not containers???
        if (en1 is not IContainer || en2 is not IContainer)
        {
#if DEBUG
            SLog.Error( "Not containers");
#endif
            Client.SendInventoryResult(1);
            return;
        }

        if (en1 is Player && en2 is OneWayContainer)
        {
#if DEBUG
            SLog.Error( "Tried adding to one way container");
#endif
            Client.SendInventoryResult(1);
            return;
        }

        if (en1.Position.Distance(en2) > ContainerMinimumDistance)
        {
#if DEBUG
            SLog.Error( "Too far away from container");
#endif
            Client.SendInventoryResult(1);
            return;
        }

        if ((en1 as Player)?.TradePartner != null ||
            (en2 as Player)?.TradePartner != null)
        {
#if DEBUG
            SLog.Error( "Tried swapping items while trading");
#endif
            Client.SendInventoryResult(1);
            return;
        }

        //Player manipulation attempt
        if (en1 is Player && slot1.ObjectId != Id ||
            en2 is Player && slot2.ObjectId != Id)
        {
#if DEBUG
            SLog.Error( "Player manipulation attempt");
#endif
            Client.SendInventoryResult(1);
            return;
        }

        //Container manipulation attempt
        if (en1 is Container && 
            (en1 as Container).OwnerId != -1 && 
            AccountId != (en1 as Container).OwnerId ||
         en2 is Container && 
         (en2 as Container).OwnerId != -1 && 
         AccountId != (en2 as Container).OwnerId)
        {
#if DEBUG
            SLog.Error( "Container manipulation attempt");
#endif
            Client.SendInventoryResult(1);
            return;
        }

        var con1 = en1 as IContainer;
        var con2 = en2 as IContainer;

        //Invalid slots
        if (!con1.ValidSlot(slot1.SlotId) || !con2.ValidSlot(slot2.SlotId))
        {
#if DEBUG
            SLog.Error( "Invalid inv swap");
#endif
            Client.SendInventoryResult(1);
            return;
        }

        //Invalid slot types
        int item1 = con1.Inventory[slot1.SlotId];
        int data1 = con1.ItemDatas[slot1.SlotId];
        int item2 = con2.Inventory[slot2.SlotId];
        int data2 = con2.ItemDatas[slot2.SlotId];
        var d = Desc as PlayerDesc;
        ItemDesc d1;
        ItemDesc d2;
        Resources.Type2Item.TryGetValue((ushort)item1, out d1);
        Resources.Type2Item.TryGetValue((ushort)item2, out d2);

        if (con1 is Player)
        {
            for (var i = 0; i < 4; i++)
            {
                if (slot1.SlotId == i)
                {
                    if (d1 != null && d.SlotTypes[i] != d1.SlotType ||
                        d2 != null && d.SlotTypes[i] != d2.SlotType)
                    {
#if DEBUG
                        SLog.Error( "Invalid slot type");
#endif
                        Client.SendInventoryResult(1);
                        return;
                    }
                }
            }
        }

        if (con2 is Player)
        {
            for (var i = 0; i < 4; i++)
            {
                if (slot2.SlotId == i)
                {
                    if (d1 != null && d.SlotTypes[i] != d1.SlotType ||
                        d2 != null && d.SlotTypes[i] != d2.SlotType)
                    {
#if DEBUG
                        SLog.Error( "Invalid slot type");
#endif
                        Client.SendInventoryResult(1);
                        return;
                    }
                }
            }
        }

        // soulbound item into non soulbound bag
        if (con1 is Player plr) {
            if (d1 != null && d1.Soulbound && en2 is Container con && con.OwnerId != AccountId) {
                DropItem(Id, slot1.SlotId);
                return;
            }
        }

        if (en1 is GiftChest)
            Database.RemoveGift(Client.Account, item1);

        con1.Inventory[slot1.SlotId] = item2;
        con1.ItemDatas[slot1.SlotId] = data2;
        con2.Inventory[slot2.SlotId] = item1;
        con2.ItemDatas[slot2.SlotId] = data1;
        con1.UpdateInventorySlot(slot1.SlotId);
        con2.UpdateInventorySlot(slot2.SlotId);
        RecalculateEquipBonuses();
        Client.SendInventoryResult(0);
    }

    public bool ValidSlot(int slot)
    {
        var maxSlots = HasBackpack ? MaxSlotsWithBackpack : MaxSlotsWithoutBackpack;
        if (slot < 0 || slot >= maxSlots)
            return false;
        return true;
    }

    public void UpdateInventory()
    {
        var length = HasBackpack ? MaxSlotsWithBackpack : MaxSlotsWithoutBackpack;
        for (var k = 0; k < length; k++)
            UpdateInventorySlot(k);
    }

    public void UpdateInventorySlot(int slot)
    {
#if DEBUG
        if (!HasBackpack && slot >= MaxSlotsWithoutBackpack)
            throw new Exception("Should not be updating backpack stats when there is no backpack present.");
        if (slot < 0 || slot >= MaxSlotsWithBackpack)
            throw new Exception("Out of bounds slot update attempt.");
#endif

        int itemType = Inventory[slot];

        if (slot <= 11)
            SetSV(StatType.Inventory0 + slot, itemType);
        else
            SetSV(StatType.Backpack0 + (slot - 12), itemType);
    }
}
