using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using static RotMG.Game.Logic.Loots.TierLoot;

namespace RotMG.Game.Logic.Database
{
    public sealed class TCave : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Golden Oryx Effigy",
                new DropPortalOnDeath("Realm Portal"),
                new State("Ini",
                    new HealthTransition(0.99f, "Q1 Spawn Minion")
                ),
                new State("Q1 Spawn Minion",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TossObject("Gold Planet", 7, 0, 10000000),
                    new TossObject("Gold Planet", 7, 45, 10000000),
                    new TossObject("Gold Planet", 7, 90, 10000000),
                    new TossObject("Gold Planet", 7, 135, 10000000),
                    new TossObject("Gold Planet", 7, 180, 10000000),
                    new TossObject("Gold Planet", 7, 225, 10000000),
                    new TossObject("Gold Planet", 7, 270, 10000000),
                    new TossObject("Gold Planet", 7, 315, 10000000),
                    new TossObject("Treasure Oryx Defender", 3, 0, 10000000),
                    new TossObject("Treasure Oryx Defender", 3, 90, 10000000),
                    new TossObject("Treasure Oryx Defender", 3, 180, 10000000),
                    new TossObject("Treasure Oryx Defender", 3, 270, 10000000),
                    new ChangeSize(-1, 60),
                    new TimedTransition(4000, "Q1 Invulnerable")
                ),
                new State("Q1 Invulnerable",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    //order Expand
                    new EntitiesNotWithinTransition(99, "Q1 Vulnerable Transition", "Treasure Oryx Defender")
                ),
                new State("Q1 Vulnerable Transition",
                    new State("T1",
                        new SetAltTexture(2),
                        new TimedTransition(50, "T2")
                    ),
                    new State("T2",
                        new SetAltTexture(0, 1, 100, loop: true)
                    ),
                    new TimedTransition(800, "Q1 Vulnerable")
                ),
                new State("Q1 Vulnerable",
                    new SetAltTexture(1),
                    new Taunt(0.75f, "My protectors!", "My guardians are gone!", "What have you done?",
                        "You destroy my guardians in my house? Blasphemy!"),
                    //order Shrink
                    new HealthTransition(0.75f, "Q2 Invulnerable Transition")
                ),
                new State("Q2 Invulnerable Transition",
                    new State("T1_2",
                        new SetAltTexture(2),
                        new TimedTransition(50, "T2_2")
                    ),
                    new State("T2_2",
                        new SetAltTexture(0, 1, 100, loop: true)
                    ),
                    new TimedTransition(800, "Q2 Spawn Minion")
                ),
                new State("Q2 Spawn Minion",
                    new SetAltTexture(0),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TossObject("Treasure Oryx Defender", 3, 0, 10000000),
                    new TossObject("Treasure Oryx Defender", 3, 90, 10000000),
                    new TossObject("Treasure Oryx Defender", 3, 180, 10000000),
                    new TossObject("Treasure Oryx Defender", 3, 270, 10000000),
                    new ChangeSize(-1, 60),
                    new TimedTransition(4000, "Q2 Invulnerable")
                ),
                new State("Q2 Invulnerable",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    //order expand
                    new EntitiesNotWithinTransition(99, "Q2 Vulnerable Transition", "Treasure Oryx Defender")
                ),
                new State("Q2 Vulnerable Transition",
                    new State("T1_3",
                        new SetAltTexture(2),
                        new TimedTransition(50, "T2_3")
                    ),
                    new State("T2_3",
                        new SetAltTexture(0, 1, 100, loop: true)
                    ),
                    new TimedTransition(800, "Q2 Vulnerable")
                ),
                new State("Q2 Vulnerable",
                    new SetAltTexture(1),
                    new Taunt(0.75f, "My protectors are no more!", "You Mongrels are ruining my beautiful treasure!",
                        "You won't leave with your pilfered loot!", "I'm weakened"),
                    //Shrink
                    new HealthTransition(0.6f, "Q3 Vulnerable Transition")
                ),
                new State("Q3 Vulnerable Transition",
                    new State("T1_4",
                        new SetAltTexture(2),
                        new TimedTransition(50, "T2_4")
                    ),
                    new State("T2_4",
                        new SetAltTexture(0, 1, 100, loop: true)
                    ),
                    new TimedTransition(800, "Q3")
                ),
                new State("Q3",
                    new SetAltTexture(1),
                    new State("Attack1",
                        new State("CardinalBarrage",
                            new TimedTransition(150, "OrdinalBarrage")
                        ),
                        new State("OrdinalBarrage",
                            new TimedTransition(150, "CardinalBarrage2")
                        ),
                        new State("CardinalBarrage2",
                            new TimedTransition(150, "OrdinalBarrage2")
                        ),
                        new State("OrdinalBarrage2",
                            new TimedTransition(150, "CardinalBarrage")
                        ),
                        new TimedTransition(8500, "Attack2")
                    ),
                    new State("Attack2",
                        new Flash(0x0000FF, 0.1f, 10),
                        new Shoot(0, 4, 90, 1, defaultAngle: 90,
                            cooldown: 10000000, cooldownOffset: 0),
                        new Shoot(0, 4, 90, 1, defaultAngle: 90,
                            cooldown: 10000000, cooldownOffset: 200),
                        new Shoot(0, 4, 90, 1, defaultAngle: 80,
                            cooldown: 10000000, cooldownOffset: 400),
                        new Shoot(0, 4, 90, 1, defaultAngle: 70,
                            cooldown: 10000000, cooldownOffset: 600),
                        new Shoot(0, 4, 90, 1, defaultAngle: 60,
                            cooldown: 10000000, cooldownOffset: 800),
                        new Shoot(0, 4, 90, 1, defaultAngle: 50,
                            cooldown: 10000000, cooldownOffset: 1000),
                        new Shoot(0, 4, 90, 1, defaultAngle: 40,
                            cooldown: 10000000, cooldownOffset: 1200),
                        new Shoot(0, 4, 90, 1, defaultAngle: 30,
                            cooldown: 10000000, cooldownOffset: 1400),
                        new Shoot(0, 4, 90, 1, defaultAngle: 20,
                            cooldown: 10000000, cooldownOffset: 1600),
                        new Shoot(0, 4, 90, 1, defaultAngle: 10,
                            cooldown: 10000000, cooldownOffset: 1800),
                        new Shoot(0, 4, 45, 1, defaultAngle: 0,
                            cooldown: 10000000, cooldownOffset: 2200),
                        new Shoot(0, 4, 45, 1, defaultAngle: 0,
                            cooldown: 10000000, cooldownOffset: 2400),
                        new Shoot(0, 4, 90, 1, defaultAngle: 0,
                            cooldown: 10000000, cooldownOffset: 2600),
                        new Shoot(0, 4, 90, 1, defaultAngle: 10,
                            cooldown: 10000000, cooldownOffset: 2800),
                        new Shoot(0, 4, 90, 1, defaultAngle: 20,
                            cooldown: 10000000, cooldownOffset: 3000),
                        new Shoot(0, 4, 90, 1, defaultAngle: 30,
                            cooldown: 10000000, cooldownOffset: 3200),
                        new Shoot(0, 4, 90, 1, defaultAngle: 40,
                            cooldown: 10000000, cooldownOffset: 3400),
                        new Shoot(0, 4, 90, 1, defaultAngle: 50,
                            cooldown: 10000000, cooldownOffset: 3600),
                        new Shoot(0, 4, 90, 1, defaultAngle: 60,
                            cooldown: 10000000, cooldownOffset: 3800),
                        new Shoot(0, 4, 90, 1, defaultAngle: 70,
                            cooldown: 10000000, cooldownOffset: 4000),
                        new Shoot(0, 4, 90, 1, defaultAngle: 80,
                            cooldown: 10000000, cooldownOffset: 4200),
                        new Shoot(0, 4, 90, 1, defaultAngle: 90,
                            cooldown: 10000000, cooldownOffset: 4400),
                        new Shoot(0, 4, 45, 1, defaultAngle: 90,
                            cooldown: 10000000, cooldownOffset: 4600),
                        new Shoot(0, 4, 90, 1, defaultAngle: 90,
                            cooldown: 10000000, cooldownOffset: 4800),
                        new Shoot(0, 4, 90, 1, defaultAngle: 90,
                            cooldown: 10000000, cooldownOffset: 5000),
                        new Shoot(0, 4, 90, 1, defaultAngle: 90,
                            cooldown: 10000000, cooldownOffset: 5200),
                        new Shoot(0, 4, 90, 1, defaultAngle: 80,
                            cooldown: 10000000, cooldownOffset: 5400),
                        new Shoot(0, 4, 90, 1, defaultAngle: 70,
                            cooldown: 10000000, cooldownOffset: 5600),
                        new Shoot(0, 4, 90, 1, defaultAngle: 60,
                            cooldown: 10000000, cooldownOffset: 5800),
                        new Shoot(0, 4, 90, 1, defaultAngle: 50,
                            cooldown: 10000000, cooldownOffset: 6000),
                        new Shoot(0, 4, 90, 1, defaultAngle: 40,
                            cooldown: 10000000, cooldownOffset: 6200),
                        new Shoot(0, 4, 90, 1, defaultAngle: 30,
                            cooldown: 10000000, cooldownOffset: 6400),
                        new Shoot(0, 4, 90, 1, defaultAngle: 20,
                            cooldown: 10000000, cooldownOffset: 6600),
                        new Shoot(0, 4, 90, 1, defaultAngle: 10,
                            cooldown: 10000000, cooldownOffset: 6800),
                        new Shoot(0, 4, 45, 1, defaultAngle: 0,
                            cooldown: 10000000, cooldownOffset: 7000),
                        new TimedTransition(7000, "Recuperate")
                    ),
                    new State("Recuperate",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new HealSelf(cooldown: 1000, amount: 200),
                        new TimedTransition(3000, "Attack1")
                    )
                ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Defense", 0.15f),
                    new ItemLoot("Potion of Attack", 0.15f),
                    new TierLoot(8, TierLoot.LootType.Weapon, 0.5f),
                    new TierLoot(8, TierLoot.LootType.Armor, 0.5f),
                    new TierLoot(9, TierLoot.LootType.Weapon, 0.1f),
                    new TierLoot(9, TierLoot.LootType.Armor, 0.1f),
                    new TierLoot(10, TierLoot.LootType.Weapon, 0.05f),
                    new TierLoot(10, TierLoot.LootType.Armor, 0.05f),
                    new TierLoot(5, TierLoot.LootType.Ability, 0.05f),
                    new TierLoot(4, TierLoot.LootType.Ability, 0.5f),
                    new TierLoot(4, TierLoot.LootType.Ring, 0.5f),
                    new TierLoot(5, TierLoot.LootType.Ring, 0.05f)
                )
            );
            db.Init("Treasure Oryx Defender",
                new Prioritize(
                    new Orbit(0.5f, 3, 6, "Golden Oryx Effigy",
                        0, 0)
                ),
                new Shoot(0, 8, 45, defaultAngle: 0, cooldown: 3000)
            );


            db.Init("Treasure Pot",
                new Threshold(0.01f,
                    new TierLoot(1, LootType.Ability, 0.12f),
                    new TierLoot(2, LootType.Ability, 0.08f),
                    new TierLoot(1, LootType.Ring, 0.1f),
                    new TierLoot(6, LootType.Weapon, 0.12f),
                    new ItemLoot("Magic Potion", 0.8f),
                    new ItemLoot("Health Potion", 0.8f)
                )
            );
            db.Init("Treasure Plunderer",
                new State("base",
                    new State("Player",
                        new PlayerWithinTransition(15, true, "Start")
                        ),
                    new State("Start",
                        new Shoot(7, 1, index: 0, cooldown: 1),
                        new Grenade(3, 75, 7, cooldown: 1000),
                        new Wander(0.4f)
                        )
                    ),
                new ItemLoot("Magic Potion", 0.3f),
                new ItemLoot("Health Potion", 0.3f),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Defense", 0.03f)
                    )
                );
            db.Init("Treasure Robber",
                new State("base",
                    new State("Player",
                        new PlayerWithinTransition(10, true, "Start")
                        ),
                    new State("Start",
                        new SetAltTexture(0, 0),
                        new Shoot(15, 3, index: 0, shootAngle: 20, cooldown: 1000),
                        new TimedTransition(2500, "Invisible"),
                        new Wander(0.4f)
                        ),
                    new State("Invisible",
                        new SetAltTexture(0, 7, 140),
                        new Shoot(15, 3, index: 0, shootAngle: 20, cooldown: 1000),
                        new Wander(0.4f),
                        new TimedTransition(6000, "Start")
                        )
                    ),
                new ItemLoot("Magic Potion", 0.3f),
                new ItemLoot("Health Potion", 0.3f)
                );
            db.Init("Treasure Thief",
                new State("base",
                    new State("Player",
                        new PlayerWithinTransition(10, true, "Start")
                        ),
                    new State("Start",
                        new Wander(0.6f),
                        new StayBack(0.6f, 6)
                        )
                    ),
                new ItemLoot("Magic Potion", 0.3f),
                new ItemLoot("Health Potion", 0.3f),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Attack", 0.01f),
                    new ItemLoot("Potion of Dexterity", 0.01f)
                    )
                );
            db.Init("Treasure Enemy",
                new State("base",
                    new State("Player",
                        new PlayerWithinTransition(4, true, "Start")
                        ),
                    new State("Start",
                        new Shoot(20, 2, index: 0, shootAngle: 20, cooldown: 1000),
                        new Shoot(20, 1, index: 1, cooldown: 1000),
                        new NoPlayerWithinTransition(4, true, "Player"),
                        new Follow(0.4f, 6, 1),
                        new Wander(0.4f)
                        )
                    )
                );
            db.Init("Treasure Rat",
                new State("base",
                    new State("Player",
                        new PlayerWithinTransition(7,true,  "Start")
                        ),
                    new State("Start",
                        new SetAltTexture(0, 1, cooldown: 140),
                        new ChangeSize(20, 200),
                        new Shoot(10, 1, index: 0, cooldown: 1500),
                        new Follow(0.3f, 10, 1),
                        new Wander(0.3f)
                        )
                    ),
                new ItemLoot("Health Potion", 0.3f),
                new ItemLoot("Magic Potion", 0.3f)
                );

            db.Init("Treasure Flame Trap 1.2 Sec",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new State("Wait",
                    new SetAltTexture(0),
                    new TimedTransition(1200, "Animation")
                ),
                new State("Animation", 
                    new SetAltTexture(1),
                    new Shoot(5, 1, 0, 0, cooldown: 50),
                    new TimedTransition(50, "Animation-1")
                ),
                new State("Animation-1",
                    new SetAltTexture(2),
                    new Shoot(5, 1, 0, 0, cooldown: 50),
                    new TimedTransition(50, "Animation-2")
                ),
                new State("Animation-2",
                    new SetAltTexture(3),
                    new Shoot(5, 1, 0, 0, cooldown: 50),
                    new TimedTransition(50, "Animation-3")
                ),
                new State("Animation-3",
                    new SetAltTexture(4),
                    new Shoot(5, 1, 0, 0, cooldown: 50),
                    new TimedTransition(50, "Animation-4")
                ),
                new State("Animation-4",
                    new SetAltTexture(5),
                    new Shoot(5, 1, 0, 0, cooldown: 50),
                    new TimedTransition(50, "Wait")
                )
            );

            db.Init("Treasure Flame Trap 1.7 Sec",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new State("Wait",
                    new SetAltTexture(0),
                    new TimedTransition(1700, "Animation")
                ),
                new State("Animation",
                    new SetAltTexture(1),
                    new Shoot(5, 1, 0, 0, cooldown: 50),
                    new TimedTransition(50, "Animation-1")
                ),
                new State("Animation-1",
                    new SetAltTexture(2),
                    new Shoot(5, 1, 0, 0, cooldown: 50),
                    new TimedTransition(50, "Animation-2")
                ),
                new State("Animation-2",
                    new SetAltTexture(3),
                    new Shoot(5, 1, 0, 0, cooldown: 50),
                    new TimedTransition(50, "Animation-3")
                ),
                new State("Animation-3",
                    new SetAltTexture(4),
                    new Shoot(5, 1, 0, 0, cooldown: 50),
                    new TimedTransition(50, "Animation-4")
                ),
                new State("Animation-4",
                    new SetAltTexture(5),
                    new Shoot(5, 1, 0, 0, cooldown: 50),
                    new TimedTransition(50, "Wait")
                )
            );

            db.Init("Gold Planet",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new EntityNotWithinTransition("Golden Oryx Effigy", 999, "Die"),
                new Prioritize(
                    new Orbit(0.5f, 7, 20, "Golden Oryx Effigy",
                        0, 0)
                ),
                new State("GreySpiral",
                    new Shoot(0, 2, 180, 1, defaultAngle: 90,
                        cooldown: 10000, cooldownOffset: 0),
                    new Shoot(0, 2, 180, 1, defaultAngle: 90,
                        cooldown: 10000, cooldownOffset: 400),
                    new Shoot(0, 2, 180, 1, defaultAngle: 80,
                        cooldown: 10000, cooldownOffset: 800),
                    new Shoot(0, 2, 180, 1, defaultAngle: 70,
                        cooldown: 10000, cooldownOffset: 1200),
                    new Shoot(0, 2, 180, 0, defaultAngle: 60,
                        cooldown: 10000, cooldownOffset: 1600),
                    new Shoot(0, 2, 180, 1, defaultAngle: 50,
                        cooldown: 10000, cooldownOffset: 2000),
                    new Shoot(0, 2, 180, 1, defaultAngle: 40,
                        cooldown: 10000, cooldownOffset: 2400),
                    new Shoot(0, 2, 180, 1, defaultAngle: 30,
                        cooldown: 10000, cooldownOffset: 2800),
                    new Shoot(0, 2, 180, 1, defaultAngle: 20,
                        cooldown: 10000, cooldownOffset: 3200),
                    new Shoot(0, 2, 180, 0, defaultAngle: 10,
                        cooldown: 10000, cooldownOffset: 3600),
                    new Shoot(0, 2, 180, 1, defaultAngle: 0,
                        cooldown: 10000, cooldownOffset: 4000),
                    new Shoot(0, 2, 180, 1, defaultAngle: -10,
                        cooldown: 10000, cooldownOffset: 4400),
                    new Shoot(0, 2, 180, 1, defaultAngle: -20,
                        cooldown: 10000, cooldownOffset: 4800),
                    new Shoot(0, 2, 180, 1, defaultAngle: -30,
                        cooldown: 10000, cooldownOffset: 5200),
                    new Shoot(0, 2, 180, 0, defaultAngle: -40,
                        cooldown: 10000, cooldownOffset: 5600),
                    new TimedTransition(5600, "Reset")
                ),
                new State("Reset",
                    new TimedTransition(0, "GreySpiral")
                ),
                new State("Die",
                    new Suicide()
                )
            );
        }
    }
}