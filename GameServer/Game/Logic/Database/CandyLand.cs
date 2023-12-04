using Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database;

public sealed class CandyLand : IBehaviorDatabase
{
    public void Init(BehaviorDb db)
    {
        db.Init("Candy Gnome",
            new State("Ini",
                new Wander(0.4f),
                new PlayerWithinTransition(14, targetStates: "Main", seeInvis: true)
            ),
            new State("Main",
                new Follow(1.4f, 17),
                new TimedTransition(1600, "Flee")
            ),
            new State("Flee",
                new PlayerWithinTransition(11, targetStates: "RunAwayMed", seeInvis: true),
                new PlayerWithinTransition(8, targetStates: "RunAwayMedFast", seeInvis: true),
                new PlayerWithinTransition(5, targetStates: "RunAwayFast", seeInvis: true),
                new PlayerWithinTransition(16, targetStates: "RunAwaySlow", seeInvis: true)
            ),
            new State("RunAwayFast",
                new StayBack(1.9f, 30),
                new TimedTransition(1000, "RunAwayMedFast", "RunAwayMed", "RunAwaySlow")
            ),
            new State("RunAwayMedFast",
                new StayBack(1.45f, 30),
                new TimedTransition(1000, "RunAwayMed", "RunAwaySlow")
            ),
            new State("RunAwayMed",
                new StayBack(1.1f, 30),
                new TimedTransition(1000, "RunAwayMedFast", "RunAwaySlow")
            ),
            new State("RunAwaySlow",
                new StayBack(0.8f, 30),
                new TimedTransition(1000, "RunAwayMedFast", "RunAwayMed")
            ),
            new DropPortalOnDeath("Candyland Portal"),
            new Threshold(0.01f,
                new ItemLoot("Rock Candy", 0.15f),
                new ItemLoot("Red Gumball", 0.15f),
                new ItemLoot("Purple Gumball", 0.15f),
                new ItemLoot("Blue Gumball", 0.15f),
                new ItemLoot("Green Gumball", 0.15f),
                new ItemLoot("Yellow Gumball", 0.15f)
            )
        );
        db.Init("MegaRototo",
            new Reproduce("Tiny Rototo", 12, 7, 7000),
            new State("Follow",
                new Shoot(0, 4, 90, defaultAngle: 45, cooldown: 1400),
                new Shoot(0, 4, 90, 1, cooldown: 1400),
                new Follow(0.45f, 11, 5),
                new TimedTransition(4300, "FlameThrower", "StayBack")
            ),
            new State("StayBack",
                new Shoot(0, 3, 16, 1, predictive: 0.6f,
                    cooldown: 1200),
                new Shoot(0, 3, 16, 0, predictive: 0.9f,
                    cooldown: 600),
                new StayBack(0.5f, 10),
                new TimedTransition(2400, "Follow")
            ),
            new State("FlameThrower",
                new State("FB1ORFB2",
                    new TimedTransition(0, "FB1", "FB2")
                ),
                new State("FB1",
                    new Shoot(12, 2, 16, 2, cooldown: 400),
                    new Shoot(12, 1, index: 3, cooldown: 200)
                ),
                new State("FB2",
                    new Shoot(12, 2, 16, 3, cooldown: 400),
                    new Shoot(12, 1, index: 2, cooldown: 200)
                ),
                new TimedTransition(4000, "Follow")
            ),
            new Threshold(0.01f,
                new ItemLoot("Rock Candy", 0.08f),
                new ItemLoot("Candy-Coated Armor", 0.01f),
                new ItemLoot("Red Gumball", 0.15f),
                new ItemLoot("Purple Gumball", 0.15f),
                new ItemLoot("Blue Gumball", 0.15f),
                new ItemLoot("Green Gumball", 0.15f),
                new ItemLoot("Yellow Gumball", 0.15f),
                new TierLoot(11, TierLoot.LootType.Weapon, 0.04f),
                new TierLoot(12, TierLoot.LootType.Weapon, 0.06f),
                new TierLoot(11, TierLoot.LootType.Armor, 0.04f),
                new TierLoot(12, TierLoot.LootType.Armor, 0.06f),
                new TierLoot(5, TierLoot.LootType.Ability, 0.05f),
                new TierLoot(6, TierLoot.LootType.Ability, 0.03f),
                new TierLoot(6, TierLoot.LootType.Ring, 0.05f)

            )
        );
        db.Init("Spoiled Creampuff",
            new Spawn("Big Creampuff", 2, 0, givesNoXp: true),
            new Reproduce("Big Creampuff", 24, 2, 25000),
            new Shoot(10, 1, index: 0, predictive: 1, cooldown: 1400),
            new Shoot(4.4f, 5, 12, 1, predictive: 0.6f,
                cooldown: 800),
            new Prioritize(
                new Charge(1.4f, 11, 4200),
                new StayBack(1, 4),
                new StayBack(0.5f, 7)
            ),
            new StayCloseToSpawn(1.35f, 13),
            new Wander(0.4f),
            new Threshold(0.01f,
                new ItemLoot("Potion of Attack", 0.1f),
                new ItemLoot("Potion of Defense", 0.1f),
                new ItemLoot("Rock Candy", 0.08f),
                new ItemLoot("Red Gumball", 0.15f),
                new ItemLoot("Purple Gumball", 0.15f),
                new ItemLoot("Blue Gumball", 0.15f),
                new ItemLoot("Green Gumball", 0.15f),
                new ItemLoot("Yellow Gumball", 0.15f),
                new TierLoot(11, TierLoot.LootType.Weapon, 0.04f),
                new TierLoot(12, TierLoot.LootType.Weapon, 0.06f),
                new TierLoot(11, TierLoot.LootType.Armor, 0.04f),
                new TierLoot(12, TierLoot.LootType.Armor, 0.06f),
                new TierLoot(5, TierLoot.LootType.Ability, 0.05f),
                new TierLoot(6, TierLoot.LootType.Ability, 0.03f),
                new TierLoot(5, TierLoot.LootType.Ring, 0.05f)
            ),
            new Threshold(0.02f,
                new ItemLoot("Candy-Coated Armor", 0.01f)
                )
        );
        db.Init("Desire Troll",
            new State("BaseAttack",
                new Shoot(10, 3, 15, 0, predictive: 1,
                    cooldown: 1400),
                new Grenade(radius: 5, damage: 100, range: 8, cooldown: 3000),
                new Shoot(10, 1, index: 1, predictive: 1, cooldown: 2000),
                new State("Choose",
                    new TimedTransition(3800, "Run", "Attack")
                ),
                new State("Run",
                    new StayBack(1.1f, 10),
                    new TimedTransition(1200, "Choose")
                ),
                new State("Attack",
                    new Charge(1.2f, 11, 1000),
                    new TimedTransition(1000, "Choose")
                ),
                new HealthTransition(0.6f, "NextAttack")
            ),
            new State("NextAttack",
                new Shoot(10, 5, 10, 2, predictive: 0.5f,
                    angleOffset: 0.4f, cooldown: 2000),
                new Shoot(10, 1, index: 1, predictive: 1, cooldown: 2000),
                new Shoot(10, 3, 15, 0, predictive: 1,
                    angleOffset: 1, cooldown: 4000),
                new Grenade(radius: 5, damage: 100, range: 8, cooldown: 3000),
                new State("Choose2",
                    new TimedTransition(3800, "Run2", "Attack2")
                ),
                new State("Run2",
                    new StayBack(1.5f, 10),
                    new TimedTransition(1500, "Choose2"),
                    new PlayerWithinTransition(3.5f, targetStates: "Boom", seeInvis: false)
                ),
                new State("Attack2",
                    new Charge(1.2f, 11, 1000),
                    new TimedTransition(1000, "Choose2"),
                    new PlayerWithinTransition(3.5f, targetStates: "Boom", seeInvis: false)
                ),
                new State("Boom",
                    new Shoot(0, 20, 18, 3, cooldown: 2000),
                    new TimedTransition(200, "Choose2")
                )
            ),
            new StayCloseToSpawn(1.5f, 15),
            new Prioritize(
                new Follow(1, 11, 5)
            ),
            new Wander(0.4f),
            new Threshold(0.01f,
                new ItemLoot("Potion of Attack", 0.1f),
                new ItemLoot("Potion of Wisdom", 0.1f),
                new ItemLoot("Rock Candy", 0.08f),
                new ItemLoot("Red Gumball", 0.15f),
                new ItemLoot("Purple Gumball", 0.15f),
                new ItemLoot("Blue Gumball", 0.15f),
                new ItemLoot("Green Gumball", 0.15f),
                new ItemLoot("Yellow Gumball", 0.15f),
                new TierLoot(11, TierLoot.LootType.Weapon, 0.04f),
                new TierLoot(12, TierLoot.LootType.Weapon, 0.06f),
                new TierLoot(11, TierLoot.LootType.Armor, 0.04f),
                new TierLoot(12, TierLoot.LootType.Armor, 0.06f),
                new TierLoot(5, TierLoot.LootType.Ability, 0.05f),
                new TierLoot(6, TierLoot.LootType.Ability, 0.03f),
                new TierLoot(5, TierLoot.LootType.Ring, 0.05f)
            ),
            new Threshold(0.02f,
                new ItemLoot("Candy-Coated Armor", 0.01f)
            )
        );

        db.Init("Wishing Troll",
            new State("BaseAttack",
                new Shoot(10, 3, 15, 0, predictive: 1,
                    cooldown: 1400),
                new Grenade(radius: 5, damage: 100, range: 8, cooldown: 3000),
                new Shoot(10, 1, index: 0, predictive: 1, cooldown: 2000),
                new State("Choose",
                    new TimedTransition(3800, "Run", "Attack")
                ),
                new State("Run",
                    new StayBack(1.1f, 10),
                    new TimedTransition(1200, "Choose")
                ),
                new State("Attack",
                    new Charge(1.2f, 11, 1000),
                    new TimedTransition(1000, "Choose")
                ),
                new HealthTransition(0.6f, "NextAttack")
            ),
            new State("NextAttack",
                new Shoot(10, 5, 10, 1, predictive: 0.5f,
                    angleOffset: 0.4f, cooldown: 2000),
                new Shoot(10, 1, index: 0, predictive: 1, cooldown: 2000),
                new Shoot(10, 3, 15, 0, predictive: 1,
                    angleOffset: 1, cooldown: 4000),
                new Grenade(radius: 5, damage: 100, range: 8, cooldown: 3000),
                new State("Choose2",
                    new TimedTransition(3800, "Run2", "Attack2")
                ),
                new State("Run2",
                    new StayBack(1.5f, 10),
                    new TimedTransition(1500, "Choose2"),
                    new PlayerWithinTransition(3.5f, targetStates: "Boom", seeInvis: false)
                ),
                new State("Attack2",
                    new Charge(1.2f, 11, 1000),
                    new TimedTransition(1000, "Choose2"),
                    new PlayerWithinTransition(3.5f, targetStates: "Boom", seeInvis: false)
                ),
                new State("Boom",
                    new Shoot(0, 20, 18, 1, cooldown: 2000),
                    new TimedTransition(200, "Choose2")
                )
            ),
            new StayCloseToSpawn(1.5f, 15),
            new Prioritize(
                new Follow(1, 11, 5)
            ),
            new Wander(0.4f)
        );
        db.Init("Swoll Fairy",
            new Spawn("Fairy", 6, 0, 10000,
                givesNoXp: false),
            new StayCloseToSpawn(0.6f, 13),
            new Prioritize(
                new Follow(0.3f, 10, 5)
            ),
            new State("Shoot",
                new Shoot(11, 2, 30, 0, predictive: 1,
                    cooldown: 600),
                new TimedTransition(3000, "Pause")
            ),
            new State("ShootB",
                new Shoot(11, 8, 45, 1, cooldown: 1000),
                new TimedTransition(3000, "Pause")
            ),
            new State("Pause",
                new TimedTransition(3000, "Shoot", "ShootB")
            ),
            new Threshold(0.01f,
                new ItemLoot("Potion of Defense", 0.1f),
                new ItemLoot("Potion of Wisdom", 0.1f),
                new ItemLoot("Rock Candy", 0.08f),
                new ItemLoot("Red Gumball", 0.15f),
                new ItemLoot("Purple Gumball", 0.15f),
                new ItemLoot("Blue Gumball", 0.15f),
                new ItemLoot("Green Gumball", 0.15f),
                new ItemLoot("Yellow Gumball", 0.15f),
                new TierLoot(11, TierLoot.LootType.Weapon, 0.04f),
                new TierLoot(12, TierLoot.LootType.Weapon, 0.06f),
                new TierLoot(11, TierLoot.LootType.Armor, 0.04f),
                new TierLoot(12, TierLoot.LootType.Armor, 0.06f),
                new TierLoot(5, TierLoot.LootType.Ability, 0.05f),
                new TierLoot(6, TierLoot.LootType.Ability, 0.03f),
                new TierLoot(5, TierLoot.LootType.Ring, 0.05f)
            ),
            new Threshold(0.02f,
                new ItemLoot("Candy-Coated Armor", 0.01f)
            )
        );
        db.Init("Unicorn",
            new Prioritize(
                new Charge(1.4f, 11, 3800),
                new StayBack(0.8f, 6)
            ),
            new State("Start",
                new State("Shoot",
                    new Shoot(10, 1, index: 0, predictive: 1, cooldown: 200),
                    new TimedTransition(850, "ShootPause")
                ),
                new State("ShootPause",
                    new Shoot(4.5f, 3, 10, 1, predictive: 0.4f,
                        cooldown: 3000, cooldownOffset: 500),
                    new Shoot(4.5f, 3, 10, 1, predictive: 0.4f,
                        cooldown: 3000, cooldownOffset: 1000),
                    new Shoot(4.5f, 3, 10, 1, predictive: 0.4f,
                        cooldown: 3000, cooldownOffset: 1500),
                    new TimedTransition(1200, "Shoot")
                )
            )
        );
        db.Init("Spilled Icecream",
            new Prioritize(
                new Charge(1.4f, 11, 3800),
                new StayBack(0.8f, 6)
            ),
            new State("Start",
                new State("Shoot",
                    new Shoot(10, 1, index: 0, predictive: 1, cooldown: 200),
                    new TimedTransition(850, "ShootPause")
                ),
                new State("ShootPause",
                    new Shoot(4.5f, 3, 10, 0, predictive: 0.4f,
                        cooldown: 3000, cooldownOffset: 500),
                    new Shoot(4.5f, 3, 10, 0, predictive: 0.4f,
                        cooldown: 3000, cooldownOffset: 1000),
                    new Shoot(4.5f, 3, 10, 0, predictive: 0.4f,
                        cooldown: 3000, cooldownOffset: 1500),
                    new TimedTransition(1200, "Shoot")
                )
            )
        );
        db.Init("Gigacorn",
            new StayCloseToSpawn(1, 13),
            new Prioritize(
                new Charge(1.4f, 11, 3800),
                new StayBack(0.8f, 6)
            ),
            new State("Start",
                new State("Shoot",
                    new Shoot(10, 1, index: 0, predictive: 1, cooldown: 200),
                    new TimedTransition(2850, "ShootPause")
                ),
                new State("ShootPause",
                    new Shoot(4.5f, 3, 10, 1, predictive: 0.4f,
                        cooldown: 3000, cooldownOffset: 500),
                    new Shoot(4.5f, 3, 10, 1, predictive: 0.4f,
                        cooldown: 3000, cooldownOffset: 1000),
                    new Shoot(4.5f, 3, 10, 1, predictive: 0.4f,
                        cooldown: 3000, cooldownOffset: 1500),
                    new TimedTransition(5700, "Shoot")
                )
            ),
            new Threshold(0.01f,
                new ItemLoot("Potion of Attack", 0.1f),
                new ItemLoot("Potion of Wisdom", 0.1f),
                new ItemLoot("Rock Candy", 0.08f),
                new ItemLoot("Red Gumball", 0.15f),
                new ItemLoot("Purple Gumball", 0.15f),
                new ItemLoot("Blue Gumball", 0.15f),
                new ItemLoot("Green Gumball", 0.15f),
                new ItemLoot("Yellow Gumball", 0.15f),
                new TierLoot(11, TierLoot.LootType.Weapon, 0.04f),
                new TierLoot(12, TierLoot.LootType.Weapon, 0.06f),
                new TierLoot(11, TierLoot.LootType.Armor, 0.04f),
                new TierLoot(12, TierLoot.LootType.Armor, 0.06f),
                new TierLoot(5, TierLoot.LootType.Ability, 0.05f),
                new TierLoot(6, TierLoot.LootType.Ability, 0.03f),
                new TierLoot(5, TierLoot.LootType.Ring, 0.05f)
                ),
            new Threshold(0.02f,
            new ItemLoot("Candy-Coated Armor", 0.01f)
            )
        );
        db.Init("Candyland Boss Spawner",
            new ConditionalEffect(ConditionEffectIndex.Invincible),
            new State("Ini",
                new NoPlayerWithinTransition(16, targetStates: "Ini2")
            ),
            new State("Ini2",
                new TimedTransition(0, "Creampuff", "Unicorn", "Troll", "Rototo", "Fairy",
                    "Gumball Machine")
            ),
            new State("Ini3",
                new EntitiesNotWithinTransition(16, new[] { "Ini" }, new[]
                {
                    "Spoiled Creampuff", "Gigacorn", "Desire Troll",
                    "MegaRototo", "Swoll Fairy", "Gumball Machine"
                })
            ),
            new State("Creampuff",
                new Spawn("Spoiled Creampuff", 1, 0, givesNoXp: true),
                new TimedTransition(3000, "Ini3")
            ),
            new State("Unicorn",
                new Spawn("Gigacorn", 1, 0, givesNoXp: true),
                new TimedTransition(3000, "Ini3")
            ),
            new State("Troll",
                new Spawn("Desire Troll", 1, 0, givesNoXp: true),
                new TimedTransition(3000, "Ini3")
            ),
            new State("Rototo",
                new Spawn("MegaRototo", 1, 0, givesNoXp: true),
                new TimedTransition(3000, "Ini3")
            ),
            new State("Fairy",
                new Spawn("Swoll Fairy", 1, 0, givesNoXp: true),
                new TimedTransition(3000, "Ini3")
            ),
            new State("Gumball Machine",
                new Spawn("Gumball Machine", 1, 0, givesNoXp: true),
                new TimedTransition(3000, "Ini3")
            )
        );

        db.Init("Fairy",
            //new StayCloseToSpawn(1, 13),
            new Prioritize(
                //new Protect(1.2f, "Beefy Fairy", 15, 8,
                //    6),
                new Orbit(1.2f, 4, 7, "Beefy Fairy")
            ),
            //new Wander(0.6f),
            new Shoot(10, 2, 30, 0, predictive: 1,
                cooldown: 2000),
            new Shoot(10, 1, index: 0, predictive: 1, cooldown: 2000,
                cooldownOffset: 1000)
        );
        db.Init("Beefy Fairy",
            new StayCloseToSpawn(1, 13),
            new Prioritize(
                new Protect(1.2f, "Beefy Fairy", 15, 8,
                    6)//,
                //new Orbit(1.2f, 4, 7)
            ),
            new Follow(1f, 10f, 1.5f),
            //new Wander(0.6f),
            new Shoot(10, 2, 30, 0, predictive: 1,
                cooldown: 2000),
            new Shoot(10, 1, index: 0, predictive: 1, cooldown: 2000,
                cooldownOffset: 1000)
        );
        db.Init("Big Creampuff",
            new Spawn("Small Creampuff", 4, 0, givesNoXp: false),
            new Shoot(10, 1, index: 0, predictive: 1, cooldown: 1400),
            new Shoot(4.4f, 5, 12, 1, predictive: 0.6f,
                cooldown: 800),
            new Prioritize(
                new Charge(1.4f, 11, 4200),
                new StayBack(1, 4),
                new StayBack(0.5f, 7)
            ),
            new StayCloseToSpawn(1.35f, 13),
            new Wander(0.4f)
        );
        db.Init("Small Creampuff",
            new Shoot(5, 3, 12, 1, predictive: 0.6f,
                cooldown: 1000),
            new StayCloseToSpawn(1.3f, 13),
            new Prioritize(
                new Charge(1.3f, 13, 2500),
                new Protect(0.8f, "Big Creampuff", 15, 7,
                    6)
            ),
            new Wander(0.6f)
        );
        db.Init("Hard Candy",
            new Shoot(5, 3, 12, 0, predictive: 0.6f,
                cooldown: 1000),
            new StayCloseToSpawn(1.3f, 13),
            new Prioritize(
                new Charge(1.3f, 13, 2500),
                new Protect(0.8f, "Big Creampuff", 15, 7,
                    6)
            ),
            new Wander(0.6f)
        );
        db.Init("Tiny Rototo",
            new Prioritize(
                new Orbit(1.2f, 4, 10, "MegaRototo"),
                new Protect(0.8f, "Rototo", 15, 7,
                    6)
            ),
            new State("Main",
                new State("Unaware",
                    new Prioritize(
                        new Orbit(0.4f, 2.6f, 8, "Rototo",
                            0.2f, 0.2f, true),
                        new Wander(0.35f)
                    ),
                    new PlayerWithinTransition(3.4f, targetStates: "Attack"),
                    new HealthTransition(0.999f, "Attack")
                ),
                new State("Attack",
                    new Shoot(0, 4, 90, 1, defaultAngle: 45,
                        cooldown: 1400),
                    new Shoot(0, 4, 90, 0, cooldown: 1400),
                    new Prioritize(
                        new Follow(0.8f, 8, 3, 3000, 2000),
                        new Charge(1.35f, 11, 1000),
                        new Wander(0.35f)
                    )
                )
            )
        );
        db.Init("Butterfly",
            new ConditionalEffect(ConditionEffectIndex.Invincible),
            new StayCloseToSpawn(0.3f, 6),
            new State("Moving",
                new Wander(0.25f),
                new PlayerWithinTransition(6, targetStates: "Follow")
            ),
            new State("Follow",
                new Prioritize(
                    new StayBack(0.23f, 1.2f),
                    new Orbit(0.2f, 1.6f, 3)
                ),
                new Follow(0.2f, 7, 3),
                new Wander(0.2f),
                new NoPlayerWithinTransition(4, targetStates: "Moving")
            )
        );
    }
}