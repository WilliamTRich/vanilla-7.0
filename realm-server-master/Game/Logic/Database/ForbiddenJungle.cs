#region
using RotMG.Common;
using RotMG.Game.Logic;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

#endregion

namespace RotMG.Game.Logic.Database
{
    public sealed class ForbiddenJungle : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Great Coil Snake",
                new State("Start",
                    new DropPortalOnDeath("Forbidden Jungle Portal", 20, lifetime: 10000),
                    new Prioritize(
                        new StayCloseToSpawn(0.8f, 5),
                        new Wander(0.4f)
                    ),
                    new State("Waiting",
                        new PlayerWithinTransition(15, false, "Attacking")
                    ),
                    new State("Attacking",
                        new Shoot(10, index: 0, cooldown: 700, cooldownOffset: 600),
                        new Shoot(10, 10, 36, 1, cooldown: 2500),
                        new TossObject("Great Snake Egg", 4, 0, 999999, cooldownOffset: 0),
                        new TossObject("Great Snake Egg", 4, 90, 999999, 600),
                        new TossObject("Great Snake Egg", 4, 180, 999999, 1200),
                        new TossObject("Great Snake Egg", 4, 270, 999999, 1800),
                        new NoPlayerWithinTransition(30, false, "Waiting")
                    )
                )
            );
            db.Init("Great Snake Egg",
                new State("Start",
                    new TransformOnDeath("Great Temple Snake", 1, 2),
                    new State("Wait",
                        new TimedTransition(2500, "Explode"),
                        new PlayerWithinTransition(2, false, "Explode")
                    ),
                    new State("Explode",
                        new Suicide()
                    )
                )
            );
            db.Init("Great Temple Snake",
                    new State("Start",
                        new Prioritize(
                            new Follow(0.6f),
                            new Wander(0.4f)
                        ),
                        new Shoot(10, 2, 7, 0, cooldown: 1000, cooldownOffset: 0),
                        new Shoot(10, 6, 60, 1, cooldown: 2000, cooldownOffset: 600)
                    )
                )
                ;
        }
    }
}

