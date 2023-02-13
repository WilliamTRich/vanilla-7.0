using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public sealed class WineCellar : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Oryx the Mad God 2",
                new State("Attack",
                    new Wander(.05f),
                    new Shoot(25, index: 0, count: 8, shootAngle: 45, cooldown: 1500,
                        cooldownOffset: 1500),
                    new Shoot(25, index: 1, count: 3, shootAngle: 10, cooldown: 1000,
                        cooldownOffset: 1000),
                    new Shoot(25, index: 2, count: 3, shootAngle: 10, predictive: 0.2f, cooldown: 1000,
                        cooldownOffset: 1000),
                    new Shoot(25, index: 3, count: 2, shootAngle: 10, predictive: 0.4f, cooldown: 1000,
                        cooldownOffset: 1000),
                    new Shoot(25, index: 4, count: 3, shootAngle: 10, predictive: 0.6f, cooldown: 1000,
                        cooldownOffset: 1000),
                    new Shoot(25, index: 5, count: 2, shootAngle: 10, predictive: 0.8f, cooldown: 1000,
                        cooldownOffset: 1000),
                    new Shoot(25, index: 6, count: 3, shootAngle: 10, predictive: 1, cooldown: 1000,
                        cooldownOffset: 1000),
                    new Taunt(1f, 6000, "Puny mortals! My {HP} HP will annihilate you!"),
                    new Spawn("Henchman of Oryx", 5, 0.5f, 5000),
                    new HealthTransition(.2f, "prepareRage")
                ),
                new State("prepareRage",
                    new Follow(.1f, 15, 3),
                    new Taunt("Can't... keep... henchmen... alive... anymore! ARGHHH!!!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(25, 30, fixedAngle: 0, index: 13, cooldown: 4000, cooldownOffset: 4000),
                    new Shoot(25, 30, fixedAngle: 30, index: 14, cooldown: 4000, cooldownOffset: 4000),
                    new TimedTransition(10000, "rage")
                ),
                new State("rage",
                    new Follow(.1f, 15, 3),
                    new Shoot(25, 30, index: 7, cooldown: 90000001, cooldownOffset: 8000),
                    new Shoot(25, 30, index: 8, cooldown: 90000001, cooldownOffset: 8500),
                    new Shoot(25, index: 0, count: 8, shootAngle: 45, cooldown: 1500,
                        cooldownOffset: 1500),
                    new Shoot(25, index: 1, count: 3, shootAngle: 10, cooldown: 1000,
                        cooldownOffset: 1000),
                    new Shoot(25, index: 2, count: 3, shootAngle: 10, predictive: 0.2f, cooldown: 1000,
                        cooldownOffset: 1000),
                    new Shoot(25, index: 3, count: 2, shootAngle: 10, predictive: 0.4f, cooldown: 1000,
                        cooldownOffset: 1000),
                    new Shoot(25, index: 4, count: 3, shootAngle: 10, predictive: 0.6f, cooldown: 1000,
                        cooldownOffset: 1000),
                    new Shoot(25, index: 5, count: 2, shootAngle: 10, predictive: 0.8f, cooldown: 1000,
                        cooldownOffset: 1000),
                    new Shoot(25, index: 6, count: 3, shootAngle: 10, predictive: 1, cooldown: 1000,
                        cooldownOffset: 1000),
                    new TossObject("Monstrosity Scarab", 7, 0),
                    new Taunt(1f, 6000, "Puny mortals! My {HP} HP will annihilate you!")
                ),
                new Threshold(0.29f,
                    new ItemLoot("Potion of Vitality", 1)
                ),
                new Threshold(0.05f,
                    new ItemLoot("Potion of Attack", 1),
                    new ItemLoot("Potion of Defense", 1),
                    new ItemLoot("Potion of Wisdom", 1),
                    new ItemLoot("Potion of Vitality", 1),
                    new ItemLoot("Potion of Dexterity", 1),
                    new ItemLoot("Potion of Speed", 1),
                    new ItemLoot("Potion of Life", .1f),
                    new ItemLoot("Potion of Mana", .1f)
                ),
                new Threshold(0.01f,
                    new TierLoot(14, TierLoot.LootType.Weapon, 0.2f),
                    new TierLoot(13, TierLoot.LootType.Weapon, 0.4f),
                    new TierLoot(6, TierLoot.LootType.Ability, 0.2f),
                    new TierLoot(14, TierLoot.LootType.Armor, 0.2f),
                    new TierLoot(14, TierLoot.LootType.Armor, 0.4f),
                    new TierLoot(6, TierLoot.LootType.Ring, 0.07f)
                )
            );

            db.Init("Henchman of Oryx",
                new State("Attack",
                    new Prioritize(
                        new Orbit(.2f, 2, target: "Oryx the Mad God 2", radiusVariance: 1),
                        new Wander(.3f)
                    ),
                    new Shoot(15, predictive: 1, cooldown: 2500),
                    new Shoot(10, 3, 10, 1, cooldown: 2500),
                    new Spawn("Vintner of Oryx", 1, 1, 5000),
                    //  new Spawn("Bile of Oryx", maxChildren: 1, initialSpawn: 1, cooldown: 5000),
                    new Spawn("Aberrant of Oryx", 1, 1, 5000),
                    new Spawn("Monstrosity of Oryx", 1, 1, 5000),
                    new Spawn("Abomination of Oryx", 1, 1, 5000)
                ),
                new State("Suicide",
                    new Decay(0)
                )
            );
            db.Init("Monstrosity of Oryx",
                new State("Wait",
                    new PlayerWithinTransition(15, false, "Attack")
                ),
                new State("Attack",
                    new TimedTransition(10000, "Wait"),
                    new Prioritize(
                        new Orbit(.1f, 6, target: "Oryx the Mad God 2", radiusVariance: 3),
                        new Follow(.1f, 15),
                        new Wander(.2f)
                    ),
                    new TossObject("Monstrosity Scarab", cooldown: 10000, range: 1, angle: 0, cooldownOffset: 1000)
                )
            );
            db.Init("Monstrosity Scarab",
                new State("Attack",
                    new State("Charge",
                        new Prioritize(
                            new Charge(range: 25, cooldown: 1000),
                            new Wander(.3f)
                        ),
                        new PlayerWithinTransition(1, false, "Boom")
                    ),
                    new State("Boom",
                        new Shoot(1, 16, 360 / 16, fixedAngle: 0),
                        new Decay(0)
                    )
                )
            );
            db.Init("Vintner of Oryx",
                new State("Attack",
                    new Prioritize(
                        new Protect(1, "Oryx the Mad God 2", protectionRange: 4, reprotectRange: 3),
                        new Charge(1, 15),
                        new Protect(1, "Henchman of Oryx"),
                        new StayBack(1, 15),
                        new Wander(.6f)
                    ),
                    new Shoot(10, cooldown: 250)
                )
            );
            db.Init("Aberrant of Oryx",
                new Prioritize(
                    new Protect(.2f, "Oryx the Mad God 2"),
                    new Wander(.6f)
                ),
                new State("Wait",
                    new PlayerWithinTransition(15, false, "Attack")
                ),
                new State("Attack",
                    new TimedTransition(10000, "Wait"),
                    new State("Randomize",
                        new TimedTransition(100, "Toss1"),
                        new TimedTransition(100, "Toss2"),
                        new TimedTransition(100, "Toss3"),
                        new TimedTransition(100, "Toss4"),
                        new TimedTransition(100, "Toss5"),
                        new TimedTransition(100, "Toss6"),
                        new TimedTransition(100, "Toss7"),
                        new TimedTransition(100, "Toss8")
                    ),
                    new State("Toss1",
                        new TossObject("Aberrant Blaster", cooldown: 40000, range: 5, angle: 0),
                        new TimedTransition(4900, "Randomize")
                    ),
                    new State("Toss2",
                        new TossObject("Aberrant Blaster", cooldown: 40000, range: 5, angle: 45),
                        new TimedTransition(4900, "Randomize")
                    ),
                    new State("Toss3",
                        new TossObject("Aberrant Blaster", cooldown: 40000, range: 5, angle: 90),
                        new TimedTransition(4900, "Randomize")
                    ),
                    new State("Toss4",
                        new TossObject("Aberrant Blaster", cooldown: 40000, range: 5, angle: 135),
                        new TimedTransition(4900, "Randomize")
                    ),
                    new State("Toss5",
                        new TossObject("Aberrant Blaster", cooldown: 40000, range: 5, angle: 180),
                        new TimedTransition(4900, "Randomize")
                    ),
                    new State("Toss6",
                        new TossObject("Aberrant Blaster", cooldown: 40000, range: 5, angle: 225),
                        new TimedTransition(4900, "Randomize")
                    ),
                    new State("Toss7",
                        new TossObject("Aberrant Blaster", cooldown: 40000, range: 5, angle: 270),
                        new TimedTransition(4900, "Randomize")
                    ),
                    new State("Toss8",
                        new TossObject("Aberrant Blaster", cooldown: 40000, range: 5, angle: 315),
                        new TimedTransition(4900, "Randomize")
                    )
                )
            );
            db.Init("Aberrant Blaster",
                new State("Wait",
                    new PlayerWithinTransition(5, false, "Boom")
                ),
                new State("Boom",
                    new Shoot(10, 5, 7),
                    new Decay(0)
                )
            );
            db.Init("Bile of Oryx",
                new Prioritize(
                    new Protect(.4f, "Oryx the Mad God 2", protectionRange: 5, reprotectRange: 4),
                    new Wander(.5f)
                ) //,
                //new Spawn("Purple Goo", maxChildren: 20, initialSpawn: 0, cooldown: 1000)
            );
            db.Init("Abomination of Oryx",
                new State("Shoot",
                    new Shoot(3, 3, 5),
                    new Shoot(3, 5, 5, 1),
                    new Shoot(3, 7, 5, 2),
                    new Shoot(3, 5, 5, 3),
                    new Shoot(3, 3, 5, 4),
                    new TimedTransition(1000, "Wait")
                ),
                new State("Wait",
                    new PlayerWithinTransition(2, false, "Shoot")),
                new Prioritize(
                    new Charge(3, 10, 3000),
                    new Wander(.5f))
            );
        }
    }
}