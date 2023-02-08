using RotMG.Common;
using RotMG.Game.Entities;

namespace RotMG.Game.Logic.Behaviors
{
    public sealed class Suicide : Behavior
    {
        public readonly int Time;

        public Suicide(int time = 0)
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
            if (host.StateCooldown[Id] > 0)
                return false;
#if DEBUG
            if (!(host is Enemy))
            {
                Program.Print(PrintType.Error, "Use Decay instead");
                return false;
            }
#endif
            ((Enemy)host).Death((host as Enemy).LastHitter);
            return true;
        }
        
        public override void Exit(Entity host)
        {
            host.StateCooldown.Remove(Id);
        }
    }
}