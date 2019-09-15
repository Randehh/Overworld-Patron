using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.UI.General;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rondo.QuestSim.UI.Blacksmith {

    public class BlacksmithItem : MonoBehaviour {

        public GameItemInstanceUI itemInstance;
        public TextMeshProUGUI priceText;

        public void SetItem(GameItem item) {
            itemInstance.SetItem(item);
            itemInstance.SetModeBuy(BlacksmithWindow.Instance.AttemptBuyItem);
            priceText.text = item.BuyPrice + "G";
        }
    }

}