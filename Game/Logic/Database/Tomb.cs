using RotMG.Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database
{
    public sealed class Tomb : IBehaviorDatabase
    {
        public void Init(BehaviorDb db)
        {
            db.Init("Tomb Defender",
                new State("idle",
                    new Taunt(true, "THIS WILL NOW BE YOUR TOMB!"),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Orbit(.6f, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new HealthTransition(.989f, "weakning")
                ),
                new State("weakning",
                    new Orbit(.6f, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Taunt(true, "Impudence! I am an Immortal, I needn't waste time on you!"),
                    new Shoot(50, 20, index: 3, cooldown: 6000),
                    new State("blue shield 1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "unset blue shield 1")
                    ),
                    new State("unset blue shield 1"),
                    new HealthTransition(.979f, "active")
                ),
                new State("active",
                    new Orbit(.7f, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Shoot(50, 8, 45, 2, 0, 0, cooldown: 1000),
                    new Shoot(50, 3, 120, 1, 0, 0, cooldown: 5000),
                    new Shoot(50, 5, 72, 0, 0, 0, cooldown: 5000),
                    new HealthTransition(.7f, "boomerang")
                ),
                new State("boomerang",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Orbit(.6f, 3, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new Taunt(true, "Nut, disable our foes!"),
                    new Shoot(50, 1, index: 0, cooldown: 3000),
                    new Shoot(50, 8, index: 2, cooldown: 1000),
                    new Shoot(50, 3, 15, 1, cooldown: 3000),
                    new Shoot(50, 2, 90, 1, cooldown: 3000),
                    new State("blue shield 2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "unset blue shield 2")
                    ),
                    new State("unset blue shield 2"),
                    new HealthTransition(.55f, "double shot")
                ),
                new State("double shot",
                    new Taunt(true, "Geb, eradicate these cretins from our tomb!"),
                    new Orbit(.7f, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new Shoot(50, 8, index: 2, cooldown: 1000),
                    new Shoot(50, 2, 10, 0, cooldown: 3000),
                    new Shoot(50, 4, 15, 1, cooldown: 3000),
                    new Shoot(50, 2, 90, 1, cooldown: 3000),
                    new HealthTransition(.4f, "artifacts")
                ),
                new State("artifacts",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Taunt(true, "Nut, let them wish they were dead!"),
                    new Orbit(.6f, 7, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new Shoot(50, 8, index: 2, cooldown: 1000),
                    new Shoot(50, 2, 10, 0, cooldown: 3000),
                    new Shoot(50, 4, 15, 1, cooldown: 3000),
                    new Shoot(50, 2, 90, 1, cooldown: 3000),
                    new Spawn("Pyramid Artifact 1", 1, 0),
                    new Spawn("Pyramid Artifact 2", 1, 0),
                    new State("blue shield 3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "unset blue shield 3")
                    ),
                    new State("unset blue shield 3"),
                    new HealthTransition(.25f, "artifacts 2")
                ),
                new State("artifacts 2",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Taunt(true, "My artifacts shall prove my wall of defense is impenetrable!"),
                    new Orbit(.6f, 7, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new Shoot(50, 8, index: 2, cooldown: 1000),
                    new Shoot(50, 3, 10, 0, cooldown: 3000),
                    new Shoot(50, 5, 15, 1, cooldown: 3000),
                    new Shoot(50, 2, 80, 1, cooldown: 3000),
                    new Shoot(50, 2, 90, 1, cooldown: 3000),
                    new Spawn("Pyramid Artifact 1", 2, 0),
                    new State("blue shield 4",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "unset blue shield 4")
                    ),
                    new State("unset blue shield 4"),
                    new HealthTransition(.06f, "rage")
                ),
                new State("rage",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Taunt(true, "The end of your path is here!"),
                    new Follow(0.6f, range: 1, duration: 5000, cooldown: 0),
                    new Flash(0xfFF0000, 1, 9000001),
                    new Shoot(50, 10, 10, 4, cooldown: 750, cooldownOffset: 750),
                    new Shoot(50, 5, 10, 4, angleOffset: 180, cooldown: 500, cooldownOffset: 500),
                    new Shoot(50),
                    new Shoot(50, 3, 15, 1, cooldown: 2000),
                    new Shoot(50, 2, 90, 1, cooldown: 2000),
                    new Spawn("Pyramid Artifact 1", 1, 0),
                    new Spawn("Pyramid Artifact 2", 1, 0),
                    new Spawn("Pyramid Artifact 3", 1, 0),
                    new State("blue shield 5",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "unset blue shield 5")
                    ),
                    new State("unset blue shield 5")
                ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Ring of the Pyramid", 0.01f),
                    new ItemLoot("Tome of Holy Protection", 0.01f)
                )
            );
            db.Init("Tomb Support",
                new State("idle",
                    new Taunt(true, "ENOUGH OF YOUR VANDALISM!"),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Orbit(.6f, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new HealthTransition(.9875f, "weakning")
                ),
                new State("weakning",
                    new Orbit(.6f, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new Taunt("Impudence! I am an immortal, I needn't take your seriously."),
                    new Shoot(50, 20, index: 7, cooldown: 6000, cooldownOffset: 2000),
                    new State("blue shield 1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "unset blue shield 1")
                    ),
                    new State("unset blue shield 1"),
                    new HealthTransition(.97875f, "active")
                ),
                new State("active",
                    new Orbit(.7f, 4, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new Shoot(20, 1, index: 5, cooldown: 1000),
                    new Shoot(12, 3, 120, 1, 0, 0, cooldown: 2500, cooldownOffset: 1000),
                    new Shoot(12, 4, 90, 2, 0, 0, cooldown: 2500, cooldownOffset: 1500),
                    new Shoot(12, 5, 72, 3, 0, 0, cooldown: 2500, cooldownOffset: 2000),
                    new Shoot(12, 6, 60, 4, 0, 0, cooldown: 2500, cooldownOffset: 2500),
                    new State("blue shield 2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "unset blue shield 2")
                    ),
                    new State("unset blue shield 2"),
                    new HealthTransition(.9f, "boomerang")
                ),
                new State("boomerang",
                    new Orbit(.6f, 6, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new Taunt(true, "Bes, protect me at once!"),
                    new Shoot(20, 1, index: 5, cooldown: 1000),
                    new Shoot(20, 1, index: 6, cooldown: 3000),
                    new Shoot(12, 3, 120, 1, 0, 0, cooldown: 2500, cooldownOffset: 1000),
                    new Shoot(12, 4, 90, 2, 0, 0, cooldown: 2500, cooldownOffset: 1500),
                    new Shoot(12, 5, 72, 3, 0, 0, cooldown: 2500, cooldownOffset: 2000),
                    new Shoot(12, 6, 60, 4, 0, 0, cooldown: 2500, cooldownOffset: 2500),
                    new HealthTransition(.7f, "paralyze")
                ),
                new State("paralyze",
                    new Orbit(.6f, 7, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new Taunt(true, "Geb, eradicate these cretins from our tomb!"),
                    new Shoot(20, 1, index: 5, cooldown: 1000),
                    new Shoot(20, 1, index: 6, cooldown: 3000),
                    new Shoot(999, 2, 10, 8, 0, 180, cooldown: 1000),
                    new Shoot(12, 3, 120, 1, 0, 0, cooldown: 2500, cooldownOffset: 1000),
                    new Shoot(12, 4, 90, 2, 0, 0, cooldown: 2500, cooldownOffset: 1500),
                    new Shoot(12, 5, 72, 3, 0, 0, cooldown: 2500, cooldownOffset: 2000),
                    new Shoot(12, 6, 60, 4, 0, 0, cooldown: 2500, cooldownOffset: 2500),
                    new HealthTransition(.5f, "artifacts")
                ),
                new State("artifacts",
                    new Orbit(.6f, 4, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new Taunt(true, "My artifacts shall make your lethargic lives end much more swiftly!"),
                    new Shoot(20, 1, index: 5, cooldown: 1000),
                    new Shoot(20, 1, index: 6, cooldown: 3000),
                    new Shoot(12, 3, 120, 1, 0, 0, cooldown: 2500, cooldownOffset: 1000),
                    new Shoot(12, 4, 90, 2, 0, 0, cooldown: 2500, cooldownOffset: 1500),
                    new Shoot(12, 5, 72, 3, 0, 0, cooldown: 2500, cooldownOffset: 2000),
                    new Shoot(12, 6, 60, 4, 0, 0, cooldown: 2500, cooldownOffset: 2500),
                    new Spawn("Sphinx Artifact 1", 1, 0),
                    new State("blue shield 3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "unset blue shield 3")
                    ),
                    new State("unset blue shield 3"),
                    new HealthTransition(.3f, "double shoot")
                ),
                new State("double shoot",
                    new Orbit(.6f, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new Shoot(20, 2, 15, 5, cooldown: 1000),
                    new Shoot(20, 2, 15, 6, cooldown: 3000),
                    new Shoot(12, 3, 120, 1, 0, 0, cooldown: 2500, cooldownOffset: 1000),
                    new Shoot(12, 4, 90, 2, 0, 0, cooldown: 2500, cooldownOffset: 1500),
                    new Shoot(12, 5, 72, 3, 0, 0, cooldown: 2500, cooldownOffset: 2000),
                    new Shoot(12, 6, 60, 4, 0, 0, cooldown: 2500, cooldownOffset: 2500),
                    new Spawn("Sphinx Artifact 1", 2, 0),
                    new State("blue shield 4",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "unset blue shield 4")
                    ),
                    new State("unset blue shield 4"),
                    new HealthTransition(.06f, "rage")
                ),
                new State("rage",
                    new Taunt(true, "This cannot be! You shall not succeed!"),
                    new Follow(0.6f, range: 1, duration: 5000, cooldown: 0),
                    new Flash(0xfFF0000, 1, 9000001),
                    new Shoot(20, 1, index: 5, cooldown: 1000),
                    new Shoot(20, 1, 15, 0, cooldown: 750),
                    new Shoot(12, 4, 90, 1, 0, 0, cooldown: 2500, cooldownOffset: 1000),
                    new Shoot(12, 5, 72, 2, 0, 0, cooldown: 2500, cooldownOffset: 1500),
                    new Shoot(12, 6, 60, 3, 0, 0, cooldown: 2500, cooldownOffset: 2000),
                    new Shoot(12, 8, 45, 4, 0, 0, cooldown: 2500, cooldownOffset: 2500),
                    new Shoot(999, 6, 10, 8, angleOffset: 180, cooldown: 500),
                    new Spawn("Sphinx Artifact 1", 1, 0),
                    new State("blue shield 5",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "unset blue shield 5")
                    ),
                    new State("unset blue shield 5")
                ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Ring of the Sphinx", 0.01f)
                )
            );
            db.Init("Tomb Attacker",
                new State("idle",
                    new Taunt(true, "ENOUGH OF YOUR VANDALISM!"),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Orbit(.6f, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new HealthTransition(.988f, "weakning")
                ),
                new State("weakning",
                    new Orbit(.6f, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new Shoot(50, 20, index: 3, cooldown: 6000, cooldownOffset: 2000),
                    new State("blue shield 1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "unset blue shield 1")
                    ),
                    new State("unset blue shield 1"),
                    new HealthTransition(.9788f, "active")
                ),
                new State("active",
                    new Orbit(.6f, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new Shoot(14, 2, 10, 2, cooldown: 500),
                    new Shoot(12, 1, index: 0, cooldown: 2000),
                    new State("Grenade 1",
                        new Grenade(3, 160, 10, cooldown: 1500),
                        new TimedTransition(1500, "Grenade 2")
                    ),
                    new State("Grenade 2",
                        new Grenade(4, 120, 10, cooldown: 1500),
                        new TimedTransition(1500, "Grenade 1")
                    ),
                    new HealthTransition(.72f, "lets dance")
                ),
                new State("lets dance",
                    new Orbit(.6f, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new Taunt(true, "Bes, protect me at once!"),
                    new Shoot(14, 1, index: 2, cooldown: 500),
                    new Shoot(14, 2, 90, 2, cooldown: 1000),
                    new Shoot(14, 2, 90, 2, angleOffset: 270, cooldown: 1000),
                    new Shoot(11 + 1 / 5, 8, 45, 1, 0, cooldown: 5000),
                    new Shoot(12, 2, 45, 0, cooldown: 1500),
                    new Shoot(99, 1, index: 4, cooldown: 500),
                    new Spawn("Scarab", 3, 0, 10000),
                    new State("blue shield 2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "unset blue shield 2")
                    ),
                    new State("unset blue shield 2",
                        new TimedTransition(3000, "Grenade 3")
                    ),
                    new State("Grenade 3",
                        new Grenade(3, 160, 10, cooldown: 1500),
                        new TimedTransition(1500, "Grenade 4")
                    ),
                    new State("Grenade 4",
                        new Grenade(4, 120, 10, cooldown: 1500),
                        new TimedTransition(1500, "Grenade 3")
                    ),
                    new HealthTransition(.675f, "more muthafucka")
                ),
                new State("more muthafucka",
                    new Orbit(.6f, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new Taunt(true, "Nut, disable our foes!"),
                    new Spawn("Scarab", 3, 0, 10000),
                    new Shoot(14, 2, 10, 2, cooldown: 500),
                    new Shoot(14, 1, index: 2, cooldown: 500),
                    new Shoot(14, 2, 90, 2, cooldown: 1000),
                    new Shoot(14, 2, 90, 2, angleOffset: 270, cooldown: 1000),
                    new Shoot(11 + 1 / 5, 10, 36, 1, 0, cooldown: 5000),
                    new Shoot(12, 1, index: 0, cooldown: 2000),
                    new Shoot(12, 2, 45, 0, cooldown: 2000),
                    new Shoot(99, 1, index: 4, cooldown: 500),
                    new Shoot(99, 1, index: 4, angleOffset: 90, cooldown: 500),
                    new State("Grenade 5",
                        new Grenade(3, 160, 10, cooldown: 1500),
                        new TimedTransition(1500, "Grenade 6")
                    ),
                    new State("Grenade 6",
                        new Grenade(4, 120, 10, cooldown: 1500),
                        new TimedTransition(1500, "Grenade 5")
                    ),
                    new HealthTransition(.4f, "artifacts")
                ),
                new State("artifacts",
                    new Orbit(.6f, 4, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new Taunt(true, "My artifacts shall destroy you from your soul your flesh!"),
                    new Spawn("Scarab", 3, 0, 10000),
                    new Shoot(14, 2, 10, 2, cooldown: 500),
                    new Shoot(14, 1, index: 2, cooldown: 500),
                    new Shoot(14, 2, 90, 2, cooldown: 1000),
                    new Shoot(14, 2, 90, 2, angleOffset: 270, cooldown: 1000),
                    new Shoot(11 + 1 / 5, 10, 36, 1, 0, cooldown: 5000),
                    new Shoot(12, 1, index: 0, cooldown: 2000),
                    new Shoot(12, 2, 45, 0, cooldown: 2000),
                    new Shoot(99, 1, index: 4, cooldown: 500),
                    new Shoot(99, 1, index: 4, angleOffset: 90, cooldown: 500),
                    new Spawn("Nile Artifact 1", 1, 0),
                    new State("blue shield 3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "unset blue shield 3")
                    ),
                    new State("unset blue shield 3",
                        new TimedTransition(3000, "Grenade 7")
                    ),
                    new State("Grenade 7",
                        new Grenade(5, 45, 10, cooldown: 1500),
                        new TimedTransition(1500, "Grenade 8")
                    ),
                    new State("Grenade 8",
                        new Grenade(4, 100, 10, cooldown: 1500),
                        new TimedTransition(1500, "Grenade 9")
                    ),
                    new State("Grenade 9",
                        new Grenade(3, 120, 10, cooldown: 1500),
                        new TimedTransition(1500, "Grenade 7")
                    ),
                    new HealthTransition(.2f, "artifacts 2")
                ),
                new State("artifacts 2",
                    new Orbit(.6f, 4, target: "Tomb Boss Anchor", radiusVariance: 0.5f),
                    new Taunt(true, "My artifacts shall destroy you from your soul your flesh!"),
                    new Spawn("Scarab", 3, 0, 10000),
                    new Shoot(14, 2, 10, 2, cooldown: 500),
                    new Shoot(14, 1, index: 2, cooldown: 500),
                    new Shoot(14, 2, 90, 2, cooldown: 1000),
                    new Shoot(14, 2, 90, 2, angleOffset: 270, cooldown: 1000),
                    new Shoot(11 + 1 / 5, 10, 36, 1, 0, cooldown: 5000),
                    new Shoot(12, 1, index: 0, cooldown: 2000),
                    new Shoot(12, 2, 45, 0, cooldown: 2000),
                    new Shoot(99, 1, index: 4, cooldown: 500),
                    new Shoot(99, 1, index: 4, angleOffset: 90, cooldown: 500),
                    new Spawn("Nile Artifact 1", 2, 0),
                    new State("blue shield 4",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "unset blue shield 4")
                    ),
                    new State("unset blue shield 4",
                        new TimedTransition(3000, "Grenade 10")
                    ),
                    new State("Grenade 10",
                        new Grenade(5, 45, 10, null, 1500),
                        new TimedTransition(1500, "Grenade 11")
                    ),
                    new State("Grenade 11",
                        new Grenade(4, 100, 10, cooldown: 1500),
                        new TimedTransition(1500, "Grenade 12")
                    ),
                    new State("Grenade 12",
                        new Grenade(3, 120, 10, cooldown: 1500),
                        new TimedTransition(1500, "Grenade 10")
                    ),
                    new HealthTransition(.06f, "rage")
                ),
                new State("rage",
                    new Taunt(true, "This cannot be! You shall not succeed!"),
                    new Flash(0xfFF0000, 1, 9000001),
                    new StayBack(.5f, 6),
                    new Shoot(11 + 1 / 5, 10, 36, 1, 0, cooldown: 5000),
                    new Shoot(14, 2, 10, 2, cooldown: 500),
                    new Shoot(14, 1, index: 2, cooldown: 500),
                    new Shoot(14, 2, 90, 2, cooldown: 1000),
                    new Shoot(14, 2, 90, 2, angleOffset: 270, cooldown: 1000),
                    new Shoot(12, 1, index: 0, cooldown: 2000),
                    new Shoot(12, 2, 45, 0, cooldown: 2000),
                    new Spawn("Scarab", 3, 0, 10000),
                    new Spawn("Nile Artifact 1", 1, 0),
                    new State("blue shield 5",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "unset blue shield 5")
                    ),
                    new State("unset blue shield 5",
                        new TimedTransition(3000, "Grenade 13")
                    ),
                    new State("Grenade 13",
                        new Grenade(3, 150, 10, cooldown: 1500),
                        new TimedTransition(1500, "Grenade 14")
                    ),
                    new State("Grenade 14",
                        new Grenade(4, 120, 10, cooldown: 1500),
                        new TimedTransition(1500, "Grenade 13")
                    )
                ),
                new Threshold(0.01f,
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Ring of the Nile", 0.01f)
                )
            );
            db.Init("Pyramid Artifact 1",
                new Prioritize(
                    new Orbit(1, 2, target: "Tomb Defender", radiusVariance: 0.5f),
                    new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                ),
                new Shoot(3, 3, 120, cooldown: 2500)
            );
            db.Init("Pyramid Artifact 2",
                new Prioritize(
                    new Orbit(1, 2, target: "Tomb Attacker", radiusVariance: 0.5f),
                    new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                ),
                new Shoot(3, 3, 120, cooldown: 2500)
            );
            db.Init("Pyramid Artifact 3",
                new Prioritize(
                    new Orbit(1, 2, target: "Tomb Support", radiusVariance: 0.5f),
                    new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                ),
                new Shoot(3, 3, 120, cooldown: 2500)
            );
            db.Init("Sphinx Artifact 1",
                new Prioritize(
                    new Orbit(1, 2, target: "Tomb Defender", radiusVariance: 0.5f),
                    new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                ),
                new Shoot(12, 3, 120, cooldown: 2500)
            );
            db.Init("Sphinx Artifact 2",
                new Prioritize(
                    new Orbit(1, 2, target: "Tomb Attacker", radiusVariance: 0.5f),
                    new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                ),
                new Shoot(12, 3, 120, cooldown: 2500)
            );
            db.Init("Sphinx Artifact 3",
                new Prioritize(
                    new Orbit(1, 2, target: "Tomb Support", radiusVariance: 0.5f),
                    new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                ),
                new Shoot(12, 3, 120, cooldown: 2500)
            );
            db.Init("Nile Artifact 1",
                new Prioritize(
                    new Orbit(1, 2, target: "Tomb Defender", radiusVariance: 0.5f),
                    new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                ),
                new Shoot(12, 3, 120, cooldown: 2500)
            );
            db.Init("Nile Artifact 2",
                new Prioritize(
                    new Orbit(1, 2, target: "Tomb Attacker", radiusVariance: 0.5f),
                    new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                ),
                new Shoot(12, 3, 120, cooldown: 2500)
            );
            db.Init("Nile Artifact 3",
                new Prioritize(
                    new Orbit(1, 2, target: "Tomb Support", radiusVariance: 0.5f),
                    new Follow(0.85f, range: 1, duration: 5000, cooldown: 0)
                ),
                new Shoot(12, 3, 120, cooldown: 2500)
            );
            db.Init("Tomb Defender Statue",
                new State("sleep",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntityNotWithinTransition("Inactive Sarcophagus", 1000, "checkActive"),
                    new EntityNotWithinTransition("Active Sarcophagus", 1000, "checkInactive")
                ),
                new State("checkActive",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntityNotWithinTransition("Active Sarcophagus", 1000, "ItsGoTime")
                ),
                new State("checkInactive",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntityNotWithinTransition("Inactive Sarcophagus", 1000, "ItsGoTime")
                ),
                new State("ItsGoTime",
                    new Transform("Tomb Defender")
                )
            );
            db.Init("Tomb Support Statue",
                new State("sleep",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntityNotWithinTransition("Inactive Sarcophagus", 1000, "checkActive"),
                    new EntityNotWithinTransition("Active Sarcophagus", 1000, "checkInactive")
                ),
                new State("checkActive",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntityNotWithinTransition("Active Sarcophagus", 1000, "ItsGoTime")
                ),
                new State("checkInactive",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntityNotWithinTransition("Inactive Sarcophagus", 1000, "ItsGoTime")
                ),
                new State("ItsGoTime",
                    new Transform("Tomb Support")
                )
            );
            db.Init("Tomb Attacker Statue",
                new State("sleep",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntityNotWithinTransition("Inactive Sarcophagus", 1000, "checkActive"),
                    new EntityNotWithinTransition("Active Sarcophagus", 1000, "checkInactive")
                ),
                new State("checkActive",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntityNotWithinTransition("Active Sarcophagus", 1000, "ItsGoTime")
                ),
                new State("checkInactive",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntityNotWithinTransition("Inactive Sarcophagus", 1000, "ItsGoTime")
                ),
                new State("ItsGoTime",
                    new Transform("Tomb Attacker")
                )
            );
            db.Init("Inactive Sarcophagus",
                new State("sleep",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntityNotWithinTransition("Beam Priestess", 14, "checkPriest"),
                    new EntityNotWithinTransition("Beam Priest", 1000, "checkPriestess")
                ),
                new State("checkPriest",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntityNotWithinTransition("Beam Priest", 1000, "activate")
                ),
                new State("checkPriestess",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntityNotWithinTransition("Beam Priestess", 1000, "activate")
                ),
                new State("activate",
                    new Transform("Active Sarcophagus")
                )
            );
            db.Init("Scarab",
                new NoPlayerWithinTransition(7, false, "Idle"),
                new PlayerWithinTransition(7, false, "Chase"),
                new State("Idle",
                    new Wander(.1f)
                ),
                new State("Chase",
                    new Follow(1.5f, 7, 0),
                    new Shoot(3, index: 1, cooldown: 500)
                )
            );
            db.Init("Active Sarcophagus",
                new State("wait",
                    new HealthTransition(60, "stun")
                ),
                new State("stun",
                    new Shoot(50, 8, 10, 0, cooldown: 9999999, cooldownOffset: 500),
                    new Shoot(50, 8, 10, 0, cooldown: 9999999, cooldownOffset: 1000),
                    new Shoot(50, 8, 10, 0, cooldown: 9999999, cooldownOffset: 1500),
                    new TimedTransition(1500, "idle")
                ),
                new State("idle",
                    new ChangeSize(100, 100)
                ),
                new ItemLoot("Magic Potion", 0.002f),
                new ItemLoot("Health Potion", 0.15f),
                new Threshold(0.32f,
                    new ItemLoot("Tincture of Mana", 0.15f),
                    new ItemLoot("Tincture of Dexterity", 0.15f),
                    new ItemLoot("Tincture of Life", 0.15f)
                )
            );
            db.Init("Tomb Boss Anchor",
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new DropPortalOnDeath("Glowing Realm Portal", 1),
                new State("Idle",
                    new EntitiesNotWithinTransition(300, "Death", "Tomb Support", "Tomb Attacker", "Tomb Defender",
                        "Active Sarcophagus", "Tomb Defender Statue", "Tomb Support Statue", "Tomb Attacker Statue")
                ),
                new State("Death",
                    new Suicide()
                )
            );
        }
    }
}