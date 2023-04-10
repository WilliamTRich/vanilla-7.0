using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;
using RotMG.Utils;
using System;

namespace RotMG.Game.Logic.Database
{
    public sealed class Spiderden : IBehaviorDatabase
    {

        public Random rnd = new Random();
        public void Init(BehaviorDb db)
        {
            db.Init("Arachna the Spider Queen",
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
                    new Shoot(Player.SightRadius, 3, 0, 0)
                )
                );

            db.Init("Arachna Web Spoke 1",
                new State("Start", 
                    new Shoot(Player.SightRadius, 1, 0, 0, 180, cooldown: 100)
                ));
            db.Init("Arachna Web Spoke 2",
                new State("Start",
                    new Shoot(Player.SightRadius, 1, 0, 0, 225, cooldown: 100)
                ));
            db.Init("Arachna Web Spoke 3",
                new State("Start",
                    new Shoot(Player.SightRadius, 1, 0, 0, -90, cooldown: 100)
                ));
            db.Init("Arachna Web Spoke 4",
                new State("Start",
                    new Shoot(Player.SightRadius, 1, 0, 0, -45, cooldown: 100)
                ));            
            db.Init("Arachna Web Spoke 5",
                new State("Start",
                    new Shoot(Player.SightRadius, 1, 0, 0, 0, cooldown: 100)
                ));            
            db.Init("Arachna Web Spoke 6",
                new State("Start",
                    new Shoot(Player.SightRadius, 1, 0, 0, 45, cooldown: 100)
                ));            
            db.Init("Arachna Web Spoke 7",
                new State("Start",
                    new Shoot(Player.SightRadius, 1, 0, 0, 90, cooldown: 100)
                ));            
            db.Init("Arachna Web Spoke 8",
                new State("Start",
                    new Shoot(Player.SightRadius, 1, 0, 0, 135, cooldown: 100)
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
                    new Protect(0.4f, "Green Den Spider Hatchling"),
                    new Shoot(5, 1, 0, 0)
                )
                );
        }
    }
}