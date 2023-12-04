using Common;
using RotMG.Game.Entities;
using RotMG.Networking;

namespace RotMG.Game.Logic.Behaviors;

public sealed class Flash : Behavior
{
    public readonly uint Color;
    public readonly float FlashPeriod;
    public readonly int Repeats;

    public Flash(uint color, float flashPeriod, int repeats)
    {
        Color = color;
        FlashPeriod = flashPeriod;
        Repeats = repeats;
    }
    public override void Enter(Entity host)
    {
        var eff = GameServer.ShowEffect(
            ShowEffectIndex.Flash, host.Id, Color, new Vector2(FlashPeriod, Repeats));
        
        foreach (var ent in host.Parent.PlayerChunks.HitTest(host.Position, Player.SightRadius))
        {
            if (!(ent is Player player)) continue;
            
            player.Client.Send(eff);
        }
    }
}