using Rondo.Generic.Utility;
using Rondo.QuestSim.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rondo.QuestSim.UI.General {

    public class GameItemPopupCaller : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler {

        public GameItem associatedItem;
        public GameItemPopupModes CallMode { get { return m_ItemInstanceUI != null ? m_ItemInstanceUI.PopupMode : defaultCallMode; } }
        public GameItemPopupModes defaultCallMode = GameItemPopupModes.DEFAULT;

        private GameItemInstanceUI m_ItemInstanceUI;
        private bool m_IsHovering = false;

        private void Awake() {
            m_ItemInstanceUI = GetComponent<GameItemInstanceUI>();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            m_IsHovering = true;
            if (eventData.button != PointerEventData.InputButton.Left) return;
            GameItemPopup.Instance.SwitchItemTarget(associatedItem);
            GameItemPopup.Instance.SetMode(CallMode);
        }

        public void OnPointerExit(PointerEventData eventData) {
            m_IsHovering = false;
            GameItemPopup.Instance.SwitchItemTarget(null);
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Left &&  
                (eventData.button != PointerEventData.InputButton.Right && (CallMode != GameItemPopupModes.DEFAULT || CallMode != GameItemPopupModes.NAKED))) return;
            GameItemPopup.Instance.SwitchItemTarget(null);
        }

        private void OnDisable() {
            if(m_IsHovering) GameItemPopup.Instance.SwitchItemTarget(null);
        }
    }

}