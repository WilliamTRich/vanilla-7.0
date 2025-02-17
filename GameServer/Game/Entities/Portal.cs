using System.Linq;
using System.Text.RegularExpressions;
using Common;
using RotMG.Networking;

namespace RotMG.Game.Entities;

public class Portal : Entity
{
    private static readonly Regex PlayerCountRegex = new Regex(@" \((\d+)\)$");
    
    public World WorldInstance;

    private bool _usable;
    public bool Usable
    {
        get => _usable;
        set => TrySetSV(StatType.Active, (_usable = value) ? 1 : 0);
    }
    
    public Portal(ushort type, int? lifetime = 30000) : base(type, lifetime) { }

    public World GetWorldInstance(Client connectingClient)
    {
        if (WorldInstance != null)
            return WorldInstance;
        
        if(Type == 0x070e || Type == 0x071c || Type == 0x0704)
        {
            return Manager.Realms.Values.First();
        }

        if (!GameResources.PortalId2World.TryGetValue(Type, out var worldDesc))
        {
#if DEBUG
            SLog.Error( $"No world data for {Desc.DungeonName}");
#endif
            return null;
        }
        
        var world = Manager.GetWorld(worldDesc.Id, connectingClient);
        if (world != null)
            return world;
        
        world = WorldInstance = Manager.AddWorld(worldDesc, connectingClient);
        world.Portal = this;

        return world;
    }

    public override void OnLifeEnd()
    {
        if (WorldInstance != null)
            WorldInstance.Portal = null;
    }

    public override void Tick()
    {
        if (Name == null)
            return;
        
        var count = WorldInstance?.Players.Count;
        Name = PlayerCountRegex.Replace(Name, $" ({count})");
    }
}