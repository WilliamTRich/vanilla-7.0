using Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using RotMG.Utils;
using System;

namespace RotMG.Game.Logic.Database;

public sealed class Spiderden : IBehaviorDatabase
{

    public Random rnd = new Random();
    public void Init(BehaviorDb db)
    {
        db.Init("Arachna the Spider Queen",
            new DropPortalOnDeath("Glowing Realm Portal"),
            new OrderOnDeath(30, "Arachna Web Spoke 1", "Die", 1),
            new OrderOnDeath(30, "Arachna Web Spoke 2", "Die", 1),
            new OrderOnDeath(30, "Arachna Web Spoke 3", "Die", 1),
            new OrderOnDeath(30, "Arachna Web Spoke 4", "Die", 1),
            new OrderOnDeath(30, "Arachna Web Spoke 5", "Die", 1),
            new OrderOnDeath(30, "Arachna Web Spoke 6", "Die", 1),
            new OrderOnDeath(30, "Arachna Web Spoke 7", "Die", 1),
            new OrderOnDeath(30, "Arachna Web Spoke 8", "Die", 1),
            new State("WaitForPlayers",
                new PlayerWithinTransition(Player.SightRadius, true, "Start")
                ),
            new State("Start", 
                new TossObject("Arachna Web Spoke 1", 10, 0, 99999),
                new TossObject("Arachna Web Spoke 2", 10, 45, 99999),
                new TossObject("Arachna Web Spoke 3", 10, 90, 99999),
                new TossObject("Arachna Web Spoke 4", 10, 135, 99999),
                new TossObject("Arachna Web Spoke 5", 10, 180, 99999),
                new TossObject("Arachna Web Spoke 6", 10, 225, 99999),
                new TossObject("Arachna Web Spoke 7", 10, 270, 99999),
                new TossObject("Arachna Web Spoke 8", 10, 315, 99999),
                //new TossObject("Arachna Web Spoke 9", 10, 360?, 99999),
                new TimedTransition(500, "Attack")
            ),
            new State("Attack",  
                new Wander(.3f),
                new Follow(.9f),
                new Shoot(Player.SightRadius, 3, 0, 0),
                new TossObject("Black Den Spider", 5, 17, 5000),
                new TossObject("Brown Den Spider", 5, 17, 5000, 0, 2000),
                new TossObject("Black Spotted Den Spider", 5, 17, 5000, 0, 4000),
                new TossObject("Red Spotted Den Spider", 5, 17, 5000, 0, 6000)
            )
            );

        db.Init("Arachna Web Spoke 1",
            new State("Start", 
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new Shoot(Player.SightRadius, 1, 0, 0, 180, cooldown: 100)
            ),
            new State("Die",
                new Decay(0)
            ));
        db.Init("Arachna Web Spoke 2",
            new State("Start",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new Shoot(Player.SightRadius, 1, 0, 0, 225, cooldown: 100)
            ),
            new State("Die",
                new Decay(0)
            ));
        db.Init("Arachna Web Spoke 3",
            new State("Start",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new Shoot(Player.SightRadius, 1, 0, 0, -90, cooldown: 100)
            ),
            new State("Die",
                new Decay(0)
            ));
        db.Init("Arachna Web Spoke 4",
            new State("Start",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new Shoot(Player.SightRadius, 1, 0, 0, -45, cooldown: 100)
            ),
            new State("Die",
                new Decay(0)
            ));
        db.Init("Arachna Web Spoke 5",
            new State("Start",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new Shoot(Player.SightRadius, 1, 0, 0, 0, cooldown: 100)
            ),
            new State("Die",
                new Decay(0)
            ));
        db.Init("Arachna Web Spoke 6",
            new State("Start",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new Shoot(Player.SightRadius, 1, 0, 0, 45, cooldown: 100)
            ),
            new State("Die",
                new Decay(0)
            ));
        db.Init("Arachna Web Spoke 7",
            new State("Start",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new Shoot(Player.SightRadius, 1, 0, 0, 90, cooldown: 100)
            ),
            new State("Die",
                new Decay(0)
            ));
        db.Init("Arachna Web Spoke 8",
            new State("Start",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new Shoot(Player.SightRadius, 1, 0, 0, 135, cooldown: 100)
            ),
            new State("Die",
                new Decay(0)
            ));
        db.Init("Spider Egg Sac",
            
            new State("WaitForPlayer",
                new PlayerWithinTransition(3, false, "SpawnMinions")
            ),
            new State("SpawnMinions",
                new Reproduce("Green Den Spider Hatchling", 4, 4, 0),
                new TimedTransition(50, "Die")
            ),
            new State("Die",
                new Suicide(0)
            )
            
            );

        db.Init("Green Den Spider Hatchling",
            new State("Attack",
                new Wander(0.1f),
                new Protect(0.4f, "Green Den Spider Hatchling"),
                new Shoot(5, 1, 0, 0)
            )
            );

        db.Init("Black Den Spider",
                 new State("Wander",
                     new StayAbove(0.2f, 50),
                     new Wander(0.4f),
                     new PlayerWithinTransition(7, true, "Attack")
                         ),
                 new State("Attack",
                     new StayAbove(0.2f, 50),
                     new Prioritize(
                         new Charge(2),
                         new Wander(0.4f)
                         ),
                     new Shoot(5, predictive: 1),
                     new TimedTransition(1000, "Wander")
                     ),
               new Threshold(0.01f, 
                    new ItemLoot("Healing Ichor", .05f)
                   )
            );

        db.Init("Brown Den Spider",
                new State("Idle",
                    new StayAbove(0.2f, 50),
                    new Wander(0.4f),
                    new PlayerWithinTransition(7, true, "Attack")
                    ),
                new State("Attack",
                    new Prioritize(
                        new StayAbove(0.4f, 160),
                        new Follow(0.9f, acquireRange: 9, range: 0),
                        new Wander(0.4f)
                        ),
                    new Shoot(8, count: 3, shootAngle: 10, cooldown: 400),
                    new TimedTransition(10000, "Idle")
                    ),
                new Threshold(0.01f,
                    new ItemLoot("Healing Ichor", .05f)
                )
                );
        db.Init("Black Spotted Den Spider",
            new State("base",
                new Wander(0.4f),
                new Shoot(7)
                ),
            new Threshold(0.01f,
                new ItemLoot("Healing Ichor", .05f)

            )
             );
        db.Init("Red Spotted Den Spider",
            new State("base",
                new Prioritize(
                new Follow(0.3f, acquireRange: 10, range: 4),
                new Wander(0.5f)
                ),
                new Shoot(10)
                ),
            new Threshold(0.01f,
              new ItemLoot("Healing Ichor", .05f)
            )
                );

    }
}