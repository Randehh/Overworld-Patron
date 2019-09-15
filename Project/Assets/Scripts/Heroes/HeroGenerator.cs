using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.UI.Reputation;
using Rondo.QuestSim.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Heroes {

    public static class HeroGenerator {

        public static HeroInstance GenerateHero(QuestSourceFaction faction, int level, bool addToRoster) {
            HeroInstance newHero = new HeroInstance();

            newHero.DisplayName = NameDatabase.GetHeroName();
            //newHero.Nickname = UnityEngine.Random.Range(0, 4) == 0 ? NameDatabase.GetCompoundName() : "";
            newHero.Class = EnumUtility.GetRandomEnumValue<HeroClasses>();
            newHero.Experience = HeroUtility.GetTotalExperienceRequiredForLevel(level);
            newHero.EquipmentLevel = UnityEngine.Random.Range(1, 10);
            newHero.HeroState = HeroStates.IDLE;

            Dictionary<QuestTypes, float> tempQuestValues = new Dictionary<QuestTypes, float>();
            float tempQuestValuesTotal = 0;
            foreach (QuestTypes questType in Enum.GetValues(typeof(QuestTypes))) {
                tempQuestValues.Add(questType, UnityEngine.Random.Range(0f, 10f));
                tempQuestValuesTotal = tempQuestValues[questType];
            }
            foreach (QuestTypes questType in Enum.GetValues(typeof(QuestTypes))) {
                newHero.QuestTypePreferences[questType] = (tempQuestValues[questType] / tempQuestValuesTotal).Map(0, 1, 0.6f, 1.25f);
            }

            float offset = 0.25f;
            newHero.QuestPrefRewardGold = UnityEngine.Random.Range(0f, 1f);
            newHero.QuestPrefRewardItem = 1 - newHero.QuestPrefRewardGold;
            newHero.QuestPrefRewardGold += offset;
            newHero.QuestPrefRewardItem += offset;


            if (addToRoster) {
                HeroManager.AddHero(newHero, faction);
            }

            return newHero;
        }

    }

}