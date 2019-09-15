using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests.Sources {

    public class QuestSourceRumor : IQuestSource {

        public QuestSourceRumor() {

        }

        //Display options
        public string DisplayName { get; set; }
        public string RequestTitle { get { return "Rumor near " + DisplayName; } }

        public int QuestDifficulty { get { return Random.Range(2, 11); } }

        public void GenerateSettings() {
            ReputationGenerator.GenerateName(this, ReputationNameConventions.POINT_OF_INTEREST, ReputationNameConventions.TERRITORY);
        }
    }
}