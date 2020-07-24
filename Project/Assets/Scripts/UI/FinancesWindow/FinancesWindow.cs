using Rondo.QuestSim.Gameplay;
using Rondo.QuestSim.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.Finances {
    public class FinancesWindow : MonoBehaviour {

        private struct FinanceEntry {
            public RectTransform gameInstance;
            public string description;
            public int goldChange;
        }

        public RectTransform parent;
        public GameObject template;
        public TextMeshProUGUI dayText;
        public TextMeshProUGUI totalText;

        public Button nextDayButton;
        public Button currentDayButton;
        public Button previousDayButton;

        private int m_ShowingCurrentDay = 1;
        private Dictionary<int, List<FinanceEntry>> m_FinanceEntries = new Dictionary<int, List<FinanceEntry>>();

        private void Start() {
            InventoryManager.OnGoldChange += (gold, goldChange, description) => AddFinanceEntry(description, goldChange);
            DayManager.Instance.OnNextDay += SetToCurrentDay;

            nextDayButton.onClick.AddListener(NextDay);
            currentDayButton.onClick.AddListener(SetToCurrentDay);
            previousDayButton.onClick.AddListener(PreviousDay);

            SetDayToShow(1);
        }

		public void AddFinanceEntry(string description, int goldChange) {
            FinanceEntry newEntry = new FinanceEntry() {
                description = description,
                goldChange = goldChange,
            };
            GetOrCreateEntries(DayManager.Instance.CurrentDay).Add(newEntry);
            SetDayToShow(DayManager.Instance.CurrentDay);
        }

        public void SetDayToShow(int day) {
            m_ShowingCurrentDay = day;

            foreach (Transform child in parent) {
                if(child == template.transform) {
                    continue;
				}

                Destroy(child.gameObject);
			}

            int netChange = 0;
            foreach(FinanceEntry entry in GetOrCreateEntries(m_ShowingCurrentDay)) {
                netChange += entry.goldChange;

                GameObject newEntry = Instantiate(template, parent);
                newEntry.GetComponent<TextMeshProUGUI>().text = $"<align=left>{entry.description}<line-height=0.1>\n<align=right>{entry.goldChange}<line-height=1em>";
                newEntry.SetActive(true);
            }

            totalText.text = $"<align=left>Net change<line-height=0.1>\n<align=right>{netChange}<line-height=1em>";

            //Update button states
            previousDayButton.interactable = day == 1 ? false : true;
            nextDayButton.interactable = day == DayManager.Instance.CurrentDay ? false : true;

            dayText.text = "Day " + day;
        }

        private List<FinanceEntry> GetOrCreateEntries(int day) {
			if (!m_FinanceEntries.ContainsKey(day)) {
                m_FinanceEntries.Add(day, new List<FinanceEntry>());
            }

            return m_FinanceEntries[day];
        }

        /*
         * Button functions
         */
        public void SetToCurrentDay() {
            SetDayToShow(DayManager.Instance.CurrentDay);
        }

        public void NextDay() {
            SetDayToShow(m_ShowingCurrentDay + 1);
        }

        public void PreviousDay() {
            SetDayToShow(m_ShowingCurrentDay - 1);
        }
    }
}