using Common;
using RotMG.Game.Logic.Behaviors;
using RotMG.Game.Logic.Loots;
using RotMG.Game.Logic.Transitions;

namespace RotMG.Game.Logic.Database;

public sealed class OryxCastle : IBehaviorDatabase
{
    public void Init(BehaviorDb db)
    {
        db.Init("Oryx Stone Guardian Right",
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new PlayerWithinTransition(7, true, "Order")
                ),
                new State("Order",
                    new Order(10, "Oryx Stone Guardian Left", "Start"),
                    new TimedTransition(200, "Start")
                ),
                new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xC0C0C0, 0.5f, 3),
                    new TimedTransition(1500, "Imma Follow")
                ),
                new State("Imma Follow",
                    new Follow(1, 2, 0.3f),
                    new Shoot(5, 5, shootAngle: 5, cooldown: 1000),
                    new TimedTransition(5000, "Imma chill")
                ),
                new State("Imma chill",
                    new Prioritize(
                        new StayCloseToSpawn(0.5f, 3),
                        new Wander(0.5f)
                        ),
                        new Shoot(10, 10, index: 2, fixedAngle: 0, cooldown: 1000),
                        new TimedTransition(5000, "Circle-Prepare")
                    ),
                new State("Circle-Prepare",
                    new MoveTo(1, 108, 43),
                    new TimedTransition(2500, "PrepareEnd")
                ),
                new State("PrepareEnd",
                    new Orbit(1, 5, target: "Oryx Guardian TaskMaster", orbitClockwise: true),
                    new Shoot(0, 2, fixedAngle: 0, index: 1, cooldownOffset: 0),
                    new Shoot(0, 2, fixedAngle: 36, index: 1, cooldownOffset: 200),
                    new Shoot(0, 2, fixedAngle: 72, index: 1, cooldownOffset: 400),
                    new Shoot(0, 2, fixedAngle: 108, index: 1, cooldownOffset: 600),
                    new Shoot(0, 2, fixedAngle: 144, index: 1, cooldownOffset: 800),
                    new Shoot(0, 2, fixedAngle: 180, index: 1, cooldownOffset: 1000),
                    new Shoot(0, 2, fixedAngle: 216, index: 1, cooldownOffset: 1200),
                    new Shoot(0, 2, fixedAngle: 252, index: 1, cooldownOffset: 1400),
                    new Shoot(0, 2, fixedAngle: 288, index: 1, cooldownOffset: 1600),
                    new Shoot(0, 2, fixedAngle: 324, index: 1, cooldownOffset: 1800),
                    new TimedTransition(6000, "checkEntities")
                ),
                new State("checkEntities",
                    new PlayerWithinTransition(3, true, "cpe_Imma Follow"),
                    new NoPlayerWithinTransition(3, true, "cpe_Imma chill")
                ),
                new State("cpe_Imma Follow",
                    new Follow(1, 3, 0.3f),
                    new Shoot(10, 5, cooldown: 1000),
                    new TimedTransition(2500, "cpe_Imma chill")
                ),
                new State("cpe_Imma chill",
                    new Prioritize(
                        new StayCloseToSpawn(0.5f, 3),
                        new Wander(0.5f)
                    ),
                    new Shoot(10, 10, index: 2, fixedAngle: 0, cooldown: 1000),
                    new TimedTransition(2500, "Move Sideways")
                ),
                new State("Move Sideways",
                    new MoveTo(speed: 1, x: 118, y: 43),
                    new Shoot(0, 2, fixedAngle: 90f, cooldownOffset: 0),
                    new Shoot(0, 2, fixedAngle: 85.5f, cooldownOffset: 100),
                    new Shoot(0, 2, fixedAngle: 81f, cooldownOffset: 200),
                    new Shoot(0, 2, fixedAngle: 76.5f, cooldownOffset: 300),
                    new Shoot(0, 2, fixedAngle: 72f, cooldownOffset: 400),
                    new Shoot(0, 2, fixedAngle: 67.5f, cooldownOffset: 500),
                    new Shoot(0, 2, fixedAngle: 63f, cooldownOffset: 600),
                    new Shoot(0, 2, fixedAngle: 58.5f, cooldownOffset: 700),
                    new Shoot(0, 2, fixedAngle: 54f, cooldownOffset: 800),
                    new Shoot(0, 2, fixedAngle: 49.5f, cooldownOffset: 900),
                    new Shoot(0, 2, fixedAngle: 45f, cooldownOffset: 1000),
                    new Shoot(0, 2, fixedAngle: 40.5f, cooldownOffset: 1100),
                    new Shoot(0, 2, fixedAngle: 36f, cooldownOffset: 1200),
                    new Shoot(0, 2, fixedAngle: 31.5f, cooldownOffset: 1300),
                    new Shoot(0, 2, fixedAngle: 27f, cooldownOffset: 1400),
                    new Shoot(0, 2, fixedAngle: 22.5f, cooldownOffset: 1500),
                    new Shoot(0, 2, fixedAngle: 18f, cooldownOffset: 1600),
                    new Shoot(0, 2, fixedAngle: 13.5f, cooldownOffset: 1700),
                    new Shoot(0, 2, fixedAngle: 9f, cooldownOffset: 1800),
                    new Shoot(0, 2, fixedAngle: 4.5f, cooldownOffset: 1900),
                    new TimedTransition(4000, "CheckFriend")
                ),
                new State("CheckFriend", 
                    new EntityNotWithinTransition("Oryx Stone Guardian Left", 100f, "Forever Alone"),
                    new TimedTransition(100, "Imma Follow")
                ),
                new State("Forever Alone",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new SetAltTexture(2),
                    new ReturnToSpawn(1),
                    new TossObject(maxDensity: 1, angle: 270, child: "Oryx Guardian Sword", cooldown: 999999),
                    new TimedTransition(1500, "WaittilDeath")
                    ),
                new State("WaittilDeath",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                    new EntityNotWithinTransition("Oryx Guardian Sword", 99, "Continue Alone")
                    ),
                new State("Continue Alone",
                    new Prioritize(
                        new Follow(1),
                        new StayCloseToSpawn(0.5f, 3)
                        ),
                    new Shoot(5, 5, shootAngle: 5, cooldown: 1000),
                    new Shoot(8, 2, index: 2, cooldownOffset: 500)
                    ),               
            new Threshold(0.01f,
                new ItemLoot("Potion of Defense", 1),
                new TierLoot(10, TierLoot.LootType.Weapon, 0.07f),
                new TierLoot(11, TierLoot.LootType.Weapon, 0.06f),
                new TierLoot(5, TierLoot.LootType.Ability, 0.07f),
                new TierLoot(11, TierLoot.LootType.Armor, 0.07f),
                new TierLoot(5, TierLoot.LootType.Ring, 0.06f)
            )

        );
        db.Init("Oryx Stone Guardian Left",
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new PlayerWithinTransition(7, true, "Order")
                ),
                new State("Order",
                    new Order(10, "Oryx Stone Guardian Right", "Start"),
                    new TimedTransition(200, "Start")
                ),
                new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xC0C0C0, 0.5f, 3),
                    new TimedTransition(1500, "Imma Follow")
                ),
                new State("Imma Follow",
                    new Follow(1, 2, 0.3f),
                    new Shoot(10, 5, shootAngle: 5, cooldown: 1000),
                    new TimedTransition(5000, "Imma chill")
                ),
                new State("Imma chill",
                    new Prioritize(
                        new StayCloseToSpawn(0.5f, 3),
                        new Wander(0.5f)
                    ),
                    new Shoot(10, 10, index: 2, fixedAngle: 0, cooldown: 1000),
                    new TimedTransition(5000, "Circle-Prepare")
                ),
                new State("Circle-Prepare",
                    new MoveTo(1, 108, 43),
                    new TimedTransition(2500, "PrepareEnd")
                ),
                new State("PrepareEnd",
                    new Orbit(1, 5, target: "Oryx Guardian TaskMaster"),
                    new Shoot(0, 2, fixedAngle: 0, index: 1, cooldownOffset: 0),
                    new Shoot(0, 2, fixedAngle: 36, index: 1, cooldownOffset: 200),
                    new Shoot(0, 2, fixedAngle: 72, index: 1, cooldownOffset: 400),
                    new Shoot(0, 2, fixedAngle: 108, index: 1, cooldownOffset: 600),
                    new Shoot(0, 2, fixedAngle: 144, index: 1, cooldownOffset: 800),
                    new Shoot(0, 2, fixedAngle: 180, index: 1, cooldownOffset: 1000),
                    new Shoot(0, 2, fixedAngle: 216, index: 1, cooldownOffset: 1200),
                    new Shoot(0, 2, fixedAngle: 252, index: 1, cooldownOffset: 1400),
                    new Shoot(0, 2, fixedAngle: 288, index: 1, cooldownOffset: 1600),
                    new Shoot(0, 2, fixedAngle: 324, index: 1, cooldownOffset: 1800),
                    new TimedTransition(6000, "checkEntities")
                ),
                new State("checkEntities",
                    new PlayerWithinTransition(3, true, "cpe_Imma Follow"),
                    new NoPlayerWithinTransition(3, true, "cpe_Imma chill")
                ),
                new State("cpe_Imma Follow",
                    new Follow(1, 3, 0.3f),
                    new Shoot(10, 5, cooldown: 1000),
                    new TimedTransition(2500, "cpe_Imma chill")
                ),
                new State("cpe_Imma chill",
                    new Prioritize(
                        new StayCloseToSpawn(0.5f, 3),
                        new Wander(0.5f)
                    ),
                    new Shoot(0, 10, index: 2, fixedAngle: 0, cooldown: 1000),
                    new TimedTransition(2500, "Move Sideways")
                ),
                new State("Move Sideways",
                    new MoveTo(speed: 1, x: 118, y: 43),
                    new Shoot(0, 2, fixedAngle: 90f, cooldownOffset: 0),
                    new Shoot(0, 2, fixedAngle: 85.5f, cooldownOffset: 100),
                    new Shoot(0, 2, fixedAngle: 81f, cooldownOffset: 200),
                    new Shoot(0, 2, fixedAngle: 76.5f, cooldownOffset: 300),
                    new Shoot(0, 2, fixedAngle: 72f, cooldownOffset: 400),
                    new Shoot(0, 2, fixedAngle: 67.5f, cooldownOffset: 500),
                    new Shoot(0, 2, fixedAngle: 63f, cooldownOffset: 600),
                    new Shoot(0, 2, fixedAngle: 58.5f, cooldownOffset: 700),
                    new Shoot(0, 2, fixedAngle: 54f, cooldownOffset: 800),
                    new Shoot(0, 2, fixedAngle: 49.5f, cooldownOffset: 900),
                    new Shoot(0, 2, fixedAngle: 45f, cooldownOffset: 1000),
                    new Shoot(0, 2, fixedAngle: 40.5f, cooldownOffset: 1100),
                    new Shoot(0, 2, fixedAngle: 36f, cooldownOffset: 1200),
                    new Shoot(0, 2, fixedAngle: 31.5f, cooldownOffset: 1300),
                    new Shoot(0, 2, fixedAngle: 27f, cooldownOffset: 1400),
                    new Shoot(0, 2, fixedAngle: 22.5f, cooldownOffset: 1500),
                    new Shoot(0, 2, fixedAngle: 18f, cooldownOffset: 1600),
                    new Shoot(0, 2, fixedAngle: 13.5f, cooldownOffset: 1700),
                    new Shoot(0, 2, fixedAngle: 9f, cooldownOffset: 1800),
                    new Shoot(0, 2, fixedAngle: 4.5f, cooldownOffset: 1900),
                    new TimedTransition(4000, "CheckFriend")
                ),
            new State("CheckFriend",
                new EntityNotWithinTransition("Oryx Stone Guardian Right", 100f, "Forever Alone"),
                new TimedTransition(100, "Imma Follow")
            ),
            new State("Forever Alone",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                new SetAltTexture(2),
                new ReturnToSpawn(1),
                new TossObject(maxDensity: 1, angle: 270, child: "Oryx Guardian Sword", cooldown: 999999),
                new TimedTransition(1500, "WaittilDeath")
                ),
            new State("WaittilDeath",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                new EntityNotWithinTransition("Oryx Guardian Sword", 99, "Continue Alone")
                ),
            new State("Continue Alone",
                new Prioritize(
                    new Follow(1),
                    new StayCloseToSpawn(0.5f, 3)
                    ),
                new Shoot(5, 5, shootAngle: 5, cooldown: 1000),
                new Shoot(8, 2, index: 2, cooldownOffset: 500)
                ),
            new Threshold(0.01f,
                new ItemLoot("Potion of Defense", 1),
                new TierLoot(10, TierLoot.LootType.Weapon, 0.07f),
                new TierLoot(11, TierLoot.LootType.Weapon, 0.06f),
                new TierLoot(5, TierLoot.LootType.Ability, 0.07f),
                new TierLoot(11, TierLoot.LootType.Armor, 0.07f),
                new TierLoot(5, TierLoot.LootType.Ring, 0.06f)
            )
        );

        db.Init("Oryx Guardian Sword",
            new ConditionalEffect(ConditionEffectIndex.StunImmune),
            new OnDeathBehavior(new Grenade(fixedAngle: 90, damage: 50, radius: 10)),
            new State("shoot1",
                new Shoot(count: 5, range: 20, fixedAngle: 0, cooldown: 1000),
                new TimedTransition(1000, "shoot2")
                ),
            new State("shoot2",
                new Shoot(count: 5, range: 20, fixedAngle: 90, cooldown: 1000)
                )
            );

        db.Init("Oryx Guardian TaskMaster",
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Idle",
                    new EntitiesNotWithinTransition(100, "Death", "Oryx Stone Guardian Right", "Oryx Stone Guardian Left")
                ),
                new State("Death",
                    new Spawn("Oryx's Chamber Portal", 1, 1),
                    new Decay(100)
                )
            );
        db.Init("Oryx's Living Floor Fire Down",
            new State("Idle",
                new PlayerWithinTransition(20, true, "Toss")
            ),
            new State("Toss",
                new TossObject("Quiet Bomb", 10, cooldown: 1000, tossInvis: true),
                new NoPlayerWithinTransition(21, true, "Idle"),
                new PlayerWithinTransition(7, false, "Shoot and Toss")
            ),
            new State("Shoot and Toss",
                new NoPlayerWithinTransition(21, true, "Idle"),
                new NoPlayerWithinTransition(6, false, "Toss"),
                new Shoot(10, 18, fixedAngle: 0, cooldown: 750),
                new TossObject("Quiet Bomb", 10, cooldown: 1000)
            )               
        );
        db.Init("Oryx Knight",
            new State("wait",
                    new PlayerWithinTransition(15, true, "start")
                    ),
                new State("start",
                    new Prioritize(
                        new Wander(0.2f),
                        new Follow(0.6f, 10, 3, -1, 0)
                        ),
                    new Shoot(10, 3, 20, 0, cooldown: 400),
                    new TimedTransition(5000, "attack")
                    ),
                new State("attack",
                    new Prioritize(
                            new Wander(0.2f),
                        new Follow(0.7f, 10, 3, -1, 0)
                        ),
                    new Shoot(10, 1, index: 0, cooldown: 400),
                    new Shoot(10, 1, index: 1, cooldown: 1000),
                    new Shoot(10, 1, index: 2, cooldown: 450),
                    new TimedTransition(2500, "start")
                    )
              );
        db.Init("Oryx Pet",
            new State("idle",
                new PlayerWithinTransition(15, true, "start")
                ),
            new State("start",
                new Prioritize(
                    new Follow(0.6f, 10, 0, -1, 0)
                    ),
                new Shoot(10, 2, shootAngle: 20, index: 0, cooldown: 400),
            new Shoot(10, 1, index: 0, cooldown: 400)
                )
            );
        db.Init("Oryx Insect Commander",
                new State("start",
                    new Prioritize(
                        new Wander(0.2f)
                        ),
                    new Reproduce("Oryx Insect Minion", 10, 15, cooldown: 1000),
                    new Shoot(10, 1, index: 0, cooldown: 900)
                    )
              );
        db.Init("Oryx Insect Minion",
            new State("start",
                new Prioritize(
                    new Follow(0.8f, 10, 1, -1, 0),
                    new StayCloseToSpawn(0.4f, 8)
                    ),
                new Shoot(10, 5, index: 0, cooldown: 1500),
                new Shoot(10, 1, index: 0, cooldown: 400)
                )
            );
        db.Init("Oryx Suit of Armor",
                new State("idle",
                    new PlayerWithinTransition(8, true, "start")
                    ),
                new State("start",
                    new HealthTransition(0.99f, "getting attacked")
                    ),
                new State("getting attacked",
                    new Prioritize(
                        new Wander(0.2f),
                        new Follow(0.4f, 10, 2, -1, 0)
                        ),
                    new SetAltTexture(1),
                    new Shoot(10, 2, 15, 0, cooldown: 600),
                    new HealthTransition(0.35f, "heal")
                    ),
                new State("heal",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new SetAltTexture(0),
                    new Shoot(10, 6, index: 0, cooldown: 400),
                    new HealSelf(cooldown: 2000, amount: 200),
                    new TimedTransition(1500, "getting attacked")
                    )
            );
        db.Init("Oryx Eye Warrior",
            new State("idle",
                new PlayerWithinTransition(15, true, "start")
                ),
            new State("start",
                    new Prioritize(
                        new Follow(0.6f, 10, 0, -1, 0)
                        ),
                    new Shoot(10, 5, index: 0, cooldown: 1000),
                    new Shoot(10, 1, index: 1, cooldown: 500)
                    )
        );
        db.Init("Oryx Brute",
                  new State("idle",
                      new PlayerWithinTransition(15, true, "start")
                    ),
                  new State("start",
                      new Prioritize(
                          new Wander(0.2f),
                          new Follow(0.4f, 10, 1, -1, 0)
                          ),
                      new Shoot(10, 5, index: 1, cooldown: 1000),
                      new Reproduce("Oryx Eye Warrior", 10, 4, cooldown: 400),
                      new TimedTransition(5000, "charge")
                      ),
                  new State("charge",
                      new Prioritize(
                          new Follow(1.2f, 10, 1, -1, 0)
                          ),
                      new Shoot(10, 5, index: 1, cooldown: 1000),
                      new Shoot(10, 5, index: 2, cooldown: 750),
                      new Reproduce("Oryx Eye Warrior", 10, 4, cooldown: 400),
                      new Shoot(10, 3, 10, index: 0, cooldown: 400),
                      new TimedTransition(4000, "start")
                     )
              );
        db.Init("Quiet Bomb",
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Tex1",
                    new TimedTransition(250, "Tex2")
                ),
                new State("Tex2",
                    new SetAltTexture(1),
                    new TimedTransition(250, "Tex3")
                ),
                new State("Tex3",
                    new SetAltTexture(0),
                    new TimedTransition(250, "Tex4")
                ),
                new State("Tex4",
                    new SetAltTexture(1),
                    new TimedTransition(250, "Explode")
                ),
                new State("Explode",
                    new SetAltTexture(0),
                    new Shoot(0, 18, fixedAngle: 0),
                    new Suicide(100)
                )
            );

    }
}