using Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database;

public sealed class Abyss : IBehaviorDatabase
{
    public void Init(BehaviorDb db)
    {
        db.Init("Malphas Protector",
            new Shoot(5, 3, 5, 0, predictive: 0.45f,
                cooldown: 1200),
            new Orbit(3.2f, 9, 20, "Archdemon Malphas", 0,
                0, true),
            new Threshold(0.01f,
                new ItemLoot("Magic Potion", 0.06f),
                new ItemLoot("Health Potion", 0.04f)
            )
        );
        db.Init("Brute Warrior of the Abyss",
            new Prioritize(
                new Follow(1, 8, 1),
                new Wander(0.25f)
            ),
            new Shoot(8, 3, 10, cooldown: 500),
            new ItemLoot("Spirit Salve Tome", 0.02f),
            new Threshold(0.5f,
                new ItemLoot("Glass Sword", 0.01f),
                new ItemLoot("Ring of Greater Dexterity", 0.01f),
                new ItemLoot("Magesteel Quiver", 0.01f)
            )
        );
        db.Init("Imp of the Abyss",
            new Wander(0.45f),
            new Shoot(8, 5, 10, cooldown: 1000),
            new ItemLoot("Health Potion", 0.1f),
            new ItemLoot("Magic Potion", 0.1f),
            new Threshold(0.5f,
                new ItemLoot("Cloak of the Red Agent", 0.01f)
            )
        );

        db.Init("White Demon of the Abyss",
            new Prioritize(
                new StayAbove(1, 200),
                new Follow(1, range: 7),
                new Wander(0.4f)
            ),
            new Shoot(10, 3, 20, predictive: 1, cooldown: 500),
            new Reproduce(densityMax: 2),
            new Threshold(.01f,
                new TierLoot(6, TierLoot.LootType.Weapon, 0.1f),
                new TierLoot(7, TierLoot.LootType.Weapon, 0.1f),
                new TierLoot(8, TierLoot.LootType.Weapon, 0.1f),
                new TierLoot(7, TierLoot.LootType.Armor, 0.1f),
                new TierLoot(8, TierLoot.LootType.Armor, 0.1f),
                new TierLoot(9, TierLoot.LootType.Armor, 0.1f),
                new TierLoot(3, TierLoot.LootType.Ring, 0.1f),
                new TierLoot(4, TierLoot.LootType.Ring, 0.1f)
            ),
            new Threshold(0.07f,
                new ItemLoot("Potion of Vitality", 0.4f)
            )
        );
        db.Init("Demon of the Abyss",
            new Prioritize(
                new Follow(1, 8, 5),
                new Wander(0.25f)
            ),
            new Shoot(8, 3, 10, cooldown: 1000),
            new ItemLoot("Fire Bow", 0.05f),
            new Threshold(0.5f,
                new ItemLoot("Mithril Armor", 0.01f)
            )
        );
        db.Init("Demon Warrior of the Abyss",
            new Prioritize(
                new Follow(1, 8, 5),
                new Wander(0.25f)
            ),
            new Shoot(8, 3, 10, cooldown: 1000),
            new ItemLoot("Fire Sword", 0.025f),
            new ItemLoot("Steel Shield", 0.025f)
        );
        db.Init("Demon Mage of the Abyss",
            new Prioritize(
                new Follow(1, 8, 5),
                new Wander(0.25f)
            ),
            new Shoot(8, 3, 10, cooldown: 1000),
            new ItemLoot("Fire Nova Spell", 0.02f),
            new Threshold(0.1f,
                new ItemLoot("Wand of Dark Magic", 0.01f),
                new ItemLoot("Avenger Staff", 0.01f),
                new ItemLoot("Robe of the Invoker", 0.01f),
                new ItemLoot("Essence Tap Skull", 0.01f),
                new ItemLoot("Demonhunter Trap", 0.01f)
            )
        );
        db.Init("Brute of the Abyss",
            new Prioritize(
                new Follow(1.5f, 8, 1),
                new Wander(0.25f)
            ),
            new Shoot(8, 3, 10, cooldown: 500),
            new ItemLoot("Health Potion", 0.1f),
            new Threshold(0.1f,
                new ItemLoot("Obsidian Dagger", 0.02f),
                new ItemLoot("Steel Helm", 0.02f)
            )
        );
        db.Init("Malphas Missile",
            new State("Start",
                new TimedTransition(50, "Attacking")
            ),
            new State("Attacking",
                new Follow(1.1f, 10, 0.2f),
                new PlayerWithinTransition(1.3f, targetStates: "FlashBeforeExplode"),
                new TimedTransition(5000, "FlashBeforeExplode")
            ),
            new State("FlashBeforeExplode",
                new Flash(0xFFFFFF, 0.1f, 6),
                new TimedTransition(600, "Explode")
            ),
            new State("Explode",
                new Shoot(0, 8, 45, 0, 0),
                new Suicide()
            )
        );
        db.Init("Archdemon Malphas",
            new State("start_the_fun",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new PlayerWithinTransition(11, targetStates: "he_is_never_alone", seeInvis: true)
            ),
            new State("he_is_never_alone",
                new State("Missile_Fire",
                    new Prioritize(
                        new StayCloseToSpawn(0.1f, 1),
                        new Shoot(10, 2, 20, predictive: 0.7f, cooldown: 300),
                        new Shoot(8, 1, index: 0, predictive: 0.2f, cooldown: 900),
                        new Shoot(8, 1, index: 0, predictive: 0.2f, cooldown: 900),
                        new Shoot(8, 1, index: 0, predictive: 0.2f, cooldown: 900),
                        new Shoot(8, 1, index: 0, predictive: 0.2f, cooldown: 900),
                        new Follow(0.3f, 8, 2)
                    ),
                    new Shoot(8, 1, index: 0, angleOffset: 1, predictive: 0.15f,
                        cooldown: 900),
                    new Reproduce("Malphas Missile", 24, 4,
                        1800),
                    new State("invulnerable1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(2000, "vulnerable")
                    ),
                    new State("vulnerable",
                        new TimedTransition(4000, "invulnerable2")
                    ),
                    new State("invulnerable2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable)
                    ),
                    new TimedTransition(9000, "Pause1")
                ),
                new State("Pause1",
                    new Prioritize(
                        new StayCloseToSpawn(0.4f),
                        new Wander(0.4f)
                    ),
                    new TimedTransition(2500, "Small_target")
                ),
                new State("Small_target",
                    new Prioritize(
                        new StayCloseToSpawn(0.8f),
                        new Wander(0.5f),
                        new Shoot(12, index: 2, count: 2, shootAngle: 72, predictive: 0.5f,
                            cooldown: 550)
                    ),
                    new Shoot(0, 3, 60, 3, 0,
                        cooldown: 1200),
                    new Shoot(8, 1, angleOffset: 0.6f, predictive: 0.15f, cooldown: 900),
                    new TimedTransition(12000, "Size_matters")
                ),
                new State("Size_matters",
                    new Prioritize(
                        new StayCloseToSpawn(0.2f),
                        new Wander(0.2f)
                    ),
                    new State("Growbig",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(1800, "Shot_rotation1")
                    ),
                    new State("Shot_rotation1",
                        new Shoot(8, 1, index: 2, predictive: 0.2f, cooldown: 900),
                        new Shoot(0, 2, 120, 3, angleOffset: 0.7f,
                            defaultAngle: 0, cooldown: 700),
                        new TimedTransition(1400, "Shot_rotation2")
                    ),
                    new State("Shot_rotation2",
                        new Shoot(8, 2, index: 2, predictive: 0.2f, cooldown: 900),
                        new Shoot(8, 2, index: 2, predictive: 0.25f, cooldown: 2000),
                        new Shoot(0, 2, 120, 3, angleOffset: 0.7f,
                            defaultAngle: 40, cooldown: 700),
                        new TimedTransition(1400, "Shot_rotation3")
                    ),
                    new State("Shot_rotation3",
                        new Shoot(8, 1, index: 2, predictive: 0.2f, cooldown: 900),
                        new Shoot(2, 3, 120, 3, angleOffset: 0.7f,
                            defaultAngle: 80, cooldown: 700),
                        new TimedTransition(1400, "Shot_rotation1")
                    ),
                    new TimedTransition(13000, "Pause2")
                ),
                new State("Pause2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Prioritize(
                        new StayCloseToSpawn(0.4f),
                        new Wander(0.4f)
                    ),
                    new TimedTransition(2500, "Bring_on_the_flamers")
                ),
                new State("Bring_on_the_flamers",
                    new Prioritize(
                        new StayCloseToSpawn(0.4f),
                        new Follow(0.4f, 9, 2)
                    ),
                    new Shoot(8, 1, predictive: 0.25f, cooldown: 2100),
                    new Shoot(10, 5, 20, predictive: 1, cooldown: 700, cooldownOffset: 300),
                    new Shoot(8, 1, index: 0, predictive: 0.2f, cooldown: 900, cooldownOffset: 300),
                    new Shoot(10, 5, 20, predictive: 1, cooldown: 300, cooldownOffset: 600),
                    new Shoot(8, 1, index: 0, predictive: 0.2f, cooldown: 900, cooldownOffset: 600),
                    new Shoot(10, 5, 20, predictive: 1, cooldown: 300, cooldownOffset: 900),
                    new Shoot(8, 1, index: 0, predictive: 0.2f, cooldown: 900, cooldownOffset: 900),
                    new TimedTransition(8000, "Temporary_exhaustion")
                ),
                new State("Temporary_exhaustion",
                    new Flash(0x484848, 0.6f, 5),
                    new StayBack(0.4f, 4),
                    new TimedTransition(3200, "Missile_Fire")
                )
            ),
            new DropPortalOnDeath("Glowing Realm Portal", 1f),
            new Threshold(0.01f,
                new TierLoot(7, TierLoot.LootType.Weapon, 0.2f),
                new TierLoot(9, TierLoot.LootType.Weapon, 0.11f),
                new TierLoot(10, TierLoot.LootType.Weapon, 0.05f),
                new TierLoot(7, TierLoot.LootType.Armor, 0.2f),
                new TierLoot(9, TierLoot.LootType.Armor, 0.11f),
                new TierLoot(10, TierLoot.LootType.Armor, 0.05f),
                new TierLoot(3, TierLoot.LootType.Ability, 0.1f),
                new TierLoot(4, TierLoot.LootType.Ability, 0.06f),
                new TierLoot(3, TierLoot.LootType.Ring, 0.1f),
                new TierLoot(4, TierLoot.LootType.Ring, 0.06f),
                new ItemLoot("Potion of Vitality", .7f),
                new ItemLoot("Potion of Defense", .7f),
                new ItemLoot("Demon Blade", 0.1f)

            )
        );
        db.Init("Malphas Flamer",
            new State("Attacking",
                new State("Charge",
                    new Prioritize(
                        new Follow(0.7f, 10, 0.1f)
                    ),
                    new PlayerWithinTransition(2, targetStates: "Bullet1", seeInvis: true)
                ),
                new State("Bullet1",
                    new Flash(0xFFAA00, 0.2f, 20),
                    new Shoot(8, cooldown: 200),
                    new TimedTransition(4000, "Wait1")
                ),
                new State("Wait1",
                    new Charge(3, 20, 600)
                ),
                new HealthTransition(0.2f, "FlashBeforeExplode")
            ),
            new State("FlashBeforeExplode",
                new Flash(0xFF0000, 0.75f, 1),
                new TimedTransition(300, "Explode")
            ),
            new State("Explode",
                new Shoot(0, 8, 45, defaultAngle: 0),
                new Decay(100)
            ),
            new Threshold(0.01f,
                new ItemLoot("Health Potion", 0.1f),
                new ItemLoot("Magic Potion", 0.1f)
            )
        );
    }
}