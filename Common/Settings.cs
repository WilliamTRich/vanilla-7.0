using Common.Utils;
using System.Xml.Linq;

namespace Common;

public static class SettingsDefaults
{
    public const int MaxClients = 256;
    public const string Address = "127.0.0.1";
    public const string ServerName = "Localhost";
    public static int[] Ports = [8080, 2050];
    public const string ResourceDirectory = "Resources";
    public const string DatabaseDirectory = "Database";
    public const int TicksPerSecond = 10;
    public const int MaxRealms = 1;
    public const bool AdminOnly = false;
}
public static class Settings
{
    public static int MaxClients;
    public static string Address;
    public static string ServerName;
    public static bool AdminOnly;
    public static int[] Ports;
    public static string ResourceDirectory;
    public static string DatabaseDirectory;
    public static int TicksPerSecond;
    public static int MillisecondsPerTick;
    public static float SecondsPerTick;
    public static int MaxRealms;

    public static void Init()
    {
        if (!File.Exists("Settings.xml")) {
            CreateSettingsFile();
        }

        var data = XElement.Parse(File.ReadAllText("Settings.xml"));
        MaxClients = data.ParseInt("MaxClients", SettingsDefaults.MaxClients);
        Address = data.ParseString("Address", SettingsDefaults.Address);
        Ports = data.ParseIntArray("Ports", ":", SettingsDefaults.Ports);
        ResourceDirectory = data.ParseString("@res", SettingsDefaults.ResourceDirectory);
        DatabaseDirectory = data.ParseString("@db", SettingsDefaults.DatabaseDirectory);
        MaxRealms = data.ParseInt("MaxRealms", SettingsDefaults.MaxRealms);
        AdminOnly = data.ParseBool("AdminOnly", SettingsDefaults.AdminOnly);
        ServerName = data.ParseString("ServerName", SettingsDefaults.ServerName);
        TicksPerSecond = data.ParseInt("TicksPerSecond", SettingsDefaults.TicksPerSecond);
        MillisecondsPerTick = 1000 / TicksPerSecond;
        SecondsPerTick = 1f / TicksPerSecond;
    }
    public static void CreateSettingsFile()
    {
        var data = new XElement("Settings");
        data.Add(new XAttribute("res", SettingsDefaults.ResourceDirectory));
        data.Add(new XAttribute("db", SettingsDefaults.DatabaseDirectory));
        data.Add(new XElement("Address", SettingsDefaults.Address));
        data.Add(new XElement("Ports", string.Join(':', SettingsDefaults.Ports)));
        data.Add(new XElement("AdminOnly", SettingsDefaults.AdminOnly));
        data.Add(new XElement("ServerName", SettingsDefaults.ServerName));
        data.Add(new XElement("TicksPerSecond", SettingsDefaults.TicksPerSecond));
        data.Add(new XElement("MaxClients", SettingsDefaults.MaxClients));
        data.Add(new XElement("MaxRealms", SettingsDefaults.MaxRealms));

        File.WriteAllText("Settings.xml", data.ToString());
    }
}
