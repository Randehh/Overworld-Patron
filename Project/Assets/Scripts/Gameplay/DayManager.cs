using Rondo.Generic.Utility;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.UI.General;
using Rondo.QuestSim.UI.PostedQuests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Gameplay {

    public class DayManager : MonoBehaviourSingleton<DayManager> {

        public GameObject nothingToReportUI;
        public Light sunObject;
        public AnimationCurve sunRotationCurve;
        public AnimationCurve sunStrengthCurve;

        public int CurrentDay { get; set; }
        public Action OnNextDay = delegate { };

        private List<QuestInstance> m_ActiveQuestsToUpdate = new List<QuestInstance>();
        private List<QuestInstance> m_QuestsToAssign = new List<QuestInstance>();
        private int m_CurrentDayStep = 0;
        private Quaternion m_SunDayRotation;
        private Quaternion m_SunNightRotation;

        private void Awake() {
            CurrentDay = 1;
            m_SunDayRotation = sunObject.transform.rotation;
            m_SunNightRotation = m_SunDayRotation * Quaternion.AngleAxis(180, Vector3.right);

            Instance = this;
        }

        public void EndDay() {
            m_CurrentDayStep = 0;

            m_QuestsToAssign = new List<QuestInstance>(QuestManager.PostedQuests);
            m_ActiveQuestsToUpdate = new List<QuestInstance>(QuestManager.ActiveQuests.Keys);
            QuestDetailsWindow.Instance.OnWindowClose += NextDayStep;

            StartCoroutine(SunRotateRoutine(false));

            NightFadeUI.Instance.Enable(() => {
                NextDayStep();
            });
        }

        private void NextDayStep() {
            if(m_QuestsToAssign.Count != 0) {
                QuestInstance nextQuest = m_QuestsToAssign[m_QuestsToAssign.Count - 1];
                m_QuestsToAssign.Remove(nextQuest);

                QuestDetailsWindow.Instance.OpenWindow(nextQuest, QuestDetailsWindowMode.HERO_SELECT);
                m_CurrentDayStep++;
                return;
            }

            while (m_ActiveQuestsToUpdate.Count != 0) {
                QuestInstance activeQuest = m_ActiveQuestsToUpdate[m_ActiveQuestsToUpdate.Count -1];
                activeQuest.DaysLeftOnQuest--;

                m_ActiveQuestsToUpdate.Remove(activeQuest);

                if (activeQuest.DaysLeftOnQuest <= 0) {
                    QuestDetailsWindow.Instance.OpenWindow(activeQuest, QuestDetailsWindowMode.COMPLETED);
                    activeQuest.CompleteQuest(QuestManager.ActiveQuests[activeQuest]);
                    m_CurrentDayStep++;
                    return;
                }
            }

            if(m_CurrentDayStep == 0) {
                nothingToReportUI.SetActive(true);
                TimeUtilities.ExecuteAfterDelay(() => {
                    nothingToReportUI.SetActive(false);
                    NextDayStep();
                }, 2.5f, this);
                m_CurrentDayStep++;
                return;
            }

            CurrentDay++;

            QuestManager.PostedQuests = UpdateQuestTimeLimits(QuestManager.PostedQuests, 0);
            QuestManager.Requests = UpdateQuestTimeLimits(QuestManager.Requests, 0);
            InventoryManager.ModifyGold(-4, "Daily upkeep");

            OnNextDay();

            StartCoroutine(SunRotateRoutine(true));
            NightFadeUI.Instance.Disable(()=> { });
            QuestDetailsWindow.Instance.OnWindowClose -= NextDayStep;
        }

        private List<QuestInstance> UpdateQuestTimeLimits(List<QuestInstance> list, int dayLimit) {
            for (int i = list.Count - 1; i >= 0; i--) {
                QuestInstance quest = list[i];
                quest.DaysLeftOnPost--;
                if (quest.DaysLeftOnPost <= dayLimit) {
                    list.Remove(quest);
                }
            }
            return list;
        }

        private IEnumerator SunRotateRoutine(bool toDay) {
            float duration = 1.5f;
            float startTime = Time.time;
            float currentValue = 0;
            Quaternion startRotation = toDay ? m_SunNightRotation : m_SunDayRotation;
            Quaternion targetRotation = (toDay ? m_SunDayRotation : m_SunNightRotation) * Quaternion.AngleAxis(0.01f, Vector3.right);
            while (currentValue != 1) {
                currentValue = TimeUtilities.GetNormalizedTime(startTime, duration, Time.time);

                sunObject.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, sunRotationCurve.Evaluate(currentValue));

                float lightValue = sunStrengthCurve.Evaluate(toDay ? (1 - currentValue) : currentValue);
                sunObject.intensity = lightValue;
                yield return null;
            }
        }

    }

}