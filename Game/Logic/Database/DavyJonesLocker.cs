using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public sealed class DavyJonesLocker : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Davy Jones",
                    new DropPortalOnDeath("Glowing Realm Portal"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("Floating",
                        new ChangeSize(100, 100),
                        new SetAltTexture(3),
                        new Wander(.6f),
                        new StayCloseToSpawn(0.2f, 8),
                        new Shoot(10, 5, 10, 0, cooldown: 1000),
                        new Shoot(10, 1, 10, 1, cooldown: 2000),
                        new EntityWithinTransition("Ghost Lanturn On", 30, "CheckOffLanterns")
                        ),
                    new State("CheckOffLanterns",
                        new SetAltTexture(2),
                        new ReturnToSpawn(1),
                        new Shoot(10, 5, 10, 0, cooldown: 1000, predictive: 0.5f),
                        new Shoot(10, 1, 10, 1, cooldown: 500),
                        new EntitiesNotWithinTransition(30, targetState: "Vunerable", "Ghost Lanturn Off")
                        ),
                    new State("Vunerable",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                        new OrderOnce(99, "Ghost Lanturn On", "Stay Here"),
                        new SetAltTexture(5, 6, 500, loop: true),
                        new TimedTransition(2500, "deactivate 1")
                        ),
                    new State("deactivate 1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true, -1),
                        new SetAltTexture(4),
                        new OrderOnce(99, "Ghost Lanturn On", "Shoot"),
                        new TimedTransition(3500, "Floating")
                        ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Wisdom", 0.80f, 3),
                    new ItemLoot("Potion of Wisdom", 0.80f, 3),
                    new ItemLoot("Potion of Wisdom", 1),
                    new ItemLoot("Ghost Pirate Rum", 0.4f),
                    new ItemLoot("Ghost Pirate Rum", 0.3f),
                    new ItemLoot("Ghost Pirate Rum", 0.2f),
                    new TierLoot(8, TierLoot.LootType.Weapon, 0.25f),
                    new TierLoot(9, TierLoot.LootType.Weapon, 0.125f),
                    new TierLoot(8, TierLoot.LootType.Armor, 0.25f),
                    new TierLoot(9, TierLoot.LootType.Armor, 0.125f),
                    new TierLoot(3, TierLoot.LootType.Ability, 0.25f),
                    new TierLoot(4, TierLoot.LootType.Ability, 0.125f),
                    new TierLoot(4, TierLoot.LootType.Ring, 0.125f),
                    new TierLoot(10, TierLoot.LootType.Armor, 0.125f),
                    new TierLoot(5, TierLoot.LootType.Ability, 0.0625f),
                    new ItemLoot("Captain's Ring", 0.01f),
                    new ItemLoot("Wine Cellar Incantation", 0.05f),
                    new ItemLoot("Spectral Cloth Armor", 0.01f),
                    new ItemLoot("Spirit Dagger", 0.01f),
                    new ItemLoot("Ghostly Prism", 0.01f)
                    ));

            db.Init("Ghost Lanturn Off",
                new State("base",
                    new TransformOnDeath("Ghost Lanturn On")
                ));
            db.Init("Ghost Lanturn On",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("idle",
                        new TimedTransition(8000, "gone")
                        ),
                    new State("Stay Here"),
                    new State("Shoot",
                        new Shoot(10, 6, cooldown: 9000001, cooldownOffset: 100),
                        new TimedTransition(250, "gone")
                        ),
                    new State("gone",
                        new Transform("Ghost Lanturn Off")
                        )
                    );

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


            db.Init("Lost Soul",
                    new State("Default",
                        new Prioritize(
                            new Orbit(0.3f, 3, 20, "Ghost of Roger"),
                            new Wander(0.1f)
                            ),
                        new PlayerWithinTransition(4, true, "Default1")
                        ),
                    new State("Default1",
                       new Charge(0.5f, 8, cooldown: 2000),
                       new TimedTransition(2200, "Blammo")
                    ),
                     new State("Blammo",
                       new Shoot(10, count: 6, index: 0, cooldown: 2000),
                       new Suicide()
                    )
                );
            
            db.Init("Ghost of Roger",
                new State("spawn",
                    new Spawn("Lost Soul", 3, 1, 5000),
                    new TimedTransition(200, "Attack")
                ),
                new State("Attack",
                    new Shoot(13, 1, 0, 0, cooldown: 400),
                    new TimedTransition(200, "Attack2")
                ),
                new State("Attack2",
                    new Shoot(13, 1, 0, 0, cooldown: 400),
                    new TimedTransition(200, "Attack3")
                ),
                new State("Attack3",
                    new Shoot(13, 1, 0, 0, cooldown: 400),
                    new TimedTransition(200, "Wait")
                ),
                new State("Wait",
                    new TimedTransition(1000, "Attack")
                )
            );

            db.Init("Lil' Ghost Pirate",
                    new State("Default",
                        new Prioritize(
                            new Follow(0.6f, 8, 1),
                            new Wander(0.1f)
                            ),
                        new Shoot(10, count: 1, index: 0, cooldown: 2000),
                        new ChangeSize(30, 120),
                        new TimedTransition(2850, "Default1")
                        ),
                    new State("Default1",
                       new StayBack(0.2f, 3),
                       new TimedTransition(1850, "Default")
                    )
                );
            
                db.Init("Zombie Pirate Sr",
                    new Shoot(10, count: 1, index: 0, cooldown: 2000),
                    new State("Default",
                        new Prioritize(
                            new Follow(0.3f, 8, 1),
                            new Wander(0.1f)
                            ),
                        new TimedTransition(2850, "Default1")
                        ),
                    new State("Default1",
                       new ConditionalEffect(ConditionEffectIndex.Armored),
                       new Prioritize(
                            new Follow(0.3f, 8, 1),
                            new Wander(0.1f)
                            ),
                        new TimedTransition(2850, "Default")
                    )
                );
            
           db.Init("Zombie Pirate Jr",
                    new State("Default",
                        new Prioritize(
                            new Follow(0.4f, 8, 1),
                            new Wander(0.1f)
                            ),
                        new Shoot(10, count: 1, index: 0, cooldown: 2500),
                        new TimedTransition(2850, "Default1")
                        ),
                    new State("Default1",
                       new Swirl(0.2f, 3),
                       new TimedTransition(1850, "Default")
                    )
                );
        db.Init("Captain Summoner",
            new State("Default",
                new ConditionalEffect(ConditionEffectIndex.Invincible)
                )
            );
           db.Init("GhostShip Rat",
                    new State("Default",
                        new Shoot(10, count: 1, index: 0, cooldown: 1750),
                        new Prioritize(
                            new Follow(0.55f, 8, 1),
                            new Wander(0.1f)
                            )
                        )
                    );
        db.Init("Violent Spirit",
                new State("Default",
                    new ChangeSize(35, 120),
                    new Shoot(10, count: 3, index: 0, cooldown: 1750),
                    new Prioritize(
                        new Follow(0.25f, 8, 1),
                        new Wander(0.1f)
                        )
                    )
                );
           db.Init("School of Ghostfish",
                new State("Default",
                    new Shoot(10, count: 3, shootAngle: 18, index: 0, cooldown: 4000),
                    new Wander(0.35f)
                    )
            );


        }
    }
}