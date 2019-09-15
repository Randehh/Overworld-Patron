using Rondo.Generic.Utility;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests.Rewards;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests {

    public static class QuestGenerator {

        private static WeightedRandom<QuestSourceTypes> m_QuestSourceChoser = new WeightedRandom<QuestSourceTypes>(
            new QuestSourceTypes[3] { QuestSourceTypes.RUMOR, QuestSourceTypes.FACTION, QuestSourceTypes.PERSON },
            new int[3] { 1, 2, 1 });

        private static WeightedRandom<int> m_QuestDurationChoser = new WeightedRandom<int>(
            new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
            new int[10] { 10, 8, 6, 4, 3, 2, 2, 1, 1, 1 });

        private static WeightedRandom<int> m_PartySizeChoser = new WeightedRandom<int>(
            new int[3] { 1, 2, 3 },
            new int[3] { 20, 8, 2 });

        public static int daysSinceHeroRecruit = Random.Range(10, 20);
        public static int daysSinceFactionRecruit = Random.Range(25, 35);

        public static QuestInstance GenerateQuestInstance() {
            return GenerateQuestInstance(m_QuestSourceChoser.GetRandomValue());
        }

        public static QuestInstance GenerateQuestInstance(QuestSourceTypes forcedType) {
            int questDuration = m_QuestDurationChoser.GetRandomValue();
            int itemRewardChance;

            IQuestReward additionalReward = null;
            IQuestSource qSource;

            switch (forcedType) {
                default:
                case QuestSourceTypes.FACTION:
                    qSource = ReputationManager.GetRandomFaction();
                    QuestSourceFaction factionSource = qSource as QuestSourceFaction;
                    itemRewardChance = 25;

                    if (Random.Range(0, daysSinceHeroRecruit) == 0) {
                        additionalReward = new QuestRewardHero(factionSource);
                        daysSinceHeroRecruit = Random.Range(10, 20);
                    } else if (Random.Range(0, daysSinceFactionRecruit) == 0) {
                        additionalReward = new QuestRewardFaction(factionSource.AverageHeroLevel + Random.Range(3, 6));
                        daysSinceFactionRecruit = Random.Range(25, 35);
                    }

                    daysSinceHeroRecruit--;
                    daysSinceFactionRecruit--;
                    break;

                case QuestSourceTypes.PERSON:
                    qSource = ReputationGenerator.GenerateReputationInstance(new QuestSourcePerson(EnumUtility.GetRandomEnumValue<ReputationBiases>()));
                    itemRewardChance = 5;
                    break;

                case QuestSourceTypes.RUMOR:
                    qSource = ReputationGenerator.GenerateReputationInstance(new QuestSourceRumor());
                    itemRewardChance = 15;
                    break;

            }

            QuestInstance quest = GenerateQuestInstance(qSource, questDuration);
            quest.AdditionalReward = additionalReward;
            quest.PartySize = m_PartySizeChoser.GetRandomValue();

            if (Random.Range(0, itemRewardChance) == 0) {
                GameItem itemReward = GameItemGenerator.GenerateItem(GameItemTypes.UNKNOWN, GetItemRarityForDifficulty(quest.DifficultyLevel));
                quest.HandlerItemReward = new QuestRewardItem(itemReward);
            }

            return quest;
        }

        public static QuestInstance GenerateQuestInstance(IQuestSource questSource, int questDuration = 1) {
            QuestInstance quest = new QuestInstance(questSource);
            quest.DurationInDays = questDuration;
            quest.DifficultyLevel = questSource.QuestDifficulty;
            quest.QuestType = EnumUtility.GetRandomEnumValue<QuestTypes>();
            return quest;
        }

        private static GameItemRarity GetItemRarityForDifficulty(int difficulty) {
            return (GameItemRarity) Mathf.Clamp(Mathf.RoundToInt(difficulty / 2f), 0, (int)GameItemRarity.LEGENDARY);
        }

    }

}