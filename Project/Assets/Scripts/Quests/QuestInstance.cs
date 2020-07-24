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
        public string QuestTypeDisplay { get { return QuestType.ToString().Replace('_', ' ').ToLower(); } }
        public string QuestTitle { get { return string.Format(QuestSource.RequestTitle, QuestTypeDisplay).ToUpperCaseFirstCharacter(); } }
        public float DifficultyLevel { get; set; }
        public QuestRewardGold[] GoldRewards { get; set; }
        public QuestRewardItem[] ItemRewards { get; set; }
        public IQuestReward AdditionalReward { get; set; }
        public QuestRewardItem HandlerItemReward { get; set; }
        public int DaysLeftOnPost { get; set; }
        public int DaysLeftOnQuest { get { return m_DaysLeftOnQuest; } set { m_DaysLeftOnQuest = value; OnDaysLeftUpdate(); } }
        public string HandlerGoldRewardEstimate { get { return Mathf.RoundToInt(HandlerAverageExpectedGoldReward * HANDLER_GOLD_VARIANCE_MIN) + " - " + Mathf.RoundToInt(HandlerAverageExpectedGoldReward * HANDLER_GOLD_VARIANCE_MAX); } }
        public float AveragePowerLevel { get { return ((DifficultyLevel * QuestConstants.LEVELS_PER_STAR + 1) * QuestConstants.BASE_POWER_LEVEL); } }

        public Action OnDaysLeftUpdate = delegate { };

        private int AverageExpectedGoldReward { get { return Mathf.RoundToInt((DifficultyLevel + 1) * 15 * (DurationInDays * 0.25f)); } }
        private float AverageExpectedItemReward { get { return (DifficultyLevel + 1) * 20 * (DurationInDays * 0.5f); } }
        private int ExperiencePoints { get { return Mathf.RoundToInt(DifficultyLevel + 1) * 5 * DurationInDays; } }
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

            DaysLeftOnPost = UnityEngine.Random.Range(1, 4);

            DisplayName = "Quest name";
        }

        public bool WouldHeroAccept(HeroInstance hero, int heroNumber) {
            if (hero.HeroState != HeroStates.IDLE) return false;

            float preferenceValue = 0;

            preferenceValue += (hero.QuestPrefRewardGold / AverageExpectedGoldReward) * GoldRewards[heroNumber].RewardValue;
            preferenceValue += (hero.QuestPrefRewardItem / AverageExpectedItemReward) * GetTotalItemRewardValue(heroNumber);

            float maxDifficultyDifference = QuestConstants.GetPowerLevel(3);
            float difficultyScaler = (maxDifficultyDifference - (QuestConstants.GetPowerLevel(hero.QuestPrefDifficulty) - QuestConstants.GetPowerLevel(DifficultyLevel))) / maxDifficultyDifference;
            preferenceValue *= difficultyScaler;

            float powerLevelScaler = (float)hero.PowerLevel / Mathf.Clamp(AveragePowerLevel, 1, float.MaxValue);
            preferenceValue *= powerLevelScaler;

            return preferenceValue > 0.5f;
        }

        public int GetTotalSuccessRate(HeroInstance[] heroes) {
            int count = 0;
            int rate = 0;
            Dictionary<IQuestSource, int> heroesPerFaction = new Dictionary<IQuestSource, int>();
            foreach (HeroInstance hero in heroes) {
                if (hero == null) continue;
                int value = GetHeroSuccessRate(hero) / PartySize;
                rate += value;
                count++;

				if (heroesPerFaction.ContainsKey(hero.Faction)) {
                    heroesPerFaction[hero.Faction] = heroesPerFaction[hero.Faction] + 1;
				} else {
                    heroesPerFaction.Add(hero.Faction, 1);
				}
            }

            foreach (int factionCount in heroesPerFaction.Values) {
                rate += (factionCount - 1) * 10;
            }
            return Mathf.Clamp(rate, 0, 100);
        }

        public int GetHeroSuccessRate(HeroInstance hero) {
            float successChance = 85 * (1 - ((float)DifficultyLevel).Map(0, 10, 0, 1));

            //Star difference
            float starDiff = hero.QuestPrefDifficulty - DifficultyLevel;
            successChance += (starDiff * 10);

            float heroPowerDiff = hero.PowerLevel - hero.BasePowerLevel;
            successChance += (heroPowerDiff / hero.Level) * 0.1f;

            successChance += hero.Class.GetSuccessRateModifier(QuestType);

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
                Debug.Log("Success rates: " + failChance + " / " + successRate);
                if (failChance > successRate) {
                    if (failChance > successRate + 20) {
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

                    hero.Experience += Mathf.CeilToInt(ExperiencePoints * hero.Class.GetExperienceModifier(QuestType));
                    QuestSourceFaction faction = HeroManager.GetHeroFaction(hero);
                    ReputationManager.GetReputationTracker(faction).ModifyReputation(ExperiencePoints * 0.01f);
                }

                HeroManager.SetHeroToState(hero, HeroStates.IDLE);
            }

            if(successHeroes == 0) {
                RefundQuestRewards(true, true);
                return false;
            }else {
                if (HandlerItemReward != null) InventoryManager.OwnedItems.Add(HandlerItemReward.Item);
                InventoryManager.ModifyGold(Mathf.RoundToInt(HandlerAverageExpectedGoldReward * UnityEngine.Random.Range(HANDLER_GOLD_VARIANCE_MIN, HANDLER_GOLD_VARIANCE_MAX)), "Quest completed");
                InventoryManager.Stars += Mathf.RoundToInt(DifficultyLevel);
                return true;
            }
        }

        public void RefundQuestRewards(bool refundGold, bool refundItem) {
            if (refundGold) {
                foreach (QuestRewardGold goldReward in GoldRewards) {
                    InventoryManager.ModifyGold(goldReward.GoldCount, "Quest refunded");
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