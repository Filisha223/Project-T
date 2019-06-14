﻿#region
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.logic;
using wServer.networking;
using wServer.networking.svrPackets;

#endregion

namespace wServer.realm.entities.player
{
    public partial class Player
    {
        private static readonly Dictionary<string, Tuple<int, int, int>> questDat =
            new Dictionary<string, Tuple<int, int, int>> //Priority, Min, Max
            {
                
                {"Lich", Tuple.Create(100, 1, 1000)},
                {"Actual Lich", Tuple.Create(100, 1, 1000)},
                {"Ent Ancient", Tuple.Create(100, 1, 1000)},
                {"Actual Ent Ancient", Tuple.Create(100, 1, 1000)},
                {"Oasis Giant", Tuple.Create(110, 1000, 1)},
                {"1Opener1", Tuple.Create(100, 1, 1000)},
                {"2Opener2", Tuple.Create(100, 1, 1000)},
                {"3Opener3", Tuple.Create(100, 1, 1000)},
                {"Phoenix Lord", Tuple.Create(100, 1, 1000)},
                {"Ghost King", Tuple.Create(100, 1, 1000)},
                {"Actual Ghost King", Tuple.Create(100, 1, 1000)},
                {"Cyclops God", Tuple.Create(100, 1, 1000)},
                {"Blue Demon", Tuple.Create(110, 1000, 1)},
                {"Lucky Djinn", Tuple.Create(1100, 1, 1000)},
                {"Lucky Ent", Tuple.Create(1100, 1, 1000)},//shtrs Bridge Sentinel
                {"Cultist of the Halls", Tuple.Create(1100, 1, 1000)},
                {"shtrs Bridge Sentinel", Tuple.Create(1100, 1, 1000)},
                {"Pentaract", Tuple.Create(1100, 1, 1000)},
                {"Cube God", Tuple.Create(1100, 1, 1000)},
                {"shtrs Defense System", Tuple.Create(1100, 1, 1000)},
                {"Mythical Nymph", Tuple.Create(1100, 1, 1000)},
                {"Grand Sphinx", Tuple.Create(1100, 1, 1000)},
                //{"shtrs Defense System", Tuple.Create(1100, 1, 1000)},
                {"Dragon Head", Tuple.Create(1100, 1, 1000)},
                {"Lord of the Lost Lands", Tuple.Create(1100, 1, 1000)},
                {"Hermit God", Tuple.Create(1100, 1, 1000)},
                {"Ghost Ship", Tuple.Create(1100, 1, 1000)},
                {"Unknown Giant Golem", Tuple.Create(1100, 1, 1000)},
                {"Evil Chicken God", Tuple.Create(1100, 1, 1000)},
                {"Biff the Resurrected Bunny", Tuple.Create(1100, 1, 1000)},
                {"Biff the Buffed Bunny", Tuple.Create(1100, 1, 1000)},
                {"biff's grave", Tuple.Create(1100, 1, 1000)},
                {"Bonegrind The Butcher", Tuple.Create(1100, 1, 1000)},
                {"Servant of the Temple", Tuple.Create(1100, 1, 1000)},
                {"Dreadstump the Pirate King", Tuple.Create(1100, 1, 1000)},
                {"Arachna the Spider Queen", Tuple.Create(1100, 1, 1000)},
                {"Primaeval The Ancient Totem", Tuple.Create(1100, 1, 1000)},
                {"Stheno the Snake Queen", Tuple.Create(1100, 1, 1000)},
                {"Mixcoatl the Masked God", Tuple.Create(1100, 1, 1000)},
                {"Bella the plant", Tuple.Create(1100, 1, 1000)},
                {"Candy Goddess", Tuple.Create(1100, 1, 1000)},
                {"Limon the Sprite God", Tuple.Create(1010, 1, 1000)},
                {"Septavius the Ghost God", Tuple.Create(1010, 1, 1000)},
                {"Davy Jones", Tuple.Create(1100, 1, 1000)},
                {"Lord Ruthven", Tuple.Create(1010, 1, 1000)},
                {"Archdemon Malphas", Tuple.Create(1010, 1, 1000)},
                {"Limon the sprite god", Tuple.Create(1010, 1, 1000)},
                {"Active Sarcophagus", Tuple.Create(1010, 1, 1000)},
                {"Elder Tree", Tuple.Create(1100, 1, 1000)},
                {"Thessal the Mermaid Goddess", Tuple.Create(1010, 1, 1000)},
                {"Dr Terrible", Tuple.Create(1100, 1, 1000)},
                {"Horrific Creation", Tuple.Create(1100, 1, 1000)},
                {"Masked Party God", Tuple.Create(1000, 1, 10000)},
                {"Stone Guardian Left", Tuple.Create(1100, 1, 1000)},
                {"Stone Guardian Right", Tuple.Create(1100, 1, 1000)},
                {"Enraged Puppet Master", Tuple.Create(1100, 1, 1000)},
                {"Oryx the Mad God 1", Tuple.Create(1100, 1, 1000)},
                {"Oryx the Mad God 2", Tuple.Create(1100, 1, 1000)},
            };

