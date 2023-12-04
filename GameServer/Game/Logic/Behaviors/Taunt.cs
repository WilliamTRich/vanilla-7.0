using System;
using System.Linq;
using Common;
using RotMG.Game.Entities;
using RotMG.Networking;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors;

public sealed class Taunt : Behavior
{
    public readonly float Probability = 1;
    public readonly bool Broadcast = false;
    public readonly int Cooldown = 0;
    public readonly int CooldownVariance = 0;
    public int Ordered = -1;
    public readonly string[] Text;
    private readonly int Distance = 15;
    
    public Taunt(params string[] text)
    {
        Text = text;
        
    }
    public Taunt(int dist, params string[] text)
    {
        Text = text;
        Distance = dist;

    }

    public Taunt(float probability, params string[] text)
    {
        Text = text;
        Probability = probability;
    }
    
    public Taunt(bool broadcast, params string[] text)
    {
        Text = text;
        Broadcast = broadcast;
    }

    public Taunt(int cooldown, int cooldownVariance, params string[] text)
    {
        Text = text;
        Cooldown = cooldown;
        CooldownVariance = cooldownVariance;
    }

    public Taunt(float probability, int cooldown, params string[] text)
    {
        Text = text;
        Cooldown = cooldown;
        Probability = probability;
    }

    public Taunt(float probability, bool broadcast, params string[] text)
    {
        Text = text;
        Probability = probability;
        Broadcast = broadcast;
    }
    public Taunt(float probability, int cooldown, int cooldownVariance, params string[] text)
    {
        Text = text;
        Probability = probability;
        Cooldown = cooldown;
        CooldownVariance = cooldownVariance;
    }
    public Taunt(bool broadcast, int cooldown, int cooldownVariance, params string[] text)
    {
        Text = text;
        Broadcast = broadcast;
        Cooldown = cooldown;
        CooldownVariance = cooldownVariance;
    }

    public Taunt(float probability, bool broadcast, int cooldown, int cooldownVariance, params string[] text)
    {
        Text = text;
        Probability = probability;
        Broadcast = broadcast;
        Cooldown = cooldown;
        CooldownVariance = cooldownVariance;
    }

    public override void Enter(Entity host)
    {
        host.StateObject[Id] = null;
    }

    public override bool Tick(Entity host)
    {
        if (host.StateObject[Id] != null && Cooldown == 0)
            return false;

        int cd;
        if (host.StateObject[Id] == null)
            cd = Cooldown.NextCooldown(CooldownVariance);
        else
            cd = (int) host.StateObject[Id];
        
        cd -= Settings.MillisecondsPerTick;
        host.StateObject[Id] = cd;

        if (cd > 0)
            return false;

        host.StateObject[Id] = Cooldown.NextCooldown(CooldownVariance);

        if (!MathUtils.Chance(Probability)) 
            return false;

        string taunt;
        if (Ordered != -1)
        {
            taunt = Text[Ordered % Text.Length];
            Ordered = (Ordered + 1) % Text.Length;
        }
        else
            taunt = Text[MathUtils.Next(Text.Length)];

        if (taunt.Contains("{PLAYER}", StringComparison.OrdinalIgnoreCase))
        {
            var player = host.GetNearestPlayer(Player.SightRadius);
            if (player == null) 
                return false;
            taunt = taunt.Replace("{PLAYER}", player.Name, StringComparison.OrdinalIgnoreCase);
        }
        taunt = taunt.Replace("{HP}", host.Hp.ToString(), StringComparison.OrdinalIgnoreCase);

        var packet = GameServer.Text("#" + host.Desc.DisplayId, host.Id, -1, 3, "", taunt, 0xff0000, 0xff2626);
        if (Broadcast)
        {
            foreach (var player in host.Parent.Players.Values)
                player.Client.Send(packet);
        }
        else
        {
            foreach (var player in host.Parent.PlayerChunks.HitTest(host.Position, Distance).OfType<Player>())
                player.Client.Send(packet);
        }

        return true;
    }

    public override void Exit(Entity host)
    {
        host.StateObject.Remove(Id);
    }
}