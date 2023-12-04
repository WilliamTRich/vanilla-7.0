using Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database;

public sealed class HighLands : IBehaviorDatabase
{
    public void Init(BehaviorDb db)
    {
        db.Init("Minotaur",
            new State("idle",
                new StayAbove(0.6f, 150),
                new PlayerWithinTransition(10, targetStates: "charge")
            ),
            new State("charge",
                new Prioritize(
                    new StayAbove(0.6f, 150),
                    new Follow(6, acquireRange: 11, range: 1.6f)
                ),
                new TimedTransition(200, "spam_blades")
            ),
            new State("spam_blades",
                new Shoot(8, index: 0, count: 1, cooldown: 100000, cooldownOffset: 1000),
                new Shoot(8, index: 0, count: 2, shootAngle: 16, cooldown: 100000,
                    cooldownOffset: 1200),
                new Shoot(8, index: 0, count: 3, predictive: 0.2f, cooldown: 100000,
                    cooldownOffset: 1600),
                new Shoot(8, index: 0, count: 1, shootAngle: 24, cooldown: 100000,
                    cooldownOffset: 2200),
                new Shoot(8, index: 0, count: 2, predictive: 0.2f, cooldown: 100000,
                    cooldownOffset: 2800),
                new Shoot(8, index: 0, count: 3, shootAngle: 16, cooldown: 100000,
                    cooldownOffset: 3200),
                new Prioritize(
                    new StayAbove(0.6f, 150),
                    new Wander(0.6f)
                ),
                new TimedTransition(4400, "blade_ring")
            ),
            new State("blade_ring",
                new Shoot(7, fixedAngle: 0, count: 12, shootAngle: 30, cooldown: 800, index: 1,
                    cooldownOffset: 600),
                new Shoot(7, fixedAngle: 15, count: 6, shootAngle: 60, cooldown: 800, index: 2,
                    cooldownOffset: 1000),
                new Prioritize(
                    new StayAbove(0.6f, 150),
                    new Follow(0.6f, acquireRange: 10, range: 1),
                    new Wander(0.6f)
                ),
                new TimedTransition(3500, "pause")
            ),
            new State("pause",
                new Prioritize(
                    new StayAbove(0.6f, 150),
                    new Wander(0.6f)
                ),
                new TimedTransition(targetStates: "idle")
            ),
            new Threshold(.01f,
                new TierLoot(7, TierLoot.LootType.Weapon, 0.16f),
                new TierLoot(8, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(9, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(7, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(8, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(9, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(3, TierLoot.LootType.Ring, 0.3f),
                new TierLoot(3, TierLoot.LootType.Ability, 0.3f),
                new ItemLoot("Purple Drake Egg", 0.005f)
            )
        );
        db.Init("Ogre King",
            new Spawn("Ogre Warrior", maxChildren: 4, cooldown: 12000, givesNoXp: false),
            new Spawn("Ogre Mage", maxChildren: 2, cooldown: 16000, givesNoXp: false),
            new Spawn("Ogre Wizard", maxChildren: 2, cooldown: 20000, givesNoXp: false),
            new State("idle",
                new Prioritize(
                    new StayAbove(0.3f, 150),
                    new Wander(0.3f)
                ),
                new PlayerWithinTransition(10, targetStates: "grenade_blade_combo")
            ),
            new State("grenade_blade_combo",
                new State("grenade1",
                    new Grenade(3, 60, cooldown: 100000),
                    new Prioritize(
                        new StayAbove(0.3f, 150),
                        new Wander(0.3f)
                    ),
                    new TimedTransition(2000, "grenade2")
                ),
                new State("grenade2",
                    new Grenade(3, 60, cooldown: 100000),
                    new Prioritize(
                        new StayAbove(0.5f, 150),
                        new Wander(0.5f)
                    ),
                    new TimedTransition(3000, "slow_follow")
                ),
                new State("slow_follow",
                    new Shoot(13, cooldown: 1000),
                    new Prioritize(
                        new StayAbove(0.4f, 150),
                        new Follow(0.4f, acquireRange: 9, range: 3.5f, duration: 4),
                        new Wander(0.4f)
                    ),
                    new TimedTransition(4000, "grenade1")
                ),
                new HealthTransition(0.45f, "furious")
            ),
            new State("furious",
                new Grenade(2.4f, 55, radius: 9, cooldown: 1500),
                new Prioritize(
                    new StayAbove(0.6f, 150),
                    new Wander(0.6f)
                ),
                new TimedTransition(12000, "idle")
            ),
            new Threshold(.01f,
                new TierLoot(6, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(7, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(5, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(6, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(7, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(2, TierLoot.LootType.Ring, 0.3f),
                new TierLoot(2, TierLoot.LootType.Ability, 0.3f)
            )
        );
        db.Init("Ogre Warrior",
            new Shoot(3, predictive: 0.5f),
            new Prioritize(
                new StayAbove(1.2f, 150),
                new Protect(1.2f, "Ogre King", acquireRange: 15, protectionRange: 10, reprotectRange: 5),
                new Follow(1.4f, acquireRange: 10.5f, range: 1.6f, duration: 2600, cooldown: 2200),
                new Orbit(0.6f, 6),
                new Wander(0.4f)
            )
        );
        db.Init("Ogre Mage",
            new Shoot(10, predictive: 0.3f),
            new Prioritize(
                new StayAbove(1.2f, 150),
                new Protect(1.2f, "Ogre King", acquireRange: 30, protectionRange: 10, reprotectRange: 1),
                new Orbit(0.5f, 6),
                new Wander(0.4f)
            )
        );
        db.Init("Ogre Wizard",
            new Shoot(10, cooldown: 300),
            new Prioritize(
                new StayAbove(1.2f, 150),
                new Protect(1.2f, "Ogre King", acquireRange: 30, protectionRange: 10, reprotectRange: 1),
                new Orbit(0.5f, 6),
                new Wander(0.4f)
            )
        );
        db.Init("Lizard God",
            new Spawn("Night Elf Archer", maxChildren: 4, givesNoXp: false),
            new Spawn("Night Elf Warrior", maxChildren: 3, givesNoXp: false),
            new Spawn("Night Elf Mage", maxChildren: 2, givesNoXp: false),
            new Spawn("Night Elf Veteran", maxChildren: 2, givesNoXp: false),
            new Spawn("Night Elf King", maxChildren: 1, givesNoXp: false),
            new Prioritize(
                new StayAbove(0.3f, 150),
                new Follow(1, range: 7),
                new Wander(0.4f)
            ),
            new State("idle",
                new PlayerWithinTransition(10.2f, targetStates: "normal_attack")
            ),
            new State("normal_attack",
                new Shoot(10, count: 3, shootAngle: 3, predictive: 0.5f),
                new TimedTransition(4000, "if_cloaked")
            ),
            new State("if_cloaked",
                new Shoot(10, count: 8, shootAngle: 45, fixedAngle: 20, cooldown: 1600, cooldownOffset: 400),
                new Shoot(10, count: 8, shootAngle: 45, fixedAngle: 42, cooldown: 1600, cooldownOffset: 1200),
                new PlayerWithinTransition(10, targetStates: "normal_attack")
            ),
            new Threshold(.01f,
                new TierLoot(6, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(7, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(8, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(6, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(7, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(8, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(3, TierLoot.LootType.Ring, 0.3f),
                new TierLoot(3, TierLoot.LootType.Ability, 0.3f),
                new ItemLoot("Purple Drake Egg", 0.005f)
            )
        );
        db.Init("Night Elf Archer",
            new Shoot(10, predictive: 1),
            new Prioritize(
                new StayAbove(0.4f, 150),
                new Follow(1.5f, range: 7),
                new Wander(0.4f)
            )
        );
        db.Init("Night Elf Warrior",
            new Shoot(3, predictive: 1),
            new Prioritize(
                new StayAbove(0.4f, 150),
                new Follow(1.5f, range: 1),
                new Wander(0.4f)
            )
        );
        db.Init("Night Elf Mage",
            new Shoot(10, predictive: 1),
            new Prioritize(
                new StayAbove(0.4f, 150),
                new Follow(1.5f, range: 7),
                new Wander(0.4f)
            )
        );
        db.Init("Night Elf Veteran",
            new Shoot(10, predictive: 1),
            new Prioritize(
                new StayAbove(0.4f, 150),
                new Follow(1.5f, range: 7),
                new Wander(0.4f)
            )
        );
        db.Init("Night Elf King",
            new Shoot(10, predictive: 1),
            new Prioritize(
                new StayAbove(0.4f, 150),
                new Follow(1.5f, range: 7),
                new Wander(0.4f)
            )
        );
        db.Init("Undead Dwarf God",
            new Spawn("Undead Dwarf Warrior", maxChildren: 3, givesNoXp: false),
            new Spawn("Undead Dwarf Axebearer", maxChildren: 3, givesNoXp: false),
            new Spawn("Undead Dwarf Mage", maxChildren: 3, givesNoXp: false),
            new Spawn("Undead Dwarf King", maxChildren: 2, givesNoXp: false),
            new Spawn("Soulless Dwarf", maxChildren: 1, givesNoXp: false),
            new Prioritize(
                new StayAbove(0.3f, 150),
                new Follow(1, range: 7),
                new Wander(0.4f)
            ),
            new Shoot(10, index: 0, count: 3, shootAngle: 15),
            new Shoot(10, index: 1, predictive: 0.5f, cooldown: 1200),
            new Threshold(.01f,
                new TierLoot(6, TierLoot.LootType.Weapon, 0.16f),
                new TierLoot(7, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(8, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(6, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(7, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(8, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(3, TierLoot.LootType.Ring, 0.3f),
                new TierLoot(3, TierLoot.LootType.Ability, 0.3f),
                new ItemLoot("Purple Drake Egg", 0.005f)
            )
        );
        db.Init("Undead Dwarf Warrior",
            new Shoot(3),
            new Prioritize(
                new StayAbove(1, 150),
                new Follow(1, range: 1),
                new Wander(0.4f)
            )
        );
        db.Init("Undead Dwarf Axebearer",
            new Shoot(3),
            new Prioritize(
                new StayAbove(1, 150),
                new Follow(1, range: 1),
                new Wander(0.4f)
            )
        );
        db.Init("Undead Dwarf Mage",
            new State("circle_player",
                new Shoot(8, predictive: 0.3f, cooldown: 1000, cooldownOffset: 500),
                new Prioritize(
                    new StayAbove(0.7f, 150),
                    new Protect(0.7f, "Undead Dwarf King", acquireRange: 11, protectionRange: 10,
                        reprotectRange: 3),
                    new Orbit(0.7f, 3.5f, acquireRange: 11),
                    new Wander(0.6f)
                ),
                new TimedTransition(3500, "circle_king")
            ),
            new State("circle_king",
                new Shoot(8, count: 5, shootAngle: 72, defaultAngle: 20, predictive: 0.3f, cooldown: 1600,
                    cooldownOffset: 500),
                new Shoot(8, count: 5, shootAngle: 72, defaultAngle: 33, predictive: 0.3f, cooldown: 1600,
                    cooldownOffset: 1300),
                new Prioritize(
                    new StayAbove(0.7f, 150),
                    new Orbit(1.2f, 2.5f, target: "Undead Dwarf King", acquireRange: 12, radiusVariance: 0.1f,
                        speedVariance: 0.1f),
                    new Wander(0.6f)
                ),
                new TimedTransition(3500, "circle_player")
            )
        );
        db.Init("Undead Dwarf King",
            new Shoot(3),
            new Prioritize(
                new StayAbove(1, 150),
                new Follow(0.8f, range: 1.4f),
                new Wander(0.4f)
            )
        );
        db.Init("Soulless Dwarf",
            new Shoot(10),
            new State("idle",
                new PlayerWithinTransition(10.5f, targetStates: "run1")
            ),
            new State("run1",
                new Prioritize(
                    new StayAbove(0.4f, 150),
                    new Protect(1.1f, "Undead Dwarf God", acquireRange: 16, protectionRange: 10,
                        reprotectRange: 1),
                    new Wander(0.4f)
                ),
                new TimedTransition(2000, "run2")
            ),
            new State("run2",
                new Prioritize(
                    new StayAbove(0.4f, 150),
                    new StayBack(0.8f, distance: 4),
                    new Wander(0.4f)
                ),
                new TimedTransition(1400, "run3")
            ),
            new State("run3",
                new Prioritize(
                    new StayAbove(0.4f, 150),
                    new Protect(1, "Undead Dwarf King", acquireRange: 16, protectionRange: 2,
                        reprotectRange: 2),
                    new Protect(1, "Undead Dwarf Axebearer", acquireRange: 16, protectionRange: 2,
                        reprotectRange: 2),
                    new Protect(1, "Undead Dwarf Warrior", acquireRange: 16, protectionRange: 2,
                        reprotectRange: 2),
                    new Wander(0.4f)
                ),
                new TimedTransition(2000, "idle")
            )
        );
        db.Init("Flayer God",
            new Spawn("Flayer", maxChildren: 2, givesNoXp: false),
            new Spawn("Flayer Veteran", maxChildren: 3, givesNoXp: false),
            new Reproduce("Flayer God", densityMax: 2),
            new Prioritize(
                new StayAbove(0.4f, 155),
                new Follow(1, range: 7),
                new Wander(0.4f)
            ),
            new Shoot(10, index: 0, predictive: 0.5f, cooldown: 400),
            new Shoot(10, index: 1, predictive: 1)
            ,
            new Threshold(.01f,
                new TierLoot(6, TierLoot.LootType.Weapon, 0.16f),
                new TierLoot(7, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(8, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(6, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(7, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(8, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(3, TierLoot.LootType.Ring, 0.3f),
                new TierLoot(3, TierLoot.LootType.Ability, 0.3f),
                new ItemLoot("Purple Drake Egg", 0.005f)
            )
        );
        db.Init("Flayer",
            new Shoot(10, predictive: 0.5f),
            new Prioritize(
                new StayAbove(1, 155),
                new Follow(1.2f, range: 7),
                new Wander(0.4f)
            )
        );
        db.Init("Flayer Veteran",
            new Shoot(10, predictive: 0.5f),
            new Prioritize(
                new StayAbove(1, 155),
                new Follow(1.2f, range: 7),
                new Wander(0.4f)
            )
        );
        db.Init("Flamer King",
            new Spawn("Flamer", maxChildren: 5, cooldown: 10000, givesNoXp: false),
            new State("Attacking",
                new State("Charge",
                    new Follow(0.7f, range: 0.1f),
                    new PlayerWithinTransition(2, targetStates: "Bullet")
                ),
                new State("Bullet",
                    new Flash(0xffffaa00, 0.2f, 20),
                    new ChangeSize(20, 140),
                    new Shoot(8, cooldown: 200),
                    new TimedTransition(4000, "Wait")
                ),
                new State("Wait",
                    new ChangeSize(-20, 80),
                    new TimedTransition(500, "Charge")
                ),
                new HealthTransition(0.2f, "FlashBeforeExplode")
            ),
            new State("FlashBeforeExplode",
                new Flash(0xffff0000, 1, 1),
                new TimedTransition(300, "Explode")
            ),
            new State("Explode",
                new Shoot(0, count: 10, shootAngle: 36, fixedAngle: 0),
                new Decay(0)
            ),
            new TierLoot(2, TierLoot.LootType.Ring, 0.04f)
        );
        db.Init("Flamer",
            new State("Attacking",
                new State("Charge",
                    new Prioritize(
                        new Protect(0.7f, "Flamer King"),
                        new Follow(0.7f, range: 0.1f)
                    ),
                    new PlayerWithinTransition(2, targetStates: "Bullet")
                ),
                new State("Bullet",
                    new Flash(0xffffaa00, 0.2f, 20),
                    new ChangeSize(20, 130),
                    new Shoot(8, cooldown: 200),
                    new TimedTransition(4000, "Wait")
                ),
                new State("Wait",
                    new ChangeSize(-20, 70),
                    new TimedTransition(600, "Charge")
                ),
                new HealthTransition(0.2f, "FlashBeforeExplode")
            ),
            new State("FlashBeforeExplode",
                new Flash(0xffff0000, 1, 1),
                new TimedTransition(300, "Explode")
            ),
            new State("Explode",
                new Shoot(0, count: 10, shootAngle: 36, fixedAngle: 0),
                new Decay(0)
            ),
            new TierLoot(5, TierLoot.LootType.Weapon, 0.04f)
        );
        db.Init("Dragon Egg",
            new TransformOnDeath("White Dragon Whelp", probability: 0.3f),
            new TransformOnDeath("Juvenile White Dragon", probability: 0.2f),
            new TransformOnDeath("Adult White Dragon", probability: 0.1f)
        );
        db.Init("White Dragon Whelp",
            new Shoot(10, count: 2, shootAngle: 20, predictive: 0.3f, cooldown: 750),
            new Prioritize(
                new StayAbove(1, 150),
                new Follow(2, range: 2.5f, acquireRange: 10.5f, duration: 2200, cooldown: 3200),
                new Wander(0.6f)
            )
        );
        db.Init("Juvenile White Dragon",
            new Shoot(10, count: 2, shootAngle: 20, predictive: 0.3f, cooldown: 750),
            new Prioritize(
                new StayAbove(9, 150),
                new Follow(1.8f, range: 2.2f, acquireRange: 10.5f, duration: 3000, cooldown: 3000),
                new Wander(0.6f)
            ),
            new Threshold(.01f,
                new TierLoot(7, TierLoot.LootType.Weapon, 0.01f),
                new TierLoot(7, TierLoot.LootType.Armor, 0.02f),
                new TierLoot(6, TierLoot.LootType.Armor, 0.07f)
            )
        );
        db.Init("Adult White Dragon",
            new Shoot(10, count: 3, shootAngle: 15, predictive: 0.3f, cooldown: 750),
            new Prioritize(
                new StayAbove(9, 150),
                new Follow(1.4f, range: 1.8f, acquireRange: 10.5f, duration: 4000, cooldown: 2000),
                new Wander(0.6f)
            ),
            new Threshold(.01f,
                new TierLoot(7, TierLoot.LootType.Armor, 0.05f),
                new ItemLoot("Seal of the Divine", 0.015f),
                new ItemLoot("White Drake Egg", 0.004f)
            )
        );
        db.Init("Shield Orc Shield",
            new Prioritize(
                new Orbit(1, 3, target: "Shield Orc Flooder"),
                new Wander(0.1f)
            ),
            new EntityNotWithinTransition("Shield Orc Key", 7, "Idling"),
            new State("Attack",
                new Flash(0xff000000, 10, 100),
                new Shoot(10, cooldown: 500),
                new HealthTransition(0.5f, "Heal")
            ),
            new State("Heal",
                new HealGroup(7, "Shield Orcs", cooldown: 500),
                new TimedTransition(500, "Attack")
            ),
            new State("Flash",
                new Flash(0xff0000, 1, 1),
                new TimedTransition(300, "Idling")
            ),
            new State("Idling"),
            new TierLoot(2, TierLoot.LootType.Ring, 0.01f)
        );
        db.Init("Shield Orc Flooder",
            new Prioritize(
                new Wander(0.1f)
            ),
            new EntityNotWithinTransition("Shield Orc Key", 7, "Idling"),
            new State("Attack",
                new Flash(0xff000000, 10, 100),
                new Shoot(10, cooldown: 500),
                new HealthTransition(0.5f, "Heal")
            ),
            new State("Heal",
                new HealGroup(7, "Shield Orcs", cooldown: 500),
                new TimedTransition(500, "Attack")
            ),
            new State("Flash",
                new Flash(0xff0000, 1, 1),
                new TimedTransition(300, "Idling")
            ),
            new State("Idling"),
            new TierLoot(4, TierLoot.LootType.Ability, 0.01f)
        );
        db.Init("Shield Orc Key",
            new Spawn("Shield Orc Flooder", maxChildren: 1, initialSpawn: 1, cooldown: 10000, givesNoXp: false),
            new Spawn("Shield Orc Shield", maxChildren: 1, initialSpawn: 1, cooldown: 10000, givesNoXp: false),
            new Spawn("Shield Orc Shield", maxChildren: 1, initialSpawn: 1, cooldown: 10000, givesNoXp: false),
            new State("Start",
                new TimedTransition(500, "Attacking")
            ),
            new State("Attacking",
                new Orbit(1, 3, target: "Shield Orc Flooder"),
                new OrderOnce(7, "Shield Orc Flooder", "Attack"),
                new OrderOnce(7, "Shield Orc Shield", "Attack"),
                new HealthTransition(0.5f, "FlashBeforeExplode")
            ),
            new State("FlashBeforeExplode",
                new OrderOnce(7, "Shield Orc Flooder", "Flash"),
                new OrderOnce(7, "Shield Orc Shield", "Flash"),
                new Flash(0xff0000, 1, 1),
                new TimedTransition(300, "Explode")
            ),
            new State("Explode",
                new Shoot(0, count: 10, shootAngle: 36, fixedAngle: 0),
                new Decay(0)
            ),
            new TierLoot(4, TierLoot.LootType.Armor, 0.01f)
        );
        db.Init("Left Horizontal Trap",
            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
            new State("weak_effect",
                new Shoot(1, fixedAngle: 0, index: 0, cooldown: 200),
                new TimedTransition(2000, "blind_effect")
            ),
            new State("blind_effect",
                new Shoot(1, fixedAngle: 0, index: 1, cooldown: 200),
                new TimedTransition(2000, "pierce_effect")
            ),
            new State("pierce_effect",
                new Shoot(1, fixedAngle: 0, index: 2, cooldown: 200),
                new TimedTransition(2000, "weak_effect")
            ),
            new Decay(6000)
        );
        db.Init("Top Vertical Trap",
            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
            new State("weak_effect",
                new Shoot(1, fixedAngle: 90, index: 0, cooldown: 200),
                new TimedTransition(2000, "blind_effect")
            ),
            new State("blind_effect",
                new Shoot(1, fixedAngle: 90, index: 1, cooldown: 200),
                new TimedTransition(2000, "pierce_effect")
            ),
            new State("pierce_effect",
                new Shoot(1, fixedAngle: 90, index: 2, cooldown: 200),
                new TimedTransition(2000, "weak_effect")
            ),
            new Decay(6000)
        );
        db.Init("45-225 Diagonal Trap",
            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
            new State("weak_effect",
                new Shoot(1, fixedAngle: 45, index: 0, cooldown: 200),
                new TimedTransition(2000, "blind_effect")
            ),
            new State("blind_effect",
                new Shoot(1, fixedAngle: 45, index: 1, cooldown: 200),
                new TimedTransition(2000, "pierce_effect")
            ),
            new State("pierce_effect",
                new Shoot(1, fixedAngle: 45, index: 2, cooldown: 200),
                new TimedTransition(2000, "weak_effect")
            ),
            new Decay(6000)
        );
        db.Init("135-315 Diagonal Trap",
            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
            new State("weak_effect",
                new Shoot(1, fixedAngle: 135, index: 0, cooldown: 200),
                new TimedTransition(2000, "blind_effect")
            ),
            new State("blind_effect",
                new Shoot(1, fixedAngle: 135, index: 1, cooldown: 200),
                new TimedTransition(2000, "pierce_effect")
            ),
            new State("pierce_effect",
                new Shoot(1, fixedAngle: 135, index: 2, cooldown: 200),
                new TimedTransition(2000, "weak_effect")
            ),
            new Decay(6000)
        );
        db.Init("Urgle",
            new DropPortalOnDeath("Spider Den Portal", 0.1f),
            new Prioritize(
                new StayCloseToSpawn(0.8f, 3),
                new Wander(0.5f)
            ),
            new Shoot(8, predictive: 0.3f),
            new State("idle",
                new PlayerWithinTransition(10.5f, targetStates: "toss_horizontal_traps")
            ),
            new State("toss_horizontal_traps",
                new TossObject("Left Horizontal Trap", range: 9, angle: 230, cooldown: 100000),
                new TossObject("Left Horizontal Trap", range: 10, angle: 180, cooldown: 100000),
                new TossObject("Left Horizontal Trap", range: 9, angle: 140, cooldown: 100000),
                new TimedTransition(targetStates: "toss_vertical_traps")
            ),
            new State("toss_vertical_traps",
                new TossObject("Top Vertical Trap", range: 8, angle: 200, cooldown: 100000),
                new TossObject("Top Vertical Trap", range: 10, angle: 240, cooldown: 100000),
                new TossObject("Top Vertical Trap", range: 10, angle: 280, cooldown: 100000),
                new TossObject("Top Vertical Trap", range: 8, angle: 320, cooldown: 100000),
                new TimedTransition(targetStates: "toss_diagonal_traps")
            ),
            new State("toss_diagonal_traps",
                new TossObject("45-225 Diagonal Trap", range: 2, angle: 45, cooldown: 100000),
                new TossObject("45-225 Diagonal Trap", range: 7, angle: 45, cooldown: 100000),
                new TossObject("45-225 Diagonal Trap", range: 11, angle: 225, cooldown: 100000),
                new TossObject("45-225 Diagonal Trap", range: 6, angle: 225, cooldown: 100000),
                new TossObject("135-315 Diagonal Trap", range: 2, angle: 135, cooldown: 100000),
                new TossObject("135-315 Diagonal Trap", range: 7, angle: 135, cooldown: 100000),
                new TossObject("135-315 Diagonal Trap", range: 11, angle: 315, cooldown: 100000),
                new TossObject("135-315 Diagonal Trap", range: 6, angle: 315, cooldown: 100000),
                new TimedTransition(targetStates: "wait")
            ),
            new State("wait",
                new TimedTransition(2400, "idle")
            ),
            new Threshold(.01f,
                new TierLoot(5, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(6, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(7, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(5, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(6, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(7, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(3, TierLoot.LootType.Ring, 0.3f),
                new TierLoot(3, TierLoot.LootType.Ability, 0.3f)
            )
        );
        db.Init("Beer God",
            new State("Waiting Player",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new PlayerWithinTransition(4, targetStates: "yay i am good")
            ),
            new State("yay i am good",
                new Taunt("*Hiccup* Comere and gimmie a hug laddie!"),
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new SetAltTexture(1),
                new ChangeSize(20, 100),
                new TimedTransition(3500, "Attack")
            ),
            new State("Attack",
                new SetAltTexture(0),
                new Follow(.6f, 15, 0),
                new Shoot(20, count: 1, index: 0, cooldown: 1000),
                new PlayerWithinTransition(1, targetStates: "BEER")
            ),
            new State("BEER",
                new Shoot(20, count: 5, index: 1, shootAngle: 36, cooldown: 700),
                new Shoot(20, count: 1, index: 0, cooldown: 1000),
                new NoPlayerWithinTransition(4, targetStates: "Attack")
            ),
            new Threshold(0.01f,
                new ItemLoot("Potion of Defense", 0.1f),
                new ItemLoot("Potion of Attack", 0.1f),
                new ItemLoot("Realm-wheat Hefeweizen", 0.2f),
                new ItemLoot("Mad God Ale", 0.2f),
                new ItemLoot("Oryx Stout", 0.2f)
            )
        );
        db.Init("Kage Kami",
            new DropPortalOnDeath("Manor of the Immortals Portal", 0.5f),
            new State("Grave",
                new HealthTransition(.90f, "yay i am good")
            ),
            new State("yay i am good",
                new Taunt(0.5f, "Kyoufu no kage!"),
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new ChangeSize(20, 120),
                new SetAltTexture(1),
                new TimedTransition(2000, "Attack")
            ),
            new State("Attack",
                new Wander(0.4f),
                new SetAltTexture(1),
                new TimedTransition(5000, "Charge"),
                new TossObject("Specter Mine", cooldown: 2000),
                new State("Shoot1",
                    new Shoot(0, count: 1, defaultAngle: 0, rotateAngle: 30, cooldown: 300, index: 1),
                    new Shoot(0, count: 1, defaultAngle: 180, rotateAngle: 30, cooldown: 300, index: 1),
                    new TimedTransition(targetStates: "Shoot2")
                ),
                new State("Shoot2",
                    new Shoot(20, count: 2, shootAngle: 180, cooldown: 400, index: 1),
                    new TimedTransition(400, "Shoot1")
                )
            ),
            new State("Charge",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new TossObject("Specter Mine", cooldown: 2000),
                new SetAltTexture(2),
                new Follow(0.8f, 20, 1),
                new Shoot(20, count: 2, shootAngle: 50, cooldown: 400),
                new TimedTransition(4000, "Attack")
            ),
            new Threshold(0.06f,
                new TierLoot(7, TierLoot.LootType.Weapon, 0.16f),
                new TierLoot(8, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(9, TierLoot.LootType.Weapon, 0.3f),
                new TierLoot(7, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(8, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(9, TierLoot.LootType.Armor, 0.3f),
                new TierLoot(4, TierLoot.LootType.Ring, 0.3f),
                new TierLoot(4, TierLoot.LootType.Ability, 0.3f),
                new ItemLoot("Potion of Attack", 0.9f)
            )
        );
        db.Init("Specter Mine",
            new State("Waiting",
                new PlayerWithinTransition(3, targetStates: "Suicide"),
                new TimedTransition(4000, "Suicide")
            ),
            new State("Suicide",
                new Shoot(60, count: 4, shootAngle: 45),
                new Suicide()
            )
        );
    }
}