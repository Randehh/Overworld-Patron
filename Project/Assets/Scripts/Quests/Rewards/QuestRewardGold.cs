using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests.Rewards {

    public class QuestRewardGold : IQuestReward {
        public int GoldCount { get; set; }
        public float RewardValue { get { return GoldCount; } }
        public string DisplayValue { get { return "" + RewardValue; } }

        public void ApplyReward(HeroInstance hero) {
            
        }
    }

}