using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public sealed class Hermit : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Hermit God",
                new DropPortalOnDeath("Ocean Trench Portal"),
                new GroundTransformOnDeath("Shallow Water", 5, from: "Dark Water"),
                new OrderOnDeath(20, "Hermit God Tentacle Spawner", "Die"),
                new State("Spawn Tentacle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(2),
                    new OrderOnce(20, "Hermit God Tentacle Spawner", "Tentacle"),
                    new TimedTransition(time: 8000, targetStates: "Wake"),
                    new EntityWithinTransition("Hermit God Tentacle", 20, "Sleep")
                ),
                new State("Sleep",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new OrderOnce(20, "Hermit God Tentacle Spawner", "Minions"),
                    new TimedTransition(1000, "Waiting")
                ),
                new State("Waiting",
                    new SetAltTexture(3),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new EntityNotWithinTransition("Hermit God Tentacle", 20, "Wake")
                ),
                new State("Wake",
                    new SetAltTexture(2),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new TossObject("Hermit Minion", 10, 0),
                    new TossObject("Hermit Minion", 10, 45),
                    new TossObject("Hermit Minion", 10, 90),
                    new TossObject("Hermit Minion", 10, 135),
                    new TossObject("Hermit Minion", 10, 180),
                    new TossObject("Hermit Minion", 10, 225),
                    new TossObject("Hermit Minion", 10, 270),
                    new TossObject("Hermit Minion", 10, 315),
                    new TimedTransition(100, "Spawn Whirlpool")
                ),
                new State("Spawn Whirlpool",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new OrderOnce(20, "Hermit God Tentacle Spawner", "Whirlpool"),
                    new TimedTransition(time: 8000, targetStates: "Attack1"),
                    new EntityWithinTransition("Whirlpool", 20, "Attack1")
                ),
                new State("Attack1",
                    new SetAltTexture(0),
                    new Prioritize(
                        new Wander(0.3f),
                        new StayCloseToSpawn(0.5f)
                    ),
                    new Shoot(20, 3, 5, cooldown: 300),
                    new TimedTransition(6000, "Attack2")
                ),
                new State("Attack2",
                    new Prioritize(
                        new Wander(0.3f),
                        new StayCloseToSpawn(0.5f)
                    ),
                    new OrderOnce(20, "Whirlpool", "Die"),
                    new Shoot(20, 1, defaultAngle: 0, fixedAngle: 0, rotateAngle: 45, index: 1,
                        cooldown: 1000),
                    new Shoot(20, 1, defaultAngle: 0, fixedAngle: 180, rotateAngle: 45, index: 1,
                        cooldown: 1000),
                    new TimedTransition(6000, "Spawn Tentacle")
                ),
                new Threshold(0.02f,
                    new ItemLoot("Helm of the Juggernaut", 0.01f)
                ),
                new Threshold(0.001f,
                    new ItemLoot("Potion of Vitality", 0.1f),
                    new ItemLoot("Potion of Dexterity", 0.1f),
                    new ItemLoot("Potion of Wisdom", 0.1f),
                    new TierLoot(8, TierLoot.LootType.Weapon, .03f),
                    new TierLoot(9, TierLoot.LootType.Weapon, .03f),
                    new TierLoot(10, TierLoot.LootType.Weapon, .015f),
                    new TierLoot(11, TierLoot.LootType.Weapon, .01f),
                    new TierLoot(4, TierLoot.LootType.Ability, .02f),
                    new TierLoot(5, TierLoot.LootType.Ability, .02f),
                    new TierLoot(8, TierLoot.LootType.Armor, .02f),
                    new TierLoot(9, TierLoot.LootType.Armor, .15f),
                    new TierLoot(10, TierLoot.LootType.Armor, .010f),
                    new TierLoot(11, TierLoot.LootType.Armor, .015f),
                    new TierLoot(12, TierLoot.LootType.Armor, .01f),
                    new TierLoot(3, TierLoot.LootType.Ring, .025f),
                    new TierLoot(4, TierLoot.LootType.Ring, .015f),
                    new TierLoot(5, TierLoot.LootType.Ring, .01f),
                    new ItemLoot("Potion of Dexterity", .1f)
                )
            );
            db.Init("Hermit Minion",
                new Prioritize(
                    new Follow(0.6f, 4, 1),
                    new Orbit(0.6f, 10, 15, "Hermit God", .2f, 1.5f),
                    new Wander(0.6f)
                ),
                new Shoot(6, 3, 10, cooldown: 1000),
                new Shoot(6, 2, 20, 1, cooldown: 2600, predictive: 0.8f),
                new ItemLoot("Health Potion", 0.1f),
                new ItemLoot("Magic Potion", 0.1f)
            );
            db.Init("Whirlpool",
                new State("Attack",
                    new EntityNotWithinTransition("Hermit God", 100, "Die"),
                    new Prioritize(
                        new Orbit(0.3f, 6, 10, "Hermit God")
                    ),
                    new Shoot(0, 1, fixedAngle: 0, rotateAngle: 30, cooldown: 400)
                ),
                new State("Die",
                    new Shoot(0, 8, fixedAngle: 360 / 8),
                    new Suicide()
                )
            );
            db.Init("Hermit God Tentacle",
                new Prioritize(
                    new Follow(0.6f, 4, 1),
                    new Orbit(0.6f, 6, 15, "Hermit God", .2f, .5f)
                ),
                new Shoot(3, 8, 360 / 8, cooldown: 500)
            );
            db.Init("Hermit God Tentacle Spawner",
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Waiting Order"),
                new State("Tentacle",
                    new Reproduce("Hermit God Tentacle", 3, 1, 2000),
                    new EntityWithinTransition("Hermit God Tentacle", 1, "Waiting Order")
                ),
                new State("Whirlpool",
                    new Reproduce("Whirlpool", 3, 1, 2000),
                    new EntityWithinTransition("Whirlpool", 1, "Waiting Order")
                ),
                new State("Minions",
                    new Reproduce("Hermit Minion", 40, 20, 1000),
                    new TimedTransition(2000, "Waiting Order")
                ),
                new State("Die",
                    new Suicide()
                )
            );
        }
    }
}