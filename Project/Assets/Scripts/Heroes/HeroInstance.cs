using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Quests.Sources;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Heroes {

    public class HeroInstance {

        public static int BASE_POWER_PER_LEVEL = 25;

        public string DisplayName { get { return GetDisplayName(); } set { m_DisplayName = value; } }
        public string Nickname { get; set; }
        public HeroClasses Class { get; set; }
        public int Experience { get { return HeroState != HeroStates.UNDISCOVERED ? m_Experience : 0; } set { m_Experience = value; OnExperienceChange(); } }
        public int ExperienceReqForNextLevel { get { return HeroState != HeroStates.UNDISCOVERED ? m_ExperienceForNextLevel : 0; } set { m_ExperienceForNextLevel = value; } }
        public int EquipmentLevel { get; set; }
        public QuestSourceFaction Faction { get { return HeroManager.GetHeroFaction(this); } }
        public HeroStates HeroState { get { return m_HeroState; } set { m_HeroState = value; OnStateChange(); } }

        public int Level { get { return m_Level; } }
        public float LevelProgress { get { return HeroState != HeroStates.UNDISCOVERED ? m_LevelProgress : 0; } }
        public float PowerLevel { get { return (EquipmentLevel * 0.1f) + BasePowerLevel; } }
        public int BasePowerLevel { get { return Level * BASE_POWER_PER_LEVEL; } }

        public float QuestPrefDifficulty { get { return Level / QuestConstants.LEVELS_PER_STAR; } }
        public float QuestPrefRewardItem { get; set; }
        public float QuestPrefRewardGold { get; set; }

        public int WoundedDays { get; set; }

        public Action OnExperienceChange = delegate { };
        public Action OnStateChange = delegate { };

        private string m_DisplayName;
        private int m_Experience = 0;
        private int m_ExperienceForNextLevel = 0;
        private int m_Level = 1;
        private float m_LevelProgress = 0;
        private HeroStates m_HeroState = HeroStates.IDLE;

        public HeroInstance() {
            HeroState = HeroStates.UNDISCOVERED;

            OnExperienceChange += CalculateLevels;
        }

        private string GetDisplayName() {
            if (HeroState != HeroStates.UNDISCOVERED) {
                if (string.IsNullOrEmpty(Nickname)) {
                    return m_DisplayName;
                } else {
                    return m_DisplayName.Replace(" ", " \"" + Nickname + "\" ");
                }
            } else {
                return "???";
            }
        }

        public string GetClassProgress(int overrideLevel = -1){
            if (overrideLevel == -1) overrideLevel = Level;
            return HeroState != HeroStates.UNDISCOVERED ? ("Lv" + overrideLevel + " " + Class.ToString().ToCamelCase()) : "???";
        }

        private void CalculateLevels() {
            HeroUtility.CalculateHeroLevel(m_Experience, out m_Level, out m_ExperienceForNextLevel, out m_LevelProgress);
        }

    }

}