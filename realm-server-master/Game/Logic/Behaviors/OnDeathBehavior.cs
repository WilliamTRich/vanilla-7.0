using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    public class OnDeathBehavior : Behavior
    {
        private Behavior _behavior;
        public OnDeathBehavior(IBehavior behavior)
        {
            _behavior = behavior as Behavior;
        }

        public override void Death(Entity host)
        {
            _behavior?.Enter(host);
            _behavior?.Tick(host);
            _behavior?.Exit(host);
            _behavior?.Death(host);
        }
    }
}
