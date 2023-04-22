using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public sealed class DavyJonesLocker : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Purple Key",
                new State("Idle",
                    new PlayerWithinTransition(1.5f, true, "Consume")
                    ),
                new State("Consume",
                    new DavyJonesDoorOpen(0),
                    new Taunt(true, "Purple door's are now open"),
                    new Decay(100)
                    )
                );            
            db.Init("Green Key",
                new State("Idle",
                    new PlayerWithinTransition(1.5f, true, "Consume")
                    ),
                new State("Consume",
                    new DavyJonesDoorOpen(1),
                    new Taunt(true, "Green door's are now open"),
                    new Decay(100)
                    )
                );
            db.Init("Red Key",
                new State("Idle",
                    new PlayerWithinTransition(1.5f, true, "Consume")
                    ),
                new State("Consume",
                    new DavyJonesDoorOpen(2),
                    new Taunt(true, "Red door's are now open"),
                    new Decay(100)
                    )
                );
            db.Init("Yellow Key",
                new State("Idle",
                    new PlayerWithinTransition(1.5f, true, "Consume")
                    ),
                new State("Consume",
                    new DavyJonesDoorOpen(3),
                    new Taunt(true, "Yellow door's are now open"),
                    new Decay(100)
                    )
                );
        }
    }
}