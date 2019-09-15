using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests.Rewards {

    public class QuestRewardHero : IQuestReward {

        public HeroInstance Hero { get; private set; }
        public float RewardValue { get { return Hero.PowerLevel; } }
        public string DisplayValue { get { return "Lv" + Hero.Level + " Hero - "+  Hero.DisplayName; } }

        public QuestRewardHero(QuestSourceFaction faction) {
            Hero = HeroGenerator.GenerateHero(faction, Mathf.Clamp(faction.AverageHeroLevel + Random.Range(-5, 5), 1, 100), false);
        }

        public void ApplyReward(HeroInstance hero) {
            HeroManager.AddHero(Hero, hero.Faction);
        }
    }

}