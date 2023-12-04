using Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database;

public sealed class Tutorial : IBehaviorDatabase
{
    public void Init(BehaviorDb db)
    {
        db.Init("East Tutorial Gun", 
            new State("Shoot", 
                new Shoot(15, 1, 0, 0, 0)
                )
            );
        db.Init("North Tutorial Gun", 
            new State("Shoot", 
                new Shoot(15, 1, 0, 0, -90)
                )
            );
        db.Init("South Tutorial Gun", 
            new State("Shoot", 
                new Shoot(15, 1, 0, 0, 90)
                )
            );
        db.Init("West Tutorial Gun", 
            new State("Shoot", 
                new Shoot(15, 1, 0, 0, 180)
                )
            );

        db.Init("Evil Chicken",
            new State("Protect",
                new Wander(0.1f),
                new Protect(0.35f, "Evil Chicken God")
                )
            );

        db.Init("Evil Chicken God",
            new DropPortalOnDeath("Glowing Realm Portal", 1, 15000),
            new State("Start",
                new Reproduce("Evil Chicken", 6, 6, 10000, 0),
                new TimedTransition(250, "Attack")
                ),
            new State("Attack",
                new Wander(0.4f),
                new Grenade(8, 10, 3, cooldown: 1500)
            )
            );
    }
}