using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests.Sources {

    public class QuestSourcePerson : IQuestSource {

        public QuestSourcePerson(ReputationBiases personality) {
            personalityType = personality;
        }

        //Display options
        public string DisplayName { get; set; }
        public string RequestTitle { get { return "Personal request from " + DisplayName; } }

        public int QuestDifficulty { get { return Random.Range(0, 11); } }

        public ReputationBiases personalityType = ReputationBiases.VILLAGERS;

        public void GenerateSettings() {
            ReputationGenerator.GenerateName(this, ReputationNameConventions.COMPOUND);
        }
    }
}