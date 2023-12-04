using Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Conditionals;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database;

public sealed class SpriteWorld : IBehaviorDatabase
{
    public void Init(BehaviorDb db)
    {
        db.Init("Native Fire Sprite",
            new StayAbove(0.4f, 95),
            new Shoot(10, 2, 7, 0, cooldown: 300),
            new Wander(.4f),
            new Threshold(0.01f,
                new TierLoot(5, TierLoot.LootType.Weapon, 0.02f),
                new ItemLoot("Magic Potion", 0.05f)
            )
        );
        db.Init("Native Ice Sprite",
            new StayAbove(0.4f, 105),
            new Shoot(10, 3, 7),
            new Wander(.4f),
            new Threshold(0.01f,
                new TierLoot(2, TierLoot.LootType.Ability, 0.04f),
                new ItemLoot("Magic Potion", 0.05f)
            )
        );
        db.Init("Native Magic Sprite",
            new StayAbove(0.4f, 115),
            new Shoot(10, 4, 7),
            new Wander(.4f),
            new Threshold(0.01f,
                new TierLoot(6, TierLoot.LootType.Armor, 0.01f),
                new ItemLoot("Magic Potion", 0.05f)
            )
        );
        db.Init("Native Nature Sprite",
            new Shoot(10, 5, 7),
            new Wander(.6f),
            new Threshold(0.01f,
                new ItemLoot("Magic Potion", 0.015f),
                new ItemLoot("Sprite Wand", 0.015f),
                new ItemLoot("Ring of Greater Magic", 0.01f)
            )
        );
        db.Init("Native Darkness Sprite",
            new Shoot(10, 5, 7),
            new Wander(.6f),
            new Threshold(0.01f,
                new ItemLoot("Health Potion", 0.015f),
                new ItemLoot("Ring of Dexterity", 0.01f)
            )
        );
        db.Init("Native Sprite God",
            new StayAbove(0.4f, 200),
            new Shoot(12, 4, 10),
            new Shoot(12, index: 1, predictive: 1, cooldown: 1000),
            new Wander(0.4f),
            new Threshold(0.01f,
                new TierLoot(6, TierLoot.LootType.Weapon, 0.02f),
                new TierLoot(7, TierLoot.LootType.Weapon, 0.01f),
                new TierLoot(8, TierLoot.LootType.Weapon, 0.005f),
                new TierLoot(7, TierLoot.LootType.Armor, 0.02f),
                new TierLoot(8, TierLoot.LootType.Armor, 0.01f),
                new TierLoot(9, TierLoot.LootType.Armor, 0.005f),
                new TierLoot(4, TierLoot.LootType.Ring, 0.01f),
                new TierLoot(4, TierLoot.LootType.Ability, 0.01f),
                new ItemLoot("Potion of Attack", 0.02f)
            )
        );
        db.Init("Limon the Sprite God",   
            new DropPortalOnDeath("Glowing Realm Portal"),
            new State("start_the_fun",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new PlayerWithinTransition(11, targetStates: "begin_teleport1", seeInvis: true)
            ),
            new State("begin_teleport1",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Prioritize(
                    new StayCloseToSpawn(0.5f, 7),
                    new Wander(0.5f)
                ),
                new Flash(0x00FF00, 0.25f, 8),
                new TimedTransition(2000, "teleport1")
            ),
            new State("teleport1",
                new Prioritize(
                    new StayCloseToSpawn(1.6f, 7),
                    new Follow(6, 10, 2),
                    new Follow(0.3f, 10, 0.2f)
                ),
                new TimedTransition(300, "circle_player")
            ),
            new State("circle_player",
                new Shoot(8, 2, 10, 0, angleOffset: 0.7f,
                    predictive: 0.4f, cooldown: 400),
                new Shoot(8, 2, 180, 0, angleOffset: 0.7f,
                    predictive: 0.4f, cooldown: 400),
                new Prioritize(
                    new StayCloseToSpawn(1.3f, 7),
                    new Orbit(1.8f, 4, 5),
                    new Follow(6, 10, 2),
                    new Follow(0.3f, 10, 0.2f)
                ),
                new IfConditionEffect(ConditionEffectIndex.Paralyzed,
                    new Shoot(8, 18, 20, 0, angleOffset: 0.4f,
                        predictive: 0.4f, cooldown: 1500)
                ),
                new TimedTransition(10000, "set_up_the_box")
            ),
            new State("set_up_the_box",
                new TossObject("Limon Element 1", 9.5f, 315, 1000000),
                new TossObject("Limon Element 2", 9.5f, 225, 1000000),
                new TossObject("Limon Element 3", 9.5f, 135, 1000000),
                new TossObject("Limon Element 4", 9.5f, 45, 1000000),
                new TossObject("Limon Element 1", 14, 315, 1000000),
                new TossObject("Limon Element 2", 14, 225, 1000000),
                new TossObject("Limon Element 3", 14, 135, 1000000),
                new TossObject("Limon Element 4", 14, 45, 1000000),
                new State("shielded1",
                    new Shoot(8, 1, predictive: 0.1f, cooldown: 1000),
                    new Shoot(8, 3, 120, angleOffset: 0.3f, predictive: 0.1f,
                        cooldown: 500),
                    new TimedTransition(1500, "shielded2")
                ),
                new State("shielded2",
                    new Shoot(8, 3, 120, angleOffset: 0.3f, predictive: 0.2f,
                        cooldown: 800),
                    new TimedTransition(3500, "shielded2")
                ),
                new TimedTransition(20000, "Summon_the_sprites")
            ),
            new State("Summon_the_sprites",
                new StayCloseToSpawn(0.5f, 8),
                new Wander(0.5f),
                new ConditionalEffect(ConditionEffectIndex.Armored),
                new Shoot(8, 3, 15, cooldown: 1300),
                new TimedTransition(11000, "begin_teleport1"),
                new HealthTransition(0.2f, "begin_teleport1")
            ),
            new GroundTransformOnDeath("White Alpha Square", 1),
            new DropPortalOnDeath("Glowing Realm Portal", 0.00001f),
            new Threshold(0.01f,
                new TierLoot(7, TierLoot.LootType.Armor, 0.15f),
                new TierLoot(3, TierLoot.LootType.Ability, 0.11f),
                new TierLoot(4, TierLoot.LootType.Ability, 0.124f),
                new TierLoot(5, TierLoot.LootType.Ability, 0.11f),
                new TierLoot(3, TierLoot.LootType.Ring, 0.11f),
                new ItemLoot("Potion of Dexterity", 1),
                new ItemLoot("Potion of Defense", 1),
                new ItemLoot("Sprite Wand", 0.01f)
            ),
            new Threshold(0.02f,
                new ItemLoot("Staff of Extreme Prejudice", 0.1f),
                new ItemLoot("Cloak of the Planewalker", 0.1f)
            )

        );
        
        db.Init("Limon Element 1",
            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
            new EntityNotWithinTransition("Limon the Sprite God", 999, "Suicide"),
            new State("Setup",
                new TimedTransition(2000, "Attacking1")
            ),
            new State("Attacking1",
                new Shoot(999, fixedAngle: 180, defaultAngle: 180, cooldown: 300),
                new Shoot(999, fixedAngle: 90, defaultAngle: 90, cooldown: 300),
                new TimedTransition(6000, "Attacking2")
            ),
            new State("Attacking2",
                new Shoot(999, fixedAngle: 180, defaultAngle: 180, cooldown: 300),
                new Shoot(999, fixedAngle: 90, defaultAngle: 90, cooldown: 300),
                new Shoot(999, fixedAngle: 135, defaultAngle: 135, cooldown: 300),
                new TimedTransition(6000, "Attacking3")
            ),
            new State("Attacking3",
                new Shoot(999, fixedAngle: 180, defaultAngle: 180, cooldown: 300),
                new Shoot(999, fixedAngle: 90, defaultAngle: 90, cooldown: 300),
                new TimedTransition(6000, "Setup")
            ),
            new State("Suicide",
                new Suicide()
            ),
            new Decay(20000)
        );
        db.Init("Limon Element 2",
            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
            new EntityNotWithinTransition("Limon the Sprite God", 999, "Suicide"),
            new State("Setup",
                new TimedTransition(2000, "Attacking1")
            ),
            new State("Attacking1",
                new Shoot(999, fixedAngle: 90, defaultAngle: 90, cooldown: 300),
                new Shoot(999, fixedAngle: 0, defaultAngle: 0, cooldown: 300),
                new TimedTransition(6000, "Attacking2")
            ),
            new State("Attacking2",
                new Shoot(999, fixedAngle: 90, defaultAngle: 90, cooldown: 300),
                new Shoot(999, fixedAngle: 0, defaultAngle: 0, cooldown: 300),
                new Shoot(999, fixedAngle: 45, defaultAngle: 45, cooldown: 300),
                new TimedTransition(6000, "Attacking3")
            ),
            new State("Attacking3",
                new Shoot(999, fixedAngle: 90, defaultAngle: 90, cooldown: 300),
                new Shoot(999, fixedAngle: 0, defaultAngle: 0, cooldown: 300),
                new TimedTransition(6000, "Setup")
            ),
            new State("Suicide",
                new Suicide()
            ),
            new Decay(20000)
        );
        db.Init("Limon Element 3",
            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
            new EntityNotWithinTransition("Limon the Sprite God", 999, "Suicide"),
            new State("Setup",
                new TimedTransition(2000, "Attacking1")
            ),
            new State("Attacking1",
                new Shoot(999, fixedAngle: 0, defaultAngle: 0, cooldown: 300),
                new Shoot(999, fixedAngle: 270, defaultAngle: 270, cooldown: 300),
                new TimedTransition(6000, "Attacking2")
            ),
            new State("Attacking2",
                new Shoot(999, fixedAngle: 0, defaultAngle: 0, cooldown: 300),
                new Shoot(999, fixedAngle: 270, defaultAngle: 270, cooldown: 300),
                new Shoot(999, fixedAngle: 315, defaultAngle: 315, cooldown: 300),
                new TimedTransition(6000, "Attacking3")
            ),
            new State("Attacking3",
                new Shoot(999, fixedAngle: 0, defaultAngle: 0, cooldown: 300),
                new Shoot(999, fixedAngle: 270, defaultAngle: 270, cooldown: 300),
                new TimedTransition(6000, "Setup")
            ),
            new State("Suicide",
                new Suicide()
            ),
            new Decay(20000)
        );
        db.Init("Limon Element 4",
            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
            new EntityNotWithinTransition("Limon the Sprite God", 999, "Suicide"),
            new State("Setup",
                new TimedTransition(2000, "Attacking1")
            ),
            new State("Attacking1",
                new Shoot(999, fixedAngle: 270, defaultAngle: 270, cooldown: 300),
                new Shoot(999, fixedAngle: 180, defaultAngle: 180, cooldown: 300),
                new TimedTransition(6000, "Attacking2")
            ),
            new State("Attacking2",
                new Shoot(999, fixedAngle: 270, defaultAngle: 270, cooldown: 300),
                new Shoot(999, fixedAngle: 180, defaultAngle: 180, cooldown: 300),
                new Shoot(999, fixedAngle: 225, defaultAngle: 225, cooldown: 300),
                new TimedTransition(6000, "Attacking3")
            ),
            new State("Attacking3",
                new Shoot(999, fixedAngle: 270, defaultAngle: 270, cooldown: 300),
                new Shoot(999, fixedAngle: 180, defaultAngle: 180, cooldown: 300),
                new TimedTransition(6000, "Setup")
            ),
            new State("Suicide",
                new Suicide()
            ),
            new Decay(20000)
        );
    }
}