using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rondo.QuestSim.UI.Reputation {

    public class ReputationInstanceUI : MonoBehaviour {

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI reputationLevelText;
        public TextMeshProUGUI reputationProgressText;
        public RectTransform reputationProgressFill;
        public RectTransform heroContentParent;

        private ReputationTracker m_Tracker;

        private void OnDestroy() {
            m_Tracker.OnReputationChange -= UpdateProgress;
        }

        public void AddPoints() {
            m_Tracker.ModifyReputation(0.5f);
        }

        public void ApplyReputation(ReputationTracker tracker) {
            //nameText.text = tracker.FactionInstance.DisplayName;
            m_Tracker = tracker;
            UpdateProgress();

            m_Tracker.OnReputationChange += UpdateProgress;
        }

        private void UpdateProgress() {
            //reputationLevelText.text = m_Tracker.ReputationLevel.ToString();
            float levelProgress = m_Tracker.ReputationLevelProgress;
            string levelProgressText = (levelProgress * 100).ToString();
            if (levelProgressText.Contains(".")) levelProgressText = levelProgressText.Split('.')[0];
            //reputationProgressText.text = levelProgressText + "%";
            reputationProgressFill.localScale = new Vector3(levelProgress, reputationProgressFill.localScale.y, reputationProgressFill.localScale.z);
        }

        public ReputationHeroInstanceUI AddHero(ReputationHeroInstanceUI uiPrefab, HeroInstance hero) {
            ReputationHeroInstanceUI heroUI = Instantiate(uiPrefab);
            heroUI.GetComponent<RectTransform>().SetParent(heroContentParent);

            heroUI.ApplyHero(hero);
            return heroUI;
        }
    }

}