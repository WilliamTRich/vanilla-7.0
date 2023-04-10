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

            db.Init("Mask Shaman", new State("base",
                new Shoot(5, 1),
                new Follow(2.0f, 5, 5),
                new StayCloseToSpawn(1.0f)
            ));

            db.Init("Totem Spirit", new State("base",
                new TimedTransition(750, "shoot")
                ),
                new State("shoot",
                new Shoot(5, 3, shootAngle: 25, cooldown: 1000),
                new Prioritize(
                    new Follow(2.0f, 5, 5),
                    new Wander(0.4f)
                ),
                new StayCloseToSpawn(1.0f)
            ));

            db.Init("Jungle Totem", new State("base",
                new State("not-activate",
                    new PlayerWithinTransition(5f, true, "activate")
                ),
                new State("activate",
                    new Flash(0xff00ff, 1.0f, 1),
                    new TossObject("Totem Spirit", cooldown: 5000, probability: 0.3f, range: 999),
                    new TimedTransition(1000, "activate")
                )
            ));

            db.Init("Mask Warrior", new State("base",
                new Shoot(5, 1, cooldown: 2000, predictive: 1),
                new Prioritize(
                    new Follow(2.0f, 5, 2),
                    new Wander(0.2f)
                ),
                new StayCloseToSpawn(1.0f)
            ));

            db.Init("Mask Hunter", new State("base",
                new Shoot(5, 4, cooldown: 2000, predictive: 1, shootAngle: 20),
                new Prioritize(
                    new Follow(2.0f, 5, 2),
                    new Wander(0.2f)
                ),
                new StayCloseToSpawn(1.0f)
            ));

            db.Init("Basilisk", new State("base",
                new Shoot(8, 1, cooldown: 300, shootAngle: 20, index: 0),
                new Shoot(8, 1, cooldown: 2000, predictive: 1, index: 1),
                new Shoot(8, 1, cooldown: 2000, predictive: 1, index: 2),
                new Shoot(8, 1, cooldown: 2000, predictive: 1, index: 3),
                new Wander(0.2f),
                new StayCloseToSpawn(1.0f)
            ));

            db.Init("Basilisk Baby", new State("base",
                new Shoot(8, 1, cooldown: 300, shootAngle: 20, index: 0),
                new Wander(0.2f),
                new StayCloseToSpawn(1.0f)
            ));

            db.Init("Boss Totem", new State("base",
                    new Shoot(1, 5, cooldown: 1600, fixedAngle: 0, cooldownOffset: 0, shootAngle: 72),
                    new Shoot(1, 5, cooldown: 1600, fixedAngle: 45, cooldownOffset: 200, shootAngle: 72),
                    new Shoot(1, 5, cooldown: 1600, fixedAngle: 90, cooldownOffset: 400, shootAngle: 72),
                    new Shoot(1, 5, cooldown: 1600, fixedAngle: 135, cooldownOffset: 600, shootAngle: 72),
                    new Shoot(1, 5, cooldown: 1600, fixedAngle: 175, cooldownOffset: 800, shootAngle: 72),
                    new Shoot(1, 5, cooldown: 1600, fixedAngle: 215, cooldownOffset: 1000, shootAngle: 72),
                    new Shoot(1, 5, cooldown: 1600, fixedAngle: 270, cooldownOffset: 1200, shootAngle: 72),
                    new Shoot(1, 5, cooldown: 1600, fixedAngle: 315, cooldownOffset: 1400, shootAngle: 72),
                    new Shoot(1, 5, cooldown: 1600, fixedAngle: 360, cooldownOffset: 1600, shootAngle: 72),
                    new TossObject("Totem Spirit", cooldown: 16000, range: 11)
                ));

            db.Init("Mixcoatl the Masked God", new State("base",
                new State("Wait",
                        new PlayerWithinTransition(10.5f, true, "start")
                    ),
                new State("restart",
                        new Flash(0xff0000, 1.0f, 5),
                        new TimedTransition(1000, "start")
                    ),
                new State("start",
                        new StayCloseToSpawn(1.0f, 1),
                        new Shoot(10, 2, index: 1, shootAngle: 45),
                        new Shoot(10, 1, index: 2, cooldown: 2000),
                        new Shoot(1, 3, cooldown: 10000, fixedAngle: 0, cooldownOffset: 0, shootAngle: 72),
                        new Shoot(1, 3, cooldown: 10000, fixedAngle: 45, cooldownOffset: 100, shootAngle: 72),
                        new Shoot(1, 3, cooldown: 10000, fixedAngle: 90, cooldownOffset: 200, shootAngle: 72),
                        new Shoot(1, 3, cooldown: 10000, fixedAngle: 135, cooldownOffset: 300, shootAngle: 72),
                        new Shoot(1, 3, cooldown: 10000, fixedAngle: 175, cooldownOffset: 400, shootAngle: 72),
                        new Shoot(1, 3, cooldown: 10000, fixedAngle: 215, cooldownOffset: 500, shootAngle: 72),
                        new Shoot(1, 3, cooldown: 10000, fixedAngle: 270, cooldownOffset: 600, shootAngle: 72),
                        new Shoot(1, 3, cooldown: 10000, fixedAngle: 315, cooldownOffset: 700, shootAngle: 72),
                        new Shoot(1, 3, cooldown: 10000, fixedAngle: 360, cooldownOffset: 800, shootAngle: 72),
                        new TimedTransition(800, "Wait"))
                    ),
                    new Threshold(0.01f, 
                        new ItemLoot("Potion of Vitality", 1f), 
                        new ItemLoot("Robe of the Tlatoani", 0.05f), 
                        new ItemLoot("Cracked Crystal Skull", 0.05f),
                        new ItemLoot("Staff of the Crystal Serpent", 0.05f), 
                        new ItemLoot("Crystal Bone Ring", 0.05f)
                    )
                    );


        }
    }
}

