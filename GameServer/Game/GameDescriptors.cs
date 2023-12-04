using Common;
using Common.Utils;
using System.IO;
using System.Xml.Linq;

namespace RotMG.Game;
public class WorldDesc {
    public readonly string Name;
    public readonly string DisplayName;
    public readonly string Music;
    public readonly int Id;
    public readonly int Background;
    public readonly bool ShowDisplays;
    public readonly bool AllowTeleport;
    public readonly int BlockSight;
    public readonly bool Persist;
    public readonly bool IsTemplate;
    public readonly ushort[] Portals;
    public readonly Map[] Maps;
    public WorldDesc(XElement e) {
        Name = e.ParseString("@name");
        DisplayName = e.ParseString("@display", Name);
        Music = e.ParseString("Music", "");
        Id = e.ParseInt("@id");
        Background = e.ParseInt("Background");
        ShowDisplays = e.ParseBool("ShowDisplays");
        AllowTeleport = e.ParseBool("AllowTeleport");
        BlockSight = e.ParseInt("BlockSight");
        Persist = e.ParseBool("Persist");
        IsTemplate = e.ParseBool("IsTemplate");
        Portals = e.ParseUshortArray("Portals", ";", []);

        var maps = e.ParseStringArray("Maps", ";", []);
        Maps = new Map[maps.Length];
        for (var i = 0; i < maps.Length; i++) {
            if (maps[0].EndsWith("wmap"))
                Maps[i] = new WMap(File.ReadAllBytes(Resources.CombineResourcePath($"Worlds/{maps[i]}")));
            else
                Maps[i] = new JSMap(File.ReadAllText(Resources.CombineResourcePath($"Worlds/{maps[i]}")));
        }
    }
}
