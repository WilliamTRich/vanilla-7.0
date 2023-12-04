using Common;

namespace RotMG.Game.Logic.Behaviors;

public sealed class ChangeSize : Behavior
{
    public readonly int Rate;
    public readonly int Target;

    public ChangeSize(int rate, int target)
    {
        Rate = rate;
        Target = target;
    }
    public override void Enter(Entity host)
    {
        host.StateCooldown[Id] = 0;
    }

    public override bool Tick(Entity host)
    {
        host.StateCooldown[Id] -= Settings.MillisecondsPerTick;
        if (host.StateCooldown[Id] <= 0)
        {
            var size = host.Size;
            if (size != Target)
            {
                size += Rate;
                if ((Rate > 0 && size > Target) ||
                    (Rate < 0 && size < Target))
                    size = Target;

                host.Size = size;
            }

            host.StateCooldown[Id] = 150;
            return true;
        }
        
        return false;
    }
    
    public override void Exit(Entity host)
    {
        host.StateCooldown.Remove(Id);
    }
}