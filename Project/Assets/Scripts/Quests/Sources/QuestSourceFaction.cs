using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests.Sources {

    public class QuestSourceFaction : IQuestSource {

        private static int MIN_HEROES_PER_FACTION = 2;
        private static int MAX_HEROES_PER_FACTION = 6;

        public QuestSourceFaction(ReputationBiases personality) {
            personalityType = personality;
            Heroes = new List<HeroInstance>();
        }

        //Display options
        public string DisplayName { get; set; }
        public string RequestTitle { get { return "Request from " + DisplayName; } }

        public Sprite displayEmblem;
		public Color FactionColor { get; set; }

        //Quest preferences
        public float questPreferenceMonsterSlaying = 0.5f;
        public float questPreferenceDelivery = 0.5f;
        public float questPreferenceWar = 0.5f;
        public float questPreferenceChores = 0.5f;

        public int QuestDifficulty { get { return GetQuestDifficulty(); } }
        public int AverageQuestDifficulty { get { return GetAverageQuestDifficulty(); } }
        public int AverageHeroLevel { get { return GetAverageHeroLevel(); } }

        public ReputationBiases personalityType = ReputationBiases.VILLAGERS;
        private ReputationNameConventions m_NamingConvention;

        //Heroes
        public List<HeroInstance> Heroes { get; set; }

        public int initialHeroLevel = 1;

        public void GenerateSettings() {
            ReputationGenerator.GenerateQuestPreferences(this, personalityType);
            ReputationGenerator.GenerateName(this, ReputationNameConventions.GROUP);

            for(int i = 0; i < Random.Range(MIN_HEROES_PER_FACTION, MAX_HEROES_PER_FACTION + 1); i++) {
                Heroes.Add(HeroGenerator.GenerateHero(this, Mathf.Clamp(initialHeroLevel + Random.Range(-1, 1), 1, 100), true));
            }

			FactionColor = new Color(
				Random.Range(0, 1f),
				Random.Range(0, 1f),
				Random.Range(0, 1f));
		}

        private int GetAverageHeroLevel() {
            int totalHeroLevel = 0;

            foreach (HeroInstance hero in Heroes) {
                totalHeroLevel += hero.Level;
            }

            totalHeroLevel /= Heroes.Count;
            return totalHeroLevel;
        }

        private int GetAverageQuestDifficulty() {
            float totalQuestDifficulty = 0;

            foreach(HeroInstance hero in Heroes) {
                totalQuestDifficulty += hero.QuestPrefDifficultyFloat;
            }

            totalQuestDifficulty /= Heroes.Count;
            return Mathf.FloorToInt(totalQuestDifficulty);
        }

        private int GetQuestDifficulty() {
            return Mathf.Clamp(GetAverageQuestDifficulty() + Random.Range(-1, 1), 0, 10);
        }
    }
}