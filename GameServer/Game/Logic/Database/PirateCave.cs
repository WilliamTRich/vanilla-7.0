using Common;
using RotMG.Game.Logic;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database;

public sealed class PirateCave: IBehaviorDatabase
{
    public void Init(BehaviorDb db)
    {

        db.Init("Dreadstump the Pirate King",
            new DropPortalOnDeath("Glowing Realm Portal"),
            new State("Idle",
                new PlayerWithinTransition(15, true, "swiggity")
            ),
            new State("swiggity",
                new StayCloseToSpawn(1, 7),
                new Wander(0.3f),
                new Shoot(range: 8, count: 1, index: 1, cooldown: 2000, predictive: 0.9f),
                new Shoot(range: 8, count: 1, index: 0, cooldown: 1000, predictive: 0.9f),
                new Taunt(0.3f, 14000,
                    "Hah! I'll drink my rum out of your skull!",
                    "Eat cannonballs!",
                    "Arrrr..."
                )
            ),
            new TierLoot(4, TierLoot.LootType.Weapon, 0.01f),
            new TierLoot(3, TierLoot.LootType.Weapon, 0.05f),
            new TierLoot(2, TierLoot.LootType.Weapon, 0.15f),
            new TierLoot(1, TierLoot.LootType.Armor, 0.2f),
            new TierLoot(2, TierLoot.LootType.Armor, 0.1f),
            new TierLoot(3, TierLoot.LootType.Armor, 0.05f),
            new TierLoot(4, TierLoot.LootType.Armor, 0.01f),
            new TierLoot(1, TierLoot.LootType.Ring, 0.1f),
            new ItemLoot("Pirate Rum", 0.01f)
        );
        db.Init("Pirate Lieutenant",
            new State("start",
                new Protect(0.4f, "Dreadstump the Pirate King"),
                new Wander(0.5f),
                new Follow(1, 6, 1, -1, 0),
                new Shoot(range: 7, index: 0, predictive: 1, cooldown: 1500)
            ),
            new TierLoot(3, TierLoot.LootType.Weapon, 0.05f),
            new TierLoot(2, TierLoot.LootType.Weapon, 0.15f),
            new TierLoot(1, TierLoot.LootType.Armor, 0.2f),
            new TierLoot(1, TierLoot.LootType.Ring, 0.1f),
            new ItemLoot("Pirate Rum", 0.01f)
        );
        db.Init("Pirate Captain",
            new State("start",
                new Prioritize(
                    new Protect(0.4f, "Dreadstump the Pirate King", protectionRange: 6),
                    new Wander(0.5f),
                    new Follow(1, 6, 1, -1, 0)
                ),
                new Shoot(range: 7, index: 0, predictive: 1, cooldown: 1500)
            ),
            new TierLoot(3, TierLoot.LootType.Weapon, 0.05f),
            new TierLoot(2, TierLoot.LootType.Weapon, 0.15f),
            new TierLoot(1, TierLoot.LootType.Armor, 0.2f),
            new TierLoot(1, TierLoot.LootType.Ring, 0.1f),
            new ItemLoot("Pirate Rum", 0.01f) //swiggity
        );
        db.Init("Pirate Commander",
            new State("start",
                new Prioritize(
                    new Protect(0.4f, "Dreadstump the Pirate King", protectionRange: 6),
                    new Wander(0.5f)
                ),
                new Shoot(range: 7, index: 0, predictive: 1, cooldown: 1500)
            ),
            new TierLoot(3, TierLoot.LootType.Weapon, 0.05f),
            new TierLoot(2, TierLoot.LootType.Weapon, 0.15f),
            new TierLoot(1, TierLoot.LootType.Armor, 0.2f),
            new TierLoot(1, TierLoot.LootType.Ring, 0.1f),
            new ItemLoot("Pirate Rum", 0.01f) //swooty
        );
        db.Init("Cave Pirate Brawler",
            new State("that",
                    new Follow(1, 6, 1, -1, 0),
                    new Wander(0.3f),
                    new Shoot(range: 5, count: 1, index: 0, cooldown: 1000)
                
            ),
            new ItemLoot("Health Potion", 0.2f)
        );
        db.Init("Cave Pirate Sailor",
            new State("booty",
                    new Wander(0.8f),
                    new Follow(0.8f, 6, 1, -1, 0),
                    new Shoot(range: 5, count: 1, index: 0, cooldown: 1000)
            ),
            new ItemLoot("Health Potion", 0.2f)
        );
        db.Init("Cave Pirate Cabin Boy",
            new State("start",
                new Prioritize(
                    new Wander(0.5f)
                )
            ),
            new TierLoot(1, TierLoot.LootType.Weapon, 0.4f)
        );
        db.Init("Cave Pirate Macaw",
            new State("start",
                new Prioritize(
                    new Wander(0.5f)
                )
            ),
            new TierLoot(1, TierLoot.LootType.Ability, 0.2f)
        );
        db.Init("Cave Pirate Moll",
            new State("start",
                new Prioritize(
                    new Wander(0.1f)
                )
            ),
            new TierLoot(1, TierLoot.LootType.Ability, 0.2f)
        );
        db.Init("Cave Pirate Monkey",
            new State("start",
                new Prioritize(
                    new Wander(0.1f)
                )
            ),
            new TierLoot(1, TierLoot.LootType.Ability, 0.2f)
        );
        db.Init("Cave Pirate Parrot",
            new State("start",
                new Prioritize(
                    new Wander(0.1f)
                )
            ),
            new TierLoot(1, TierLoot.LootType.Ability, 0.2f)
        );
        db.Init("Cave Pirate Hunchback",
            new State("start",
                new Prioritize(
                    new Wander(0.5f)
                )
            ),
            new TierLoot(1, TierLoot.LootType.Ability, 0.2f)
        );
        db.Init("Cave Pirate Veteran",
            new State("start",
                new State("woot",
                    new Follow(1, 6, 1, -1, 0),
                    new Wander(0.8f),
                    new Shoot(range: 5, count: 1, index: 0, cooldown: 1000)
                )
            ),
            new ItemLoot("Health Potion", 0.2f)
        );
        db.Init("Pirate Admiral",
            new State("start",
                new Prioritize(
                    new Protect(0.4f, "Dreadstump the Pirate King", protectionRange: 6),
                    new Wander(0.5f),
                    new Follow(1, 6, 1, -1, 0)
                ),
                new Shoot(range: 7, index: 0, predictive: 1, cooldown: 1500)
            ),
            new TierLoot(3, TierLoot.LootType.Weapon, 0.05f),
            new TierLoot(2, TierLoot.LootType.Weapon, 0.15f),
            new TierLoot(1, TierLoot.LootType.Armor, 0.2f),
            new TierLoot(2, TierLoot.LootType.Armor, 0.1f),
            new TierLoot(1, TierLoot.LootType.Ring, 0.1f),
            new ItemLoot("Pirate Rum", 0.01f) //coming for that booty
        );
    }
}