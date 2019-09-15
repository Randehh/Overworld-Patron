using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests.Rewards {

    public class QuestRewardFaction : IQuestReward {

        public QuestSourceFaction Faction { get; private set; }
        public float RewardValue { get { return (Faction.QuestDifficulty + 1) * 20; } }
        public string DisplayValue { get { return (Faction.AverageQuestDifficulty) + " Star faction - "+  Faction.DisplayName; } }

        public QuestRewardFaction(int level) {
            QuestSourceFaction newFaction = new QuestSourceFaction(ReputationBiases.UNKNOWN);
            newFaction.initialHeroLevel = level;
            newFaction = ReputationGenerator.GenerateReputationInstance(newFaction, ReputationBiases.UNKNOWN);
            Faction = newFaction;
        }

        public void ApplyReward(HeroInstance hero) {
            ReputationManager.AddFaction(Faction);
        }
    }

}