        public Entity Quest { get; private set; }

        private static int GetExpGoal(int level)
        {
            return 50 + (level - 1) * 100;
        }


        private static int GetLevelExp(int level)
        {
            if (level == 1) return 0;
            return 50 * (level - 1) + (level - 2) * (level - 1) * 50;
        }

        static int GetFameGoal(int fame)
        {
            if (fame >= 4000) return 0;
            else if (fame >= 2000) return 4000;
            else if (fame >= 800) return 2000;
            else if (fame >= 400) return 800;
            else if (fame >= 150) return 400;
            else if (fame >= 20) return 150;
            else return 20;

            return 0;
        }

        public int GetStars()
        {
            var ret = 0;
            foreach (var i in Client.Account.Stats.ClassStates)
            {
                if (i.BestFame >= 4000) ret += 6;
                else if (i.BestFame >= 2000) ret += 5;
                else if (i.BestFame >= 800) ret += 4;
                else if (i.BestFame >= 400) ret += 3;
                else if (i.BestFame >= 150) ret += 2;
                else if (i.BestFame >= 20) ret += 1;
            }
            return ret;
        }

        private static float Dist(Entity a, Entity b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private Entity FindQuest()
        {
            Entity ret = null;
            try
            {
                float bestScore = 0;
                foreach (var i in Owner.Quests.Values
                    .OrderBy(quest => MathsUtils.DistSqr(quest.X, quest.Y, X, Y)).Where(i => i.ObjectDesc != null && i.ObjectDesc.Quest))
                {
                    Tuple<int, int, int> x;
                    if (!questDat.TryGetValue(i.ObjectDesc.ObjectId, out x)) continue;

                    if ((Level < x.Item2 || Level > x.Item3)) continue;
                    var score = (20 - Math.Abs((i.ObjectDesc.Level ?? 0) - Level)) * x.Item1 -
                                //priority * level diff
                                Dist(this, i) / 100; //minus 1 for every 100 tile distance
                    if (score < 0)
                        score = 1;
                    if (!(score > bestScore)) continue;
                    bestScore = score;
                    ret = i;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return ret;
        }

        private void HandleQuest(RealmTime time)
        {
            if (time.tickCount % 500 != 0 && Quest?.Owner != null) return;
            var newQuest = FindQuest();
            if (newQuest == null || newQuest == Quest) return;
            Owner.Timers.Add(new WorldTimer(100, (w, t) =>
            {
                Client.SendPacket(new QuestObjIdPacket
                {
                    ObjectId = newQuest.Id
                });
            }));
            Quest = newQuest;
        }

        private void CalculateFame()
        {
            int newFame;
            if (Experience < 200 * 1000) newFame = Experience / 1000;
            else newFame = 200 + (Experience - 200 * 1000) / 1000;
            if (newFame == Fame) return;
            Fame = newFame;
            int newGoal;
            var state =
                Client.Account.Stats.ClassStates.SingleOrDefault(_ => Utils.FromString(_.ObjectType) == ObjectType);
            if (state != null && state.BestFame > Fame)
                newGoal = GetFameGoal(state.BestFame);
            else
                newGoal = GetFameGoal(Fame);
            if (newGoal > FameGoal)
            {
                Owner.BroadcastPacket(new NotificationPacket
                {
                    ObjectId = Id,
                    Color = new ARGB(0xFF00FF00),
                    Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"Class Quest Complete!\"}}",
                }, null);
                Stars = GetStars();
            }
            FameGoal = newGoal;
            UpdateCount++;
        }
        public int Adding7()
        {
            if (Level >= 150)
            {
                return 50;
            }
            return 0;
        }
        public int Adding6()
        {
            if (Level >= 150)
            {
                return 2;
            }
            return 0;
        }
        public int Adding2()
        {
            if (Level >= 125)
            {
                return 2;
            }
            return 0;
        }
        public int Adding5()
        {
            if (Level >= 100)
            {
                return 1;
            }
            return 0;
        }
        public int Adding4()
        {
            if (Level >= 75)
            {
                return 1;
            }
            return 0;
        }
        public int Adding3()
        {
            if (Level >= 50)
            {
                return 1;
            }
            return 0;
        }
        public int Adding1()
        {
            if (Level >= 25)
            {
                return 1;
            }
            return 0;
        }

        private bool CheckLevelUp()
        {
            if (Experience - GetLevelExp(Level) >= ExperienceGoal && Level < 150)
            {
                Level++;
                ExperienceGoal = GetExpGoal(Level);
                if (Level < 21)
                {
                    foreach (var i in Manager.GameData.ObjectTypeToElement[ObjectType].Elements("LevelIncrease"))
                    {



                        var rand = new Random();
                        var min = int.Parse(i.Attribute("min").Value);
                        var max = int.Parse(i.Attribute("max").Value) + 1;
                        var xElement = Manager.GameData.ObjectTypeToElement[ObjectType].Element(i.Value);
                        if (xElement == null) continue;
                        var limit =
                            int.Parse(
                                xElement.Attribute("max").Value);
                        var idx = StatsManager.StatsNameToIndex(i.Value);
                        Stats[idx] += rand.Next(min, max);
                        if (Stats[idx] > limit) Stats[idx] = limit;

                    }
                }
                HP = Stats[0] + Boost[0];
                Mp = Stats[1] + Boost[1];

                UpdateCount++;



                if (Level == 25)
                {
                    foreach (var i in Owner.Players.Values)
                        i.SendInfo(Name + " reached level 20 +1 to all stats!");

                    XpBoosted = false;
                    XpBoostTimeLeft = 0;
                }
                else if (Level == 50)
                {
                    foreach (var i in Owner.Players.Values)
                        i.SendInfo(Name + " reached level 50 +1 to all stats!");
                    XpBoosted = false;
                    XpBoostTimeLeft = 0;
                }

                else if (Level == 75)
                {
                    foreach (var i in Owner.Players.Values)
                        i.SendInfo(Name + " reached level 75 +1 to all stats!");
                    XpBoosted = false;
                    XpBoostTimeLeft = 0;
                }

                else if (Level == 100)
                {
                    foreach (var i in Owner.Players.Values)
                        i.SendInfo(Name + " reached level 100 +1 to all stats!");
                    XpBoosted = false;
                    XpBoostTimeLeft = 0;
                }

                else if (Level == 125)
                {
                    foreach (var i in Owner.Players.Values)
                        i.SendInfo(Name + " reached level 125 +2 to all stats!");
                    XpBoosted = false;
                    XpBoostTimeLeft = 0;
                }

                else if (Level == 150)
                {
                    foreach (var i in Owner.Players.Values)
                        i.SendInfo(Name + " reached level 150 +2 to all stats and +50 to Hp and Mp!");
                    XpBoosted = false;
                    XpBoostTimeLeft = 0;
                }

                Quest = null;
                return true;
            }
            CalculateFame();
            return false;
        }

        public bool EnemyKilled(Enemy enemy, int exp, bool killer)
        {
            if (enemy == Quest)
                Owner.BroadcastPacket(new NotificationPacket
                {
                    ObjectId = Id,
                    Color = new ARGB(0xFF00FF00),
                    Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"Quest Complete!\"}}",
                }, null);
            if (exp > 0)
            {
                if (XpBoosted)
                    Experience += exp * 2;
                else
                    Experience += exp;
                UpdateCount++;
                foreach (var i in Owner.PlayersCollision.HitTest(X, Y, 16).Where(i => i != this).OfType<Player>())
                {
                    try
                    {
                        i.Experience += i.XpBoosted ? exp * 2 : exp;
                        i.UpdateCount++;
                        i.CheckLevelUp();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }
            FameCounter.Killed(enemy, killer);
            return CheckLevelUp();
        }
    }
}