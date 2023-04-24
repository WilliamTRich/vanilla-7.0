using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public sealed class HauntedCemetery : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            #region first

            db.Init("Arena Dn Spawner",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Leech"
                    ),
                    new State("Stage 1",
                        new Spawn("Classic Ghost", maxChildren: 1)
                    ),
                    new State("Stage 2",
                        new Spawn("Classic Ghost", maxChildren: 2)
                    ),
                    new State("Stage 3",
                        new Spawn("Classic Ghost", maxChildren: 2)
                    ),
                    new State("Stage 4",
                        new Spawn("Werewolf", maxChildren: 1),
                        new Spawn("Zombie Hulk", maxChildren: 1)
                    ),
                    new State("Stage 5",
                        new Suicide()
                    )
                 );
            
            db.Init("Arena Up Spawner",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Leech"
                    ),
                    new State("Stage 1",
                        new Spawn("Classic Ghost", maxChildren: 1)
                    ),
                    new State("Stage 2",
                        new Spawn("Werewolf", maxChildren: 1)
                    ),
                    new State("Stage 3",
                        new Spawn("Classic Ghost", maxChildren: 2)
                    ),
                    new State("Stage 4",
                        new Spawn("Classic Ghost", maxChildren: 3)
                    ),
                    new State("Stage 5",
                        new Suicide()
                    )
                );
            db.Init("Arena Lf Spawner",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Leech"
                    ),
                    new State("Stage 1",
                        new Spawn("Zombie Hulk", maxChildren: 1)
                    ),
                    new State("Stage 2",
                        new Spawn("Werewolf", maxChildren: 1),
                        new Spawn("Classic Ghost", maxChildren: 1)
                    ),
                    new State("Stage 3",
                        new Spawn("Classic Ghost", maxChildren: 1),
                        new Spawn("Werewolf", maxChildren: 1)
                    ),
                    new State("Stage 4",
                        new Spawn("Werewolf", maxChildren: 1),
                        new Spawn("Zombie Hulk", maxChildren: 1)
                    ),
                    new State("Stage 5",
                        new Suicide()
                    )
                );
            
            db.Init("Arena Rt Spawner",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Leech"
                    ),
                    new State("Stage 1",
                        new Spawn("Zombie Hulk", maxChildren: 1)
                    ),
                    new State("Stage 2",
                        new Spawn("Werewolf", maxChildren: 1)
                    ),
                    new State("Stage 3",
                        new Spawn("Classic Ghost", maxChildren: 2)
                    ),
                    new State("Stage 4",
                        new Spawn("Classic Ghost", maxChildren: 3)
                    ),
                    new State("Stage 5",
                        new Suicide()
                    )
                );
            
            db.Init("Area 1 Controller",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, perm: true),
                    new TransformOnDeath("Haunted Cemetery Gates Portal"),
                    new State("Start",
                        new PlayerWithinTransition(4, false, "1")
                    ),
                    new State("1",
                        new TimedTransition(0, "2")
                    ),
                    new State("2",
                        new SetAltTexture(2),
                        new TimedTransition(100, "3")
                    ),
                    new State("3",
                        new SetAltTexture(1),
                        new Taunt(true, "Welcome to my domain. l challenge you, warrior, to defeat my undead hordes and claim your prize...."),
                        new TimedTransition(2000, "4")
                    ),
                    new State("4",
                        new Taunt(true, "The waves are starting in 5 seconds..."),
                        new TimedTransition(5000, "7")
                    ),
                    new State("5",
                        new PlayerTextTransition("7", "ready"),
                        new SetAltTexture(3),
                        new TimedTransition(500, "6")
                    ),
                    new State("6",
                        new PlayerTextTransition("7", "ready"),
                        new SetAltTexture(4),
                        new TimedTransition(500, "6_1")
                    ),
                    new State("6_1",
                        new PlayerTextTransition("7", "ready"),
                        new SetAltTexture(3),
                        new TimedTransition(500, "6_2")
                    ),
                    new State("6_2",
                        new PlayerTextTransition("7", "ready"),
                        new SetAltTexture(4),
                        new TimedTransition(500, "6_3")
                    ),
                    new State("6_3",
                        new SetAltTexture(3),
                        new PlayerTextTransition("7", "ready"),
                        new TimedTransition(500, "6_4")
                    ),
                    new State("6_4",
                        new SetAltTexture(1),
                        new PlayerTextTransition("7", "ready")
                    ),
                    new State("7",
                        new SetAltTexture(0),
                        new OrderOnce(100, "Arena South Gate Spawner", "Stage 1"),
                        new OrderOnce(100, "Arena West Gate Spawner", "Stage 1"),
                        new OrderOnce(100, "Arena East Gate Spawner", "Stage 1"),
                        new OrderOnce(100, "Arena North Gate Spawner", "Stage 1"),
                        new EntityNotWithinTransition("Arena Skeleton", 999, "Check 1")
                    ),
                    new State("Check 1",
                        new EntitiesNotWithinTransition(100, "8", "Arena Skeleton", "Troll 1", "Troll 2")
                    ),
                    new State("8",
                        new SetAltTexture(2),
                        new TimedTransition(0, "9")
                    ),
                    new State("9",
                        new SetAltTexture(1),
                        new TimedTransition(2000, "10")
                    ),
                    new State("10",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "11")
                    ),
                    new State("11",
                        new SetAltTexture(3),
                        new TimedTransition(500, "12")
                    ),
                    new State("12",
                        new SetAltTexture(4),
                        new TimedTransition(500, "13")
                    ),
                    new State("13",
                        new SetAltTexture(3),
                        new TimedTransition(500, "14")
                    ),
                    new State("14",
                        new SetAltTexture(4),
                        new TimedTransition(500, "15")
                    ),
                    new State("15",
                        new SetAltTexture(3),
                        new TimedTransition(500, "16")
                    ),
                    new State("16",
                        new SetAltTexture(4),
                        new TimedTransition(500, "17")
                    ),
                    new State("17",
                        new SetAltTexture(3),
                        new TimedTransition(500, "18")
                    ),
                    new State("18",
                        new SetAltTexture(4),
                        new TimedTransition(500, "19")
                    ),
                    new State("19",
                        new SetAltTexture(3),
                        new TimedTransition(500, "20")
                    ),
                    new State("20",
                        new SetAltTexture(4),
                        new TimedTransition(500, "21")
                    ),
                    new State("21",
                        new SetAltTexture(3),
                        new TimedTransition(500, "22")
                    ),
                    new State("22",
                        new SetAltTexture(4),
                        new TimedTransition(500, "23")
                    ),
                    new State("23",
                        new SetAltTexture(3),
                        new TimedTransition(500, "24")
                    ),
                    new State("24",
                        new SetAltTexture(4),
                        new TimedTransition(500, "25")
                    ),
                    new State("25",
                        new SetAltTexture(1),
                        new TimedTransition(500, "26")
                    ),
                    new State("26",
                        new SetAltTexture(2),
                        new TimedTransition(100, "27")
                    ),
                    new State("27",
                        new SetAltTexture(0),
                        new OrderOnce(100, "Arena South Gate Spawner", "Stage 2"),
                        new OrderOnce(100, "Arena West Gate Spawner", "Stage 2"),
                        new OrderOnce(100, "Arena East Gate Spawner", "Stage 2"),
                        new OrderOnce(100, "Arena North Gate Spawner", "Stage 2"),
                        new EntityNotWithinTransition("Arena Skeleton", 999, "Check 2")
                        ),
                    new State("Check 2",
                        new EntitiesNotWithinTransition(100, "28", "Arena Skeleton", "Troll 1", "Troll 2")
                        ),
                    new State("28",
                        new SetAltTexture(2),
                        new TimedTransition(0, "29")
                        ),
                    new State("29",
                        new SetAltTexture(1),
                        new TimedTransition(2000, "30")
                        ),
                    new State("30",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "31")
                        ),
                    new State("31",
                        new SetAltTexture(3),
                        new TimedTransition(500, "32")
                        ),
                    new State("32",
                        new SetAltTexture(4),
                        new TimedTransition(500, "33")
                        ),
                    new State("33",
                        new SetAltTexture(3),
                        new TimedTransition(500, "34")
                        ),
                    new State("34",
                        new SetAltTexture(4),
                        new TimedTransition(500, "35")
                        ),
                    new State("35",
                        new SetAltTexture(3),
                        new TimedTransition(500, "36")
                        ),
                    new State("36",
                        new SetAltTexture(4),
                        new TimedTransition(500, "37")
                        ),
                    new State("37",
                        new SetAltTexture(3),
                        new TimedTransition(500, "38")
                        ),
                    new State("38",
                        new SetAltTexture(4),
                        new TimedTransition(500, "39")
                        ),
                    new State("39",
                        new SetAltTexture(3),
                        new TimedTransition(500, "40")
                        ),
                    new State("40",
                        new SetAltTexture(4),
                        new TimedTransition(500, "41")
                        ),
                    new State("41",
                        new SetAltTexture(3),
                        new TimedTransition(500, "42")
                        ),
                    new State("42",
                        new SetAltTexture(4),
                        new TimedTransition(500, "43")
                        ),
                    new State("43",
                        new SetAltTexture(3),
                        new TimedTransition(500, "44")
                        ),
                    new State("44",
                        new SetAltTexture(4),
                        new TimedTransition(500, "45")
                        ),
                    new State("45",
                        new SetAltTexture(1),
                        new TimedTransition(100, "46")
                        ),
                    new State("46",
                        new SetAltTexture(2),
                        new TimedTransition(100, "47")
                        ),
                    new State("47",
                        new SetAltTexture(0),
                        new OrderOnce(100, "Arena South Gate Spawner", "Stage 3"),
                        new OrderOnce(100, "Arena West Gate Spawner", "Stage 3"),
                        new OrderOnce(100, "Arena East Gate Spawner", "Stage 3"),
                        new OrderOnce(100, "Arena North Gate Spawner", "Stage 3"),
                        new EntityNotWithinTransition("Arena Skeleton", 999, "Check 3")
                        ),
                    new State("Check 3",
                        new EntitiesNotWithinTransition(100, "48", "Arena Skeleton", "Troll 1", "Troll 2")
                        ),
                    new State("48",
                        new SetAltTexture(2),
                        new TimedTransition(0, "49")
                        ),
                    new State("49",
                        new SetAltTexture(1),
                        new TimedTransition(2000, "50")
                        ),
                    new State("50",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "51")
                        ),
                    new State("51",
                        new SetAltTexture(3),
                        new TimedTransition(500, "52")
                        ),
                    new State("52",
                        new SetAltTexture(4),
                        new TimedTransition(500, "53")
                        ),
                    new State("53",
                        new SetAltTexture(3),
                        new TimedTransition(500, "54")
                        ),
                    new State("54",
                        new SetAltTexture(4),
                        new TimedTransition(500, "55")
                        ),
                    new State("55",
                        new SetAltTexture(3),
                        new TimedTransition(500, "56")
                        ),
                    new State("56",
                        new SetAltTexture(4),
                        new TimedTransition(500, "57")
                        ),
                    new State("57",
                        new SetAltTexture(3),
                        new TimedTransition(500, "58")
                        ),
                    new State("58",
                        new SetAltTexture(4),
                        new TimedTransition(500, "59")
                        ),
                    new State("59",
                        new SetAltTexture(3),
                        new TimedTransition(500, "60")
                        ),
                    new State("60",
                        new SetAltTexture(4),
                        new TimedTransition(500, "61")
                        ),
                    new State("61",
                        new SetAltTexture(3),
                        new TimedTransition(500, "62")
                        ),
                    new State("62",
                        new SetAltTexture(4),
                        new TimedTransition(500, "63")
                        ),
                    new State("63",
                        new SetAltTexture(3),
                        new TimedTransition(500, "64")
                        ),
                    new State("64",
                        new SetAltTexture(4),
                        new TimedTransition(500, "65")
                        ),
                    new State("65",
                        new SetAltTexture(1),
                        new TimedTransition(100, "66")
                        ),
                    new State("66",
                        new SetAltTexture(2),
                        new TimedTransition(100, "67")
                        ),
                    new State("67",
                        new SetAltTexture(0),
                        new OrderOnce(100, "Arena South Gate Spawner", "Stage 4"),
                        new OrderOnce(100, "Arena West Gate Spawner", "Stage 4"),
                        new OrderOnce(100, "Arena East Gate Spawner", "Stage 4"),
                        new OrderOnce(100, "Arena North Gate Spawner", "Stage 4"),
                        new EntitiesNotWithinTransition(999, "Check 4", "Arena Skeleton")
                        ),
                    new State("Check 4",
                        new EntitiesNotWithinTransition(100, "68", "Arena Skeleton", "Troll 1", "Troll 2")
                        ),
                    new State("68",
                        new SetAltTexture(2),
                        new TimedTransition(0, "69")
                        ),
                    new State("69",
                        new SetAltTexture(1),
                        new TimedTransition(2000, "70")
                        ),
                    new State("70",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "71")
                        ),
                    new State("71",
                        new SetAltTexture(3),
                        new TimedTransition(500, "72")
                        ),
                    new State("72",
                        new SetAltTexture(4),
                        new TimedTransition(500, "73")
                        ),
                    new State("73",
                        new SetAltTexture(3),
                        new TimedTransition(500, "74")
                        ),
                    new State("74",
                        new SetAltTexture(4),
                        new TimedTransition(500, "75")
                        ),
                    new State("75",
                        new SetAltTexture(3),
                        new TimedTransition(500, "76")
                        ),
                    new State("76",
                        new SetAltTexture(4),
                        new TimedTransition(500, "77")
                        ),
                    new State("77",
                        new SetAltTexture(3),
                        new TimedTransition(500, "78")
                        ),
                    new State("78",
                        new SetAltTexture(4),
                        new TimedTransition(500, "79")
                        ),
                    new State("79",
                        new SetAltTexture(3),
                        new TimedTransition(500, "80")
                        ),
                    new State("80",
                        new SetAltTexture(4),
                        new TimedTransition(500, "81")
                        ),
                    new State("81",
                        new SetAltTexture(3),
                        new TimedTransition(500, "82")
                        ),
                    new State("82",
                        new SetAltTexture(4),
                        new TimedTransition(500, "83")
                        ),
                    new State("83",
                        new SetAltTexture(3),
                        new TimedTransition(500, "84")
                        ),
                    new State("84",
                        new SetAltTexture(4),
                        new TimedTransition(500, "85")
                        ),
                    new State("85",
                        new SetAltTexture(1),
                        new TimedTransition(100, "86")
                        ),
                    new State("86",
                        new SetAltTexture(2),
                        new TimedTransition(100, "87")
                        ),
                    new State("87",
                        new SetAltTexture(0),
                        new OrderOnce(100, "Arena South Gate Spawner", "Stage 5"),
                        new OrderOnce(100, "Arena West Gate Spawner", "Stage 5"),
                        new OrderOnce(100, "Arena East Gate Spawner", "Stage 5"),
                        new OrderOnce(100, "Arena North Gate Spawner", "Stage 5"),
                        new EntitiesNotWithinTransition(100, "Check 5", "Troll 3")
                        ),
                    new State("Check 5",
                        new EntitiesNotWithinTransition(100, "88", "Troll 3")
                        ),
                    new State("88",
                        new Suicide()
                        )
                    );
            db.Init("Arena South Gate Spawner",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Leech"
                    ),
                new State("Stage 1",
                    new Spawn("Arena Skeleton", maxChildren: 2)
                    ),
                new State("Stage 2",
                    new Spawn("Arena Skeleton", maxChildren: 2)
                    ),
                new State("Stage 3",
                    new Spawn("Troll 1", maxChildren: 1)
                    ),
                new State("Stage 4",
                    new Spawn("Troll 1", maxChildren: 1)
                    ),
                new State("Stage 5",
                    new Suicide()
                    ),
                new State("Stage 6",
                    new Spawn("Arena Ghost 2", maxChildren: 1)
                    ),
                new State("Stage 7",
                    new Spawn("Arena Ghost 2", maxChildren: 1)
                    ),
               new State("Stage 8",
                    new Spawn("Arena Ghost 2", maxChildren: 1)
                    ),
                new State("Stage 9",
                    new Spawn("Arena Possessed Girl", maxChildren: 1)
                    ),
                new State("Stage 10",
                    new Suicide()
                    ),
                new State("Stage 11",
                    new Spawn("Arena Risen Brawler", maxChildren: 1)
                    ),
                new State("Stage 12",
                    new Spawn("Arena Risen Warrior", maxChildren: 1)
                    ),
                new State("Stage 13",
                    new Spawn("Arena Risen Warrior", maxChildren: 1)
                    ),
                new State("Stage 14",
                    new Spawn("Arena Risen Warrior", maxChildren: 1),
                    new Spawn("Arena Risen Archer", maxChildren: 1)
                    ),
                new State("Stage 15",
                    new Suicide()
                    )
                );
            
            db.Init("Arena East Gate Spawner",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Leech"
                    ),
                new State("Stage 1",
                    new Spawn("Arena Skeleton", maxChildren: 1)
                    ),
                new State("Stage 2",
                    new Spawn("Arena Skeleton", maxChildren: 2)
                    ),
                new State("Stage 3",
                    new Spawn("Arena Skeleton", maxChildren: 1),
                    new Spawn("Troll 2", maxChildren: 1)
                    ),
                new State("Stage 4",
                    new Spawn("Arena Skeleton", maxChildren: 1),
                    new Spawn("Troll 2", maxChildren: 1)
                    ),
                new State("Stage 5",
                    new Suicide()
                    ),
                new State("Stage 6",
                    new Spawn("Arena Ghost 1", maxChildren: 1)
                    ),
                new State("Stage 7",
                    new Spawn("Arena Ghost 1", maxChildren: 1)
                    ),
                new State("Stage 8",
                    new Spawn("Arena Ghost 2", maxChildren: 1)
                    ),
                new State("Stage 9",
                    new Spawn("Arena Ghost 2", maxChildren: 1)
                    ),
                new State("Stage 10",
                    new Spawn("Arena Possessed Girl", maxChildren: 1)
                    ),
                new State("Stage 11",
                    new Spawn("Arena Risen Warrior", maxChildren: 1)
                    ),
                new State("Stage 12",
                    new Spawn("Arena Risen Brawler", maxChildren: 1)
                    ),
                new State("Stage 13",
                    new Spawn("Arena Risen Brawler", maxChildren: 1)
                    ),
                new State("Stage 14",
                    new Spawn("Arena Risen Brawler", maxChildren: 1)
                    ),
                new State("Stage 15",
                    new Suicide()
                    )
                );
            db.Init("Arena West Gate Spawner",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Leech"
                    ),
                new State("Stage 1",
                    new Spawn("Arena Skeleton", maxChildren: 1)
                    ),
                new State("Stage 2",
                    new Spawn("Arena Skeleton", maxChildren: 2)
                    ),
                new State("Stage 3",
                    new Spawn("Arena Skeleton", maxChildren: 1),
                    new Spawn("Troll 2", maxChildren: 1)
                    ),
                new State("Stage 4",
                    new Spawn("Arena Skeleton", maxChildren: 1),
                    new Spawn("Troll 2", maxChildren: 1)
                    ),
                new State("Stage 5",
                    new Suicide()
                    ),
                new State("Stage 6",
                    new Spawn("Arena Ghost 1", maxChildren: 1)
                    ),
                new State("Stage 7",
                    new Spawn("Arena Ghost 2", maxChildren: 1)
                    ),
                new State("Stage 8",
                    new Spawn("Arena Ghost 2", maxChildren: 1)
                    ),
                new State("Stage 9",
                    new Spawn("Arena Possessed Girl", maxChildren: 1)
                    ),
                new State("Stage 10",
                    new Spawn("Arena Possessed Girl", maxChildren: 1)
                    ),
                new State("Stage 11",
                    new Spawn("Arena Risen Archer", maxChildren: 1)
                    ),
                new State("Stage 12",
                    new Spawn("Arena Risen Brawler", maxChildren: 1),
                    new Spawn("Arena Risen Warrior", maxChildren: 1)
                    ),
                new State("Stage 13",
                    new Spawn("Arena Risen Archer", maxChildren: 1)
                    ),
                new State("Stage 14",
                    new Spawn("Arena Risen Archer", maxChildren: 1),
                    new Spawn("Arena Risen Mage", maxChildren: 1)
                    ),
                new State("Stage 15",
                    new Suicide()
                    )
                );
            db.Init("Arena North Gate Spawner",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("Leech"),
                new State("Stage 1",
                    new Spawn("Arena Skeleton", maxChildren: 1)
                    ),
                new State("Stage 2",
                    new Spawn("Troll 1", maxChildren: 1)
                    ),
                new State("Stage 3",
                    new Spawn("Troll 2", maxChildren: 1)
                    ),
                new State("Stage 4",
                    new Spawn("Troll 2", maxChildren: 1)
                    ),
                new State("Stage 5",
                    new Spawn("Troll 3", maxChildren: 1)
                    ),
                new State("Stage 6",
                    new Spawn("Arena Ghost 1", maxChildren: 1)
                    ),
                new State("Stage 7",
                    new Spawn("Arena Possessed Girl", maxChildren: 1)
                    ),
                new State("Stage 8",
                    new Spawn("Arena Ghost 2", maxChildren: 1)
                    ),
                new State("Stage 9",
                    new Spawn("Arena Ghost 1", maxChildren: 1)
                    ),
                new State("Stage 10",
                    new Spawn("Arena Ghost Bride", maxChildren: 1)
                    ),
                new State("Stage 11",
                    new Spawn("Arena Risen Mage", maxChildren: 1)
                    ),
                new State("Stage 12",
                    new Spawn("Arena Risen Warrior", maxChildren: 2)
                    ),
                new State("Stage 13",
                    new Spawn("Arena Risen Warrior", maxChildren: 1),
                    new Spawn("Arena Risen Mage", maxChildren: 1)
                    ),
                new State("Stage 14",
                    new Spawn("Arena Risen Warrior", maxChildren: 2)
                    ),
                new State("Stage 15",
                    new Spawn("Arena Grave Caretaker", maxChildren: 1)
                    )
                );
            db.Init("Arena Skeleton",
                new State("base",
                    new Prioritize(
                        new Follow(speed: 0.5f, acquireRange: 8, range: 2, duration: 2000, cooldown: 3500),
                        new Wander(speed: 0.5f)
                    ),
                    new Shoot(range: 8, count: 1, index: 0, cooldown: 800)
                )
            );
            db.Init("Troll 1",
                new State("base",
                    new Prioritize(
                        new Charge(speed: 1.1f, range: 8, cooldown: 3000),
                        new Follow(speed: 0.5f, acquireRange: 15, range: 2, duration: 4000, cooldown: 2000)
                    ),
                    new Shoot(range: 5, count: 1, index: 0, cooldown: 1000)
                )
            );
            db.Init("Troll 2",
                new State("base",
                    new Orbit(speed: 0.5f, radius: 5, acquireRange: 10, target: null),
                    new Prioritize(
                        new Follow(speed: 1.1f, acquireRange: 15, range: 6, duration: 4000, cooldown: 5000)
                    ),
                    new Shoot(range: 8, count: 1, index: 0, predictive: 1, cooldown: 1600),
                    new Grenade(radius: 3, range: 6, damage: 85, cooldown: 2000)
                    )
            );
            db.Init("Troll 3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true, -1),
                    new State("Check1",
                        new TimedTransition(1500, "2")
                    ),
                    new State("2",
                        new MoveTo(speed: 0.9f, x: 21, y: 21),
                        new Taunt("This forest will be your tomb!"),
                        new TimedTransition(time: 2000, targetStates: "Normal")
                    ),
                    new State("Normal",
                        new Prioritize(
                            new Wander(speed: 0.3f)
                        ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                        new TossObject(child: "Arena Mushroom", range: 7, cooldown: 3000),
                        new Follow(speed: 0.6f, acquireRange: 10, range: 3, duration: 5000, cooldown: 5500),
                        new Shoot(range: 8, count: 1, index: 0, cooldown: 1000),
                        new Shoot(range: 24, count: 6, shootAngle: 60, index: 1, fixedAngle: 30, cooldown: 2000),
                        new TossObject(child: "Arena Mushroom", range: 7, cooldown: 3000),
                        new HealthTransition(threshold: 0.7f, targetStates: "Summon")
                    ),
                    new State("Summon",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true, -1),
                        new Taunt("I call upon the aid of warriors past! Smite these trespassers!"),
                        new Spawn(children: "Arena Skeleton", maxChildren: 5, cooldown: 4000),
                        new Shoot(range: 24, count: 6, shootAngle: 60, index: 1, fixedAngle: 0, cooldown: 1500),
                        new Shoot(range: 24, count: 6, shootAngle: 60, index: 1, fixedAngle: 30, cooldown: 1500),
                        new TossObject(child: "Arena Mushroom", range: 7, cooldown: 3000),
                        new EntitiesNotWithinTransition(99, "Enrage", "Arena Skeleton")
                    ),
                    new State("Enrage",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                        new Flash(color: 0xFFFFFF, flashPeriod: 0.1f, repeats: 15),
                        new ChangeSize(rate: 1, target: 200),
                        new Follow(speed: 1.1f, acquireRange: 10, range: 3, duration: 5000, cooldown: 5500),
                        new Charge(speed: 1.3f, range: 7, cooldown: 3000),
                        new Shoot(range: 24, count: 6, shootAngle: 60, index: 1, fixedAngle: 0, cooldown: 900),
                        new TossObject(child: "Arena Mushroom", range: 7, cooldown: 1500),
                        new TimedTransition(time: 15000, targetStates: "Normal 2")
                        ),
                    new State("Normal 2",
                        new Wander(speed: 0.3f),
                        new ChangeSize(rate: 1, target: 150),
                        new Follow(speed: 1.1f, acquireRange: 10, range: 3, duration: 5000, cooldown: 5500),
                        new TossObject(child: "Arena Mushroom", range: 7, cooldown: 3000),
                        new Shoot(range: 8, count: 1, index: 0, cooldown: 1000),
                        new HealthTransition(threshold: 0.4f, targetStates: "Summon")
                    ),
                new Threshold(0.01f,
                    new TierLoot(6, TierLoot.LootType.Weapon, 0.4f),
                    new TierLoot(6, TierLoot.LootType.Weapon, 0.4f),
                    new TierLoot(6, TierLoot.LootType.Armor, 0.4f),
                    new TierLoot(7, TierLoot.LootType.Weapon, 0.25f),
                    new TierLoot(8, TierLoot.LootType.Weapon, 0.25f),
                    new TierLoot(8, TierLoot.LootType.Weapon, 0.25f),
                    new TierLoot(9, TierLoot.LootType.Weapon, 0.125f),
                    new TierLoot(3, TierLoot.LootType.Ability, 0.25f),
                    new TierLoot(4, TierLoot.LootType.Ability, 0.125f),
                    new TierLoot(7, TierLoot.LootType.Armor, 0.4f),
                    new TierLoot(8, TierLoot.LootType.Armor, 0.25f),
                    new TierLoot(9, TierLoot.LootType.Armor, 0.25f),
                    new TierLoot(3, TierLoot.LootType.Ring, 0.25f),
                    new TierLoot(4, TierLoot.LootType.Ring, 0.125f),
                    new TierLoot(5, TierLoot.LootType.Ability, 0.0625f),
                    new ItemLoot("Wine Cellar Incantation", 0.05f),
                    new ItemLoot("Potion of Speed", 0.3f, 3),
                    new ItemLoot("Potion of Wisdom", 0.3f)
                )
            );
            db.Init("Arena Mushroom",
                    new State("Ini",
                        new PlayerWithinTransition(radius: 3, targetStates: "A1", seeInvis: true)
                    ),
                    new State("A1",
                        new TimedTransition(time: 750, targetStates: "A2")
                    ),
                    new State("A2",
                        new SetAltTexture(1),
                        new TimedTransition(time: 750, targetStates: "A3")
                    ),
                    new State("A3",
                        new SetAltTexture(2),
                        new TimedTransition(time: 1000, targetStates: "Explode")
                    ),
                    new State("Explode",
                        new Flash(color: 0xFFFFFF, flashPeriod: 0.1f, repeats: 30),
                        new TimedTransition(time: 3000, targetStates: "Suicide")
                        ),
                    new State("Suicide",
                        new Shoot(range: 24, count: 6, shootAngle: 60, fixedAngle: 30),
                        new Suicide()
                    )
                );

            #endregion

            #region gates/second

            db.Init("Area 2 Controller",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new TransformOnDeath("Haunted Cemetery Graves Portal"),
                    new State("Start",
                        new PlayerWithinTransition(4, true, "1")
                        ),
                    new State("1",
                        new TimedTransition(0, "2")
                        ),
                    new State("2",
                        new SetAltTexture(2),
                        new TimedTransition(100, "3")
                        ),
                    new State("3",
                        new SetAltTexture(1),
                        new Taunt(true, "Congratulations on making it past the first area! This area will not be so easy!"),
                        new TimedTransition(2000, "4")
                        ),
                    new State("4",
                        new Taunt(true, "The waves are starting in 5 seconds..."),
                        new TimedTransition(5000, "7")
                        ),
                    //repeat
                    new State("5",
                        new PlayerTextTransition("7", "ready"),
                        new SetAltTexture(3),
                        new TimedTransition(500, "6")
                        ),
                    new State("6",
                        new PlayerTextTransition("7", "ready"),
                        new SetAltTexture(4),
                        new TimedTransition(500, "6_1")
                        ),
                    new State("6_1",
                        new PlayerTextTransition("7", "ready"),
                        new SetAltTexture(3),
                        new TimedTransition(500, "6_2")
                        ),
                    new State("6_2",
                        new PlayerTextTransition("7", "ready"),
                        new SetAltTexture(4),
                        new TimedTransition(500, "6_3")
                        ),
                    new State("6_3",
                        new PlayerTextTransition("7", "ready"),
                        new SetAltTexture(3),
                        new TimedTransition(500, "6_4")
                        ),
                    new State("6_4",
                        new SetAltTexture(1),
                        new PlayerTextTransition("7", "ready")
                        ),
                    new State("7",
                        new SetAltTexture(0),
                        new OrderOnce(100, "Arena South Gate Spawner", "Stage 6"),
                        new OrderOnce(100, "Arena West Gate Spawner", "Stage 6"),
                        new OrderOnce(100, "Arena East Gate Spawner", "Stage 6"),
                        new OrderOnce(100, "Arena North Gate Spawner", "Stage 6"),
                        new EntityWithinTransition("Arena Ghost 2", 999, "Check 1")
                        ),
                    new State("Check 1",
                        new EntitiesNotWithinTransition(100, "8", "Arena Ghost 1", "Arena Ghost 2", "Arena Possessed Girl")
                        ),
                    new State("8",
                        new SetAltTexture(2),
                        new TimedTransition(0, "9")
                        ),
                    new State("9",
                        new SetAltTexture(1),
                        new TimedTransition(2000, "10")
                        ),
                    new State("10",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "11")
                        ),
                    new State("11",
                        new SetAltTexture(3),
                        new TimedTransition(500, "12")
                        ),
                    new State("12",
                        new SetAltTexture(4),
                        new TimedTransition(500, "13")
                        ),
                    new State("13",
                        new SetAltTexture(3),
                        new TimedTransition(500, "14")
                        ),
                    new State("14",
                        new SetAltTexture(4),
                        new TimedTransition(500, "15")
                        ),
                    new State("15",
                        new SetAltTexture(3),
                        new TimedTransition(500, "16")
                        ),
                    new State("16",
                        new SetAltTexture(4),
                        new TimedTransition(500, "17")
                        ),
                    new State("17",
                        new SetAltTexture(3),
                        new TimedTransition(500, "18")
                        ),
                    new State("18",
                        new SetAltTexture(4),
                        new TimedTransition(500, "19")
                        ),
                    new State("19",
                        new SetAltTexture(3),
                        new TimedTransition(500, "20")
                        ),
                    new State("20",
                        new SetAltTexture(4),
                        new TimedTransition(500, "21")
                        ),
                    new State("21",
                        new SetAltTexture(3),
                        new TimedTransition(500, "22")
                        ),
                    new State("22",
                        new SetAltTexture(4),
                        new TimedTransition(500, "23")
                        ),
                    new State("23",
                        new SetAltTexture(3),
                        new TimedTransition(500, "24")
                        ),
                    new State("24",
                        new SetAltTexture(4),
                        new TimedTransition(500, "25")
                        ),
                    new State("25",
                        new SetAltTexture(1),
                        new TimedTransition(100, "26")
                        ),
                    new State("26",
                        new SetAltTexture(2),
                        new TimedTransition(100, "27")
                        ),
                    new State("27",
                        new SetAltTexture(0),
                        new OrderOnce(100, "Arena South Gate Spawner", "Stage 7"),
                        new OrderOnce(100, "Arena West Gate Spawner", "Stage 7"),
                        new OrderOnce(100, "Arena East Gate Spawner", "Stage 7"),
                        new OrderOnce(100, "Arena North Gate Spawner", "Stage 7"),
                        new EntityWithinTransition("Arena Ghost 2", 999, "Check 2")
                        ),
                    new State("Check 2",
                        new EntitiesNotWithinTransition(100, "28", "Arena Ghost 1", "Arena Ghost 2", "Arena Possessed Girl")
                        ),
                    new State("28",
                        new SetAltTexture(2),
                        new TimedTransition(0, "29")
                        ),
                    new State("29",
                        new SetAltTexture(1),
                        new TimedTransition(2000, "30")
                        ),
                    new State("30",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "31")
                        ),
                    new State("31",
                        new SetAltTexture(3),
                        new TimedTransition(500, "32")
                        ),
                    new State("32",
                        new SetAltTexture(4),
                        new TimedTransition(500, "33")
                        ),
                    new State("33",
                        new SetAltTexture(3),
                        new TimedTransition(500, "34")
                        ),
                    new State("34",
                        new SetAltTexture(4),
                        new TimedTransition(500, "35")
                        ),
                    new State("35",
                        new SetAltTexture(3),
                        new TimedTransition(500, "36")
                        ),
                    new State("36",
                        new SetAltTexture(4),
                        new TimedTransition(500, "37")
                        ),
                    new State("37",
                        new SetAltTexture(3),
                        new TimedTransition(500, "38")
                        ),
                    new State("38",
                        new SetAltTexture(4),
                        new TimedTransition(500, "39")
                        ),
                    new State("39",
                        new SetAltTexture(3),
                        new TimedTransition(500, "40")
                        ),
                    new State("40",
                        new SetAltTexture(4),
                        new TimedTransition(500, "41")
                        ),
                    new State("41",
                        new SetAltTexture(3),
                        new TimedTransition(500, "42")
                        ),
                    new State("42",
                        new SetAltTexture(4),
                        new TimedTransition(500, "43")
                        ),
                    new State("43",
                        new SetAltTexture(3),
                        new TimedTransition(500, "44")
                        ),
                    new State("44",
                        new SetAltTexture(4),
                        new TimedTransition(500, "45")
                        ),
                    new State("45",
                        new SetAltTexture(1),
                        new TimedTransition(500, "46")
                        ),
                    new State("46",
                        new SetAltTexture(2),
                        new TimedTransition(500, "47")
                        ),
                    new State("47",
                        new SetAltTexture(0),
                        new OrderOnce(100, "Arena South Gate Spawner", "Stage 8"),
                        new OrderOnce(100, "Arena West Gate Spawner", "Stage 8"),
                        new OrderOnce(100, "Arena East Gate Spawner", "Stage 8"),
                        new OrderOnce(100, "Arena North Gate Spawner", "Stage 8"),
                        new EntityWithinTransition("Arena Ghost 2", 999, "Check 3")
                        ),
                    new State("Check 3",
                        new EntitiesNotWithinTransition(100, "48", "Arena Ghost 1", "Arena Ghost 2", "Arena Possessed Girl")
                        ),
                    new State("48",
                        new SetAltTexture(2),
                        new TimedTransition(0, "49")
                        ),
                    new State("49",
                        new SetAltTexture(1),
                        new TimedTransition(2000, "50")
                        ),
                    new State("50",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "51")
                        ),
                    new State("51",
                        new SetAltTexture(3),
                        new TimedTransition(500, "52")
                        ),
                    new State("52",
                        new SetAltTexture(4),
                        new TimedTransition(500, "53")
                        ),
                    new State("53",
                        new SetAltTexture(3),
                        new TimedTransition(500, "54")
                        ),
                    new State("54",
                        new SetAltTexture(4),
                        new TimedTransition(500, "55")
                        ),
                    new State("55",
                        new SetAltTexture(3),
                        new TimedTransition(500, "56")
                        ),
                    new State("56",
                        new SetAltTexture(4),
                        new TimedTransition(500, "57")
                        ),
                    new State("57",
                        new SetAltTexture(3),
                        new TimedTransition(500, "58")
                        ),
                    new State("58",
                        new SetAltTexture(4),
                        new TimedTransition(500, "59")
                        ),
                    new State("59",
                        new SetAltTexture(3),
                        new TimedTransition(500, "60")
                        ),
                    new State("60",
                        new SetAltTexture(4),
                        new TimedTransition(500, "61")
                        ),
                    new State("61",
                        new SetAltTexture(3),
                        new TimedTransition(500, "62")
                        ),
                    new State("62",
                        new SetAltTexture(4),
                        new TimedTransition(500, "63")
                        ),
                    new State("63",
                        new SetAltTexture(3),
                        new TimedTransition(500, "64")
                        ),
                    new State("64",
                        new SetAltTexture(4),
                        new TimedTransition(500, "65")
                        ),
                    new State("65",
                        new SetAltTexture(1),
                        new TimedTransition(100, "66")
                        ),
                    new State("66",
                        new SetAltTexture(2),
                        new TimedTransition(100, "67")
                        ),
                    new State("67",
                        new SetAltTexture(0),
                        new OrderOnce(100, "Arena South Gate Spawner", "Stage 9"),
                        new OrderOnce(100, "Arena West Gate Spawner", "Stage 9"),
                        new OrderOnce(100, "Arena East Gate Spawner", "Stage 9"),
                        new OrderOnce(100, "Arena North Gate Spawner", "Stage 9"),
                        new EntityWithinTransition("Arena Ghost 2", 999, "Check 4")
                        ),
                    new State("Check 4",
                        new EntitiesNotWithinTransition(100, "68", "Arena Ghost 1", "Arena Ghost 2", "Arena Possessed Girl")
                        ),
                    new State("68",
                        new SetAltTexture(2),
                        new TimedTransition(0, "69")
                        ),
                    new State("69",
                        new SetAltTexture(1),
                        new TimedTransition(2000, "70")
                        ),
                    new State("70",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "71")
                        ),
                    new State("71",
                        new SetAltTexture(3),
                        new TimedTransition(500, "72")
                        ),
                    new State("72",
                        new SetAltTexture(4),
                        new TimedTransition(500, "73")
                        ),
                    new State("73",
                        new SetAltTexture(3),
                        new TimedTransition(500, "74")
                        ),
                    new State("74",
                        new SetAltTexture(4),
                        new TimedTransition(500, "75")
                        ),
                    new State("75",
                        new SetAltTexture(3),
                        new TimedTransition(500, "76")
                        ),
                    new State("76",
                        new SetAltTexture(4),
                        new TimedTransition(500, "77")
                        ),
                    new State("77",
                        new SetAltTexture(3),
                        new TimedTransition(500, "78")
                        ),
                    new State("78",
                        new SetAltTexture(4),
                        new TimedTransition(500, "79")
                        ),
                    new State("79",
                        new SetAltTexture(3),
                        new TimedTransition(500, "80")
                        ),
                    new State("80",
                        new SetAltTexture(4),
                        new TimedTransition(500, "81")
                        ),
                    new State("81",
                        new SetAltTexture(3),
                        new TimedTransition(500, "82")
                        ),
                    new State("82",
                        new SetAltTexture(4),
                        new TimedTransition(500, "83")
                        ),
                    new State("83",
                        new SetAltTexture(3),
                        new TimedTransition(500, "84")
                        ),
                    new State("84",
                        new SetAltTexture(4),
                        new TimedTransition(500, "85")
                        ),
                    new State("85",
                        new SetAltTexture(1),
                        new TimedTransition(100, "86")
                        ),
                    new State("86",
                        new SetAltTexture(2),
                        new TimedTransition(100, "87")
                        ),
                    new State("87",
                        new SetAltTexture(0),
                        new OrderOnce(100, "Arena South Gate Spawner", "Stage 10"),
                        new OrderOnce(100, "Arena West Gate Spawner", "Stage 10"),
                        new OrderOnce(100, "Arena East Gate Spawner", "Stage 10"),
                        new OrderOnce(100, "Arena North Gate Spawner", "Stage 10"),
                        new EntityWithinTransition("Arena Possessed Girl", 999, "Check 5")
                        ),
                    new State("Check 5",
                        new EntitiesNotWithinTransition(100, "88", "Arena Ghost Bride", "Arena Possessed Girl")
                        ),
                    new State("88",
                        new Suicide()
                        )
                    );
                
                db.Init("Arena Ghost 1",
                    new State("base",
                        new Prioritize(
                            new Orbit(speed: 0.5f, radius: 2.8f, acquireRange: 10, target: null),
                            new Charge(speed: 0.8f, range: 11, cooldown: 2000)
                        ),
                        new Wander(speed: 0.6f),
                        new Shoot(range: 5.5f, count: 1, index: 0, cooldown: 1000)
                    )
                 );
            db.Init("Arena Ghost 2",
                new State("base",
                    new State("Ini",
                        new SetAltTexture(0),
                        new Prioritize(
                            new Wander(speed: 0.3f),
                            new Charge(speed: 2.6f, range: 12, cooldown: 2000),
                            new StayBack(speed: 0.8f, distance: 4, entity: null)
                        ),
                        new Shoot(range: 5, count: 3, shootAngle: 20, index: 0, cooldown: 1000),
                        new TimedTransition(time: 2000, targetStates: "Disappear")
                    ),
                    new State("Disappear",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new SetAltTexture(1, 2, 300, loop: true),
                        new Wander(speed: 0.5f),
                        new TimedTransition(time: 1500, targetStates: "Ini")
                        )
                    )
                );
            
            db.Init("Arena Possessed Girl",
                new State("base",
                    new Prioritize(
                        new Follow(speed: 0.5f, acquireRange: 15, range: 3, duration: 5000, cooldown: 3000),
                        new Wander(speed: 0.3f)
                        ),
                    new Shoot(range: 24, count: 8, shootAngle: 45, index: 0, fixedAngle: 22.5f, cooldown: 600)
                )
            );
            db.Init("Arena Ghost Bride",
                    new State("Ini",
                        new Prioritize(
                            new Wander(speed: 0.3f),
                            new Follow(speed: 0.6f, acquireRange: 8, range: 3, duration: 3000, cooldown: 4000),
                            new StayBack(speed: 0.4f, distance: 4, entity: null)
                        ),
                        new Shoot(range: 8, count: 1, index: 0, predictive: 0.9f, cooldown: 3000),
                        new Shoot(range: 8, count: 2, shootAngle: 30, index: 1, fixedAngle: 0, cooldown: 1500, cooldownOffset: 0),
                        new Shoot(range: 8, count: 2, shootAngle: 30, index: 1, fixedAngle: 180, cooldown: 1500, cooldownOffset: 0),
                        new Shoot(range: 8, count: 2, shootAngle: 30, index: 1, fixedAngle: 90, cooldown: 1500, cooldownOffset: 300),
                        new Shoot(range: 8, count: 2, shootAngle: 30, index: 1, fixedAngle: 270, cooldown: 1500, cooldownOffset: 300),
                        new HealthTransition(threshold: 0.75f, targetStates: "ActivateBigDemon")
                    ),
                    new State("ActivateBigDemon",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new SetAltTexture(1),
                        new OrderOnce(range: 100, children: "Arena Statue Left", targetState: "Active"),
                        new EntityNotWithinTransition(target: "Arena Statue Left", radius: 100, targetStates: "Attack1")
                    ),
                    new State("Attack1",
                        new SetAltTexture(0),
                        new Prioritize(
                            new Wander(speed: 0.3f),
                            new Follow(speed: 0.6f, acquireRange: 8, range: 3, duration: 3000, cooldown: 4000),
                            new StayBack(speed: 0.4f, distance: 4, entity: null)
                        ),
                        new Shoot(range: 8, count: 1, index: 0, predictive: 0.9f, cooldown: 3000),
                        new Shoot(range: 8, count: 2, shootAngle: 30, index: 1, fixedAngle: 0, cooldown: 1500, cooldownOffset: 0),
                        new Shoot(range: 8, count: 2, shootAngle: 30, index: 1, fixedAngle: 180, cooldown: 1500, cooldownOffset: 0),
                        new Shoot(range: 8, count: 2, shootAngle: 30, index: 1, fixedAngle: 90, cooldown: 1500, cooldownOffset: 300),
                        new Shoot(range: 8, count: 2, shootAngle: 30, index: 1, fixedAngle: 270, cooldown: 1500, cooldownOffset: 300),
                        new HealthTransition(threshold: 0.5f, targetStates: "ActivateWerewolf")
                    ),
                    new State("ActivateWerewolf",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new SetAltTexture(1),
                        new OrderOnce(range: 100, children: "Arena Statue Right", targetState: "Active"),
                        new EntityNotWithinTransition(target: "Arena Statue Right", radius: 100, targetStates: "Attack2")
                    ),
                    new State("Attack2",
                        new SetAltTexture(0),
                        new Prioritize(
                            new Wander(speed: 0.3f),
                            new Follow(speed: 0.6f, acquireRange: 8, range: 3, duration: 3000, cooldown: 4000),
                            new StayBack(speed: 0.4f, distance: 4, entity: null)
                        ),
                        new Shoot(range: 8, count: 1, index: 0, predictive: 0.9f, cooldown: 3000),
                        new Shoot(range: 8, count: 2, shootAngle: 30, index: 1, fixedAngle: 0, cooldown: 1500, cooldownOffset: 0),
                        new Shoot(range: 8, count: 2, shootAngle: 30, index: 1, fixedAngle: 180, cooldown: 1500, cooldownOffset: 0),
                        new Shoot(range: 8, count: 2, shootAngle: 30, index: 1, fixedAngle: 90, cooldown: 1500, cooldownOffset: 300),
                        new Shoot(range: 8, count: 2, shootAngle: 30, index: 1, fixedAngle: 270, cooldown: 1500, cooldownOffset: 300)
                    ),
                new Threshold(0.01f,
                    new TierLoot(7, TierLoot.LootType.Weapon, 0.25f),
                    new TierLoot(8, TierLoot.LootType.Weapon, 0.25f),
                    new TierLoot(8, TierLoot.LootType.Weapon, 0.25f),
                    new TierLoot(9, TierLoot.LootType.Weapon, 0.125f),
                    new TierLoot(10, TierLoot.LootType.Weapon, 0.0625f),
                    new TierLoot(4, TierLoot.LootType.Ability, 0.125f),
                    new TierLoot(5, TierLoot.LootType.Ability, 0.0625f),
                    new TierLoot(8, TierLoot.LootType.Armor, 0.25f),
                    new TierLoot(9, TierLoot.LootType.Armor, 0.25f),
                    new TierLoot(10, TierLoot.LootType.Armor, 0.125f),
                    new TierLoot(3, TierLoot.LootType.Ring, 0.25f),
                    new TierLoot(4, TierLoot.LootType.Ring, 0.125f),
                    new ItemLoot("Wine Cellar Incantation", 0.05f),
                    new ItemLoot("Potion of Speed", 0.3f, 3),
                    new ItemLoot("Potion of Wisdom", 0.3f)
                )
            );
            db.Init("Arena Statue Right",
                    new State("Ini",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new SetAltTexture(1)
                    ),
                    new State("Active",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(time: 0, targetStates: "Active_fix")
                    ),
                    new State("Active_fix",
                        new SetAltTexture(0),
                        new Prioritize(
                            new Follow(speed: 0.7f, acquireRange: 10, range: 2, duration: 6000, cooldown: 3000),
                            new Orbit(speed: 0.6f, radius: 3, acquireRange: 10, target: null)
                        ),
                        new Shoot(range: 8, count: 1, index: 0, predictive: 1, cooldown: 700),
                        new Shoot(range: 9, count: 2, shootAngle: 35, index: 1, predictive: 1, cooldown: 1500, cooldownOffset: 0),
                        new HealthTransition(threshold: 0.75f, targetStates: "Phase2")
                    ),
                    new State("Phase2",
                        new Prioritize(
                            new Follow(speed: 0.7f, acquireRange: 10, range: 2, duration: 6000, cooldown: 3000),
                            new Orbit(speed: 0.6f, radius: 3, acquireRange: 10, target: null)
                        ),
                        new Shoot(range: 8, count: 1, index: 0, predictive: 1, cooldown: 700),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 35, cooldown: 1500, cooldownOffset: 0),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 70, cooldown: 1500, cooldownOffset: 200),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 105, cooldown: 1500, cooldownOffset: 400),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 140, cooldown: 1500, cooldownOffset: 600),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 175, cooldown: 1500, cooldownOffset: 800),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 210, cooldown: 1500, cooldownOffset: 1000),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 245, cooldown: 1500, cooldownOffset: 1200),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 280, cooldown: 1500, cooldownOffset: 1400),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 315, cooldown: 1500, cooldownOffset: 1600),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 359, cooldown: 1500, cooldownOffset: 1800),
                        new HealthTransition(threshold: 0.5f, targetStates: "Active2")
                    ),
                    new State("Active2",
                        new SetAltTexture(0),
                        new Prioritize(
                            new Follow(speed: 0.7f, acquireRange: 10, range: 2, duration: 6000, cooldown: 3000),
                            new Orbit(speed: 0.6f, radius: 3, acquireRange: 10, target: null)
                        ),
                        new Shoot(range: 8, count: 1, index: 0, predictive: 1, cooldown: 700),
                        new Shoot(range: 9, count: 2, shootAngle: 35, index: 1, predictive: 1, cooldown: 1500, cooldownOffset: 0),
                        new HealthTransition(threshold: 0.25f, targetStates: "Phase4")
                    ),
                    new State("Phase4",
                        new Prioritize(
                            new Follow(speed: 0.7f, acquireRange: 10, range: 2, duration: 6000, cooldown: 3000),
                            new Orbit(speed: 0.6f, radius: 3, acquireRange: 10, target: null)
                        ),
                        new Shoot(range: 8, count: 1, index: 0, predictive: 1, cooldown: 700),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 35, cooldown: 1500, cooldownOffset: 0),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 70, cooldown: 1500, cooldownOffset: 200),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 105, cooldown: 1500, cooldownOffset: 400),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 140, cooldown: 1500, cooldownOffset: 600),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 175, cooldown: 1500, cooldownOffset: 800),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 210, cooldown: 1500, cooldownOffset: 1000),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 245, cooldown: 1500, cooldownOffset: 1200),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 280, cooldown: 1500, cooldownOffset: 1400),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 315, cooldown: 1500, cooldownOffset: 1600),
                        new Shoot(range: 9, count: 1, index: 1, fixedAngle: 359, cooldown: 1500, cooldownOffset: 1800)
                    )
                );
                
                db.Init("Arena Statue Left",
                        new State("Ini",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new SetAltTexture(1)
                        ),
                        new State("Active",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new TimedTransition(time: 0, targetStates: "Active_fix")
                        ),
                        new State("Active_fix",
                            new SetAltTexture(0),
                            new ChangeSize(rate: 1, target: 200),
                            new Prioritize(
                                new Follow(speed: 0.7f, acquireRange: 10, range: 2, duration: 6000, cooldown: 3000),
                                new Orbit(speed: 0.6f, radius: 3, acquireRange: 10, target: null)
                            ),
                            new Shoot(range: 9, count: 2, shootAngle: 35, index: 0, predictive: 0.7f, cooldown: 1000),
                            new PlayerWithinTransition(radius: 1, targetStates: "SpiralBlast", seeInvis: true),
                            new TimedTransition(time: 2000, targetStates: "SpiralBlast")
                        ),
                        new State("SpiralBlast",
                            new Shoot(range: 24, count: 1, index: 0, fixedAngle: 0, cooldown: 10000, cooldownOffset: 0),
                            new Shoot(range: 24, count: 1, index: 0, fixedAngle: 45, cooldown: 10000, cooldownOffset: 100),
                            new Shoot(range: 24, count: 1, index: 0, fixedAngle: 90, cooldown: 10000, cooldownOffset: 200),
                            new Shoot(range: 24, count: 1, index: 0, fixedAngle: 135, cooldown: 10000, cooldownOffset: 300),
                            new Shoot(range: 24, count: 1, index: 0, fixedAngle: 180, cooldown: 10000, cooldownOffset: 400),
                            new Shoot(range: 24, count: 1, index: 0, fixedAngle: 225, cooldown: 10000, cooldownOffset: 500),
                            new Shoot(range: 24, count: 1, index: 0, fixedAngle: 270, cooldown: 10000, cooldownOffset: 600),
                            new Shoot(range: 24, count: 1, index: 0, fixedAngle: 315, cooldown: 10000, cooldownOffset: 700),
                            new Shoot(range: 24, count: 1, index: 0, fixedAngle: 0, cooldown: 10000, cooldownOffset: 800),
                            new Shoot(range: 24, count: 1, index: 0, fixedAngle: 45, cooldown: 10000, cooldownOffset: 900),
                            new Shoot(range: 24, count: 1, index: 0, fixedAngle: 90, cooldown: 10000, cooldownOffset: 1000),
                            new Shoot(range: 24, count: 1, index: 0, fixedAngle: 135, cooldown: 10000, cooldownOffset: 1100),
                            new Shoot(range: 24, count: 1, index: 0, fixedAngle: 180, cooldown: 10000, cooldownOffset: 1200),
                            new Shoot(range: 24, count: 1, index: 0, fixedAngle: 225, cooldown: 10000, cooldownOffset: 1300),
                            new Shoot(range: 24, count: 1, index: 0, fixedAngle: 270, cooldown: 10000, cooldownOffset: 1400),
                            new Shoot(range: 24, count: 1, index: 0, fixedAngle: 315, cooldown: 10000, cooldownOffset: 1500),
                            new TimedTransition(time: 1800, targetStates: "Active")
                        )
                    );
                
            

            #endregion
            #region graves/third
            db.Init("Area 3 Controller",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new TransformOnDeath("Haunted Cemetery Final Rest Portal"),
                    new State("Start",
                        new PlayerWithinTransition(4, true, "1")
                        ),
                    new State("1",
                        new TimedTransition(0, "2")
                        ),
                    new State("2",
                        new SetAltTexture(2),
                        new TimedTransition(100, "3")
                        ),
                    new State("3",
                        new SetAltTexture(1),
                        new Taunt("Your prowess in battle is impressive... and most annoying. This round shall crush you."),
                        new TimedTransition(2000, "4")
                        ),
                    new State("4",
                        new Taunt(true, "The waves are starting in 5 seconds..."),
                        new TimedTransition(5000, "7")
                        ),
                    //repeat
                    new State("5",
                        new PlayerTextTransition("7", "ready"),
                        new SetAltTexture(3),
                        new TimedTransition(500, "6")
                        ),
                    new State("6",
                        new PlayerTextTransition("7", "ready"),
                        new SetAltTexture(4),
                        new TimedTransition(500, "6_1")
                        ),
                    new State("6_1",
                        new PlayerTextTransition("7", "ready"),
                        new SetAltTexture(3),
                        new TimedTransition(500, "6_2")
                        ),
                    new State("6_2",
                        new PlayerTextTransition("7", "ready"),
                        new SetAltTexture(4),
                        new TimedTransition(500, "6_3")
                        ),
                    new State("6_3",
                        new PlayerTextTransition("7", "ready"),
                        new SetAltTexture(3),
                        new TimedTransition(500, "6_4")
                        ),
                    new State("6_4",
                        new SetAltTexture(1),
                        new PlayerTextTransition("7", "ready")
                        ),
                    new State("7",
                        new SetAltTexture(0),
                        new OrderOnce(100, "Arena South Gate Spawner", "Stage 11"),
                        new OrderOnce(100, "Arena West Gate Spawner", "Stage 11"),
                        new OrderOnce(100, "Arena East Gate Spawner", "Stage 11"),
                        new OrderOnce(100, "Arena North Gate Spawner", "Stage 11"),
                        new EntityWithinTransition("Arena Risen Warrior", 999, "Check 1")
                        ),
                    new State("Check 1",
                        new EntitiesNotWithinTransition(100, "8", "Arena Risen Archer", "Arena Risen Mage", "Arena Risen Brawler", "Arena Risen Warrior")
                        ),
                    new State("8",
                        new SetAltTexture(2),
                        new TimedTransition(0, "9")
                        ),
                    new State("9",
                        new SetAltTexture(1),
                        new TimedTransition(2000, "10")
                        ),
                    new State("10",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "11")
                        ),
                    new State("11",
                        new SetAltTexture(3),
                        new TimedTransition(500, "12")
                        ),
                    new State("12",
                        new SetAltTexture(4),
                        new TimedTransition(500, "13")
                        ),
                    new State("13",
                        new SetAltTexture(3),
                        new TimedTransition(500, "14")
                        ),
                    new State("14",
                        new SetAltTexture(4),
                        new TimedTransition(500, "15")
                        ),
                    new State("15",
                        new SetAltTexture(3),
                        new TimedTransition(500, "16")
                        ),
                    new State("16",
                        new SetAltTexture(4),
                        new TimedTransition(500, "17")
                        ),
                    new State("17",
                        new SetAltTexture(3),
                        new TimedTransition(500, "18")
                        ),
                    new State("18",
                        new SetAltTexture(4),
                        new TimedTransition(500, "19")
                        ),
                    new State("19",
                        new SetAltTexture(3),
                        new TimedTransition(500, "20")
                        ),
                    new State("20",
                        new SetAltTexture(4),
                        new TimedTransition(500, "21")
                        ),
                    new State("21",
                        new SetAltTexture(3),
                        new TimedTransition(500, "22")
                        ),
                    new State("22",
                        new SetAltTexture(4),
                        new TimedTransition(500, "23")
                        ),
                    new State("23",
                        new SetAltTexture(3),
                        new TimedTransition(500, "24")
                        ),
                    new State("24",
                        new SetAltTexture(4),
                        new TimedTransition(100, "25")
                        ),
                    new State("25",
                        new SetAltTexture(1),
                        new TimedTransition(100, "26")
                        ),
                    new State("26",
                        new SetAltTexture(2),
                        new TimedTransition(100, "27")
                        ),
                    new State("27",
                        new SetAltTexture(0),
                        new OrderOnce(100, "Arena South Gate Spawner", "Stage 12"),
                        new OrderOnce(100, "Arena West Gate Spawner", "Stage 12"),
                        new OrderOnce(100, "Arena East Gate Spawner", "Stage 12"),
                        new OrderOnce(100, "Arena North Gate Spawner", "Stage 12"),
                        new EntityWithinTransition("Arena Risen Warrior", 999, "Check 2")
                        ),
                    new State("Check 2",
                        new EntitiesNotWithinTransition(100, "28", "Arena Risen Archer", "Arena Risen Mage", "Arena Risen Brawler", "Arena Risen Warrior")
                        ),
                    new State("28",
                        new SetAltTexture(2),
                        new TimedTransition(0, "29")
                        ),
                    new State("29",
                        new SetAltTexture(1),
                        new TimedTransition(2000, "30")
                        ),
                    new State("30",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "31")
                        ),
                    new State("31",
                        new SetAltTexture(3),
                        new TimedTransition(500, "32")
                        ),
                    new State("32",
                        new SetAltTexture(4),
                        new TimedTransition(500, "33")
                        ),
                    new State("33",
                        new SetAltTexture(3),
                        new TimedTransition(500, "34")
                        ),
                    new State("34",
                        new SetAltTexture(4),
                        new TimedTransition(500, "35")
                        ),
                    new State("35",
                        new SetAltTexture(3),
                        new TimedTransition(500, "36")
                        ),
                    new State("36",
                        new SetAltTexture(4),
                        new TimedTransition(500, "37")
                        ),
                    new State("37",
                        new SetAltTexture(3),
                        new TimedTransition(500, "38")
                        ),
                    new State("38",
                        new SetAltTexture(4),
                        new TimedTransition(500, "39")
                        ),
                    new State("39",
                        new SetAltTexture(3),
                        new TimedTransition(500, "40")
                        ),
                    new State("40",
                        new SetAltTexture(4),
                        new TimedTransition(500, "41")
                        ),
                    new State("41",
                        new SetAltTexture(3),
                        new TimedTransition(500, "42")
                        ),
                    new State("42",
                        new SetAltTexture(4),
                        new TimedTransition(500, "43")
                        ),
                    new State("43",
                        new SetAltTexture(3),
                        new TimedTransition(500, "44")
                        ),
                    new State("44",
                        new SetAltTexture(4),
                        new TimedTransition(100, "45")
                        ),
                    new State("45",
                        new SetAltTexture(1),
                        new TimedTransition(100, "46")
                        ),
                    new State("46",
                        new SetAltTexture(2),
                        new TimedTransition(100, "47")
                        ),
                    new State("47",
                        new SetAltTexture(0),
                        new OrderOnce(100, "Arena South Gate Spawner", "Stage 13"),
                        new OrderOnce(100, "Arena West Gate Spawner", "Stage 13"),
                        new OrderOnce(100, "Arena East Gate Spawner", "Stage 13"),
                        new OrderOnce(100, "Arena North Gate Spawner", "Stage 13"),
                        new EntityWithinTransition("Arena Risen Warrior", 999, "Check 3")
                        ),
                    new State("Check 3",
                        new EntitiesNotWithinTransition(100, "48", "Arena Risen Archer", "Arena Risen Mage", "Arena Risen Brawler", "Arena Risen Warrior")
                        ),
                    new State("48",
                        new SetAltTexture(2),
                        new TimedTransition(0, "49")
                        ),
                    new State("49",
                        new SetAltTexture(1),
                        new TimedTransition(2000, "50")
                        ),
                    new State("50",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "51")
                        ),
                    new State("51",
                        new SetAltTexture(3),
                        new TimedTransition(500, "52")
                        ),
                    new State("52",
                        new SetAltTexture(4),
                        new TimedTransition(500, "53")
                        ),
                    new State("53",
                        new SetAltTexture(3),
                        new TimedTransition(500, "54")
                        ),
                    new State("54",
                        new SetAltTexture(4),
                        new TimedTransition(500, "55")
                        ),
                    new State("55",
                        new SetAltTexture(3),
                        new TimedTransition(500, "56")
                        ),
                    new State("56",
                        new SetAltTexture(4),
                        new TimedTransition(500, "57")
                        ),
                    new State("57",
                        new SetAltTexture(3),
                        new TimedTransition(500, "58")
                        ),
                    new State("58",
                        new SetAltTexture(4),
                        new TimedTransition(500, "59")
                        ),
                    new State("59",
                        new SetAltTexture(3),
                        new TimedTransition(500, "60")
                        ),
                    new State("60",
                        new SetAltTexture(4),
                        new TimedTransition(500, "61")
                        ),
                    new State("61",
                        new SetAltTexture(3),
                        new TimedTransition(500, "62")
                        ),
                    new State("62",
                        new SetAltTexture(4),
                        new TimedTransition(500, "63")
                        ),
                    new State("63",
                        new SetAltTexture(3),
                        new TimedTransition(500, "64")
                        ),
                    new State("64",
                        new SetAltTexture(4),
                        new TimedTransition(500, "65")
                        ),
                    new State("65",
                        new SetAltTexture(1),
                        new TimedTransition(100, "66")
                        ),
                    new State("66",
                        new SetAltTexture(2),
                        new TimedTransition(100, "67")
                        ),
                    new State("67",
                        new SetAltTexture(0),
                        new OrderOnce(100, "Arena South Gate Spawner", "Stage 14"),
                        new OrderOnce(100, "Arena West Gate Spawner", "Stage 14"),
                        new OrderOnce(100, "Arena East Gate Spawner", "Stage 14"),
                        new OrderOnce(100, "Arena North Gate Spawner", "Stage 14"),
                        new EntityWithinTransition("Arena Risen Warrior", 999, "Check 4")
                        ),
                    new State("Check 4",
                        new EntitiesNotWithinTransition(100, "68", "Arena Risen Archer", "Arena Risen Mage", "Arena Risen Brawler", "Arena Risen Warrior")
                        ),
                    new State("68",
                        new SetAltTexture(2),
                        new TimedTransition(0, "69")
                        ),
                    new State("69",
                        new SetAltTexture(1),
                        new TimedTransition(2000, "70")
                        ),
                    new State("70",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "71")
                        ),
                    new State("71",
                        new SetAltTexture(3),
                        new TimedTransition(500, "72")
                        ),
                    new State("72",
                        new SetAltTexture(4),
                        new TimedTransition(500, "73")
                        ),
                    new State("73",
                        new SetAltTexture(3),
                        new TimedTransition(500, "74")
                        ),
                    new State("74",
                        new SetAltTexture(4),
                        new TimedTransition(500, "75")
                        ),
                    new State("75",
                        new SetAltTexture(3),
                        new TimedTransition(500, "76")
                        ),
                    new State("76",
                        new SetAltTexture(4),
                        new TimedTransition(500, "77")
                        ),
                    new State("77",
                        new SetAltTexture(3),
                        new TimedTransition(500, "78")
                        ),
                    new State("78",
                        new SetAltTexture(4),
                        new TimedTransition(500, "79")
                        ),
                    new State("79",
                        new SetAltTexture(3),
                        new TimedTransition(500, "80")
                        ),
                    new State("80",
                        new SetAltTexture(4),
                        new TimedTransition(500, "81")
                        ),
                    new State("81",
                        new SetAltTexture(3),
                        new TimedTransition(500, "82")
                        ),
                    new State("82",
                        new SetAltTexture(4),
                        new TimedTransition(500, "83")
                        ),
                    new State("83",
                        new SetAltTexture(3),
                        new TimedTransition(500, "84")
                        ),
                    new State("84",
                        new SetAltTexture(4),
                        new TimedTransition(500, "85")
                        ),
                    new State("85",
                        new SetAltTexture(1),
                        new TimedTransition(100, "86")
                        ),
                    new State("86",
                        new SetAltTexture(2),
                        new TimedTransition(100, "87")
                        ),
                    new State("87",
                        new SetAltTexture(0),
                        new OrderOnce(100, "Arena South Gate Spawner", "Stage 15"),
                        new OrderOnce(100, "Arena West Gate Spawner", "Stage 15"),
                        new OrderOnce(100, "Arena East Gate Spawner", "Stage 15"),
                        new OrderOnce(100, "Arena North Gate Spawner", "Stage 15"),
                        new EntityWithinTransition("Arena Grave Caretaker", 999, "Check 5")
                        ),
                    new State("Check 5",
                        new EntitiesNotWithinTransition(100, "88", "Arena Blue Flame", "Arena Grave Caretaker")
                        ),
                    new State("88",
                        new Suicide()
                    )
                );

            db.Init("Arena Risen Brawler",
                new State("base",
                    new Shoot(range: 10, count: 3, shootAngle: 10, index: 0, cooldown: 2000),
                    new Prioritize(
                        new Orbit(speed: 0.7f, radius: 1, acquireRange: 14, target: null),
                        new Follow(speed: 0.6f, acquireRange: 10, range: 7)
                    )
                )
            );
            db.Init("Arena Risen Warrior",
                new State("base",
                    new Shoot(range: 4, index: 0, cooldown: 1200),
                    new Prioritize(
                        new Follow(speed: 0.9f, acquireRange: 10, range: 1.2f),
                        new Orbit(speed: 0.4f, radius: 3, acquireRange: 10, target: null)
                    )
                )
            );
            db.Init("Arena Risen Mage",
                new State("base",
                    new Shoot(range: 10, index: 0, cooldown: 1000),
                    new Prioritize(
                        new Follow(speed: 0.9f, acquireRange: 10, range: 7),
                        new Orbit(speed: 0.4f, radius: 3, acquireRange: 10, target: null)
                    ),
                    new HealGroup(radius: 7, name: "Hallowrena", cooldown: 2500, amount: 175)
                )
            );
            db.Init("Arena Risen Archer",
                new State("base",
                    new Shoot(range: 10, count: 3, shootAngle: 33, index: 0, cooldown: 3000),
                    new Shoot(range: 10, count: 1, index: 1, cooldown: 1300),
                    new Prioritize(
                        new Orbit(speed: 0.4f, radius: 6, acquireRange: 10, target: null),
                        new Follow(speed: 0.8f, acquireRange: 10, range: 7)
                    )
                )
            );
            db.Init("Arena Blue Flame",
                new State("base",
                    new State("Ini",
                        new Orbit(speed: 1.6f, radius: 1.5f, acquireRange: 15, target: "Arena Grave Caretaker"),
                        new PlayerWithinTransition(radius: 3, targetStates: "AttackPlayer", seeInvis: true)
                    ),
                    new State("AttackPlayer",
                        new Charge(speed: 4.1f, range: 9),
                        new PlayerWithinTransition(radius: 1, targetStates: "Explode", seeInvis: true),
                        new PlayerWithinTransition(radius: 4, targetStates: "Follow", seeInvis: true)
                    ),
                    new State("Follow",
                        new Follow(speed: 1, acquireRange: 6, range: 0.5f),
                        new PlayerWithinTransition(radius: 1, targetStates: "Explode", seeInvis: true)
                    ),
                    new State("Explode",
                        new Shoot(range: 6, count: 10, shootAngle: 36, index: 0, fixedAngle: 18),
                        new Suicide()
                    )
                )
            );
            db.Init("Arena Grave Caretaker",
                new State("base",
                    new State("Ini",
                        new Prioritize(
                            new StayBack(speed: 0.6f, distance: 4, entity: null),
                            new Wander(speed: 0.3f)
                        ),
                        new Shoot(range: 8, count: 1, index: 0, cooldown: 1000),
                        new Shoot(range: 8, count: 2, shootAngle: 45, index: 1, cooldown: 1000),
                        new Spawn(children: "Arena Blue Flame", maxChildren: 5, initialSpawn: 0, cooldown: 1000, givesNoXp: true),
                        new TimedTransition(time: 10000, targetStates: "Circle")
                    ),
                    new State("Circle",
                        new Orbit(speed: 0.8f, radius: 3, acquireRange: 10, target: null),
                        new Shoot(range: 8, count: 2, shootAngle: 45, index: 1, cooldown: 1000),
                        new Spawn(children: "Arena Blue Flame", maxChildren: 5, initialSpawn: 0, cooldown: 2000, givesNoXp: true),
                        new TimedTransition(time: 8000, targetStates: "Ini")
                    )
                ),
                new Threshold(0.01f,
                    new TierLoot(8, TierLoot.LootType.Weapon, 0.25f),
                    new TierLoot(8, TierLoot.LootType.Weapon, 0.25f),
                    new TierLoot(9, TierLoot.LootType.Weapon, 0.125f),
                    new TierLoot(10, TierLoot.LootType.Weapon, 0.0625f),
                    new TierLoot(11, TierLoot.LootType.Weapon, 0.0625f),
                    new TierLoot(4, TierLoot.LootType.Ability, 0.125f),
                    new TierLoot(5, TierLoot.LootType.Ability, 0.0625f),
                    new TierLoot(8, TierLoot.LootType.Armor, 0.25f),
                    new TierLoot(9, TierLoot.LootType.Armor, 0.25f),
                    new TierLoot(10, TierLoot.LootType.Armor, 0.125f),
                    new TierLoot(11, TierLoot.LootType.Armor, 0.125f),
                    new TierLoot(3, TierLoot.LootType.Ring, 0.25f),
                    new TierLoot(4, TierLoot.LootType.Ring, 0.125f),
                    new ItemLoot("Wine Cellar Incantation", 0.05f),
                    new ItemLoot("Potion of Speed", 0.3f, 3),
                    new ItemLoot("Potion of Wisdom", 0.3f)
                )
            );
            #endregion
            #region final
            db.Init("Zombie Hulk",
                new State("base",
                    new Wander(speed: 0.35f),
                    new State("Attack2",
                        new SetAltTexture(0),
                        new StayBack(speed: 0.55f, distance: 7, entity: null),
                        new Follow(speed: 0.3f, acquireRange: 11, range: 5),
                        new Shoot(range: 10, count: 1, index: 1, cooldown: 400, cooldownOffset: 500),
                        new TimedTransition(3000, "Attack1", "Attack3")
                    ),
                    new State("Attack1",
                        new SetAltTexture(1),
                        new Shoot(range: 8, count: 3, shootAngle: 40, index: 0, cooldown: 400, cooldownOffset: 250),
                        new Shoot(range: 8, count: 2, shootAngle: 60, index: 0, cooldown: 400, cooldownOffset: 250),
                        new Charge(speed: 1.1f, range: 11, cooldown: 200),
                        new TimedTransition(3000, "Attack2", "Attack3")
                    ),
                    new State("Attack3",
                        new SetAltTexture(0),
                        new Wander(speed: 0.4f),
                        new Shoot(range: 10, count: 3, shootAngle: 30, index: 1, cooldown: 400, cooldownOffset: 500),
                        new TimedTransition(3000, "Attack1", "Attack2")
                    )
                ),
                new Threshold(0.01f,
                    new TierLoot(5, TierLoot.LootType.Weapon, 0.4f),
                    new TierLoot(6, TierLoot.LootType.Weapon, 0.4f),
                    new TierLoot(5, TierLoot.LootType.Armor, 0.4f),
                    new TierLoot(6, TierLoot.LootType.Armor, 0.4f),
                    new TierLoot(3, TierLoot.LootType.Ring, 0.25f)
                )
            );
            db.Init("Classic Ghost",
                new State("base",
                    new Wander(speed: 0.4f),
                    new Follow(speed: 0.55f, acquireRange: 10, range: 3, duration: 1000, cooldown: 2000),
                    new Orbit(speed: 0.55f, radius: 4, acquireRange: 7, target: null),
                    new Shoot(range: 5, count: 4, shootAngle: 16, index: 0, cooldown: 1000, cooldownOffset: 0)
                )
            );
            db.Init("Werewolf",
                new State("base",
                    new Spawn(children: "Werewolf Cub", maxChildren: 2, initialSpawn: 0, cooldown: 5400, givesNoXp: false),
                    new Spawn(children: "Werewolf Cub", maxChildren: 3, initialSpawn: 0, cooldown: 5400, givesNoXp: false),
                    new StayCloseToSpawn(speed: 0.8f, range: 6),
                    new State("Circling",
                        new Shoot(range: 10, count: 3, shootAngle: 20, index: 0, cooldown: 1600),
                        new Prioritize(
                            new Orbit(speed: 0.4f, radius: 5.4f, acquireRange: 8, target: null),
                            new Wander(speed: 0.4f)
                        ),
                        new TimedTransition(time: 3400, targetStates: "Engaging")
                    ),
                    new State("Engaging",
                        new Shoot(range: 5.5f, count: 5, shootAngle: 13, index: 0, cooldown: 1600),
                        new Follow(speed: 0.6f, acquireRange: 10, range: 1),
                        new TimedTransition(time: 2600, targetStates: "Circling")

                    )
                )
            );
            db.Init("Werewolf Cub",
                new State("base",
                    new Shoot(range: 4, count: 1, index: 0, cooldown: 1000),
                    new Prioritize(
                            new Follow(speed: 0.6f, acquireRange: 15, range: 1),
                            new Protect(speed: 0.8f, protectee: "Werewolf", acquireRange: 15, protectionRange: 6, reprotectRange: 3),
                            new Wander(speed: 0.4f)
                    )
                )
            );
            db.Init("Ghost of Skuld",
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true, -1),
                    new State("Start",
                        new SetAltTexture(11),
                        new PlayerWithinTransition(10, true, "1")
                        ),
                    new State("1",
                        new TimedTransition(0, "2")
                        ),
                    new State("2",
                        new SetAltTexture(1),
                        new TimedTransition(100, "3")
                        ),
                    new State("3",
                        new SetAltTexture(0),
                        new TimedTransition(2000, "4")
                        ),
                    new State("4",
                        new Taunt(true, "I am near, the waves are starting in 5 seconds..."),
                        new TimedTransition(5000, "7")
                        ),
                    //repeat
                    new State("5",
                        new PlayerTextTransition("85", "ready"),
                        new TimedTransition(2000, "4")
                        ),
                    new State("6",
                        new PlayerTextTransition("85", "ready"),
                        new SetAltTexture(13),
                        new TimedTransition(2000, "85")
                        ),
                    new State("6_1",
                        new PlayerTextTransition("85", "ready"),
                        new SetAltTexture(12),
                       new TimedTransition(2000, "85")
                        ),
                    new State("6_2",
                        new PlayerTextTransition("85", "ready"),
                        new SetAltTexture(13),
                         new TimedTransition(2000, "85")
                        ),
                    new State("6_3",
                        new PlayerTextTransition("85", "ready"),
                        new SetAltTexture(12),
                         new TimedTransition(2000, "85")
                        ),
                    new State("6_4",
                        new SetAltTexture(0),
                        new PlayerTextTransition("85", "ready"),
                         new TimedTransition(2000, "7")
                        ),
                    new State("7",
                        new SetAltTexture(11),
                        new OrderOnce(100, "Arena Up Spawner", "Stage 1"),
                        new OrderOnce(100, "Arena Lf Spawner", "Stage 1"),
                        new OrderOnce(100, "Arena Dn Spawner", "Stage 1"),
                        new OrderOnce(100, "Arena Rt Spawner", "Stage 1"),
                        new EntityWithinTransition("Classic Ghost", 999, "Check 1")
                        ),
                    new State("Check 1",
                        new EntitiesNotWithinTransition(100, "8", "Werewolf", "Werewolf Cub", "Zombie Hulk", "Classic Ghost")
                        ),
                    new State("8",
                        new SetAltTexture(1),
                        new TimedTransition(0, "9")
                        ),
                    new State("9",
                        new SetAltTexture(0),
                        new TimedTransition(2000, "10")
                        ),
                    new State("10",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "11")
                        ),
                    new State("11",
                        new SetAltTexture(12),
                        new TimedTransition(500, "12")
                        ),
                    new State("12",
                        new SetAltTexture(13),
                        new TimedTransition(500, "13")
                        ),
                    new State("13",
                        new SetAltTexture(12),
                        new TimedTransition(500, "14")
                        ),
                    new State("14",
                        new SetAltTexture(13),
                        new TimedTransition(500, "15")
                        ),
                    new State("15",
                        new SetAltTexture(12),
                        new TimedTransition(500, "16")
                        ),
                    new State("16",
                        new SetAltTexture(13),
                        new TimedTransition(500, "17")
                        ),
                    new State("17",
                        new SetAltTexture(12),
                        new TimedTransition(500, "18")
                        ),
                    new State("18",
                        new SetAltTexture(13),
                        new TimedTransition(500, "19")
                        ),
                    new State("19",
                        new SetAltTexture(12),
                        new TimedTransition(500, "20")
                        ),
                    new State("20",
                        new SetAltTexture(13),
                        new TimedTransition(500, "21")
                        ),
                    new State("21",
                        new SetAltTexture(12),
                        new TimedTransition(500, "22")
                        ),
                    new State("22",
                        new SetAltTexture(13),
                        new TimedTransition(500, "23")
                        ),
                    new State("23",
                        new SetAltTexture(12),
                        new TimedTransition(500, "24")
                        ),
                    new State("24",
                        new SetAltTexture(13),
                        new TimedTransition(500, "25")
                        ),
                    new State("25",
                        new SetAltTexture(0),
                        new TimedTransition(100, "26")
                        ),
                    new State("26",
                        new SetAltTexture(1),
                        new TimedTransition(100, "27")
                        ),
                    new State("27",
                        new SetAltTexture(11),
                        new OrderOnce(100, "Arena Up Spawner", "Stage 2"),
                        new OrderOnce(100, "Arena Lf Spawner", "Stage 2"),
                        new OrderOnce(100, "Arena Dn Spawner", "Stage 2"),
                        new OrderOnce(100, "Arena Rt Spawner", "Stage 2"),
                        new EntityWithinTransition("Classic Ghost", 999, "Check 2")
                        ),
                    new State("Check 2",
                        new EntitiesNotWithinTransition(100, "28", "Werewolf", "Werewolf Cub", "Zombie Hulk", "Classic Ghost")
                        ),
                    new State("28",
                        new SetAltTexture(1),
                        new TimedTransition(0, "29")
                        ),
                    new State("29",
                        new SetAltTexture(0),
                        new TimedTransition(2000, "30")
                        ),
                    new State("30",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "31")
                        ),
                    new State("31",
                        new SetAltTexture(12),
                        new TimedTransition(500, "32")
                        ),
                    new State("32",
                        new SetAltTexture(13),
                        new TimedTransition(500, "33")
                        ),
                    new State("33",
                        new SetAltTexture(12),
                        new TimedTransition(500, "34")
                        ),
                    new State("34",
                        new SetAltTexture(13),
                        new TimedTransition(500, "35")
                        ),
                    new State("35",
                        new SetAltTexture(12),
                        new TimedTransition(500, "36")
                        ),
                    new State("36",
                        new SetAltTexture(13),
                        new TimedTransition(500, "37")
                        ),
                    new State("37",
                        new SetAltTexture(12),
                        new TimedTransition(500, "38")
                        ),
                    new State("38",
                        new SetAltTexture(13),
                        new TimedTransition(500, "39")
                        ),
                    new State("39",
                        new SetAltTexture(12),
                        new TimedTransition(500, "40")
                        ),
                    new State("40",
                        new SetAltTexture(13),
                        new TimedTransition(500, "41")
                        ),
                    new State("41",
                        new SetAltTexture(12),
                        new TimedTransition(500, "42")
                        ),
                    new State("42",
                        new SetAltTexture(13),
                        new TimedTransition(500, "43")
                        ),
                    new State("43",
                        new SetAltTexture(12),
                        new TimedTransition(500, "44")
                        ),
                    new State("44",
                        new SetAltTexture(13),
                        new TimedTransition(100, "45")
                        ),
                    new State("45",
                        new SetAltTexture(0),
                        new TimedTransition(100, "46")
                        ),
                    new State("46",
                        new SetAltTexture(1),
                        new TimedTransition(100, "47")
                        ),
                    new State("47",
                        new SetAltTexture(11),
                        new OrderOnce(100, "Arena Up Spawner", "Stage 3"),
                        new OrderOnce(100, "Arena Lf Spawner", "Stage 3"),
                        new OrderOnce(100, "Arena Dn Spawner", "Stage 3"),
                        new OrderOnce(100, "Arena Rt Spawner", "Stage 3"),
                        new EntityWithinTransition("Classic Ghost", 999, "Check 3")
                        ),
                    new State("Check 3",
                        new EntitiesNotWithinTransition(100, "48", "Werewolf", "Werewolf Cub", "Zombie Hulk", "Classic Ghost")
                        ),
                    new State("48",
                        new SetAltTexture(1),
                        new TimedTransition(0, "49")
                        ),
                    new State("49",
                        new SetAltTexture(0),
                        new TimedTransition(2000, "50")
                        ),
                    new State("50",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "51")
                        ),
                    new State("51",
                        new SetAltTexture(12),
                        new TimedTransition(500, "52")
                        ),
                    new State("52",
                        new SetAltTexture(13),
                        new TimedTransition(500, "53")
                        ),
                    new State("53",
                        new SetAltTexture(12),
                        new TimedTransition(500, "54")
                        ),
                    new State("54",
                        new SetAltTexture(13),
                        new TimedTransition(500, "55")
                        ),
                    new State("55",
                        new SetAltTexture(12),
                        new TimedTransition(500, "56")
                        ),
                    new State("56",
                        new SetAltTexture(13),
                        new TimedTransition(500, "57")
                        ),
                    new State("57",
                        new SetAltTexture(12),
                        new TimedTransition(500, "58")
                        ),
                    new State("58",
                        new SetAltTexture(13),
                        new TimedTransition(500, "59")
                        ),
                    new State("59",
                        new SetAltTexture(12),
                        new TimedTransition(500, "60")
                        ),
                    new State("60",
                        new SetAltTexture(13),
                        new TimedTransition(500, "61")
                        ),
                    new State("61",
                        new SetAltTexture(12),
                        new TimedTransition(500, "62")
                        ),
                    new State("62",
                        new SetAltTexture(13),
                        new TimedTransition(500, "63")
                        ),
                    new State("63",
                        new SetAltTexture(12),
                        new TimedTransition(500, "64")
                        ),
                    new State("64",
                        new SetAltTexture(13),
                        new TimedTransition(500, "65")
                        ),
                    new State("65",
                        new SetAltTexture(0),
                        new TimedTransition(100, "66")
                        ),
                    new State("66",
                        new SetAltTexture(1),
                        new TimedTransition(100, "67")
                        ),
                    new State("67",
                        new SetAltTexture(11),
                        new OrderOnce(100, "Arena Up Spawner", "Stage 4"),
                        new OrderOnce(100, "Arena Lf Spawner", "Stage 4"),
                        new OrderOnce(100, "Arena Dn Spawner", "Stage 4"),
                        new OrderOnce(100, "Arena Rt Spawner", "Stage 4"),
                        new EntityWithinTransition("Classic Ghost", 999, "Check 4")
                        ),
                    new State("Check 4",
                        new EntitiesNotWithinTransition(100, "68", "Werewolf", "Werewolf Cub", "Zombie Hulk", "Classic Ghost")
                        ),
                    new State("68",
                        new SetAltTexture(1),
                        new TimedTransition(0, "69")
                        ),
                    new State("69",
                        new SetAltTexture(0),
                        new TimedTransition(2000, "70")
                        ),
                    new State("70",
                        new Taunt(true, "Congratulations on your victory, warrior. Your reward shall be..."),
                        new TimedTransition(0, "71")
                        ),
                    new State("71",
                        new SetAltTexture(12),
                        new TimedTransition(500, "72")
                        ),
                    new State("72",
                        new SetAltTexture(13),
                        new TimedTransition(500, "73")
                        ),
                    new State("73",
                        new SetAltTexture(12),
                        new TimedTransition(500, "74")
                        ),
                    new State("74",
                        new SetAltTexture(13),
                        new TimedTransition(500, "75")
                        ),
                    new State("75",
                        new SetAltTexture(12),
                        new TimedTransition(500, "76")
                        ),
                    new State("76",
                        new SetAltTexture(13),
                        new TimedTransition(500, "77")
                        ),
                    new State("77",
                        new SetAltTexture(12),
                        new TimedTransition(500, "78")
                        ),
                    new State("78",
                        new SetAltTexture(13),
                        new TimedTransition(500, "79")
                        ),
                    new State("79",
                        new SetAltTexture(12),
                        new TimedTransition(500, "80")
                        ),
                    new State("80",
                        new SetAltTexture(13),
                        new TimedTransition(500, "81")
                        ),
                    new State("81",
                        new SetAltTexture(12),
                        new TimedTransition(500, "82")
                        ),
                    new State("82",
                        new SetAltTexture(13),
                        new TimedTransition(500, "83")
                        ),
                    new State("83",
                        new SetAltTexture(12),
                        new TimedTransition(500, "84")
                        ),
                    new State("84",
                        new SetAltTexture(13),
                        new TimedTransition(500, "85")
                        ),
                    new State("85",
                        new Taunt("Your death will be SWIFT!!!"),
                        new SetAltTexture(0),
                        new TimedTransition(500, "86")
                        ),
                    new State("86",
                        new SetAltTexture(2),
                        new TimedTransition(500, "87")
                        ),
                    new State("87",
                        new SetAltTexture(3),
                        new TimedTransition(500, "88")
                        ),
                    new State("88",
                        new SetAltTexture(2),
                        new TimedTransition(500, "89")
                        ),
                    new State("89",
                        new SetAltTexture(3),
                        new TimedTransition(500, "90")
                        ),
                    new State("90",
                        new SetAltTexture(2),
                        new Shoot(10, 36, index: 1, fixedAngle: 0, angleOffset: 22.5f),
                        new TimedTransition(100, "91")
                        ),
                    new State("91",
                        new TimedTransition(100, "92")
                        ),
                    new State("92",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, false, 0),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 0),
                        new SetAltTexture(0),
                        new OrderOnce(100, "Halloween Zombie Spawner", "1"),
                        new Shoot(20, 4, shootAngle: 13, index: 0, predictive: 0.5f, cooldown: 500),
                        new Shoot(100, 1, index: 2, fixedAngle: 0, angleOffset: 0, cooldown: 5000),
                        new Shoot(100, 1, index: 2, fixedAngle: 0, angleOffset: 90, cooldown: 5000),
                        new Shoot(100, 1, index: 2, fixedAngle: 0, angleOffset: 180, cooldown: 5000),
                        new Shoot(100, 1, index: 2, fixedAngle: 0, angleOffset: 270, cooldown: 5000),
                        new HealthTransition(0.5f, "93")
                        ),
                    new State("93",
                        new Spawn("Flying Flame Skull", maxChildren: 2),
                        new Shoot(20, 4, shootAngle: 13, index: 0, predictive: 0.5f, cooldown: 500),
                        new Shoot(100, 1, index: 2, fixedAngle: 0, angleOffset: 0, cooldown: 5000),
                        new Shoot(100, 1, index: 2, fixedAngle: 0, angleOffset: 90, cooldown: 5000),
                        new Shoot(100, 1, index: 2, fixedAngle: 0, angleOffset: 180, cooldown: 5000),
                        new Shoot(100, 1, index: 2, fixedAngle: 0, angleOffset: 270, cooldown: 5000),
                        new TimedTransition(5000, "94")
                        ),
                    new State("94",
                        new Shoot(20, 4, shootAngle: 13, index: 0, predictive: 0.5f, cooldown: 500),
                        new Shoot(100, 1, index: 2, fixedAngle: 0, angleOffset: 0, cooldown: 5000),
                        new Shoot(100, 1, index: 2, fixedAngle: 0, angleOffset: 90, cooldown: 5000),
                        new Shoot(100, 1, index: 2, fixedAngle: 0, angleOffset: 180, cooldown: 5000),
                        new Shoot(100, 1, index: 2, fixedAngle: 0, angleOffset: 270, cooldown: 5000),
                        new Shoot(100, 1, index: 2, fixedAngle: 0, angleOffset: 45, cooldown: 2000),
                        new Shoot(100, 1, index: 2, fixedAngle: 0, angleOffset: 135, cooldown: 2000),
                        new Shoot(100, 1, index: 2, fixedAngle: 0, angleOffset: 225, cooldown: 2000),
                        new Shoot(100, 1, index: 2, fixedAngle: 0, angleOffset: 315, cooldown: 2000)
                        ),
                new Threshold(0.0001f,
                    new TierLoot(8, TierLoot.LootType.Weapon, 0.25f),
                    new TierLoot(8, TierLoot.LootType.Weapon, 0.25f),
                    new TierLoot(9, TierLoot.LootType.Weapon, 0.125f),
                    new TierLoot(10, TierLoot.LootType.Weapon, 0.0625f),
                    new TierLoot(11, TierLoot.LootType.Weapon, 0.0625f),
                    new TierLoot(4, TierLoot.LootType.Ability, 0.125f),
                    new TierLoot(5, TierLoot.LootType.Ability, 0.0625f),
                    new TierLoot(9, TierLoot.LootType.Armor, 0.25f),
                    new TierLoot(10, TierLoot.LootType.Armor, 0.125f),
                    new TierLoot(11, TierLoot.LootType.Armor, 0.125f),
                    new TierLoot(4, TierLoot.LootType.Ring, 0.125f),
                    new TierLoot(5, TierLoot.LootType.Ring, 0.0625f),
                    new ItemLoot("Wine Cellar Incantation", 0.05f),
                    new ItemLoot("Potion of Vitality", 1),
                    new ItemLoot("Potion of Wisdom", 1),
                    new ItemLoot("Plague Poison", 0.01f),
                    new ItemLoot("Resurrected Warrior's Armor", 0.01f)
                )
                );
            db.Init("Halloween Zombie Spawner",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Leech"),
                    new State("1",
                        new Spawn("Zombie Rise", maxChildren: 1),
                        new EntityNotWithinTransition("Ghost of Skuld", 100, "2")
                    ),
                    new State("2",
                        new Suicide()
                    )
                );
            db.Init("Zombie Rise",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new TransformOnDeath("Blue Zombie"),
                    new State("1",
                        new SetAltTexture(1),
                        new TimedTransition(750, "2")
                    ),
                    new State("2",
                        new SetAltTexture(2),
                        new TimedTransition(750, "3")
                    ),
                    new State("3",
                        new SetAltTexture(3),
                        new TimedTransition(750, "4")
                    ),
                    new State("4",
                        new SetAltTexture(4),
                        new TimedTransition(750, "5")
                    ),
                    new State("5",
                        new SetAltTexture(5),
                        new TimedTransition(750, "6")
                    ),
                    new State("6",
                        new SetAltTexture(6),
                        new TimedTransition(750, "7")
                    ),
                    new State("7",
                        new SetAltTexture(7),
                        new TimedTransition(750, "8")
                    ),
                    new State("8",
                        new SetAltTexture(8),
                        new TimedTransition(750, "9")
                    ),
                    new State("9",
                        new SetAltTexture(9),
                        new TimedTransition(750, "10")
                    ),
                    new State("10",
                        new SetAltTexture(10),
                        new TimedTransition(750, "11")
                    ),
                    new State("11",
                        new SetAltTexture(11),
                        new TimedTransition(750, "12")
                    ),
                    new State("12",
                        new SetAltTexture(12),
                        new TimedTransition(750, "13")
                    ),
                    new State("13",
                        new SetAltTexture(13),
                        new TimedTransition(750, "14")
                    ),
                    new State("14",
                        new Suicide()
                    )
                );
            db.Init("Blue Zombie",
                    new State("1",
                        new Follow(0.03f, 100, 1),
                        new Shoot(10, 1, index: 0, cooldown: 1000),
                        new EntityNotWithinTransition("Ghost of Skuld", 100, "2")
                    ),
                    new State("2",
                        new Suicide()
                    )
                );
            db.Init("Flying Flame Skull",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("1",
                        new Orbit(1, 5, 10, target: "Ghost of Skuld"),
                        new Shoot(100, 10, shootAngle: 36, index: 0, cooldown: 1000),
                        new EntityNotWithinTransition("Ghost of Skuld", 100, "2")
                    ),
                    new State("2",
                        new Suicide()
                    )
                );          
            
            #endregion
        }
    }
}