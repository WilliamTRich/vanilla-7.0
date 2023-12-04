using Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database;

public sealed class LotLL : IBehaviorDatabase
{
    public void Init(BehaviorDb db)
    {
        db.Init("Lord of the Lost Lands",
            new State("Alive",
                new HealthTransition(0.1f, "Dead"),
                new State("Waiting",
                    new HealthTransition(0.99f, "Start")
                ),
                new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                    new SetAltTexture(0),
                    new Prioritize(
                        new Wander(0.8f)
                    ),
                    new Shoot(0, 7, 10, fixedAngle: 180, cooldown: 2000),
                    new Shoot(0, 7, 10, fixedAngle: 0, cooldown: 2000),
                    new TossObject("Guardian of the Lost Lands"),
                    new TimedTransition(100, "Spawning Guardian")
                ),
                new State("Spawning Guardian",
                    new TossObject("Guardian of the Lost Lands"),
                    new TimedTransition(3100, "Attack")
                ),
                new State("Attack",
                    new SetAltTexture(0),
                    new Wander(0.8f),
                    new PlayerWithinTransition(1, false, "Follow"),
                    new TimedTransition(10000, "Gathering"),
                    new State("Choose",
                        new TimedTransition(3000, "Attack1.1f", "Attack1.2f")
                    ),
                    new State("Attack1.1f",
                        new Shoot(12, 7, 10, cooldown: 2000),
                        new Shoot(12, 7, 190, cooldown: 2000),
                        new TimedTransition(2000, "Choose")
                    ),
                    new State("Attack1.2f",
                        new Shoot(0, 7, 10, fixedAngle: 180, cooldown: 3000),
                        new Shoot(0, 7, 10, fixedAngle: 0, cooldown: 3000),
                        new TimedTransition(2000, "Choose")
                    )
                ),
                new State("Follow",
                    new Prioritize(
                        new Follow(1, 20, 3),
                        new Wander(0.4f)
                    ),
                    new Shoot(20, 7, 10, cooldown: 1300),
                    new TimedTransition(5000, "Gathering")
                ),
                new State("Gathering",
                    new Taunt(0.99f, "Gathering power!"),
                    new SetAltTexture(3),
                    new TimedTransition(2000, "Gathering1.0f")
                ),
                new State("Gathering1.0f",
                    new TimedTransition(5000, "Protection"),
                    new State("Gathering1.1f",
                        new Shoot(30, 4, fixedAngle: 90, index: 1, cooldown: 2000),
                        new TimedTransition(1500, "Gathering1.2f")
                    ),
                    new State("Gathering1.2f",
                        new Shoot(30, 4, fixedAngle: 45, index: 1, cooldown: 2000),
                        new TimedTransition(1500, "Gathering1.1f")
                    )
                ),
                new State("Protection",
                    new SetAltTexture(0),
                    new TossObject("Protection Crystal", 4, 0, 5000),
                    new TossObject("Protection Crystal", 4, 45, 5000),
                    new TossObject("Protection Crystal", 4, 90, 5000),
                    new TossObject("Protection Crystal", 4, 135, 5000),
                    new TossObject("Protection Crystal", 4, 180, 5000),
                    new TossObject("Protection Crystal", 4, 225, 5000),
                    new TossObject("Protection Crystal", 4, 270, 5000),
                    new TossObject("Protection Crystal", 4, 315, 5000),
                    new EntityWithinTransition("Protection Crystal", 10, "Waiting")
                ),
                new State("Waiting",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new SetAltTexture(1),
                    new EntityNotWithinTransition("Protection Crystal", 10, "Start")
                )
            ),
            new State("Dead",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new SetAltTexture(3),
                new Taunt(0.99f, "NOOOO!!!!!!"),
                new Flash(0xFF0000, .1f, 1000),
                new TimedTransition(2000, "Suicide")
            ),
            new State("Suicide",
                new ConditionalEffect(ConditionEffectIndex.StunImmune, true),
                new Shoot(0, 8, fixedAngle: 360 / 8, index: 1),
                new Suicide()
            ),
            new Threshold(0.02f,
                new ItemLoot("Shield of Ogmur", 0.01f)
            ),
            new Threshold(0.001f,
                new TierLoot(8, TierLoot.LootType.Weapon, .015f),
                new TierLoot(9, TierLoot.LootType.Weapon, .015f),
                new TierLoot(10, TierLoot.LootType.Weapon, .01f),
                new TierLoot(11, TierLoot.LootType.Weapon, .01f),
                new TierLoot(4, TierLoot.LootType.Ability, .01f),
                new TierLoot(5, TierLoot.LootType.Ability, .01f),
                new TierLoot(8, TierLoot.LootType.Armor, .01f),
                new TierLoot(9, TierLoot.LootType.Armor, .015f),
                new TierLoot(10, TierLoot.LootType.Armor, .010f),
                new TierLoot(11, TierLoot.LootType.Armor, .01f),
                new TierLoot(12, TierLoot.LootType.Armor, .014f),
                new TierLoot(3, TierLoot.LootType.Ring, .015f),
                new TierLoot(4, TierLoot.LootType.Ring, .017f),
                new TierLoot(5, TierLoot.LootType.Ring, .015f),
                new ItemLoot("Potion of Defense", .1f),
                new ItemLoot("Potion of Attack", .1f),
                new ItemLoot("Potion of Vitality", .1f),
                new ItemLoot("Potion of Wisdom", .1f),
                new ItemLoot("Potion of Speed", .1f),
                new ItemLoot("Potion of Dexterity", .1f)
            )
        );
        db.Init("Protection Crystal",
            new Prioritize(
                new Orbit(0.3f, 4, 10, "Lord of the Lost Lands")
            ),
            new Shoot(8, 4, 7, cooldown: 500)
        );
        db.Init("Guardian of the Lost Lands",
            new State("Full",
                new Spawn("Knight of the Lost Lands", 2, 1, 4000),
                new Prioritize(
                    new Follow(0.6f, 20),
                    new Wander(0.2f)
                ),
                new Shoot(10, 8, fixedAngle: 360 / 8, cooldown: 3000, index: 1),
                new Shoot(10, 5, 10, cooldown: 1500),
                new HealthTransition(0.25f, "Low")
            ),
            new State("Low",
                new Prioritize(
                    new StayBack(0.6f, 5),
                    new Wander(0.2f)
                ),
                new Shoot(10, 8, fixedAngle: 360 / 8, cooldown: 3000, index: 1),
                new Shoot(10, 5, 10, cooldown: 1500)
            ),
            new ItemLoot("Health Potion", 0.1f),
            new ItemLoot("Magic Potion", 0.1f)
        );
        db.Init("Knight of the Lost Lands",
            new Prioritize(
                new Follow(1, 20, 4),
                new StayBack(0.5f, 2),
                new Wander(0.3f)
            ),
            new Shoot(13, 1, cooldown: 700),
            new ItemLoot("Health Potion", 0.1f),
            new ItemLoot("Magic Potion", 0.1f)
        );
    }
}