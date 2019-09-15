using Rondo.Generic.Utility;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests.Rewards;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Reputation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests {

    public class QuestInstance {

        public static int MAX_HEROES_PER_QUEST = 3;
        private static float HANDLER_GOLD_VARIANCE_MIN = 0.8f;
        private static float HANDLER_GOLD_VARIANCE_MAX = 1.2f;

        public string DisplayName { get; set; }
        public int PartySize { get; set; }
        public int DurationInDays { get { return m_DurationInDays; } set { m_DurationInDays = value; DaysLeftOnQuest = value; } }
        public IQuestSource QuestSource { get; private set; }
        public QuestTypes QuestType { get; set; }
        public string QuestTypeDisplay { get { return QuestType.ToString().Replace('_', ' '); } }
        public int DifficultyLevel { get; set; }
        public QuestRewardGold[] GoldRewards { get; set; }
        public QuestRewardItem[] ItemRewards { get; set; }
        public IQuestReward AdditionalReward { get; set; }
        public QuestRewardItem HandlerItemReward { get; set; }
        public int DaysLeftOnPost { get; set; }
        public int DaysLeftOnQuest { get { return m_DaysLeftOnQuest; } set { m_DaysLeftOnQuest = value; OnDaysLeftUpdate(); } }
        public string HandlerGoldRewardEstimate { get { return Mathf.RoundToInt(HandlerAverageExpectedGoldReward * HANDLER_GOLD_VARIANCE_MIN) + " - " + Mathf.RoundToInt(HandlerAverageExpectedGoldReward * HANDLER_GOLD_VARIANCE_MAX); } }
        public int AveragePowerLevel { get { return ((DifficultyLevel * HeroInstance.LEVELS_PER_QUEST_STAR + 1) * HeroInstance.BASE_POWER_PER_LEVEL); } }

        public Action OnDaysLeftUpdate = delegate { };

        private int AverageExpectedGoldReward { get { return Mathf.RoundToInt((DifficultyLevel + 1) * 15 * (DurationInDays * 0.25f)); } }
        private float AverageExpectedItemReward { get { return (DifficultyLevel + 1) * 20 * (DurationInDays * 0.5f); } }
        private int ExperiencePoints { get { return (DifficultyLevel + 1) * 5 * DurationInDays; } }
        private int HandlerAverageExpectedGoldReward { get { return Mathf.RoundToInt(AverageExpectedGoldReward * 2 * (HandlerItemReward == null ? 1 : 0.5f)) * PartySize; } }

        private int m_DurationInDays;
        private int m_DaysLeftOnQuest;

        public QuestInstance(IQuestSource source) {
            QuestSource = source;
            ItemRewards = new QuestRewardItem[MAX_HEROES_PER_QUEST];
            GoldRewards = new QuestRewardGold[MAX_HEROES_PER_QUEST];

            for (int i = 0; i < GoldRewards.Length; i++) {
                GoldRewards[i] = new QuestRewardGold();
            }

            DaysLeftOnPost = 5;

            DisplayName = "Quest name";
        }

        public bool WouldHeroAccept(HeroInstance hero, int heroNumber) {
            if (hero.HeroState != HeroStates.IDLE) return false;

            float preferenceValue = 0;

            preferenceValue += (hero.QuestPrefRewardGold / AverageExpectedGoldReward) * (GoldRewards[heroNumber].RewardValue);
            preferenceValue += (hero.QuestPrefRewardItem / AverageExpectedItemReward) * (GetTotalItemRewardValue(heroNumber));

            float maxDifficultyDifference = 3;
            float difficultyScaler = (maxDifficultyDifference - Mathf.Abs(hero.QuestPrefDifficulty - DifficultyLevel)) / maxDifficultyDifference;
            preferenceValue *= difficultyScaler;

            float powerLevelScaler = (float)hero.PowerLevel / Mathf.Clamp(AveragePowerLevel, 1, float.MaxValue);
            preferenceValue *= powerLevelScaler;

            preferenceValue *= hero.QuestTypePreferences[QuestType];

            return preferenceValue > 0.7f;
        }

        public int GetTotalSuccessRate(HeroInstance[] heroes) {
            int count = 0;
            int rate = 0;
            foreach (HeroInstance hero in heroes) {
                if (hero == null) continue;
                int value = GetHeroSuccessRate(hero) / PartySize;
                rate += value;
                count++;
            }
            return rate;
        }

        public int GetHeroSuccessRate(HeroInstance hero) {
            float successChance = 95 * (1 - ((float)DifficultyLevel).Map(0, 10, 0, 1));

            //Star difference
            float starDiff = hero.QuestPrefDifficultyFloat - DifficultyLevel;
            successChance += (starDiff * 10);

            float heroPowerDiff = hero.PowerLevel - hero.BasePowerLevel;
            successChance += (heroPowerDiff / hero.Level) * 0.1f;

            successChance *= hero.Class.GetQuestModifier(QuestType);

            return Mathf.Clamp(Mathf.RoundToInt(successChance), 0, 100);
        }

        private float GetTotalItemRewardValue(int heroNumber) {
            return ItemRewards[heroNumber] != null ? ItemRewards[heroNumber].RewardValue : 0;
        }

        public bool CompleteQuest(HeroInstance[] heros) {

            //Check if the hero completed it or not
            int successRate = GetTotalSuccessRate(heros);

            int totalHeroes = 0;
            int successHeroes = 0;

            for(int i = 0; i < heros.Length; i++) {
                HeroInstance hero = heros[i];
                if (hero == null) continue;
                totalHeroes++;

                bool giveRewards = true;
                int failChance = UnityEngine.Random.Range(0, 101);
                if (failChance > successRate) {
                    failChance = UnityEngine.Random.Range(0, 101);
                    if (failChance < 100 - successRate + 20) {
                        HeroManager.SetHeroToState(hero, HeroStates.DEAD);
                        giveRewards = false;
                    } else {
                        HeroManager.SetHeroToState(hero, HeroStates.WOUNDED);
                        hero.WoundedDays = ((100 - successRate) / 10) + 4;
                        successHeroes++;
                    }
                } else {
                    successHeroes++;
                }

                if (giveRewards) {
                    if (ItemRewards[i] != null) ItemRewards[i].ApplyReward(hero);
                    if (AdditionalReward != null && i == 0) AdditionalReward.ApplyReward(hero);

                    hero.Experience += ExperiencePoints;
                    QuestSourceFaction faction = HeroManager.GetHeroFaction(hero);
                    ReputationManager.GetReputationTracker(faction).ModifyReputation(ExperiencePoints * 0.1f);
                }

                HeroManager.SetHeroToState(hero, HeroStates.IDLE);
            }

            if(successHeroes == 0) {
                RefundQuestRewards(true, true);
                return false;
            }else {
                if (HandlerItemReward != null) InventoryManager.OwnedItems.Add(HandlerItemReward.Item);
                InventoryManager.Gold += Mathf.RoundToInt(HandlerAverageExpectedGoldReward * UnityEngine.Random.Range(HANDLER_GOLD_VARIANCE_MIN, HANDLER_GOLD_VARIANCE_MAX));
                InventoryManager.Stars += DifficultyLevel;
                return true;
            }
        }

        public void RefundQuestRewards(bool refundGold, bool refundItem) {
            if (refundGold) {
                foreach (QuestRewardGold goldReward in GoldRewards) {
                    InventoryManager.Gold += goldReward.GoldCount;
                }
            }

            if (refundItem) {
                foreach (QuestRewardItem itemReward in ItemRewards) {
                    if (itemReward == null) continue;
                    InventoryManager.MoveItemToOwned(itemReward.Item);
                }
            }
        }

        public int GetTotalGoldCount() {
            int count = 0;
            foreach(QuestRewardGold goldReward in GoldRewards) {
                count += goldReward.GoldCount;
            }
            return count;
        }
    }

}