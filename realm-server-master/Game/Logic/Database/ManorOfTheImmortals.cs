using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public sealed class ManorOfTheImmortals : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Lord Ruthven",
                    new DropPortalOnDeath(target: "Glowing Realm Portal"),
                    new State("Ini",
                        new Wander(speed: 0.3f),
                        new StayCloseToSpawn(speed: 0.3f, range: 4),
                        new HealthTransition(threshold: 0.995f, targetStates: "Choose"),
                        new PlayerWithinTransition(radius: 9, targetStates: "Choose")
                    ),
                    new State("Choose",
                        new Wander(speed: 0.3f),
                        new StayCloseToSpawn(speed: 0.3f, range: 4),
                        new HealthTransition(threshold: 0.25f, targetStates: "Daggers4"),
                        new HealthTransition(threshold: 0.5f, targetStates: "Daggers3"),
                        new HealthTransition(threshold: 0.75f, targetStates: "Daggers2"),
                        new HealthTransition(threshold: 1, targetStates: "Daggers1")
                    ),
                    new State("Daggers1",
                        new Shoot(range: 24, count: 3, shootAngle: 12, index: 0, cooldown: 1600, cooldownOffset: 0),
                        new Shoot(range: 24, count: 3, shootAngle: 12, index: 0, cooldown: 1600, cooldownOffset: 400),
                        new Shoot(range: 24, count: 3, shootAngle: 12, index: 0, cooldown: 1600, cooldownOffset: 800),
                        new HealthTransition(threshold: 0.75f, targetStates: "CheckCoffins"),
                        new TimedTransition(time: 6000, targetStates: "CheckCoffins")
                    ),
                    new State("Daggers2",
                        new Shoot(range: 24, count: 5, shootAngle: 15, index: 0, cooldown: 1600, cooldownOffset: 0),
                        new Shoot(range: 24, count: 5, shootAngle: 15, index: 0, cooldown: 1600, cooldownOffset: 400),
                        new Shoot(range: 24, count: 5, shootAngle: 15, index: 0, cooldown: 1600, cooldownOffset: 800),
                        new HealthTransition(threshold: 0.5f, targetStates: "CheckCoffins"),
                        new TimedTransition(time: 6000, targetStates: "CheckCoffins")
                    ),
                    new State("Daggers3",
                        new Shoot(range: 24, count: 9, shootAngle: 22, index: 0, cooldown: 1600, cooldownOffset: 0),
                        new Shoot(range: 24, count: 9, shootAngle: 22, index: 0, cooldown: 1600, cooldownOffset: 400),
                        new Shoot(range: 24, count: 9, shootAngle: 22, index: 0, cooldown: 1600, cooldownOffset: 800),
                        new HealthTransition(threshold: 0.25f, targetStates: "CheckCoffins"),
                        new TimedTransition(time: 6000, targetStates: "CheckCoffins")
                    ),
                    new State("Daggers4",
                        new Shoot(range: 24, count: 18, shootAngle: 20, index: 0, cooldown: 1600, cooldownOffset: 0),
                        new Shoot(range: 24, count: 18, shootAngle: 20, index: 0, cooldown: 1600, cooldownOffset: 400),
                        new Shoot(range: 24, count: 18, shootAngle: 20, index: 0, cooldown: 1600, cooldownOffset: 800),
                        new TimedTransition(time: 6000, targetStates: "CheckCoffins")
                    ),
                    new State("CheckCoffins",
                        new HealthTransition(threshold: 0.50f, targetStates: "pre ThrowCoffins"),
                        new EntitiesNotWithinTransition(99, "pre ThrowCoffins", "Coffin Creature")
                    ),
                    new State("pre ThrowCoffins",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(range: 24, count: 24, shootAngle: 15, index: 1, fixedAngle: 0, cooldown: 6000, cooldownOffset: 600),
                        new Shoot(range: 24, count: 12, shootAngle: 30, index: 1, fixedAngle: 0, cooldown: 6000, cooldownOffset: 1000),
                        new Shoot(range: 24, count: 24, shootAngle: 15, index: 1, fixedAngle: 0, cooldown: 6000, cooldownOffset: 1400),
                        new Shoot(range: 24, count: 12, shootAngle: 30, index: 1, fixedAngle: 0, cooldown: 6000, cooldownOffset: 1800),
                        new Shoot(range: 24, count: 24, shootAngle: 15, index: 1, fixedAngle: 0, cooldown: 6000, cooldownOffset: 2200),
                        new TimedTransition(time: 2400, targetStates: "ThrowCoffins")
                    ),
                    new State("ThrowCoffins",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new ReturnToSpawn(speed: 0.9f),
                        new TimedTransition(time: 1000, targetStates: "Shoot")
                    ),
                    new State("Shoot",
                        new ReturnToSpawn(speed: 0.9f),
                        new TossObject(child: "Coffin Creature", range: 8.5f, angle: 0, cooldown: 5000),
                        new TossObject(child: "Coffin Creature", range: 8.5f, angle: 90, cooldown: 5000),
                        new TossObject(child: "Coffin Creature", range: 8.5f, angle: 180, cooldown: 5000),
                        new TossObject(child: "Coffin Creature", range: 8.5f, angle: 270, cooldown: 5000),
                        new TimedTransition(time: 1200, targetStates: "Position")
                    ),
                    new State("Position",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new ReturnToSpawn(speed: 0.85f),
                        new TimedTransition(time: 1600, targetStates: "Split")
                    ),
                    new State("Split",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true, -1),
                        new ChangeSize(rate: -100, target: 0),
                        new Spawn(children: "Bat Swarm 1", maxChildren: 1, initialSpawn: 0),
                        new TimedTransition(time: 800, targetStates: "Attack")
                    ),
                    new State("Attack",
                        new Reproduce(children: "Vampire Bat", densityRadius: 99, densityMax: 10, cooldown: 100),
                        new Spawn(children: "Bat Swarm 2", maxChildren: 1, initialSpawn: 0),
                        new StayCloseToSpawn(speed: 2, range: 15),
                        new Follow(speed: 1, acquireRange: 12, range: 1),
                        new TimedTransition(time: 10000, targetStates: "Regroup")
                    ),
                    new State("Regroup",
                        new ReturnToSpawn(speed: 1.6f),
                        new TimedTransition(time: 1000, targetStates: "Done")
                    ),
                    new State("Done",
                        new ReturnToSpawn(speed: 1.1f),
                        new TimedTransition(time: 1000, targetStates: "TurnBack")
                    ),
                    new State("TurnBack",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, false, 0),
                        new ChangeSize(rate: 100, target: 120),
                        new OrderOnce(range: 99, children: "Bat Swarm 1", targetState: "Die"),
                        new OrderOnce(range: 99, children: "Bat Swarm 2", targetState: "Die"),
                        new OrderOnce(range: 99, children: "Vampire Bat", targetState: "Die"),
                        new OrderOnce(range: 99, children: "Vampire Bat Swarmer 1", targetState: "Die"),
                        new OrderOnce(range: 99, children: "Vampire Bat Swarmer 2", targetState: "Die"),
                        new TimedTransition(time: 1000, targetStates: "Choose")
                    ),
                    new State("TurnIntoBats",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ChangeSize(rate: -100, target: 0),
                        new Reproduce(children: "Vampire Bat", densityRadius: 99, densityMax: 15, cooldown: 100),
                        new Follow(speed: 1, acquireRange: 12, range: 1),
                        new TimedTransition(time: 10000, targetStates: "TurnBack")
                    ),
                new Threshold(0.01f,
                    new TierLoot(6, TierLoot.LootType.Armor, 0.4f),
                    new ItemLoot("Holy Water", 1),
                    new ItemLoot("Holy Water", 0.3f),
                    new ItemLoot("Holy Water", 0.2f),
                    new ItemLoot("Holy Water", 0.1f),
                    new TierLoot(7, TierLoot.LootType.Weapon, 0.25f),
                    new TierLoot(8, TierLoot.LootType.Weapon, 0.25f),
                    new TierLoot(9, TierLoot.LootType.Weapon, 0.125f),
                    new TierLoot(7, TierLoot.LootType.Armor, 0.4f),
                    new TierLoot(4, TierLoot.LootType.Ability, 0.125f),
                    new TierLoot(4, TierLoot.LootType.Ring, 0.125f),
                    new TierLoot(10, TierLoot.LootType.Weapon, 0.0625f),
                    new TierLoot(5, TierLoot.LootType.Ability, 0.0625f),
                    new ItemLoot("Bone Dagger", 0.05f),
                    new ItemLoot("St. Abraham's Wand",0.01f),
                    new ItemLoot("Chasuble of Holy Light", 0.01f),
                    new ItemLoot("Ring of Divine Faith", 0.01f),
                    new ItemLoot("Potion of Attack", 0.3f, 3),
                    new ItemLoot("Tome of Purification", 0.01f)
                    )
            );
            db.Init("Coffin Creature",
                    new TransformOnDeath(target: "Lil Feratu", probability: 0.5f),
                    new TransformOnDeath(target: "Lil Feratu", probability: 0.5f),
                    new TransformOnDeath(target: "Lil Feratu", probability: 0.4f),
                    new TransformOnDeath(target: "Lil Feratu", probability: 0.3f),
                    new State("Ini",
                        new TimedTransition(time: 400, targetStates: "Opening")
                    ),
                    new State("Opening",
                        new TimedTransition(time: 600, targetStates: "Shoot")
                    ),
                    new State("Shoot",
                        new Shoot(range: 9, count: 1, index: 0, cooldown: 200),
                        new TimedTransition(time: 3000, targetStates: "Closing")
                    ),
                    new State("Closing",
                        new TimedTransition(600, "Ini", "Die")
                    ),
                    new State("Die",
                        new Suicide()
                    )
                );
            
            db.Init("Lil Feratu",
                new State("base",
                    new Shoot(range: 10, count: 5, shootAngle: 15, index: 0, predictive: 0.75f, cooldown: 1000),
                    new Prioritize(
                        new Follow(speed: 0.4f, acquireRange: 10, range: 2, duration: 2000, cooldown: 2600),
                        new StayBack(speed: 0.4f, distance: 2, entity: null),
                        new Charge(speed: 0.85f, range: 8, cooldown: 1000),
                        new Wander(speed: 0.4f)
                    )
                ));
            
            db.Init("Bat Swarm 1",
                    new State("Ini",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Follow(speed: 1, acquireRange: 12, range: 1),
                        new Reproduce(children: "Vampire Bat Swarmer 1", densityRadius: 99, densityMax: 10, cooldown: 100),
                        new EntitiesNotWithinTransition(99, "Die", "Lord Ruthven", "Dracula")
                    ),
                    new State("Die",
                        new Suicide()
                    )
                );

            db.Init("Bat Swarm 2",
                    new State("Ini",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Follow(speed: 1, acquireRange: 12, range: 1),
                        new Reproduce(children: "Vampire Bat Swarmer 2", densityRadius: 99, densityMax: 10, cooldown: 100),
                        new EntitiesNotWithinTransition(99, "Die", "Lord Ruthven", "Dracula")
                    ),
                    new State("Die",
                        new Suicide()
                    )
                );
            db.Init("Vampire Bat",
                    new State("Ini",
                        new Prioritize(
                            new Protect(speed: 1.8f, protectee: "Lord Ruthven", acquireRange: 7, protectionRange: 2, reprotectRange: 0.75f),
                            new Protect(speed: 1.8f, protectee: "Dracula", acquireRange: 7, protectionRange: 2, reprotectRange: 0.75f),
                            new Wander(speed: 0.8f)
                        ),
                        new Shoot(range: 24, count: 1, index: 0, cooldown: 100)
                    ),
                    new State("Die",
                        new Suicide()
                    )
                );
            db.Init("Vampire Bat Swarmer 1",
                    new State("Ini",
                        new Prioritize(
                            new Protect(speed: 1.8f, protectee: "Bat Swarm 1", acquireRange: 7, protectionRange: 2, reprotectRange: 0.75f),
                            new Wander(speed: 0.8f)
                        ),
                        new Shoot(range: 24, count: 1, index: 0, cooldown: 100),
                        new EntityNotWithinTransition(target: "Bat Swarm 1", radius: 99, targetStates: "Die")
                    ),
                    new State("Die",
                        new Suicide()
                    )
                );
            
            db.Init("Vampire Bat Swarmer 2",
                    new State("Ini",
                        new Prioritize(
                            new Protect(speed: 1.8f, protectee: "Bat Swarm 2", acquireRange: 7, protectionRange: 2, reprotectRange: 0.75f),
                            new Wander(speed: 0.8f)
                        ),
                        new Shoot(range: 24, count: 1, index: 0, cooldown: 100),
                        new EntityNotWithinTransition(target: "Bat Swarm 2", radius: 99, targetStates: "Die")
                    ),
                    new State("Die",
                        new Suicide()
                    )
                );


            db.Init("Hellhound",
                new State("base",
                    new Follow(1.25f, 8, 1, cooldown: 400),
                    new Shoot(10, count: 5, shootAngle: 7, cooldown: 2000)
                    ),
                new ItemLoot("Magic Potion", 0.05f),
                new Threshold(0.5f,
                    new ItemLoot("Timelock Orb", 0.01f)
                    )
            );
            db.Init("Lesser Bald Vampire",
                new State("base",
                    new Follow(0.35f, 8, 1),
                    new Shoot(10, count: 5, shootAngle: 6, cooldown: 1000)
                    ),
                new Threshold(0.5f,
                    new ItemLoot("Health Potion", 0.05f),
                    new ItemLoot("Steel Helm", 0.01f)
                    )
                );
                db.Init("Nosferatu",
                    new State("Base",
                        new Wander(0.25f),
                        new Shoot(10, count: 5, shootAngle: 2, index: 1, cooldown: 1000),
                        new Shoot(10, count: 6, shootAngle: 90, index: 0, cooldown: 1500)
                        ),
                    new Threshold(0.5f,
                        new ItemLoot("Health Potion", 0.05f),
                        new ItemLoot("Bone Dagger", 0.0f),
                        new ItemLoot("Wand of Death", 0.05f),
                        new ItemLoot("Golden Bow", 0.04f),
                        new ItemLoot("Steel Helm", 0.05f),
                        new ItemLoot("Ring of Paramount Defense", 0.09f)
                    )
                );
            db.Init("Armor Guard",
              new State("base",
                  new Wander(0.2f),
                  new TossObject("RockBomb", 7, cooldown: 3000),
                  new Shoot(10, count: 1, index: 0, predictive: 7, cooldown: 1000),
                  new Shoot(10, count: 1, index: 1, cooldown: 750)
                  ),
              new Threshold(0.5f,
                  new ItemLoot("Magic Potion", 0.05f),
                  new ItemLoot("Glass Sword", 0.01f),
                  new ItemLoot("Staff of Destruction", 0.01f),
                  new ItemLoot("Golden Shield", 0.01f),
                  new ItemLoot("Ring of Paramount Speed", 0.01f)
                  )
            );
            db.Init("Vampire Bat Swarmer",
                new State("base",
                    new Follow(1.5f, 8, 1),
                    new Shoot(10, count: 1, cooldown: 400)
                )
                );

            db.Init("RockBomb",
                new State("base",
                    new TimedTransition(1050, "boom")
                ),
                new State("boom",
                    new Shoot(8.4f, count: 1, fixedAngle: 0, index: 0, cooldown: 1000),
                    new Shoot(8.4f, count: 1, fixedAngle: 90, index: 0, cooldown: 1000),
                    new Shoot(8.4f, count: 1, fixedAngle: 180, index: 0, cooldown: 1000),
                    new Shoot(8.4f, count: 1, fixedAngle: 270, index: 0, cooldown: 1000),
                    new Shoot(8.4f, count: 1, fixedAngle: 45, index: 0, cooldown: 1000),
                    new Shoot(8.4f, count: 1, fixedAngle: 135, index: 0, cooldown: 1000),
                    new Shoot(8.4f, count: 1, fixedAngle: 235, index: 0, cooldown: 1000),
                    new Shoot(8.4f, count: 1, fixedAngle: 315, index: 0, cooldown: 1000),
                    new Suicide()
                )
                );
        }
    }
}