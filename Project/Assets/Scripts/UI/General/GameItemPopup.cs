using Rondo.Generic.Utility;
using Rondo.QuestSim.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.General {

    public class GameItemPopup : MonoBehaviourSingleton<GameItemPopup> {

        public GameObject content;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI attackText;
        public TextMeshProUGUI defenceText;
        public TextMeshProUGUI overallPowerText;
        public Image icon;

        public TextMeshProUGUI leftClickHint;

        private RectTransform m_RectTransform;
        private GameItem m_CurrentItem = null;

        private void Awake() {
            m_RectTransform = GetComponent<RectTransform>();
        }

        private void Update() {
            if (m_CurrentItem == null) return;

            Vector2 windowPos = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0);

            //Check if out of bounds on X
            if(windowPos.x + m_RectTransform.sizeDelta.x > Screen.width / 2) {
                windowPos.x -= m_RectTransform.sizeDelta.x;
            }

            //Check if out of bounds on Y
            if (windowPos.y - m_RectTransform.sizeDelta.y + (Screen.height / 2) < 0) {
                windowPos.y += m_RectTransform.sizeDelta.y;
            }

            m_RectTransform.localPosition = windowPos;
        }

        public void SwitchItemTarget(GameItem item) {
            m_CurrentItem = item;

            if (m_CurrentItem == null) {
                content.SetActive(false);
                return;
            }

            content.SetActive(true);

            titleText.text = "<b>" + item.DisplayName + "</b>\n<size=18><i>" + item.Rarity.ToString() + "</size></i>";
            attackText.text = ""+item.AttackPower;
            defenceText.text = "" + item.DefencePower;
            overallPowerText.text = "" + item.OverallPower;
            icon.overrideSprite = item.GetIcon();
        }

        public void SetMode(GameItemPopupModes mode) {
            switch (mode) {
                case GameItemPopupModes.DEFAULT:
                    leftClickHint.gameObject.SetActive(false);
                    break;

                case GameItemPopupModes.BUY:
                    leftClickHint.gameObject.SetActive(true);
                    leftClickHint.text = "<b>Left click</b> to buy for " + m_CurrentItem.BuyPrice + "g";
                    break;

                case GameItemPopupModes.SELL:
                    leftClickHint.gameObject.SetActive(true);
                    leftClickHint.text = "<b>Left click</b> to sell for " + m_CurrentItem.SellPrice + "g";
                    break;

                case GameItemPopupModes.QUEST_SELECT:
                    leftClickHint.gameObject.SetActive(true);
                    leftClickHint.text = "<b>Left click</b> to set as quest reward";
                    break;

                default:
                case GameItemPopupModes.NAKED:
                    leftClickHint.gameObject.SetActive(false);
                    break;

            }
        }
    }

}