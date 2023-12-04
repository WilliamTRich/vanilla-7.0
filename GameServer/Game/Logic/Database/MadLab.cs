using Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database;

public sealed class MadLab : IBehaviorDatabase
{
    public void Init(BehaviorDb db)
    {
        db.Init("Dr Terrible",
            new DropPortalOnDeath("Realm Portal"),
            new State("idle",
                new PlayerWithinTransition(12, false, "GP"),
                new HealthTransition(.2f, "rage")
            ),
            new State("rage",
                new OrderOnce(100, "Monster Cage", "no spawn"),
                new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                new OrderOnce(100, "Red Gas Spawner UL", "OFF"),
                new OrderOnce(100, "Red Gas Spawner UR", "OFF"),
                new OrderOnce(100, "Red Gas Spawner LL", "OFF"),
                new OrderOnce(100, "Red Gas Spawner LR", "OFF"),
                new Wander(0.5f),
                new SetAltTexture(0),
                new TossObject("Green Potion", cooldown: 1500, cooldownOffset: 0),
                new TimedTransition(12000, "rage TA")
            ),
            new State("rage TA",
                new OrderOnce(100, "Monster Cage", "no spawn"),
                new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                new OrderOnce(100, "Red Gas Spawner UL", "OFF"),
                new OrderOnce(100, "Red Gas Spawner UR", "OFF"),
                new OrderOnce(100, "Red Gas Spawner LL", "OFF"),
                new OrderOnce(100, "Red Gas Spawner LR", "OFF"),
                new Wander(0.5f),
                new SetAltTexture(0),
                new TossObject("Turret Attack", cooldown: 1500, cooldownOffset: 0),
                new TimedTransition(10000, "rage")
            ),
            new State("GP",
                new OrderOnce(100, "Monster Cage", "no spawn"),
                new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                new OrderOnce(100, "Red Gas Spawner UL", "OFF"),
                new OrderOnce(100, "Red Gas Spawner UR", "ON"),
                new OrderOnce(100, "Red Gas Spawner LL", "ON"),
                new OrderOnce(100, "Red Gas Spawner LR", "ON"),
                new Wander(0.5f),
                new SetAltTexture(0),
                new Taunt(0.5f, "For Science"),
                new TossObject("Green Potion", cooldown: 2000, cooldownOffset: 0),
                new TimedTransition(12000, "TA")
            ),
            new State("TA",
                new OrderOnce(100, "Monster Cage", "no spawn"),
                new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                new OrderOnce(100, "Red Gas Spawner UL", "OFF"),
                new OrderOnce(100, "Red Gas Spawner UR", "ON"),
                new OrderOnce(100, "Red Gas Spawner LL", "ON"),
                new OrderOnce(100, "Red Gas Spawner LR", "ON"),
                new Wander(0.5f),
                new SetAltTexture(0),
                new TossObject("Turret Attack", cooldown: 2000, cooldownOffset: 0),
                new TimedTransition(10000, "hide")
            ),
            new State("hide",
                new OrderOnce(100, "Monster Cage", "spawn"),
                new OrderOnce(100, "Dr Terrible Bubble", "Bubble time"),
                new OrderOnce(100, "Red Gas Spawner UL", "OFF"),
                new OrderOnce(100, "Red Gas Spawner UR", "OFF"),
                new OrderOnce(100, "Red Gas Spawner LL", "OFF"),
                new OrderOnce(100, "Red Gas Spawner LR", "OFF"),
                new ReturnToSpawn(1),
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new SetAltTexture(1),
                new TimedTransition(15000, "nohide")
            ),
            new State("nohide",
                new OrderOnce(100, "Monster Cage", "no spawn"),
                new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                new OrderOnce(100, "Red Gas Spawner UL", "ON"),
                new OrderOnce(100, "Red Gas Spawner UR", "OFF"),
                new OrderOnce(100, "Red Gas Spawner LL", "ON"),
                new OrderOnce(100, "Red Gas Spawner LR", "ON"),
                new Wander(0.5f),
                new SetAltTexture(0),
                new TossObject("Green Potion", cooldown: 2000, cooldownOffset: 0),
                new TimedTransition(12000, "TA2")
            ),
            new State("TA2",
                new OrderOnce(100, "Monster Cage", "no spawn"),
                new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                new OrderOnce(100, "Red Gas Spawner UL", "ON"),
                new OrderOnce(100, "Red Gas Spawner UR", "OFF"),
                new OrderOnce(100, "Red Gas Spawner LL", "ON"),
                new OrderOnce(100, "Red Gas Spawner LR", "ON"),
                new Wander(0.5f),
                new SetAltTexture(0),
                new TossObject("Green Potion", cooldown: 2000, cooldownOffset: 0),
                new TimedTransition(10000, "hide2")
            ),
            new State("hide2",
                new OrderOnce(100, "Monster Cage", "spawn"),
                new OrderOnce(100, "Dr Terrible Bubble", "Bubble time"),
                new OrderOnce(100, "Red Gas Spawner UL", "OFF"),
                new OrderOnce(100, "Red Gas Spawner UR", "OFF"),
                new OrderOnce(100, "Red Gas Spawner LL", "OFF"),
                new OrderOnce(100, "Red Gas Spawner LR", "OFF"),
                new ReturnToSpawn(1),
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new SetAltTexture(1),
                new TimedTransition(15000, "nohide2")
            ),
            new State("nohide2",
                new OrderOnce(100, "Monster Cage", "no spawn"),
                new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                new OrderOnce(100, "Red Gas Spawner UL", "ON"),
                new OrderOnce(100, "Red Gas Spawner UR", "ON"),
                new OrderOnce(100, "Red Gas Spawner LL", "OFF"),
                new OrderOnce(100, "Red Gas Spawner LR", "ON"),
                new Wander(0.5f),
                new SetAltTexture(0),
                new TossObject("Green Potion", cooldown: 2000, cooldownOffset: 0),
                new TimedTransition(12000, "TA3")
            ),
            new State("TA3",
                new OrderOnce(100, "Monster Cage", "no spawn"),
                new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                new OrderOnce(100, "Red Gas Spawner UL", "ON"),
                new OrderOnce(100, "Red Gas Spawner UR", "ON"),
                new OrderOnce(100, "Red Gas Spawner LL", "OFF"),
                new OrderOnce(100, "Red Gas Spawner LR", "ON"),
                new Wander(0.5f),
                new SetAltTexture(0),
                new TossObject("Green Potion", cooldown: 2000, cooldownOffset: 0),
                new TimedTransition(10000, "hide3")
            ),
            new State("hide3",
                new OrderOnce(100, "Monster Cage", "spawn"),
                new OrderOnce(100, "Dr Terrible Bubble", "Bubble time"),
                new OrderOnce(100, "Red Gas Spawner UL", "OFF"),
                new OrderOnce(100, "Red Gas Spawner UR", "OFF"),
                new OrderOnce(100, "Red Gas Spawner LL", "OFF"),
                new OrderOnce(100, "Red Gas Spawner LR", "OFF"),
                new ReturnToSpawn(1),
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new SetAltTexture(1),
                new TimedTransition(15000, "nohide3")
            ),
            new State("nohide3",
                new OrderOnce(100, "Monster Cage", "no spawn"),
                new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                new OrderOnce(100, "Red Gas Spawner UL", "ON"),
                new OrderOnce(100, "Red Gas Spawner UR", "ON"),
                new OrderOnce(100, "Red Gas Spawner LL", "ON"),
                new OrderOnce(100, "Red Gas Spawner LR", "OFF"),
                new Wander(0.5f),
                new SetAltTexture(0),
                new TossObject("Green Potion", cooldown: 2000, cooldownOffset: 0),
                new TimedTransition(12000, "TA4")
            ),
            new State("TA4",
                new OrderOnce(100, "Monster Cage", "no spawn"),
                new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                new OrderOnce(100, "Red Gas Spawner UL", "ON"),
                new OrderOnce(100, "Red Gas Spawner UR", "ON"),
                new OrderOnce(100, "Red Gas Spawner LL", "ON"),
                new OrderOnce(100, "Red Gas Spawner LR", "OFF"),
                new Wander(0.5f),
                new SetAltTexture(0),
                new TossObject("Green Potion", cooldown: 2000, cooldownOffset: 0),
                new TimedTransition(10000, "hide4")
            ),
            new State("hide4",
                new OrderOnce(100, "Monster Cage", "spawn"),
                new OrderOnce(100, "Dr Terrible Bubble", "Bubble time"),
                new OrderOnce(100, "Red Gas Spawner UL", "OFF"),
                new OrderOnce(100, "Red Gas Spawner UR", "OFF"),
                new OrderOnce(100, "Red Gas Spawner LL", "OFF"),
                new OrderOnce(100, "Red Gas Spawner LR", "OFF"),
                new ReturnToSpawn(1),
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new SetAltTexture(1),
                new TimedTransition(15000, "idle")
            ),
            new Threshold(0.32f,
                new ItemLoot("Potion of Wisdom", .7f),
                new ItemLoot("Potion of Wisdom", .25f),
                new ItemLoot("Potion of Wisdom", .05f)
            ),
            new Threshold(0.02f,
                new ItemLoot("Scepter of Fulmination", 0.01f),
                new ItemLoot("Conducting Wand", 0.01f),
                new ItemLoot("Robe of the Mad Scientist", 0.01f),
                new ItemLoot("Experimental Ring", 0.01f),
                new TierLoot(8, TierLoot.LootType.Weapon, 0.2f),
                new TierLoot(9, TierLoot.LootType.Weapon, 0.15f),
                new TierLoot(10, TierLoot.LootType.Weapon, 0.1f),
                new TierLoot(11, TierLoot.LootType.Weapon, 0.05f),
                new TierLoot(8, TierLoot.LootType.Armor, 0.2f),
                new TierLoot(9, TierLoot.LootType.Armor, 0.15f),
                new TierLoot(10, TierLoot.LootType.Armor, 0.1f),
                new TierLoot(11, TierLoot.LootType.Armor, 0.05f),
                new TierLoot(4, TierLoot.LootType.Ability, 0.15f),
                new TierLoot(5, TierLoot.LootType.Ability, 0.1f)
            )
        );
        db.Init("Dr Terrible Mini Bot",
            new Wander(0.5f),
            new Shoot(10, 2, 20, angleOffset: 45, index: 0, cooldown: 1000)
        );
        db.Init("Dr Terrible Rampage Cyborg",
            new State("idle",
                new PlayerWithinTransition(10, false, "normal")
            ),
            new State("normal",
                new Wander(0.5f),
                new Follow(0.6f, range: 1, duration: 5000, cooldown: 0),
                new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, predictive: 1,
                    cooldown: 800, cooldownOffset: 0),
                new HealthTransition(.2f, "blink"),
                new TimedTransition(10000, "rage blink")
            ),
            new State("rage blink",
                new Wander(0.5f),
                new Flash(0xf0e68c, repeats: 5, flashPeriod: 0.1f),
                new Follow(0.6f, range: 1, duration: 5000, cooldown: 0),
                new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 1, predictive: 1,
                    cooldown: 800, cooldownOffset: 0),
                new HealthTransition(.2f, "blink"),
                new TimedTransition(3000, "rage")
            ),
            new State("rage",
                new Wander(0.5f),
                new Flash(0xf0e68c, repeats: 5, flashPeriod: 0.1f),
                new Follow(0.6f, range: 1, duration: 5000, cooldown: 0),
                new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 1, predictive: 1,
                    cooldown: 800, cooldownOffset: 0),
                new HealthTransition(.2f, "blink")
            ),
            new State("blink",
                new Wander(0.5f),
                new Follow(0.6f, range: 1, duration: 5000, cooldown: 0),
                new Flash(0xfFF0000, repeats: 10000, flashPeriod: 0.1f),
                new TimedTransition(2000, "explode")
            ),
            new State("explode",
                new Flash(0xfFF0000, 1, 9000001),
                new Shoot(10, 8, index: 2, fixedAngle: 22.5f),
                new Suicide()
            )
        );
        db.Init("Dr Terrible Escaped Experiment",
            new Wander(0.5f),
            new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, predictive: 1,
                cooldown: 800, cooldownOffset: 0)
        );
        db.Init("Mini Bot",
            new Wander(0.5f),
            new Shoot(10, 2, 20, index: 0, cooldown: 1000)
        );
        db.Init("Rampage Cyborg",
            new State("idle",
                new PlayerWithinTransition(10, false, "normal")
            ),
            new State("normal",
                new Wander(0.5f),
                new Follow(0.6f, range: .1f, duration: 5000, cooldown: 0),
                new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, predictive: 1,
                    cooldown: 800, cooldownOffset: 0),
                new HealthTransition(.2f, "blink"),
                new TimedTransition(10000, "rage blink")
            ),
            new State("rage blink",
                new Wander(0.5f),
                new Flash(0xf0e68c, repeats: 5, flashPeriod: 0.1f),
                new Follow(0.6f, range: .1f, duration: 5000, cooldown: 0),
                new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 1, predictive: 1,
                    cooldown: 800, cooldownOffset: 0),
                new HealthTransition(.2f, "blink"),
                new TimedTransition(3000, "rage")
            ),
            new State("rage",
                new Wander(0.5f),
                new Flash(0xf0e68c, repeats: 5, flashPeriod: 0.1f),
                new Follow(0.6f, range: .1f, duration: 5000, cooldown: 0),
                new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 1, predictive: 1,
                    cooldown: 800, cooldownOffset: 0),
                new HealthTransition(.2f, "blink")
            ),
            new State("blink",
                new Wander(0.5f),
                new Follow(0.6f, range: .1f, duration: 5000, cooldown: 0),
                new Flash(0xfFF0000, repeats: 10000, flashPeriod: 0.1f),
                new TimedTransition(2000, "explode")
            ),
            new State("explode",
                new Flash(0xfFF0000, 1, 9000001),
                new Shoot(10, 8, index: 2, fixedAngle: 22.5f),
                new Suicide()
            )
        );
        db.Init("Escaped Experiment",
            new Wander(0.5f),
            new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, index: 0, predictive: 1,
                cooldown: 800, cooldownOffset: 0)
        );
        db.Init("West Automated Defense Turret",
            new ConditionalEffect(ConditionEffectIndex.Invincible),
            new Shoot(32, fixedAngle: 0, cooldown: 3000, cooldownVariance: 1000)
        );
        db.Init("East Automated Defense Turret",
            new ConditionalEffect(ConditionEffectIndex.Invincible),
            new Shoot(32, fixedAngle: 180, cooldown: 3000, cooldownVariance: 1000)
        );
        db.Init("Crusher Abomination",
            new State("1 step",
                new Wander(0.5f),
                new Shoot(10, 3, 20, angleOffset: 30, index: 0, cooldown: 1000),
                new HealthTransition(.75f, "2 step")
            ),
            new State("2 step",
                new Wander(0.5f),
                new ChangeSize(11, 150),
                new Shoot(10, 2, 20, angleOffset: 30, index: 1, cooldown: 1000),
                new HealthTransition(.5f, "3 step")
            ),
            new State("3 step",
                new Wander(0.5f),
                new ChangeSize(11, 175),
                new Shoot(10, 2, 20, angleOffset: 30, index: 2, cooldown: 1000),
                new HealthTransition(.25f, "4 step")
            ),
            new State("4 step",
                new Wander(0.5f),
                new ChangeSize(11, 200),
                new Shoot(10, 2, 20, angleOffset: 30, index: 3, cooldown: 1000)
            )
        );
        db.Init("Enforcer Bot 3000",
            new Wander(0.5f),
            new Shoot(10, 3, 20, index: 0, cooldown: 1000),
            new Shoot(10, 4, 20, angleOffset: 90 / 4f, index: 1, cooldown: 1000),
            new TransformOnDeath("Mini Bot", 0, 3)
        );
        db.Init("Green Potion",
            new State("Idle",
                new TimedTransition(2000, "explode")
            ),
            new State("explode",
                new Shoot(10, 6, index: 0, fixedAngle: 22.5f),
                new Suicide()
            )
        );
        db.Init("Red Gas Spawner UL",
            new EntityNotWithinTransition("Dr Terrible", 50, "OFF"),
            new State("OFF",
                new ConditionalEffect(ConditionEffectIndex.Invincible)
            ),
            new State("ON",
                new EntityNotWithinTransition("Dr Terrible", 50f, "OFF"),
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new Shoot(10, 20, index: 0, fixedAngle: 22.5f, cooldown: 2000)
            )
        );
        db.Init("Red Gas Spawner UR",
            new EntityNotWithinTransition("Dr Terrible", 50, "OFF"),
            new State("OFF",

                new ConditionalEffect(ConditionEffectIndex.Invincible)
            ),
            new State("ON",
                new EntityNotWithinTransition("Dr Terrible", 50f, "OFF"),
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new Shoot(10, 20, index: 0, fixedAngle: 22.5f, cooldown: 2000)
            )
        );
        db.Init("Red Gas Spawner LL",
            new EntityNotWithinTransition("Dr Terrible", 50, "OFF"),
            new State("OFF",
                new ConditionalEffect(ConditionEffectIndex.Invincible)
            ),
            new State("ON",
                new EntityNotWithinTransition("Dr Terrible", 50f, "OFF"),
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new Shoot(10, 20, index: 0, fixedAngle: 22.5f, cooldown: 2000)
            )
        );
        db.Init("Red Gas Spawner LR",
            new EntityNotWithinTransition("Dr Terrible", 50, "OFF"),
            new State("OFF",
                new ConditionalEffect(ConditionEffectIndex.Invincible)
            ),
            new State("ON",
                new EntityNotWithinTransition("Dr Terrible", 50f, "OFF"),
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new Shoot(10, 20, index: 0, fixedAngle: 22.5f, cooldown: 2000)
            )
        );

        db.Init("Turret Attack",
            new Shoot(10, 2, 20, index: 0, cooldown: 1000)
        );
        
        db.Init("Mad Scientist Summoner",
            new ConditionalEffect(ConditionEffectIndex.Invincible, true),
            new State("idle",
                new EntityNotWithinTransition("Dr Terrible", 300, "Death")
            ),
            new State("Death",
                new Suicide()
            )
        );

        db.Init("Dr Terrible Bubble",
            new State("nothing change",
                new ConditionalEffect(ConditionEffectIndex.Invincible)
            //new SetAltTexture(0)
            ),
            new State("Bubble time",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                //new SetAltTexture(1),
                new TimedTransition(1000, "Bubble time2")
            ),
            new State("Bubble time2",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                //new SetAltTexture(2),
                new TimedTransition(1000, "Bubble time")
            )
        );
        db.Init("Mad Gas Controller", //don't need xD
            new ConditionalEffect(ConditionEffectIndex.Invincible, true)
        );
        db.Init("Monster Cage",
            new State("no spawn",
                new ConditionalEffect(ConditionEffectIndex.Invincible)
            //new SetAltTexture(0)
            ),
            new State("spawn",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                // new SetAltTexture(2),
                new Spawn("Dr Terrible Rampage Cyborg", 1, initialSpawn: 0, cooldown: 15000, cooldownVariance: 5000),
                new Spawn("Dr Terrible Mini Bot", 1, initialSpawn: 0, cooldown: 15000, cooldownVariance: 5000),
                new Spawn("Dr Terrible Escaped Experiment", 1, initialSpawn: 0, cooldown: 15000, cooldownVariance: 5000)
            )
        );
    }
}