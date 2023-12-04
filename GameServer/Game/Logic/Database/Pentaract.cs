using Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database;

public sealed class Pentaract : IBehaviorDatabase
{
    public void Init(BehaviorDb db)
    {
        db.Init("Pentaract Eye",
            new Prioritize(
                new Swirl(2, 8, 20, true),
                new Protect(2, "Pentaract Tower", 20, 6, 4)
            ),
            new Shoot(9, 1, cooldown: 1000)
        );
        db.Init("Pentaract Tower",
            new Spawn("Pentaract Eye", 5, cooldown: 5000, givesNoXp: false),
            new Grenade(4, 100, 8, cooldown: 5000),
            new TransformOnDeath("Pentaract Tower Corpse"),
            new TransferDamageOnDeath("Pentaract Tower Corpse")
        );
        db.Init("Pentaract",
            new ConditionalEffect(ConditionEffectIndex.Invincible),
            new State("Waiting",
                new EntityNotWithinTransition("Pentaract Tower", 50, "Die")
            ),
            new State("Die",
                new Suicide()
            )
        );
        db.Init("Pentaract Tower Corpse",
            new ConditionalEffect(ConditionEffectIndex.Invincible),
            new State("Waiting",
                new TimedTransition(15000, "Spawn"),
                new EntityNotWithinTransition("Pentaract Tower", 50, "Die")
            ),
            new State("Spawn",
                new Transform("Pentaract Tower")
            ),
            new State("Die",
                new Suicide()
            ),
            new Threshold(0.02f,
                new ItemLoot("Seal of Blasphemous Prayer", 0.01f)
            ),
            new Threshold(0.01f,
                new TierLoot(8, TierLoot.LootType.Weapon, .19f),
                new TierLoot(9, TierLoot.LootType.Weapon, .19f),
                new TierLoot(10, TierLoot.LootType.Weapon, .17f),
                new TierLoot(11, TierLoot.LootType.Weapon, .15f),
                new TierLoot(4, TierLoot.LootType.Ability, .17f),
                new TierLoot(5, TierLoot.LootType.Ability, .15f),
                new TierLoot(8, TierLoot.LootType.Armor, .17f),
                new TierLoot(9, TierLoot.LootType.Armor, .11f),
                new TierLoot(10, TierLoot.LootType.Armor, .10f),
                new TierLoot(11, TierLoot.LootType.Armor, .18f),
                new TierLoot(12, TierLoot.LootType.Armor, .17f),
                new TierLoot(3, TierLoot.LootType.Ring, .21f),
                new TierLoot(4, TierLoot.LootType.Ring, .2f),
                new TierLoot(5, TierLoot.LootType.Ring, .15f),
                new ItemLoot("Potion of Defense", .1f),
                new ItemLoot("Potion of Attack", .1f),
                new ItemLoot("Potion of Vitality", .1f),
                new ItemLoot("Potion of Wisdom", .1f),
                new ItemLoot("Potion of Speed", .1f),
                new ItemLoot("Potion of Dexterity", .1f)
            )
        );
    }
}