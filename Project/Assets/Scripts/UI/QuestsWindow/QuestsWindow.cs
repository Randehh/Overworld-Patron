using Rondo.Generic.Utility;
using Rondo.Generic.Utility.UI;
using Rondo.QuestSim.Gameplay;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.ActiveQuests {

    public class QuestsWindow : MonoBehaviourSingleton<QuestsWindow> {

        private static string HIGHLIGHT_GROUP_ID = "quest_list";

        public QuestInstanceUI instancePrefab;
        public RectTransform postedQuestsParent;
        public RectTransform activeQuestsParent;
        public RectTransform requestQuestsParent;
        public Button openCloseToggle;

        public Color questTurnColor1;
        public Color requestTimeColor;

        private RectTransform m_RectTransform;

        private void Awake() {
            Instance = this;

            m_RectTransform = GetComponent<RectTransform>();

            openCloseToggle.onClick.AddListener(ToggleOpenCloseState);
            gameObject.SetActive(false);
        }

        private void Start() {
            DayManager.Instance.OnNextDay += Reload;
        }

        private void OnEnable() {
            Reload();
        }

        private void ToggleOpenCloseState() {
            gameObject.SetActive(!gameObject.activeSelf);
        }


        public void Reload() {
            UIHighlighter.Instance.GetGroup(HIGHLIGHT_GROUP_ID).RemoveAll();

            DeleteInstancesFromParent(postedQuestsParent);
            DeleteInstancesFromParent(activeQuestsParent);
            DeleteInstancesFromParent(requestQuestsParent);

            foreach(QuestInstance quest in QuestManager.PostedQuests) {
                QuestInstanceUI newInstance = Instantiate(instancePrefab);
                newInstance.GetComponent<RectTransform>().SetParent(postedQuestsParent, false);
                newInstance.ApplyQuestChain(quest);
            }

            foreach (QuestInstance quest in QuestManager.ActiveQuests.Keys) {
                QuestInstanceUI newInstance = Instantiate(instancePrefab);
                newInstance.GetComponent<RectTransform>().SetParent(activeQuestsParent, false);
                newInstance.ApplyQuestChain(quest);

                if (quest.DaysLeftOnQuest == 1) {
                    UIHighlighter.Instance.GetGroup(HIGHLIGHT_GROUP_ID).AddObject(newInstance.GetComponent<Image>(), questTurnColor1, newInstance.GetComponent<Image>().color);
                }
            }

            foreach (QuestInstance quest in QuestManager.Requests) {
                QuestInstanceUI newInstance = Instantiate(instancePrefab);
                newInstance.GetComponent<RectTransform>().SetParent(requestQuestsParent, false);
                newInstance.ApplyQuestChain(quest);

                if(quest.DaysLeftOnPost == 1) {
                    UIHighlighter.Instance.GetGroup(HIGHLIGHT_GROUP_ID).AddObject(newInstance.GetComponent<Image>(), requestTimeColor, newInstance.GetComponent<Image>().color);
                }
            }
        }

        private void DeleteInstancesFromParent(RectTransform parent) {
            bool skipFirst = true;
            foreach(RectTransform child in parent) {
                if (skipFirst) {
                    skipFirst = false;
                    continue;
                }

                Destroy(child.gameObject);
            }
        }

    }

}