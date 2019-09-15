using Rondo.QuestSim.Gameplay;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.UI.General;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.Main {

    public class MainUI : MonoBehaviour {

        [Header("Main resources")]
        public TextMeshProUGUI goldText;

        [Header("Days")]
        public TextMeshProUGUI daysText;
        public Button endDayButton;

        [Header("Stars")]
        public TextMeshProUGUI starsText;

        private void Awake() {
            endDayButton.onClick.AddListener(DayManager.Instance.EndDay);
            DayManager.Instance.OnNextDay += () => { daysText.text = "Day <b>" + DayManager.Instance.CurrentDay; };
            InventoryManager.OnGoldChange += (gold) => { goldText.text = gold + " <b>Gold"; };
            InventoryManager.OnStarsChange += (stars) => { starsText.text = stars + " <b>Stars"; };
        }
    }

}