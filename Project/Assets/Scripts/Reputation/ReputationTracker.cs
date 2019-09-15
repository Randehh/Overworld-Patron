using Rondo.QuestSim.Quests.Sources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Reputation {

    public class ReputationTracker {

        public QuestSourceFaction FactionInstance { get; private set; }
        public ReputationLevels ReputationLevel { get; private set; }
        public float ReputationScore {
            get { return m_ReputationScore; }
            private set {
                m_ReputationScore = value;
                if(m_NextReputationLevel == ReputationLevel) {
                    ReputationLevelProgress = 1;
                } else {
                    float nextLevelDelta = ((float)m_NextReputationLevel - (float)ReputationLevel);
                    float currentLevelProgress = (float)m_NextReputationLevel - ReputationScore;
                    ReputationLevelProgress = 1 - (currentLevelProgress / nextLevelDelta);
                }
                OnReputationChange();
            }
        }
        public float ReputationLevelProgress { get; private set; }
        public Action OnReputationChange = delegate { };

        private ReputationLevels m_NextReputationLevel;
        private float m_ReputationScore = 0;

        public ReputationTracker(QuestSourceFaction instance) {
            FactionInstance = instance;
        }

        public void ModifyReputation(float i) {
            float newScore = Mathf.Clamp(ReputationScore + i, 0, 10);

            //Modify the rep level
            ReputationLevels nextReputationLevel = ReputationLevels.STRANGER;
            foreach(ReputationLevels testLevel in Enum.GetValues(typeof(ReputationLevels))) {
                if ((float)testLevel > newScore) {
                    m_NextReputationLevel = testLevel;
                    break;
                }

                if(testLevel == ReputationLevels.EXALTED) {
                    m_NextReputationLevel = testLevel;
                }

                nextReputationLevel = testLevel;
            }

            ReputationLevel = nextReputationLevel;
            ReputationScore = newScore;
        }
    }

}