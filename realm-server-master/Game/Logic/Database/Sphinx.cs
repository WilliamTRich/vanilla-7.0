using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public sealed class Sphinx : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Grand Sphinx",
                new DropPortalOnDeath("Tomb of the Ancients Portal", 0.9f),
                new State("Spawned",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Reproduce("Horrid Reaper", 30, 4, 100),
                    new TimedTransition(500, "Attack1")
                ),
                new State("Attack1",
                    new Prioritize(
                        new Wander(0.5f)
                    ),
                    new Shoot(12, 1, cooldown: 800),
                    new Shoot(12, 3, 10, cooldown: 1000),
                    new Shoot(12, 1, 130, cooldown: 1000),
                    new Shoot(12, 1, 230, cooldown: 1000),
                    new TimedTransition(6000, "TransAttack2")
                ),
                new State("TransAttack2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Wander(0.5f),
                    new Flash(0x00FF0C, .25f, 8),
                    new Taunt(0.99f, "You hide behind rocks like cowards but you cannot hide from this!"),
                    new TimedTransition(2000, "Attack2")
                ),
                new State("Attack2",
                    new Prioritize(
                        new Wander(0.5f)
                    ),
                    new Shoot(0, 8, 10, fixedAngle: 0, rotateAngle: 70, cooldown: 2000,
                        index: 1),
                    new Shoot(0, 8, 10, fixedAngle: 180, rotateAngle: 70, cooldown: 2000,
                        index: 1),
                    new TimedTransition(6200, "TransAttack3")
                ),
                new State("TransAttack3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Wander(0.5f),
                    new Flash(0x00FF0C, .25f, 8),
                    new TimedTransition(2000, "Attack3")
                ),
                new State("Attack3",
                    new Prioritize(
                        new Wander(0.5f)
                    ),
                    new Shoot(20, 9, fixedAngle: 360 / 9, index: 2, cooldown: 2300),
                    new TimedTransition(6000, "TransAttack1"),
                    new State("Shoot1",
                        new Shoot(20, 2, 4, 2, cooldown: 700),
                        new TimedTransition(1000, "Shoot1", "Shoot2")
                    ),
                    new State("Shoot2",
                        new Shoot(20, 8, 5, 2, cooldown: 1100),
                        new TimedTransition(1000, "Shoot1","Shoot2")
                    )
                ),
                new State("TransAttack1",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Wander(0.5f),
                    new Flash(0x00FF0C, .25f, 8),
                    new TimedTransition(2000, "Attack1"),
                    new HealthTransition(0.15f, "Order")
                ),
                new State("Order",
                    new Wander(0.5f),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new OrderOnce(30, "Horrid Reaper", "Die"),
                    new TimedTransition(1900, "Attack1")
                ),
                new Threshold(0.001f,
                    new TierLoot(8, TierLoot.LootType.Weapon, .03f),
                    new TierLoot(9, TierLoot.LootType.Weapon, .03f),
                    new TierLoot(10, TierLoot.LootType.Weapon, .02f),
                    new TierLoot(11, TierLoot.LootType.Weapon, .02f),
                    new TierLoot(4, TierLoot.LootType.Ability, .02f),
                    new TierLoot(5, TierLoot.LootType.Ability, .02f),
                    new TierLoot(8, TierLoot.LootType.Armor, .02f),
                    new TierLoot(9, TierLoot.LootType.Armor, .015f),
                    new TierLoot(10, TierLoot.LootType.Armor, .010f),
                    new TierLoot(11, TierLoot.LootType.Armor, .02f),
                    new TierLoot(12, TierLoot.LootType.Armor, .024f),
                    new TierLoot(3, TierLoot.LootType.Ring, .025f),
                    new TierLoot(4, TierLoot.LootType.Ring, .027f),
                    new TierLoot(5, TierLoot.LootType.Ring, .023f),
                    new ItemLoot("Potion of Defense", .4f),
                    new ItemLoot("Potion of Attack", .4f),
                    new ItemLoot("Potion of Vitality", .4f),
                    new ItemLoot("Potion of Wisdom", .4f),
                    new ItemLoot("Potion of Speed", .4f),
                    new ItemLoot("Potion of Dexterity", .4f)
                ),
                new Threshold(0.02f,
                    new ItemLoot("Potion of Vitality", 1),
                    new ItemLoot("Potion of Wisdom", 1),
                    new ItemLoot("Helm of the Juggernaut", 0.01f)
                )
            );
            db.Init("Horrid Reaper",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Move",
                    new Prioritize(
                        new StayCloseToSpawn(3, 10),
                        new Wander(.6f)
                    ),
                    new EntityNotWithinTransition("Grand Sphinx", 50, "Die"), //Just to be sure
                    new TimedTransition(2000, "Attack")
                ),
                new State("Attack",
                    new Shoot(0, 6, fixedAngle: 360 / 6, cooldown: 700),
                    new PlayerWithinTransition(2, false, "Follow"),
                    new TimedTransition(5000, "Move")
                ),
                new State("Follow",
                    new Prioritize(
                        new Follow(0.7f, 10, 3)
                    ),
                    new Shoot(7, 1, cooldown: 700),
                    new TimedTransition(5000, "Move")
                ),
                new State("Die",
                    new Taunt(0.99f, "OOaoaoAaAoaAAOOAoaaoooaa!!!"),
                    new Decay(1000)
                )
            );
        }
    }
}