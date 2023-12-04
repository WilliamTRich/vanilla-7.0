using System.Xml.Linq;
using Common.Utils;

namespace Common;
public static class Resources
{
    public static Dictionary<ushort, ObjectDesc> Type2Object = [];
    public static Dictionary<string, ObjectDesc> Id2Object = [];
    public static Dictionary<string, ObjectDesc> IdLower2Object = [];
    public static Dictionary<ushort, PlayerDesc> Type2Player = [];
    public static Dictionary<string, PlayerDesc> Id2Player = [];
    public static Dictionary<ushort, SkinDesc> Type2Skin = [];
    public static Dictionary<string, SkinDesc> Id2Skin = [];
    public static Dictionary<ushort, TileDesc> Type2Tile = [];
    public static Dictionary<string, TileDesc> Id2Tile = [];
    public static Dictionary<ushort, ItemDesc> Type2Item = [];
    public static Dictionary<string, ItemDesc> Id2Item = [];
    public static Dictionary<string, ItemDesc> IdLower2Item = [];
    public static Dictionary<string, byte[]> WebFiles = [];
    public static List<XElement> News = [];
    public static string CombineResourcePath(string path) {
        return $"{Settings.ResourceDirectory}/{path}";
    }

    public static void Init() {
        LoadGameData();
        LoadWebFiles();
        LoadNews();
    }

    private static void LoadGameData() {
        if(!Directory.Exists(Path.Combine(Settings.ResourceDirectory, "GameData"))) {
            SLog.Warn("GameData Directory does not exist. Skipping.");
            return;
        }

        var paths = Directory.EnumerateFiles(CombineResourcePath("GameData/"), "*.xml", SearchOption.TopDirectoryOnly).ToArray();
        for (var i = 0; i < paths.Length; i++)
        {

#if DEBUG
            SLog.Debug($"Parsing GameData <{paths[i].Split('/').Last()}>");
#endif
            var data = XElement.Parse(File.ReadAllText(paths[i]));

            foreach (var e in data.Elements("Object"))
            {
                var id = e.ParseString("@id");
                var type = e.ParseUshort("@type");
#if DEBUG
                if (string.IsNullOrWhiteSpace(id))
                    throw new Exception("Invalid ID.");
                if (Type2Object.ContainsKey(type) || Type2Item.ContainsKey(type))
                    throw new Exception($"Duplicate type <{id}:{type}>");
                if (Id2Object.ContainsKey(id) || Id2Item.ContainsKey(id))
                    throw new Exception($"Duplicate ID <{id}:{type}>");
#endif

                switch (e.ParseString("Class"))
                {
                    case "Skin":
                        Id2Skin[id] = Type2Skin[type] = new SkinDesc(e, id, type);
                        break;
                    case "Player":
                        Id2Object[id] = IdLower2Object[id.ToLower()] = Type2Object[type] = Id2Player[id] = Type2Player[type] = new PlayerDesc(e, id, type);
                        break;
                    case "Equipment":
                    case "Dye":
                        Id2Item[id] = IdLower2Item[id.ToLower()] = Type2Item[type] = new ItemDesc(e, id, type);
                        break;
                    default:
                        Id2Object[id] = IdLower2Object[id.ToLower()] = Type2Object[type] = new ObjectDesc(e, id, type);
                        break;
                }
            }

            foreach (var e in data.Elements("Ground"))
            {
                var id = e.ParseString("@id");
                var type = e.ParseUshort("@type");
#if DEBUG
                if (string.IsNullOrWhiteSpace(id))
                    throw new Exception("Invalid ID.");
                if (Type2Tile.ContainsKey(type))
                    throw new Exception($"Duplicate type <{id}:{type}>");
                if (Id2Tile.ContainsKey(id))
                    throw new Exception($"Duplicate ID <{id}:{type}>");
#endif

                Id2Tile[id] = Type2Tile[type] = new TileDesc(e, id, type);
            }
        }

#if DEBUG
        SLog.Debug($"Parsed <{Type2Object.Count}> Objects");
        SLog.Debug($"Parsed <{Type2Player.Count}> Player Classes");
        SLog.Debug($"Parsed <{Type2Skin.Count}> Skins");
        SLog.Debug($"Parsed <{Type2Item.Count}> Items");
        SLog.Debug($"Parsed <{Type2Tile.Count}> Tiles");
#endif
    }

    private static void LoadWebFiles() {
        if (!Directory.Exists(Path.Combine(Settings.ResourceDirectory, "Web"))) {
            SLog.Warn("Web Directory does not exist. Skipping.");
            return;
        }

        var paths = Directory.EnumerateFiles(CombineResourcePath("Web/"), "*", SearchOption.AllDirectories).ToArray();
        for (var i = 0; i < paths.Length; i++)
        {
            var display = '/' + paths[i].Split('/')[2].Replace(@"\", "/");
#if DEBUG
            SLog.Debug($"Loading Web File <{display}>");
#endif
            WebFiles[display] = File.ReadAllBytes(paths[i]);
        }
    }

    private static void LoadNews() {
        if (!File.Exists(Path.Combine(Settings.ResourceDirectory, "News.xml"))) {
            SLog.Warn("News file does not exist. Skipping.");
            return;
        }

        News.Clear();
        var data = File.ReadAllText(CombineResourcePath("News.xml"));
        if (!string.IsNullOrWhiteSpace(data))
        {
            var news = XElement.Parse(data);
            foreach (var item in news.Elements("Item").OrderByDescending(k => k.ParseInt("Date", Database.UnixTime())))
                News.Add(item);
        }
    }
}
