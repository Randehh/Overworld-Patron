using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Reputation;
using Rondo.QuestSim.UI.PostedQuests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.ActiveQuests {

    public class QuestInstanceUI : MonoBehaviour {

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI difficultyText;
        public TextMeshProUGUI daysLeftText;

        private Button m_Button;
        private QuestInstance m_QuestInstance;
        private QuestDetailsWindowMode m_QuestMode;

        void Awake() {
            m_Button = GetComponent<Button>();
        }

        void Start() {
            m_Button.onClick.AddListener(OpenQuestWindow);
        }

        private void OnDestroy() {
            m_QuestInstance.OnDaysLeftUpdate -= UpdateText;
        }

        private void OpenQuestWindow() {
            QuestDetailsWindow.Instance.OpenWindow(m_QuestInstance, m_QuestMode);
        }

        public void ApplyQuestChain(QuestInstance chain) {
            m_QuestInstance = chain;

            if (QuestManager.PostedQuests.Contains(m_QuestInstance)) {
                m_QuestMode = QuestDetailsWindowMode.POSTED_REVIEW;
            } else if (QuestManager.Requests.Contains(m_QuestInstance)) {
                m_QuestMode = QuestDetailsWindowMode.SETUP;
            } else {
                m_QuestMode = QuestDetailsWindowMode.ACTIVE_REVIEW;
            }

            m_QuestInstance.OnDaysLeftUpdate += UpdateText;

            UpdateText();
        }

        private void UpdateText() {
            nameText.text = m_QuestInstance.QuestTitle;
            difficultyText.text = "" + m_QuestInstance.DifficultyLevel;

            string daysToDisplay = "";
			switch (m_QuestMode) {
                case QuestDetailsWindowMode.SETUP:
                case QuestDetailsWindowMode.HERO_SELECT:
                case QuestDetailsWindowMode.POSTED_REVIEW:
                    daysToDisplay += m_QuestInstance.DaysLeftOnPost;
                    break;

                case QuestDetailsWindowMode.COMPLETED:
                    daysToDisplay += "-";
                    break;

                case QuestDetailsWindowMode.ACTIVE_REVIEW:
                    daysToDisplay += m_QuestInstance.DaysLeftOnQuest;
                    break;

            }
            daysLeftText.text = daysToDisplay;
        }
    }

}