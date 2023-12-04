using RotMG.Game.Entities;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database;

public sealed class SkullShrine : IBehaviorDatabase
{
    public void Init(BehaviorDb db)
    {
        db.Init("Skull Shrine",
            new Shoot(30, 9, 10, cooldown: 750, predictive: 1), // add prediction after fixing it...
            new Reproduce("Red Flaming Skull", 40, 20, 500),
            new Reproduce("Blue Flaming Skull", 40, 20, 500),
            new Threshold(0.02f,
                new ItemLoot("Orb of Conflict", 0.01f)
            ),
            new Threshold(0.001f,
                new TierLoot(8, TierLoot.LootType.Weapon, .015f),
                new TierLoot(9, TierLoot.LootType.Weapon, .01f),
                new TierLoot(10, TierLoot.LootType.Weapon, .07f),
                new TierLoot(11, TierLoot.LootType.Weapon, .05f),
                new TierLoot(4, TierLoot.LootType.Ability, .15f),
                new TierLoot(5, TierLoot.LootType.Ability, .07f),
                new TierLoot(8, TierLoot.LootType.Armor, .02f),
                new TierLoot(9, TierLoot.LootType.Armor, .015f),
                new TierLoot(10, TierLoot.LootType.Armor, .010f),
                new TierLoot(11, TierLoot.LootType.Armor, .07f),
                new TierLoot(12, TierLoot.LootType.Armor, .04f),
                new TierLoot(3, TierLoot.LootType.Ring, .015f),
                new TierLoot(4, TierLoot.LootType.Ring, .07f),
                new TierLoot(5, TierLoot.LootType.Ring, .03f),
                new ItemLoot("Potion of Defense", .05f),
                new ItemLoot("Potion of Attack", .05f),
                new ItemLoot("Potion of Vitality", .05f),
                new ItemLoot("Potion of Wisdom", .05f),
                new ItemLoot("Potion of Speed", .05f)
            )
        );
        db.Init("Red Flaming Skull",
            new State("Orbit Skull Shrine",
                new Prioritize(
                    new Protect(.3f, "Skull Shrine", 30, 15, 15),
                    new Wander(.3f)
                ),
                new EntityNotWithinTransition("Skull Shrine", 40, "Wander")
            ),
            new State("Follow-Player",
                new Wander(.5f),
                new Follow(.125f, Player.SightRadius, .9f)
                
            ),
            new State("Wander",
                new Wander(.3f)
            ),
            new Shoot(12, 2, 10, cooldown: 750)
        );
        db.Init("Blue Flaming Skull",
            new State("Orbit Skull Shrine",
                new Orbit(1.5f, 15, 40, "Skull Shrine", .6f, 10, null),
                new EntityNotWithinTransition("Skull Shrine", 40, "Wander")
            ),
            new State("Follow-Player",
                new Wander(.5f),
                new Follow(.125f, Player.SightRadius, .9f)
            ),
            new State("Wander",
                new Wander(.5f)
            ),
            new Shoot(12, 2, 10, cooldown: 750)
        );
    }
}