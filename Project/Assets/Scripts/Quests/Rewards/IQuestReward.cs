using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests.Rewards {

    public interface IQuestReward {
        float RewardValue { get; }
        string DisplayValue { get; }

        void ApplyReward(HeroInstance hero);
    }

}