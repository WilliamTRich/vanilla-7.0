using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public sealed class Misc : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("White Fountain",
                new HealPlayer(5, 1000, 1000),
                new HealPlayer(5, 1000, 1000, 0, true)
            );
        }
    }
}