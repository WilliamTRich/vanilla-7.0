using Common;
using RotMG.Utils;

namespace RotMG.Game.Logic.Behaviors;

public sealed class SetAltTexture : Behavior
{
    private class TextureState
    {
        public int CurrentTexture;
        public int RemainingTime;
    }
    public readonly int IndexMin;
    public readonly int IndexMax;
    public readonly int Cooldown;
    public readonly int CooldownVariance;
    public readonly bool Loop;

    public SetAltTexture(int minValue, int maxValue = -1, int cooldown = 0, int cooldownVariance = 0, bool loop = false)
    {
        IndexMin = minValue;
        IndexMax = maxValue;
        Cooldown = cooldown;
        CooldownVariance = cooldownVariance;
        Loop = loop;
    }
    public override void Enter(Entity host)
    {
        var state = new TextureState()
        {
            CurrentTexture = IndexMin,
            RemainingTime = Cooldown.NextCooldown(CooldownVariance)
        };
        host.AltTextureIndex = IndexMin;
        host.StateObject[Id] = state;
    }

    public override bool Tick(Entity host)
    {
        var textureState = (TextureState)host.StateObject[Id];

        textureState.RemainingTime -= Settings.MillisecondsPerTick;
        if (IndexMax == -1 || (textureState.CurrentTexture == IndexMax && !Loop))
            return false;

        if (textureState.RemainingTime <= 0)
        {
            var newTexture = (textureState.CurrentTexture >= IndexMax) ? IndexMin : textureState.CurrentTexture + 1;
            host.AltTextureIndex = newTexture;
            textureState.CurrentTexture = newTexture;
            textureState.RemainingTime = Cooldown.NextCooldown(CooldownVariance);
            return true;
        }

        return false;
    }

    public override void Exit(Entity host)
    {
        host.StateObject.Remove(Id);
    }
}