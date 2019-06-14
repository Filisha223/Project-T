using wServer.logic.behaviors;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ PlanetBehaviors = () => Behav()

        #region Nexus Misc

            .Init("Planet Crier",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Player Within",
                        new PlayerWithinTransition(5, "Speech1")
                        ),

                        new State("Speech1",
                            new Taunt("Well, This rocket isn't done yet but once finished you'll beable to adventure far into the universe."),
                        new SetAltTexture(3)
                        ),
                        new State("Speech",
                        new SetAltTexture(3),
                        new Taunt("Hey adventurer, if you find rocket fuel you can power this rocket for 30 minutes for everyone to explore.", "This rocket allows you to explore multiple planets with all sorts of creatures, use rocket fuel to power it up for 30 minutes of adventuring."),
                        new TimedTransition(8000, "Return")
                             ),
                        new State("Activated",
                        new SetAltTexture(3),
                        new Taunt("The rocket has been fully powered up, go explore before the time runs out!.", "The rocket is ready to take off, hurry and go explore!"),
                        new TimedTransition(8000, "Activated")
                        ),
                        new State("Return",
                            new TimedTransition(10000, "Speech")

                            )
                )
            )
        .Init("Planet Space Ship",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Player Within",
                        new Order(50, "Planet Crier", "Speech"),
                        new InvisiToss("Planet Portal Spawner", 0, 0, 90000000, coolDownOffset: 0)
                        ),
                    new State("portal",
                        new Taunt("Space Ship (Activated)"),
                        new Order(50, "Planet Portal Spawner", "spawn"),
                        new Order(50, "Planet Crier", "Activated"),
                        new TimedTransition(1799999, "Player Within")



                            )
                )
            )
         .Init("Planet Portal Spawner",
                new State(
                    new DropPortalOnDeath("LightSpeed Selector", percent: 100, dropDelaySec: 0, XAdjustment: 0, YAdjustment: 0, PortalDespawnTimeSec: 1800),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("nothing"
                        ),
                    new State("spawn",
                        new Suicide()
                            )
                )
            )

        .Init("Planet Portal Spawned",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("nothing",
                         new Order(50, "Planet Space Ship", "portal"),
                          new TimedTransition(9999, "spawn")
                        ),
                    new State("spawn",
                        new Suicide()
                            )
                )
            )
        #endregion

        #region Moon

        .Init("Moon Crier",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Player Within",
                        new PlayerWithinTransition(5, "Speech")
                        ),
                        new State("Speech",
                        new SetAltTexture(3),
                        new Taunt("Whew, I thought I'd be stuck on this cold planet all by myself. Welcome to the moon!", "You must have been able to power the rocket! This place is what we call the moon!."),
                        new TimedTransition(10000, "Player Within")


                            )
                )
            )

        #endregion







        ;
    }
}