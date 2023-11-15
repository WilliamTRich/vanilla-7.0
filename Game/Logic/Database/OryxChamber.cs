using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public sealed class OryxChamber : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Oryx the Mad God 1",
                new DropPortalOnDeath("Locked Wine Cellar Portal", 100),
                new HealthTransition(.2f, "rage"),
                new State("Slow",
                    new Taunt("Fools! I still have {HP} hitpoints!"),
                    new Spawn("Minion of Oryx", 5, 0, 350000),
                    new Reproduce("Minion of Oryx", 10, 5, 1500),
                    new Shoot(25, 4, 10, 4, cooldown: 1000),
                    new TimedTransition(20000, "Dance 1")
                ),
                new State("Dance 1",
                    new Flash(0xf389E13, 0.5f, 60),
                    new Taunt("BE SILENT!!!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(50, 8, index: 6, cooldown: 700, cooldownOffset: 200),
                    new TossObject("Ring Element", 9, 24, 320000),
                    new TossObject("Ring Element", 9, 48, 320000),
                    new TossObject("Ring Element", 9, 72, 320000),
                    new TossObject("Ring Element", 9, 96, 320000),
                    new TossObject("Ring Element", 9, 120, 320000),
                    new TossObject("Ring Element", 9, 144, 320000),
                    new TossObject("Ring Element", 9, 168, 320000),
                    new TossObject("Ring Element", 9, 192, 320000),
                    new TossObject("Ring Element", 9, 216, 320000),
                    new TossObject("Ring Element", 9, 240, 320000),
                    new TossObject("Ring Element", 9, 264, 320000),
                    new TossObject("Ring Element", 9, 288, 320000),
                    new TossObject("Ring Element", 9, 312, 320000),
                    new TossObject("Ring Element", 9, 336, 320000),
                    new TossObject("Ring Element", 9, 360, 320000),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    //new Grenade(radius: 4, damage: 150, fixedAngle: new Random().Next(0, 359), range: 5, cooldown: 2000),
                    //new Grenade(radius: 4, damage: 150, fixedAngle: new Random().Next(0, 359), range: 5, cooldown: 2000),
                    //new Grenade(radius: 4, damage: 150, fixedAngle: new Random().Next(0, 359), range: 5, cooldown: 2000),
                    new TimedTransition(8000, "artifacts")
                ),
                new State("artifacts",
                    new Taunt("My Artifacts will protect me!"),
                    new Flash(0xf389E13, 0.5f, 60),
                    new Shoot(50, 3, index: 9, cooldown: 1500, cooldownOffset: 200),
                    new Shoot(50, 10, index: 8, cooldown: 2000, cooldownOffset: 200),
                    new Shoot(50, 10, index: 7, cooldown: 500, cooldownOffset: 200),
                    //Inner Elements
                    new TossObject("Guardian Element 1", 1, 0, 90000001, 1000),
                    new TossObject("Guardian Element 1", 1, 90, 90000001, 1000),
                    new TossObject("Guardian Element 1", 1, 180, 90000001, 1000),
                    new TossObject("Guardian Element 1", 1, 270, 90000001, 1000),
                    new TossObject("Guardian Element 2", 9, 0, 90000001, 1000),
                    new TossObject("Guardian Element 2", 9, 90, 90000001, 1000),
                    new TossObject("Guardian Element 2", 9, 180, 90000001, 1000),
                    new TossObject("Guardian Element 2", 9, 270, 90000001, 1000),
                    new TimedTransition(25000, "gaze")
                ),
                new State("gaze",
                    new Taunt("All who looks upon my face shall die."),
                    new Shoot(count: 2, cooldown: 1000, index: 1, range: 7, shootAngle: 10,
                        cooldownOffset: 800),
                    new TimedTransition(10000, "Dance 2")

                ),
                new State("Dance 2",
                    new Flash(0xf389E13, 0.5f, 60),
                    new Taunt("Time for more dancing!"),
                    new Shoot(50, 8, index: 6, cooldown: 700, cooldownOffset: 200),
                    new TossObject("Ring Element", 9, 24, 320000),
                    new TossObject("Ring Element", 9, 48, 320000),
                    new TossObject("Ring Element", 9, 72, 320000),
                    new TossObject("Ring Element", 9, 96, 320000),
                    new TossObject("Ring Element", 9, 120, 320000),
                    new TossObject("Ring Element", 9, 144, 320000),
                    new TossObject("Ring Element", 9, 168, 320000),
                    new TossObject("Ring Element", 9, 192, 320000),
                    new TossObject("Ring Element", 9, 216, 320000),
                    new TossObject("Ring Element", 9, 240, 320000),
                    new TossObject("Ring Element", 9, 264, 320000),
                    new TossObject("Ring Element", 9, 288, 320000),
                    new TossObject("Ring Element", 9, 312, 320000),
                    new TossObject("Ring Element", 9, 336, 320000),
                    new TossObject("Ring Element", 9, 360, 320000),
                    new TimedTransition(1000, "Dance2, 1")
                ),
                new State("Dance2, 1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(0, index: 8, count: 4, shootAngle: 90, fixedAngle: 0, cooldown: 170),
                    new TimedTransition(200, "Dance2, 2")
                ),
                new State("Dance2, 2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(0, index: 8, count: 4, shootAngle: 90, fixedAngle: 30, cooldown: 170),
                    new TimedTransition(200, "Dance2, 3")
                ),
                new State("Dance2, 3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(0, index: 8, count: 4, shootAngle: 90, fixedAngle: 15, cooldown: 170),
                    new TimedTransition(200, "Dance2, 4")
                ),
                new State("Dance2, 4",
                    new Shoot(0, index: 8, count: 4, shootAngle: 90, fixedAngle: 45, cooldown: 170),
                    new TimedTransition(200, "Dance2, 1")
                ),
                new State("rage",
                    new ChangeSize(10, 200),
                    new Taunt(.3f, "I HAVE HAD ENOUGH OF YOU!!!",
                        "ENOUGH!!!",
                        "DIE!!!"),
                    new Spawn("Minion of Oryx", 10, 0, 350000),
                    new Reproduce("Minion of Oryx", 10, 5, 1500),
                    new Shoot(count: 2, cooldown: 1500, index: 1, range: 7, shootAngle: 10,
                        cooldownOffset: 2000),
                    new Shoot(count: 5, cooldown: 1500, index: 16, range: 7, shootAngle: 10,
                        cooldownOffset: 2000),
                    new Follow(0.85f, range: 1, cooldown: 0),
                    new Flash(0xfFF0000, 0.5f, 9000001)
                ),
                new Threshold(0.05f,
                    new ItemLoot("Potion of Attack", 0.3f),
                    new ItemLoot("Potion of Defense", 0.3f)
                ),
                new Threshold(0.1f,
                    new TierLoot(10, TierLoot.LootType.Weapon, 0.07f),
                    new TierLoot(11, TierLoot.LootType.Weapon, 0.06f),
                    new TierLoot(5, TierLoot.LootType.Ability, 0.07f),
                    new TierLoot(11, TierLoot.LootType.Armor, 0.07f),
                    new TierLoot(5, TierLoot.LootType.Ring, 0.06f)
                )
            );
            db.Init("Ring Element",
                new State("norm",
                    new Shoot(50, 12, index: 0, cooldown: 700, cooldownOffset: 200),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TimedTransition(20000, "Despawn")
                ),
                new State("Despawn", //new Decay(time:0)
                    new Suicide()
                )
            );
            db.Init("Minion of Oryx",
                new Wander(0.4f),
                new Shoot(3, 3, 10),
                new Shoot(3, 3, index: 1, shootAngle: 10, cooldown: 1000),
                new TierLoot(7, TierLoot.LootType.Weapon, 0.2f),
                new ItemLoot("Magic Potion", 0.03f)
            );
            db.Init("Guardian Element 1",
                new State("norm",
                    new Orbit(1, 1, target: "Oryx the Mad God 1", radiusVariance: 0),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(25, 3, 10),
                    new TimedTransition(10000, "Grow")
                ),
                new State("Grow",
                    new ChangeSize(100, 200),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Orbit(1, 1, target: "Oryx the Mad God 1", radiusVariance: 0),
                    new Shoot(3, 1, 10, 0, cooldown: 700),
                    new TimedTransition(10000, "Despawn")
                ),
                new State("Despawn",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Orbit(1, 1, target: "Oryx the Mad God 1", radiusVariance: 0),
                    new ChangeSize(100, 100),
                    new Suicide()
                )
            );
            db.Init("Guardian Element 2",
                new State("norm",
                    new Orbit(1.3f, 9, target: "Oryx the Mad God 1", radiusVariance: 0),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(25, 3, 10),
                    new TimedTransition(20000, "Despawn")
                ),
                new State("Despawn", new Suicide()
                )
            );
        }
    }
}