using Rondo.Generic.Utility;
using Rondo.QuestSim.General;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.Reputation {

    public class ReputationHeroInstanceUI : MonoBehaviour {

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI classText;
        public Image statusIcon;
        public RectTransform levelProgressFill;

        public HeroInstance Hero { get; private set; }

        private Coroutine m_UpdateExperienceRoutine = null;
        private float m_CurrentExperience = 0;
        private CanvasGroup m_CanvasGroup;
        private Coroutine m_UpdateAlphaRoutine = null;

        void Awake() {
            m_CanvasGroup = GetComponent<CanvasGroup>();
            ApplyHero(null);
        }

        void OnDestroy() {
            if (Hero == null) return;
            Hero.OnExperienceChange -= UpdateProgressSmooth;
            Hero.OnStateChange -= UpdateHeroStatus;
        }

        public void ApplyHero(HeroInstance hero) {
            if(Hero != null) {
                Hero.OnExperienceChange -= UpdateProgressSmooth;
                Hero.OnStateChange -= UpdateHeroStatus;
            }

            Hero = hero;

            if(Hero != null) {
                Hero.OnExperienceChange += UpdateProgressSmooth;
                Hero.OnStateChange += UpdateHeroStatus;
            }

            UpdateProgressInstant();
            UpdateHeroStatus();
        }

        public void UpdateHeroStatus() {
            statusIcon.sprite = SpriteFetcher.PlayerIcons.GetSpriteForStatus(Hero != null ? Hero.HeroState : HeroStates.UNDISCOVERED);
        }

        private void UpdateProgressInstant() {
            if(Hero != null) {
                //nameText.text = Hero.DisplayName;
                SetExperience(Hero.Experience);
            } else {
                //nameText.text = "-";
                //classText.text = "-";
                levelProgressFill.localScale = new Vector3(0, levelProgressFill.localScale.y, levelProgressFill.localScale.z);
            }
        }

        private void UpdateProgressSmooth() {
            if (!gameObject.activeInHierarchy) return;
            if (m_UpdateExperienceRoutine != null) StopCoroutine(m_UpdateExperienceRoutine);
            m_UpdateExperienceRoutine = StartCoroutine(SmoothExperienceUpdate(Hero.Experience));
        }

        private void SetExperience(float experience) {
            m_CurrentExperience = experience;

            int level;
            int expForNextLevel;
            float levelProgress;
            HeroUtility.CalculateHeroLevel(m_CurrentExperience, out level, out expForNextLevel, out levelProgress);

            //classText.text = Hero.GetClassProgress(level);
            levelProgressFill.localScale = new Vector3(levelProgress, levelProgressFill.localScale.y, levelProgressFill.localScale.z);
        }

        private IEnumerator SmoothExperienceUpdate(float targetExp) {
            while(Mathf.Abs(targetExp - m_CurrentExperience) >= 0.01f) {
                m_CurrentExperience = Mathf.Lerp(m_CurrentExperience, targetExp, 0.1f);
                SetExperience(m_CurrentExperience);
                yield return null;
            }
            m_CurrentExperience = targetExp;
            SetExperience(Mathf.RoundToInt(m_CurrentExperience));
            StopCoroutine(m_UpdateExperienceRoutine);
            m_UpdateExperienceRoutine = null;
            yield return null;
        }

        public void SetAlpha(float alpha) {
            m_CanvasGroup.alpha = alpha;
        }

        public bool IsAvailable() {
            return m_CanvasGroup.alpha == 1;
        }
    }

}