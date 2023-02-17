using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public sealed class CubeGod : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Cube God",
                new Wander(.3f),
                new Shoot(30, 9, 10, 0, predictive: .5f, cooldown: 750),
                new Shoot(30, 4, 10, 1, predictive: .5f, cooldown: 1500),
                new Reproduce("Cube Overseer", 30, 10, cooldown: 10000),
                new Threshold(.0001f,
                    new TierLoot(8, TierLoot.LootType.Weapon, .015f),
                    new TierLoot(9, TierLoot.LootType.Weapon, .025f),
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
                    new TierLoot(5, TierLoot.LootType.Ring, .013f),
                    new ItemLoot("Potion of Defense", 0.3f),
                    new ItemLoot("Potion of Attack", 0.3f),
                    new ItemLoot("Potion of Speed", 0.3f),
                    new ItemLoot("Potion of Vitality", 0.3f),
                    new ItemLoot("Potion of Wisdom", 0.3f),
                    new ItemLoot("Potion of Dexterity", 0.3f),
                    new ItemLoot("Potion of Dexterity", .3f)
                ),
                new Threshold(.02f,
                    new ItemLoot("Dirk of Cronus", 0.01f)
                ),
                new Threshold(.0002f,
                    new TierLoot(9, TierLoot.LootType.Weapon, .015f),
                    new TierLoot(10, TierLoot.LootType.Weapon, .01f),
                    new TierLoot(11, TierLoot.LootType.Weapon, .01f),
                    new TierLoot(12, TierLoot.LootType.Weapon, .01f),
                    new TierLoot(4, TierLoot.LootType.Ability, .01f),
                    new TierLoot(5, TierLoot.LootType.Ability, .01f),
                    new TierLoot(9, TierLoot.LootType.Armor, .01f),
                    new TierLoot(10, TierLoot.LootType.Armor, .015f),
                    new TierLoot(11, TierLoot.LootType.Armor, .010f),
                    new TierLoot(12, TierLoot.LootType.Armor, .01f),
                    new TierLoot(3, TierLoot.LootType.Ring, .01f),
                    new TierLoot(4, TierLoot.LootType.Ring, .01f),
                    new TierLoot(5, TierLoot.LootType.Ring, .01f)
                )
            );
            db.Init("Cube Overseer",
                new Prioritize(
                    new Orbit(.375f, 10, 30, "Cube God", .075f, 5),
                    new Wander(.375f)
                ),
                new Reproduce("Cube Defender", 12, 10, cooldown: 10000),
                new Reproduce("Cube Blaster", 30, 10, cooldown: 10000),
                new Shoot(10, 4, 10, 0, cooldown: 750),
                new Shoot(10, index: 1, cooldown: 1500),
                new Threshold(.01f,
                    new ItemLoot("Fire Sword", .05f)
                )
            );
            db.Init("Cube Defender",
                new Prioritize(
                    new Orbit(1.05f, 5, 15, "Cube Overseer", .15f, 3),
                    new Wander(.6f)
                ),
                new Shoot(10, cooldown: 500)
            );
            db.Init("Cube Blaster",
                new State("Orbit",
                    new Prioritize(
                        new Orbit(1.05f, 7.5f, 40, "Cube Overseer", .15f, 3),
                        new Wander(.6f)
                    ),
                    new EntityNotWithinTransition("Cube Overseer", 10, "Follow")
                ),
                new State("Follow",
                    new Prioritize(
                        new Follow(.75f, 10, 1, 5000),
                        new Wander(.6f)
                    ),
                    new EntityNotWithinTransition("Cube Defender", 10, "Orbit"),
                    new TimedTransition(5000, "Orbit")
                ),
                new Shoot(10, 2, 10, 1, predictive: 1, cooldown: 500),
                new Shoot(10, predictive: 1, cooldown: 1500)
            );
        }
    }
}