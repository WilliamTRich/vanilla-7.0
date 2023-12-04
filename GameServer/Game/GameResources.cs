using Common;
using Common.Utils;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace RotMG.Game;
public static class GameResources {
    public static Dictionary<string, WorldDesc> Worlds = [];
    public static Dictionary<ushort, WorldDesc> PortalId2World = [];
    public static Dictionary<string, Map> SetPieces = [];
    public static Dictionary<ushort, QuestDesc> Quests = [];
    public static void Init() {
        LoadSetPieces();
        LoadWorlds();
        LoadQuests();

#if DEBUG
        SLog.Debug($"Parsed <{Worlds.Count}> Worlds");
        SLog.Debug($"Parsed <{PortalId2World.Count}> Portals");
        SLog.Debug($"Parsed <{SetPieces.Count}> SetPieces");
        SLog.Debug($"Parsed <{Quests.Count}> Quests");
#endif
    }
    private static void LoadQuests()
    {
        foreach (var desc in Resources.Type2Object.Values)
        {
            if (!desc.Quest) continue;
            var priority = desc.Level;
            if (desc.Hero) priority += 1000;
            Quests[desc.Type] = new QuestDesc(desc.Level, priority);
        }
    }
    private static void LoadWorlds()
    {
        foreach (var e in XElement.Parse(File.ReadAllText(Resources.CombineResourcePath("Worlds/Worlds.xml"))).Elements("World"))
        {
#if DEBUG
            SLog.Debug($"Parsing World <{e.ParseString("@name")}>");
#endif
            var desc = new WorldDesc(e);
            Worlds[desc.Name] = desc;
            foreach (var portal in desc.Portals)
                PortalId2World[portal] = desc;
        }
    }
    private static void LoadSetPieces()
    {
        foreach (var e in XElement.Parse(File.ReadAllText(Resources.CombineResourcePath("SetPieces/SetPieces.xml"))).Elements("SetPiece"))
        {
#if DEBUG
            SLog.Debug($"Parsing World <{e.ParseString("@id")}>");
#endif
            var name = e.ParseString("Map");
            Map map;
            if (name.EndsWith("wmap"))
                map = new WMap(File.ReadAllBytes(Resources.CombineResourcePath($"SetPieces/{name}")));
            else
                map = new JSMap(File.ReadAllText(Resources.CombineResourcePath($"SetPieces/{name}")));

            SetPieces[e.ParseString("@id")] = map;
        }
    }

}
