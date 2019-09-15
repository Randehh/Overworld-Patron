using Rondo.QuestSim.General;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests.Rewards;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace Rondo.QuestSim.UI.General {

    [RequireComponent(typeof(GameItemPopupCaller))]
    public class GameItemInstanceUI : MonoBehaviour, IPointerClickHandler {

        public Image icon;
        public TextMeshProUGUI titleText;

        public GameItem Item { get; private set; }
        public GameItemPopupModes PopupMode { get; private set; }

        private GameItemPopupCaller m_ItemPopupCaller;

        private Action<GameItemInstanceUI> OnLeftClick;


        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Left) return;

            if (OnLeftClick != null) OnLeftClick(this);
        }

        public void SetItem(QuestRewardItem reward) {
            if (reward == null) {
                SetItem((GameItem)null);
            } else {
                SetItem(reward.Item);
            }
        }

        public void SetItem(GameItem item) {
            if(m_ItemPopupCaller == null) m_ItemPopupCaller = GetComponent<GameItemPopupCaller>();
            Item = item;

            if (item == null) {
                titleText.text = "-";
                icon.overrideSprite = SpriteFetcher.Instance.itemIcons.unknownIcon;
                GetComponent<Image>().color = SpriteFetcher.Instance.itemIcons.colorCommon;
            } else {
                titleText.text = item.DisplayName;
                icon.overrideSprite = item.GetIcon();
                GetComponent<Image>().color = item.GetItemColor();
            }

            m_ItemPopupCaller.associatedItem = item;
        }

        public void SetModeDefault() {
            PopupMode = GameItemPopupModes.DEFAULT;
            OnLeftClick = null;
        }

        public void SetModeBuy(Action<GameItemInstanceUI> buyCall) {
            PopupMode = GameItemPopupModes.BUY;
            OnLeftClick = buyCall;
        }

        public void SetModeSell(Action<GameItemInstanceUI> sellCall) {
            PopupMode = GameItemPopupModes.SELL;
            OnLeftClick = sellCall;
        }

        public void SetModeQuestSelect(Action<GameItemInstanceUI> setCall) {
            PopupMode = GameItemPopupModes.QUEST_SELECT;
            OnLeftClick = setCall;
        }

        public void SetModeNaked() {
            PopupMode = GameItemPopupModes.NAKED;
            OnLeftClick = null;
        }

    }

}