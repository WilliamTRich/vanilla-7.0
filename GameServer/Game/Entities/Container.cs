using Common;
using System;

namespace RotMG.Game.Entities;

public interface IContainer
{
    public int[] Inventory { get; set; }
    public int[] ItemDatas { get; set; }
    public void UpdateInventory();
    public void UpdateInventorySlot(int slot);
    public bool ValidSlot(int slot);
}

public class Container : Entity, IContainer
{
    public const int MaxSlots = 8;

    public const ushort BrownBag = 0x0500;
    public const ushort PurpleBag = 0x0506;
    public const ushort CyanBag = 0x0507;
    public const ushort BlueBag = 0x0508;
    public const ushort WhiteBag = 0x0509;
    public static ushort FromBagType(int bagType) 
    {
        switch (bagType) 
        {
            case 0: return BrownBag;
            case 1: return PurpleBag;
            case 2: return CyanBag;
            case 3: return BlueBag;
            case 4: return WhiteBag;
        }
        throw new Exception("Invalid bag type");
    }
    
    private int _ownerId = -1;
    public int OwnerId
    {
        get => _ownerId;
        set => TrySetSV(StatType.OwnerAccountId, _ownerId = value);
    }
    
    public int[] Inventory { get; set; }
    public int[] ItemDatas { get; set; }

    public Container(ushort type, int ownerId, int? lifetime) : base(type, lifetime)
    {
        OwnerId = ownerId;
        Inventory = new int[MaxSlots];
        ItemDatas = new int[MaxSlots];
        for (var i = 0; i < MaxSlots; i++)
        {
            Inventory[i] = -1;
            ItemDatas[i] = -1;
        }
    }

    public override void Tick()
    {
        if (Lifetime == null)
        {
            base.Tick();
            return;
        }
        
        var disappear = true;
        for (var i = 0; i < MaxSlots; i++)
            if (Inventory[i] != -1)
            {
                disappear = false;
                break;
            }

        if (disappear)
        {
            Parent.RemoveEntity(this);
            return;
        }

        base.Tick();
    }

    public bool ValidSlot(int slot)
    {
        if (slot < 0 || slot >= MaxSlots)
            return false;
        return true;
    }

    public void UpdateInventory()
    {
        for (var k = 0; k < MaxSlots; k++)
            UpdateInventorySlot(k);
    }

    public virtual void UpdateInventorySlot(int slot)
    {
#if DEBUG
        if (slot < 0 || slot >= MaxSlots)
            throw new Exception("Out of bounds slot update attempt.");
#endif
        SetSV(StatType.Inventory0 + slot, Inventory[slot]);
    }
}
