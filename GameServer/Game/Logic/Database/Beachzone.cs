using Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database;

public sealed class Beachzone : IBehaviorDatabase
{
    public void Init(BehaviorDb db)
    {
        db.Init("Masked Party God",
            new State("Idle",
                new SetAltTexture(1, 3, 500, loop: true),
                new Taunt(true,
                    "Lets have a fun-time in the sun-shine!",
                    "Oh no, Mixcoatl is my brother, I prefer partying to fighting.",
                    "Nothing like relaxin' on the beach.",
                    "Chillin' is the name of the game!",
                    "I hope you're having a good time!",
                    "How do you like my shades?",
                    "EVERYBODY BOOGEY!",
                    "What a beautiful day!",
                    "Whoa there!",
                    "Oh SNAP!",
                    "Ho!"),
                new HealSelf(5000, 30000)
            ),
            new Threshold(0.01f,
                new ItemLoot("Blue Paradise", 0.4f),
                new ItemLoot("Pink Passion Breeze", 0.4f),
                new ItemLoot("Bahama Sunrise", 0.4f),
                new ItemLoot("Lime Jungle Bay", 0.4f)
            )
        );
    }
}