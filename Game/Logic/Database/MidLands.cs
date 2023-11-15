using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public sealed class MidLands : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Fire Sprite",
                new Reproduce(densityMax: 2),
                new Shoot(10, count: 2, shootAngle: 7, cooldown: 300),
                new Prioritize(
                    new StayAbove(1.4f, 55),
                    new Wander(.6f)
                ),
                new TierLoot(5, TierLoot.LootType.Weapon, 0.02f)
            );
            db.Init("Ice Sprite",
                new Reproduce(densityMax: 2),
                new Shoot(10, count: 3, shootAngle: 7),
                new Prioritize(
                    new StayAbove(1.4f, 60),
                    new Wander(.6f)
                ),
                new TierLoot(2, TierLoot.LootType.Ability, 0.3f)
            );
            db.Init("Magic Sprite",
                new Reproduce(densityMax: 2),
                new Shoot(10, count: 4, shootAngle: 7),
                new Prioritize(
                    new StayAbove(1.4f, 60),
                    new Wander(.6f)
                ),
                new TierLoot(6, TierLoot.LootType.Armor, 0.3f)
            );
            db.Init("Orc King",
                new DropPortalOnDeath("Spider Den Portal", 0.1f),
                new Shoot(3),
                new Spawn("Orc Queen", maxChildren: 2, cooldown: 60000, givesNoXp: false),
                new Prioritize(
                    new StayAbove(1.4f, 60),
                    new Follow(0.6f, range: 1, duration: 3000, cooldown: 3000),
                    new Wander(0.6f)
                ),
                new TierLoot(4, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(5, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(5, TierLoot.LootType.Armor, 0.4f),
                new TierLoot(6, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(2, TierLoot.LootType.Ring, 0.3f),
                new TierLoot(2, TierLoot.LootType.Ability, 0.3f)
            );
            db.Init("Orc Queen",
                new Spawn("Orc Mage", maxChildren: 2, cooldown: 8000, givesNoXp: false),
                new Spawn("Orc Warrior", maxChildren: 3, cooldown: 8000, givesNoXp: false),
                new Prioritize(
                    new StayAbove(1.4f, 60),
                    new Protect(0.8f, "Orc King", acquireRange: 11, protectionRange: 7, reprotectRange: 5.4f),
                    new Wander(0.8f)
                ),
                new HealGroup(10, "OrcKings", 300)
            );
            db.Init("Orc Mage",
                new State("circle_player",
                    new Shoot(8, predictive: 0.3f, cooldown: 1000, cooldownOffset: 500),
                    new Prioritize(
                        new StayAbove(1.4f, 60),
                        new Protect(0.7f, "Orc Queen", acquireRange: 11, protectionRange: 10, reprotectRange: 3),
                        new Orbit(0.7f, 3.5f, acquireRange: 11)
                    ),
                    new TimedTransition(3500, "circle_queen")
                ),
                new State("circle_queen",
                    new Shoot(8, count: 3, predictive: 0.3f, shootAngle: 120, cooldown: 1000, cooldownOffset: 500),
                    new Prioritize(
                        new StayAbove(1.4f, 60),
                        new Orbit(1.2f, 2.5f, target: "Orc Queen", acquireRange: 12, speedVariance: 0.1f,
                            radiusVariance: 0.1f)
                    ),
                    new TimedTransition(3500, "circle_player")
                )
            );
            db.Init("Orc Warrior",
                new Shoot(3, predictive: 1, cooldown: 500),
                new Prioritize(
                    new StayAbove(1.4f, 60),
                    new Orbit(1.35f, 2.5f, target: "Orc Queen", acquireRange: 12, speedVariance: 0.1f,
                        radiusVariance: 0.1f)
                )
            );
            db.Init("Pink Blob",
                new StayAbove(0.4f, 50),
                new Shoot(6, count: 3, shootAngle: 7),
                new Prioritize(
                    new Follow(0.8f, acquireRange: 15, range: 5),
                    new Wander(0.4f)
                ),
                new Reproduce(densityMax: 5, densityRadius: 10)
            );
            db.Init("Gray Blob",
                new State("searching",
                    new StayAbove(0.2f, 50),
                    new Prioritize(
                        new Charge(2),
                        new Wander(0.4f)
                    ),
                    new Reproduce(densityMax: 5, densityRadius: 10),
                    new PlayerWithinTransition(2, targetStates: "creeping")
                ),
                new State("creeping",
                    new Shoot(0, count: 10, shootAngle: 36, fixedAngle: 0),
                    new Decay(0)
                )
            );
            db.Init("Big Green Slime",
                new StayAbove(0.4f, 50),
                new Shoot(9),
                new Wander(0.4f),
                new Reproduce(densityMax: 5, densityRadius: 10),
                new TransformOnDeath("Little Green Slime"),
                new TransformOnDeath("Little Green Slime"),
                new TransformOnDeath("Little Green Slime"),
                new TransformOnDeath("Little Green Slime")
            );
            db.Init("Little Green Slime",
                new StayAbove(0.4f, 50),
                new Shoot(6),
                new Wander(0.4f),
                new Protect(0.4f, "Big Green Slime")
            );
            db.Init("Wasp Queen",
                new Spawn("Worker Wasp", maxChildren: 5, cooldown: 3400, givesNoXp: false),
                new Spawn("Warrior Wasp", maxChildren: 2, cooldown: 4400, givesNoXp: false),
                new State("idle",
                    new StayAbove(0.4f, 60),
                    new Wander(0.55f),
                    new PlayerWithinTransition(10, targetStates: "froth")
                ),
                new State("froth",
                    new Shoot(8, predictive: 0.1f, cooldown: 1600),
                    new Prioritize(
                        new StayAbove(0.4f, 60),
                        new Wander(0.55f)
                    )
                ),
                new TierLoot(5, TierLoot.LootType.Weapon, 0.14f),
                new TierLoot(6, TierLoot.LootType.Weapon, 0.05f),
                new TierLoot(5, TierLoot.LootType.Armor, 0.19f),
                new TierLoot(6, TierLoot.LootType.Armor, 0.02f),
                new TierLoot(2, TierLoot.LootType.Ring, 0.07f),
                new TierLoot(3, TierLoot.LootType.Ring, 0.001f),
                new TierLoot(2, TierLoot.LootType.Ability, 0.28f),
                new TierLoot(3, TierLoot.LootType.Ability, 0.01f)
            );
            db.Init("Worker Wasp",
                new Shoot(8, cooldown: 4000),
                new Prioritize(
                    new Orbit(1, 2, target: "Wasp Queen", radiusVariance: 0.5f),
                    new Wander(0.75f)
                )
            );
            db.Init("Warrior Wasp",
                new Shoot(8, predictive: 200, cooldown: 1000),
                new State("protecting",
                    new Prioritize(
                        new Orbit(1, 2, target: "Wasp Queen", radiusVariance: 0),
                        new Wander(0.75f)
                    ),
                    new TimedTransition(3000, "attacking")
                ),
                new State("attacking",
                    new Prioritize(
                        new Follow(0.8f, acquireRange: 9, range: 3.4f),
                        new Orbit(1, 2, target: "Wasp Queen", radiusVariance: 0),
                        new Wander(0.75f)
                    ),
                    new TimedTransition(2200, "protecting")
                )
            );
            db.Init("Shambling Sludge",
                new State("idle",
                    new StayAbove(0.5f, 55),
                    new PlayerWithinTransition(10, targetStates: "toss_sludge")
                ),
                new State("toss_sludge",
                    new Prioritize(
                        new StayAbove(0.5f, 55),
                        new Wander(0.5f)
                    ),
                    new Shoot(8, cooldown: 1200),
                    new TossObject("Sludget", range: 3, angle: 20, cooldown: 100000),
                    new TossObject("Sludget", range: 3, angle: 92, cooldown: 100000),
                    new TossObject("Sludget", range: 3, angle: 164, cooldown: 100000),
                    new TossObject("Sludget", range: 3, angle: 236, cooldown: 100000),
                    new TossObject("Sludget", range: 3, angle: 308, cooldown: 100000),
                    new TimedTransition(8000, "pause")
                ),
                new State("pause",
                    new Prioritize(
                        new StayAbove(0.5f, 55),
                        new Wander(0.5f)
                    ),
                    new TimedTransition(targetStates: "idle")
                ),
                new TierLoot(4, TierLoot.LootType.Weapon, 0.4f),
                new TierLoot(5, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(5, TierLoot.LootType.Armor, 0.4f),
                new TierLoot(6, TierLoot.LootType.Armor, 0.4f),
                new TierLoot(2, TierLoot.LootType.Ring, 0.4f),
                new TierLoot(2, TierLoot.LootType.Ability, 0.4f)
            );
            db.Init("Sludget",
                new State("idle",
                    new Shoot(8, predictive: 0.5f, cooldown: 600),
                    new Prioritize(
                        new Protect(0.5f, "Shambling Sludge", 11, 7.5f, 7.4f),
                        new Wander(0.5f)
                    ),
                    new TimedTransition(1400, "wander")
                ),
                new State("wander",
                    new Prioritize(
                        new Protect(0.5f, "Shambling Sludge", 11, 7.5f, 7.4f),
                        new Wander(0.5f)
                    ),
                    new TimedTransition(5400, "jump")
                ),
                new State("jump",
                    new Prioritize(
                        new Protect(0.5f, "Shambling Sludge", 11, 7.5f, 7.4f),
                        new Follow(7, acquireRange: 6, range: 1),
                        new Wander(0.5f)
                    ),
                    new TimedTransition(200, "attack")
                ),
                new State("attack",
                    new Shoot(8, predictive: 0.5f, cooldown: 600, cooldownOffset: 300),
                    new Prioritize(
                        new Protect(0.5f, "Shambling Sludge", 11, 7.5f, 7.4f),
                        new Follow(0.5f, acquireRange: 6, range: 1),
                        new Wander(0.5f)
                    ),
                    new TimedTransition(4000, "idle")
                ),
                new Decay(9000)
            );
            db.Init("Swarm",
                new State("circle",
                    new Prioritize(
                        new StayAbove(0.4f, 60),
                        new Follow(4, acquireRange: 11, range: 3.5f, duration: 1000, cooldown: 5000),
                        new Orbit(1.9f, 3.5f, acquireRange: 12),
                        new Wander(0.4f)
                    ),
                    new Shoot(4, predictive: 0.1f, cooldown: 500),
                    new TimedTransition(3000, "dart_away")
                ),
                new State("dart_away",
                    new Prioritize(
                        new StayAbove(0.4f, 60),
                        new StayBack(2, distance: 5),
                        new Wander(0.4f)
                    ),
                    new Shoot(8, count: 5, shootAngle: 72, fixedAngle: 20, cooldown: 100000, cooldownOffset: 800),
                    new Shoot(8, count: 5, shootAngle: 72, fixedAngle: 56, cooldown: 100000, cooldownOffset: 1400),
                    new TimedTransition(1600, "circle")
                ),
                new Reproduce(densityMax: 1, densityRadius: 100),
                new TierLoot(3, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(4, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(3, TierLoot.LootType.Armor, 0.32f),
                new TierLoot(4, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(5, TierLoot.LootType.Armor, 0.32f),
                new TierLoot(1, TierLoot.LootType.Ring, 0.3f),
                new TierLoot(1, TierLoot.LootType.Ability, 0.3f)
            );
            db.Init("Black Bat",
                new Prioritize(
                    new Charge(4f),
                    new Wander(0.4f)
                ),
                new Shoot(1),
                new Reproduce(null, densityMax: 5, densityRadius: 20, cooldown: 20000, cooldownVariance: 0),
                new TierLoot(2, TierLoot.LootType.Armor, 0.01f)
            );
            db.Init("Red Spider",
                new Wander(0.8f),
                new Shoot(9),
                new Reproduce(null, densityMax: 3, densityRadius: 15, cooldown: 45000, cooldownVariance: 0)
            );
            db.Init("Dwarf Axebearer",
                new Shoot(3.4f),
                new State("Default",
                    new Wander(0.4f)
                ),
                new State("Circling",
                    new Prioritize(
                        new Orbit(0.4f, 2.7f, acquireRange: 11),
                        new Protect(1.2f, "Dwarf King", acquireRange: 15, protectionRange: 6, reprotectRange: 3),
                        new Wander(0.4f)
                    ),
                    new TimedTransition(3300, "Default"),
                    new EntityNotWithinTransition("Dwarf King", 8, "Default")
                ),
                new State("Engaging",
                    new Prioritize(
                        new Follow(1f, acquireRange: 15, range: 1),
                        new Protect(1.2f, "Dwarf King", acquireRange: 15, protectionRange: 6, reprotectRange: 3),
                        new Wander(0.4f)
                    ),
                    new TimedTransition(2500, "Circling"),
                    new EntityNotWithinTransition("Dwarf King", 8, "Default")
                )
            );
            db.Init("Dwarf Mage",
                new State("Default",
                    new Prioritize(
                        new Protect(1.2f, "Dwarf King", acquireRange: 15, protectionRange: 6, reprotectRange: 3),
                        new Wander(0.6f)
                    ),
                    new State("fire1_def",
                        new Shoot(10, predictive: 0.2f, cooldown: 100000),
                        new TimedTransition(1500, "fire2_def")
                    ),
                    new State("fire2_def",
                        new Shoot(5, predictive: 0.2f, cooldown: 100000),
                        new TimedTransition(1500, "fire1_def")
                    )
                ),
                new State("Circling",
                    new Prioritize(
                        new Orbit(0.4f, 2.7f, acquireRange: 11),
                        new Protect(1.2f, "Dwarf King", acquireRange: 15, protectionRange: 6, reprotectRange: 3),
                        new Wander(0.6f)
                    ),
                    new State("fire1_cir",
                        new Shoot(10, predictive: 0.2f, cooldown: 100000),
                        new TimedTransition(1500, "fire2_cir")
                    ),
                    new State("fire2_cir",
                        new Shoot(5, predictive: 0.2f, cooldown: 100000),
                        new TimedTransition(1500, "fire1_cir")
                    ),
                    new TimedTransition(3300, "Default"),
                    new EntityNotWithinTransition("Dwarf King", 8, "Default")
                ),
                new State("Engaging",
                    new Prioritize(
                        new Follow(1f, acquireRange: 15, range: 1),
                        new Protect(1.2f, "Dwarf King", acquireRange: 15, protectionRange: 6, reprotectRange: 3),
                        new Wander(0.4f)
                    ),
                    new State("fire1_eng",
                        new Shoot(10, predictive: 0.2f, cooldown: 100000),
                        new TimedTransition(1500, "fire2_eng")
                    ),
                    new State("fire2_eng",
                        new Shoot(5, predictive: 0.2f, cooldown: 100000),
                        new TimedTransition(1500, "fire1_eng")
                    ),
                    new TimedTransition(2500, "Circling"),
                    new EntityNotWithinTransition("Dwarf King", 8, "Default")
                )
            );
            db.Init("Dwarf Veteran",
                new Shoot(4),
                new State("Default",
                    new Prioritize(
                        new Follow(1f, acquireRange: 9, range: 2, duration: 3000, cooldown: 1000),
                        new Wander(0.4f)
                    )
                ),
                new State("Circling",
                    new Prioritize(
                        new Orbit(0.4f, 2.7f, acquireRange: 11),
                        new Protect(1.2f, "Dwarf King", acquireRange: 15, protectionRange: 6, reprotectRange: 3),
                        new Wander(0.4f)
                    ),
                    new TimedTransition(3300, "Default"),
                    new EntityNotWithinTransition("Dwarf King", 8, "Default")
                ),
                new State("Engaging",
                    new Prioritize(
                        new Follow(1f, acquireRange: 15, range: 1),
                        new Protect(1.2f, "Dwarf King", acquireRange: 15, protectionRange: 6, reprotectRange: 3),
                        new Wander(0.4f)
                    ),
                    new TimedTransition(2500, "Circling"),
                    new EntityNotWithinTransition("Dwarf King", 8, "Default")
                )
            );
            db.Init("Dwarf King",
                new SpawnGroup("Dwarves", maxChildren: 10, cooldown: 8000),
                new Shoot(4, cooldown: 2000),
                new State("Circling",
                    new Prioritize(
                        new Orbit(0.4f, 2.7f, acquireRange: 11),
                        new Wander(0.4f)
                    ),
                    new TimedTransition(3400, "Engaging")
                ),
                new State("Engaging",
                    new Taunt(0.2f, "You'll taste my axe!"),
                    new Prioritize(
                        new Follow(1f, acquireRange: 15, range: 1),
                        new Wander(0.4f)
                    ),
                    new TimedTransition(2600, "Circling")
                ),
                new TierLoot(4, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(5, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(4, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(5, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(6, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(2, TierLoot.LootType.Ring, 0.3f),
                new TierLoot(1, TierLoot.LootType.Ability, 0.5f)
            );
            db.Init("Werelion",
                new DropPortalOnDeath("Spider Den Portal", 0.1f),
                new Spawn("Weretiger", maxChildren: 1, cooldown: 23000, givesNoXp: false),
                new Spawn("Wereleopard", maxChildren: 2, cooldown: 9000, givesNoXp: false),
                new Spawn("Werepanther", maxChildren: 3, cooldown: 15000, givesNoXp: false),
                new Shoot(4, cooldown: 2000),
                new State("idle",
                    new Prioritize(
                        new StayAbove(0.6f, 60),
                        new Wander(0.6f)
                    ),
                    new PlayerWithinTransition(11, targetStates: "player_nearby")
                ),
                new State("player_nearby",
                    new State("normal_attack",
                        new Shoot(10, count: 3, shootAngle: 15, predictive: 1, cooldown: 10000),
                        new TimedTransition(900, "if_cloaked")
                    ),
                    new State("if_cloaked",
                        new Shoot(10, count: 8, shootAngle: 45, defaultAngle: 20, cooldown: 1600,
                            cooldownOffset: 400),
                        new Shoot(10, count: 8, shootAngle: 45, defaultAngle: 42, cooldown: 1600,
                            cooldownOffset: 1200),
                        new PlayerWithinTransition(10, targetStates: "normal_attack")
                    ),
                    new Prioritize(
                        new StayAbove(0.6f, 60),
                        new Follow(0.4f, acquireRange: 7, range: 3),
                        new Wander(0.6f)
                    ),
                    new TimedTransition(30000, "idle")
                ),
                new TierLoot(4, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(5, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(5, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(6, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(2, TierLoot.LootType.Ring, 0.3f),
                new TierLoot(2, TierLoot.LootType.Ability, 0.5f)
            );
            db.Init("Weretiger",
                new Shoot(8, predictive: 0.3f, cooldown: 1000),
                new Prioritize(
                    new StayAbove(0.6f, 60),
                    new Protect(1.1f, "Werelion", acquireRange: 12, protectionRange: 10, reprotectRange: 5),
                    new Follow(0.8f, range: 6.3f),
                    new Wander(0.6f)
                )
            );
            db.Init("Wereleopard",
                new Shoot(4.5f, predictive: 0.4f, cooldown: 900),
                new Prioritize(
                    new Protect(1.1f, "Werelion", acquireRange: 12, protectionRange: 10, reprotectRange: 5),
                    new Follow(1.1f, range: 3),
                    new Wander(.6f)
                )
            );
            db.Init("Werepanther",
                new State("idle",
                    new Protect(0.65f, "Werelion", acquireRange: 11, protectionRange: 7.5f, reprotectRange: 7.4f),
                    new PlayerWithinTransition(9.5f, targetStates: "wander")
                ),
                new State("wander",
                    new Prioritize(
                        new Protect(0.65f, "Werelion", acquireRange: 11, protectionRange: 7.5f, reprotectRange: 7.4f),
                        new Follow(0.65f, range: 5, acquireRange: 10),
                        new Wander(0.65f)
                    ),
                    new PlayerWithinTransition(4, targetStates: "jump")
                ),
                new State("jump",
                    new Prioritize(
                        new Protect(0.65f, "Werelion", acquireRange: 11, protectionRange: 7.5f, reprotectRange: 7.4f),
                        new Follow(7, range: 1, acquireRange: 6),
                        new Wander(0.55f)
                    ),
                    new TimedTransition(200, "attack")
                ),
                new State("attack",
                    new Prioritize(
                        new Protect(0.65f, "Werelion", acquireRange: 11, protectionRange: 7.5f, reprotectRange: 7.4f),
                        new Follow(0.5f, range: 1, acquireRange: 6),
                        new Wander(0.5f)
                    ),
                    new Shoot(4, predictive: 0.5f, cooldown: 800, cooldownOffset: 300),
                    new TimedTransition(4000, "idle")
                )
            );
            db.Init("Horned Drake",
                new Spawn("Drake Baby", maxChildren: 1, initialSpawn: 1, cooldown: 50000, givesNoXp: false),
                new State("idle",
                    new StayAbove(0.8f, 60),
                    new PlayerWithinTransition(10, targetStates: "get_player")
                ),
                new State("get_player",
                    new Prioritize(
                        new StayAbove(0.8f, 60),
                        new Follow(0.8f, range: 2.7f, acquireRange: 10, duration: 5000, cooldown: 1800),
                        new Wander(0.6f)
                    ),
                    new State("one_shot",
                        new Shoot(8, predictive: 0.1f, cooldown: 800),
                        new TimedTransition(900, "three_shot")
                    ),
                    new State("three_shot",
                        new Shoot(8, count: 3, shootAngle: 40, predictive: 0.1f, cooldown: 100000,
                            cooldownOffset: 800),
                        new TimedTransition(2000, "one_shot")
                    )
                ),
                new State("protect_me",
                    new Protect(0.8f, "Drake Baby", acquireRange: 12, protectionRange: 2.5f, reprotectRange: 1.5f),
                    new State("one_shot",
                        new Shoot(8, predictive: 0.1f, cooldown: 700),
                        new TimedTransition(800, "three_shot")
                    ),
                    new State("three_shot",
                        new Shoot(8, count: 3, shootAngle: 40, predictive: 0.1f, cooldown: 100000,
                            cooldownOffset: 700),
                        new TimedTransition(1800, "one_shot")
                    ),
                    new EntityNotWithinTransition("Drake Baby", 8, "idle")
                ),
                new TierLoot(5, TierLoot.LootType.Weapon, 0.14f),
                new TierLoot(6, TierLoot.LootType.Weapon, 0.05f),
                new TierLoot(5, TierLoot.LootType.Armor, 0.19f),
                new TierLoot(6, TierLoot.LootType.Armor, 0.02f),
                new TierLoot(2, TierLoot.LootType.Ring, 0.07f),
                new TierLoot(3, TierLoot.LootType.Ring, 0.001f),
                new TierLoot(2, TierLoot.LootType.Ability, 0.28f),
                new TierLoot(3, TierLoot.LootType.Ability, 0.001f)
            );
            db.Init("Drake Baby",
                new State("unharmed",
                    new Shoot(8, cooldown: 1500),
                    new State("wander",
                        new Prioritize(
                            new StayAbove(0.8f, 60),
                            new Wander(0.6f)
                        ),
                        new TimedTransition(2000, "find_mama")
                    ),
                    new State("find_mama",
                        new Prioritize(
                            new StayAbove(0.8f, 60),
                            new Protect(1.4f, "Horned Drake", acquireRange: 15, protectionRange: 4,
                                reprotectRange: 4)
                        ),
                        new TimedTransition(2000, "wander")
                    ),
                    new HealthTransition(0.65f, "call_mama")
                ),
                new State("call_mama",
                    new Flash(0xff484848, 0.6f, 5000),
                    new State("get_close_to_mama",
                        new Taunt("Awwwk! Awwwk!"),
                        new Protect(1.4f, "Horned Drake", acquireRange: 15, protectionRange: 1, reprotectRange: 1),
                        new TimedTransition(1500, "cry_for_mama")
                    ),
                    new State("cry_for_mama",
                        new StayBack(0.65f),
                        new Order(8, "Horned Drake", "protect_me")
                    )
                )
            );
            db.Init("Nomadic Shaman",
                new Prioritize(
                    new StayAbove(0.8f, 55),
                    new Wander(0.6f)
                ),
                new State("fire1",
                    new Shoot(10, index: 0, count: 3, shootAngle: 11, cooldown: 500, cooldownOffset: 500),
                    new TimedTransition(3100, "fire2")
                ),
                new State("fire2",
                    new Shoot(10, index: 1, cooldown: 700, cooldownOffset: 700),
                    new TimedTransition(2200, "fire1")
                )
            );
            db.Init("Sand Phantom",
                new Prioritize(
                    new StayAbove(0.85f, 60),
                    new Follow(0.85f, acquireRange: 10.5f, range: 1),
                    new Wander(0.6f)
                ),
                new Shoot(8, predictive: 0.4f, cooldown: 400, cooldownOffset: 600),
                new State("follow_player",
                    new PlayerWithinTransition(4.4f, targetStates: "sneak_away_from_player")
                ),
                new State("sneak_away_from_player",
                    new Transform("Sand Phantom Wisp")
                )
            );
            db.Init("Sand Phantom Wisp",
                new Shoot(8, predictive: 0.4f, cooldown: 400, cooldownOffset: 600),
                new State("move_away_from_player",
                    new State("keep_back",
                        new Prioritize(
                            new StayBack(0.6f, distance: 5),
                            new Wander(0.6f)
                        ),
                        new TimedTransition(800, "wander")
                    ),
                    new State("wander",
                        new Wander(0.6f),
                        new TimedTransition(800, "keep_back")
                    ),
                    new TimedTransition(6500, "wisp_finished")
                ),
                new State("wisp_finished",
                    new Transform("Sand Phantom")
                )
            );
            db.Init("Great Lizard",
                new State("idle",
                    new StayAbove(0.6f, 60),
                    new Wander(0.6f),
                    new PlayerWithinTransition(10, targetStates: "charge")
                ),
                new State("charge",
                    new Prioritize(
                        new StayAbove(0.6f, 60),
                        new Follow(6, acquireRange: 11, range: 1.5f)
                    ),
                    new TimedTransition(200, "spit")
                ),
                new State("spit",
                    new Shoot(8, index: 0, count: 1, cooldown: 100000, cooldownOffset: 1000),
                    new Shoot(8, index: 0, count: 2, shootAngle: 16, cooldown: 100000,
                        cooldownOffset: 1200),
                    new Shoot(8, index: 0, count: 1, predictive: 0.2f, cooldown: 100000,
                        cooldownOffset: 1600),
                    new Shoot(8, index: 0, count: 2, shootAngle: 24, cooldown: 100000,
                        cooldownOffset: 2200),
                    new Shoot(8, index: 0, count: 1, predictive: 0.2f, cooldown: 100000,
                        cooldownOffset: 2800),
                    new Shoot(8, index: 0, count: 2, shootAngle: 16, cooldown: 100000,
                        cooldownOffset: 3200),
                    new Shoot(8, index: 0, count: 1, predictive: 0.1f, cooldown: 100000,
                        cooldownOffset: 3800),
                    new Prioritize(
                        new StayAbove(0.6f, 60),
                        new Wander(0.6f)
                    ),
                    new TimedTransition(5000, "flame_ring")
                ),
                new State("flame_ring",
                    new Shoot(7, index: 1, count: 30, shootAngle: 12, cooldown: 400, cooldownOffset: 600),
                    new Prioritize(
                        new StayAbove(0.6f, 60),
                        new Follow(0.6f, acquireRange: 9, range: 1),
                        new Wander(0.6f)
                    ),
                    new TimedTransition(3500, "pause")
                ),
                new State("pause",
                    new Prioritize(
                        new StayAbove(0.6f, 60),
                        new Wander(0.6f)
                    ),
                    new TimedTransition(targetStates: "idle")
                ),
                new TierLoot(4, TierLoot.LootType.Weapon, 0.14f),
                new TierLoot(5, TierLoot.LootType.Weapon, 0.05f),
                new TierLoot(5, TierLoot.LootType.Armor, 0.19f),
                new TierLoot(6, TierLoot.LootType.Armor, 0.02f),
                new TierLoot(2, TierLoot.LootType.Ring, 0.07f),
                new TierLoot(2, TierLoot.LootType.Ability, 0.27f)
            );
            db.Init("Tawny Warg",
                new Shoot(3.4f),
                new Prioritize(
                    new Protect(1.2f, "Desert Werewolf", acquireRange: 14, protectionRange: 8, reprotectRange: 5),
                    new Follow(0.7f, acquireRange: 9, range: 2),
                    new Wander(0.6f)
                )
            );
            db.Init("Demon Warg",
                new Shoot(4.5f),
                new Prioritize(
                    new Protect(1.2f, "Desert Werewolf", acquireRange: 14, protectionRange: 8, reprotectRange: 5),
                    new Wander(0.6f)
                )
            );
            db.Init("Desert Werewolf",
                new SpawnGroup("Wargs", maxChildren: 8, cooldown: 8000),
                new State("unharmed",
                    new Shoot(8, index: 0, predictive: 0.3f, cooldown: 1000, cooldownOffset: 500),
                    new Prioritize(
                        new Follow(0.5f, acquireRange: 10.5f, range: 2.5f),
                        new Wander(0.5f)
                    ),
                    new HealthTransition(0.75f, "enraged")
                ),
                new State("enraged",
                    new Shoot(8, index: 0, predictive: 0.3f, cooldown: 1000, cooldownOffset: 500),
                    new Taunt(0.7f, "GRRRRAAGH!"),
                    new ChangeSize(20, 170),
                    new Flash(0xffff0000, 0.4f, 5000),
                    new Prioritize(
                        new Follow(0.65f, acquireRange: 9, range: 2),
                        new Wander(0.65f)
                    )
                ),
                new TierLoot(3, TierLoot.LootType.Weapon, 0.2f),
                new TierLoot(4, TierLoot.LootType.Weapon, 0.12f),
                new TierLoot(5, TierLoot.LootType.Armor, 0.2f),
                new TierLoot(4, TierLoot.LootType.Armor, 0.15f),
                new TierLoot(5, TierLoot.LootType.Armor, 0.02f),
                new TierLoot(1, TierLoot.LootType.Ring, 0.11f),
                new TierLoot(2, TierLoot.LootType.Ability, 0.38f)
            );
            db.Init("Red Satellite",
                new Prioritize(
                    new Orbit(1.7f, 2, target: "Fire Golem", acquireRange: 15, speedVariance: 0, radiusVariance: 0),
                    new Orbit(1.7f, 2, target: "Metal Golem", acquireRange: 15, speedVariance: 0, radiusVariance: 0)
                ),
                new Decay(16000)
            );
            db.Init("Green Satellite",
                new Prioritize(
                    new Orbit(1.1f, 2, target: "Darkness Golem", acquireRange: 15, speedVariance: 0,
                        radiusVariance: 0),
                    new Orbit(1.1f, 2, target: "Earth Golem", acquireRange: 15, speedVariance: 0, radiusVariance: 0)
                ),
                new Decay(16000)
            );
            db.Init("Blue Satellite",
                new Prioritize(
                    new Orbit(1.1f, 2, target: "Clockwork Golem", acquireRange: 15, speedVariance: 0,
                        radiusVariance: 0),
                    new Orbit(1.1f, 2, target: "Paper Golem", acquireRange: 15, speedVariance: 0, radiusVariance: 0)
                ),
                new Decay(16000)
            );
            db.Init("Gray Satellite 1",
                new Shoot(6, count: 3, shootAngle: 34, predictive: 0.3f, cooldown: 850),
                new Prioritize(
                    new Orbit(2.2f, 0.75f, target: "Red Satellite", acquireRange: 15, speedVariance: 0,
                        radiusVariance: 0),
                    new Orbit(2.2f, 0.75f, target: "Blue Satellite", acquireRange: 15, speedVariance: 0,
                        radiusVariance: 0)
                ),
                new Decay(16000)
            );
            db.Init("Gray Satellite 2",
                new Shoot(7, predictive: 0.3f, cooldown: 600),
                new Prioritize(
                    new Orbit(2.2f, 0.75f, target: "Green Satellite", acquireRange: 15, speedVariance: 0,
                        radiusVariance: 0),
                    new Orbit(2.2f, 0.75f, target: "Blue Satellite", acquireRange: 15, speedVariance: 0,
                        radiusVariance: 0)
                ),
                new Decay(16000)
            );
            db.Init("Gray Satellite 3",
                new Shoot(7, count: 5, shootAngle: 72, cooldown: 3200, cooldownOffset: 600),
                new Shoot(7, count: 4, shootAngle: 90, cooldown: 3200, cooldownOffset: 1400),
                new Shoot(7, count: 5, shootAngle: 72, defaultAngle: 36, cooldown: 3200, cooldownOffset: 2200),
                new Shoot(7, count: 4, shootAngle: 90, defaultAngle: 45, cooldown: 3200, cooldownOffset: 3000),
                new Prioritize(
                    new Orbit(2.2f, 0.75f, target: "Red Satellite", acquireRange: 15, speedVariance: 0,
                        radiusVariance: 0),
                    new Orbit(2.2f, 0.75f, target: "Green Satellite", acquireRange: 15, speedVariance: 0,
                        radiusVariance: 0)
                ),
                new Decay(16000)
            );
            db.Init("Earth Golem",
                new State("idle",
                    new PlayerWithinTransition(11, targetStates: "player_nearby")
                ),
                new State("player_nearby",
                    new Shoot(8, count: 2, shootAngle: 12, cooldown: 600),
                    new State("first_satellites",
                        new Spawn("Green Satellite", maxChildren: 1, cooldown: 200),
                        new Spawn("Gray Satellite 3", maxChildren: 1, cooldown: 200),
                        new TimedTransition(300, "next_satellite")
                    ),
                    new State("next_satellite",
                        new Spawn("Gray Satellite 3", maxChildren: 1, cooldown: 200),
                        new TimedTransition(200, "follow")
                    ),
                    new State("follow",
                        new Prioritize(
                            new StayAbove(1.4f, 65),
                            new Follow(1.4f, range: 3),
                            new Wander(0.6f)
                        ),
                        new TimedTransition(2000, "wander1")
                    ),
                    new State("wander1",
                        new Prioritize(
                            new StayAbove(1.55f, 65),
                            new Wander(0.55f)
                        ),
                        new TimedTransition(4000, "circle")
                    ),
                    new State("circle",
                        new Prioritize(
                            new StayAbove(1.2f, 65),
                            new Orbit(1.2f, 5.4f, acquireRange: 11)
                        ),
                        new TimedTransition(4000, "wander2")
                    ),
                    new State("wander2",
                        new Prioritize(
                            new StayAbove(0.55f, 65),
                            new Wander(0.55f)
                        ),
                        new TimedTransition(3000, "back_and_forth")
                    ),
                    new State("back_and_forth",
                        new Prioritize(
                            new StayAbove(0.55f, 65),
                            new BackAndForth(0.8f)
                        ),
                        new TimedTransition(3000, "first_satellites")
                    )
                ),
                new Reproduce(densityMax: 1),
                new TierLoot(2, TierLoot.LootType.Ring, 0.02f)
            );
            db.Init("Paper Golem",
                new State("idle",
                    new PlayerWithinTransition(11, targetStates: "player_nearby")
                ),
                new State("player_nearby",
                    new Spawn("Blue Satellite", maxChildren: 1, cooldown: 200),
                    new Spawn("Gray Satellite 1", maxChildren: 1, cooldown: 200),
                    new Shoot(10, predictive: 0.5f, cooldown: 700),
                    new Prioritize(
                        new StayAbove(1.4f, 65),
                        new Follow(1, range: 3, duration: 3000, cooldown: 3000),
                        new Wander(0.4f)
                    ),
                    new TimedTransition(12000, "idle")
                ),
                new Reproduce(densityMax: 1),
                new TierLoot(5, TierLoot.LootType.Weapon, 0.02f)
            );
            db.Init("Fire Golem",
                new State("idle",
                    new PlayerWithinTransition(11, targetStates: "player_nearby")
                ),
                new State("player_nearby",
                    new Prioritize(
                        new StayAbove(1.4f, 65),
                        new Follow(1, range: 3, duration: 3000, cooldown: 3000),
                        new Wander(0.4f)
                    ),
                    new Spawn("Red Satellite", maxChildren: 1, cooldown: 200),
                    new Spawn("Gray Satellite 1", maxChildren: 1, cooldown: 200),
                    new State("slowshot",
                        new Shoot(10, index: 0, predictive: 0.5f, cooldown: 300, cooldownOffset: 600),
                        new TimedTransition(5000, "megashot")
                    ),
                    new State("megashot",
                        new Flash(0xffffffff, 0.2f, 5),
                        new Shoot(10, index: 1, predictive: 0.2f, cooldown: 90, cooldownOffset: 1000),
                        new TimedTransition(1200, "slowshot")
                    )
                ),
                new Reproduce(densityMax: 1),
                new TierLoot(6, TierLoot.LootType.Armor, 0.015f)
            );
            db.Init("Darkness Golem",
                new State("idle",
                    new PlayerWithinTransition(11, targetStates: "player_nearby")
                ),
                new State("player_nearby",
                    new State("first_satellites",
                        new Spawn("Green Satellite", maxChildren: 1, cooldown: 200),
                        new Spawn("Gray Satellite 2", maxChildren: 1, cooldown: 200),
                        new TimedTransition(200, "next_satellite")
                    ),
                    new State("next_satellite",
                        new Spawn("Gray Satellite 2", maxChildren: 1, cooldown: 200),
                        new TimedTransition(200, "follow")
                    ),
                    new State("follow",
                        new Shoot(6, index: 0, cooldown: 200),
                        new Prioritize(
                            new StayAbove(1.2f, 65),
                            new Follow(1.2f, range: 1),
                            new Wander(0.5f)
                        ),
                        new TimedTransition(3000, "wander1")
                    ),
                    new State("wander1",
                        new Shoot(6, index: 0, cooldown: 200),
                        new Prioritize(
                            new StayAbove(0.65f, 65),
                            new Wander(0.65f)
                        ),
                        new TimedTransition(3800, "back_up")
                    ),
                    new State("back_up",
                        new Flash(0xffffffff, 0.2f, 25),
                        new Shoot(9, index: 1, cooldown: 1400, cooldownOffset: 1000),
                        new Prioritize(
                            new StayAbove(0.4f, 65),
                            new StayBack(0.4f, 4),
                            new Wander(0.4f)
                        ),
                        new TimedTransition(5400, "wander2")
                    ),
                    new State("wander2",
                        new Shoot(6, index: 0, cooldown: 200),
                        new Prioritize(
                            new StayAbove(0.65f, 65),
                            new Wander(0.65f)
                        ),
                        new TimedTransition(3800, "first_satellites")
                    )
                ),
                new Reproduce(densityMax: 1),
                new TierLoot(2, TierLoot.LootType.Ring, 0.02f)
            );
            db.Init("Clockwork Golem",
                new State("idle",
                    new PlayerWithinTransition(11, targetStates: "player_nearby")
                ),
                new State("player_nearby",
                    new Shoot(10, cooldown: 4000, cooldownOffset: 2000),
                    new Shoot(10, cooldown: 4000, cooldownOffset: 3000),
                    new State("first_satellites",
                        new Spawn("Blue Satellite", maxChildren: 1, cooldown: 200),
                        new Spawn("Gray Satellite 2", maxChildren: 1, cooldown: 200),
                        new TimedTransition(200, "next_satellite")
                    ),
                    new State("next_satellite",
                        new Spawn("Gray Satellite 2", maxChildren: 1, cooldown: 200),
                        new TimedTransition(200, "follow")
                    ),
                    new State("follow",
                        new Prioritize(
                            new StayAbove(1, 65),
                            new Follow(.6f, range: 1),
                            new Wander(.5f)
                        ),
                        new TimedTransition(3000, "wander1")
                    ),
                    new State("wander1",
                        new Prioritize(
                            new StayAbove(1, 65),
                            new Wander(.6f)
                        ),
                        new TimedTransition(3000, "back_up")
                    ),
                    new State("back_up",
                        new Prioritize(
                            new StayAbove(1, 65),
                            new StayBack(.5f, 4)
                        ),
                        new TimedTransition(7000, "wander2")
                    ),
                    new State("wander2",
                        new Prioritize(
                            new StayAbove(1, 65),
                            new Wander(.8f)
                        ),
                        new TimedTransition(3000, "first_satellites")
                    )
                ),
                new TierLoot(6, TierLoot.LootType.Armor, .01f)
            );
            db.Init("Metal Golem",
                new State("idle",
                    new PlayerWithinTransition(11, targetStates: "player_nearby")
                ),
                new State("player_nearby",
                    new Prioritize(
                        new StayAbove(1, 65),
                        new Follow(1, range: 3, cooldown: 3000, duration: 3000),
                        new Wander(.4f)
                    ),
                    new Spawn("Red Satellite", maxChildren: 1, cooldown: 200),
                    new Spawn("Gray Satellite 3", maxChildren: 1, cooldown: 200),
                    new Shoot(10, predictive: .5f, cooldown: 500),
                    new TimedTransition(12000, "idle")
                ),
                new TierLoot(5, TierLoot.LootType.Weapon, .01f)
            );
        }
    }
}