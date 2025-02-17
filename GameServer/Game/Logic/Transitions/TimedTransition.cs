﻿using Common;

namespace RotMG.Game.Logic.Transitions;

public class TimedTransition : Transition
{
    public readonly int Time;

    public TimedTransition(int time = 1000, params string[] targetStates) : base(targetStates)
    {
        Time = time;
    }
    public override void Enter(Entity host)
    {
        host.StateCooldown[Id] = Time;
    }

    public override bool Tick(Entity host)
    {
        host.StateCooldown[Id] -= Settings.MillisecondsPerTick;
        if (host.StateCooldown[Id] <= 0)
        {
            host.StateCooldown[Id] = Time;
            return true;
        }
        return false;
    }
    
    public override void Exit(Entity host)
    { 
        host.StateCooldown.Remove(Id);
    }
}
