using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public sealed class Beach : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Pirate",
                new Prioritize(
                    new Follow(0.85f, range: 1, duration: 5000, cooldown: 0),
                    new Wander(0.4f)
                ),
                new Shoot(3, cooldown: 2500),
                new TierLoot(1, TierLoot.LootType.Weapon, 0.2f)
            );
            db.Init("Piratess",
                new Prioritize(
                    new Follow(1.1f, range: 1, duration: 3000, cooldown: 1500),
                    new Wander(0.4f)
                ),
                new Shoot(3, cooldown: 2500),
                new Reproduce("Pirate", densityMax: 5),
                new Reproduce("Piratess", densityMax: 5),
                new TierLoot(1, TierLoot.LootType.Armor, 0.2f)
            );
            db.Init("Snake",
                new Wander(0.5f),
                new Shoot(10, cooldown: 2000),
                new Reproduce(densityMax: 5)
            );
            db.Init("Poison Scorpion",
                new Prioritize(
                    new Protect(0.4f, "Scorpion Queen"),
                    new Wander(0.4f)
                ),
                new Shoot(8, cooldown: 2000)
            );
            db.Init("Scorpion Queen",
                new ChangeSize(100, 200),
                new Wander(0.2f),
                new Spawn("Poison Scorpion", givesNoXp: false),
                new Reproduce("Poison Scorpion", cooldown: 10000, densityMax: 10),
                new Reproduce(densityMax: 2, densityRadius: 40),
                new TierLoot(2, TierLoot.LootType.Armor, 0.4f),
                new TierLoot(2, TierLoot.LootType.Weapon, 0.3f)
            );
            db.Init("Bandit Enemy",
                new State("fast_follow",
                    new Shoot(3),
                    new Prioritize(
                        new Protect(0.6f, "Bandit Leader", acquireRange: 9, protectionRange: 7, reprotectRange: 3),
                        new Follow(1, range: 1),
                        new Wander(0.4f)
                    ),
                    new TimedTransition(3000, "scatter1")
                ),
                new State("scatter1",
                    new Prioritize(
                        new Protect(0.6f, "Bandit Leader", acquireRange: 9, protectionRange: 7, reprotectRange: 3),
                        new Wander(.5f),
                        new Wander(0.6f)
                    ),
                    new TimedTransition(2000, "slow_follow")
                ),
                new State("slow_follow",
                    new Shoot(4.5f),
                    new Prioritize(
                        new Protect(0.6f, "Bandit Leader", acquireRange: 9, protectionRange: 7, reprotectRange: 3),
                        new Follow(0.5f, acquireRange: 9, range: 3.5f, duration: 4000),
                        new Wander(0.5f)
                    ),
                    new TimedTransition(3000, "scatter2")
                ),
                new State("scatter2",
                    new Prioritize(
                        new Protect(0.6f, "Bandit Leader", acquireRange: 9, protectionRange: 7, reprotectRange: 3),
                        new Wander(.5f),
                        new Wander(0.6f)
                    ),
                    new TimedTransition(2000, "fast_follow")
                ),
                new State("escape",
                    new StayBack(0.5f),
                    new TimedTransition(15000, "fast_follow")
                )
            );
            db.Init("Bandit Leader",
                new Spawn("Bandit Enemy", cooldown: 8000, maxChildren: 4, givesNoXp: false),
                new State("bold",
                    new State("warn_about_grenades",
                        new Taunt(0.15f, "Catch!"),
                        new TimedTransition(400, "wimpy_grenade1")
                    ),
                    new State("wimpy_grenade1",
                        new Grenade(1.4f, 12, cooldown: 10000),
                        new Prioritize(
                            new StayAbove(0.3f, 7),
                            new Wander(0.3f)
                        ),
                        new TimedTransition(2000, "wimpy_grenade2")
                    ),
                    new State("wimpy_grenade2",
                        new Grenade(1.4f, 12, cooldown: 10000),
                        new Prioritize(
                            new StayAbove(0.5f, 7),
                            new Wander(0.5f)
                        ),
                        new TimedTransition(3000, "slow_follow")
                    ),
                    new State("slow_follow",
                        new Shoot(13, cooldown: 1000),
                        new Prioritize(
                            new StayAbove(0.4f, 7),
                            new Follow(0.4f, acquireRange: 9, range: 3.5f, duration: 4000),
                            new Wander(0.4f)
                        ),
                        new TimedTransition(4000, "warn_about_grenades")
                    ),
                    new HealthTransition(0.45f, "meek")
                ),
                new State("meek",
                    new Taunt(0.5f, "Forget this... run for it!"),
                    new StayBack(0.5f, 6),
                    new Order(10, "Bandit Enemy", "escape"),
                    new TimedTransition(12000, "bold")
                )
                ,
                new TierLoot(2, TierLoot.LootType.Weapon, 0.4f),
                new TierLoot(2, TierLoot.LootType.Armor, 0.4f),
                new TierLoot(2, TierLoot.LootType.Weapon, 0.4f),
                new TierLoot(2, TierLoot.LootType.Armor, 0.4f)
            );
            db.Init("Red Gelatinous Cube",
                new Shoot(8, count: 2, shootAngle: 10, predictive: 0.2f, cooldown: 1000),
                new Wander(0.4f),
                new Reproduce(densityMax: 5),
                new DropPortalOnDeath("Pirate Cave Portal", .01f)
            );
            db.Init("Purple Gelatinous Cube",
                new Shoot(8, predictive: 0.2f, cooldown: 600),
                new Wander(0.4f),
                new Reproduce(densityMax: 5),
                new DropPortalOnDeath("Pirate Cave Portal", .01f)
            );
            db.Init("Green Gelatinous Cube",
                new Shoot(8, count: 5, shootAngle: 72, predictive: 0.2f, cooldown: 1800),
                new Wander(0.4f),
                new Reproduce(densityMax: 5),
                new DropPortalOnDeath("Pirate Cave Portal", .01f)
            );
        }
    }
}