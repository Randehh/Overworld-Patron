
using Rondo.Generic.Utility;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.UI.Blacksmith;
using Rondo.QuestSim.UI.General;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.Inventory {

    public class InventoryWindow : MonoBehaviourSingleton<InventoryWindow> {

        public GameItemInstanceUI itemPrefab;
        public RectTransform availableItemsParent;
        public RectTransform reservedItemsParent;
        public Button openCloseToggle;

        private RectTransform m_RectTransform;
        private bool m_SellMode = false;
        private bool m_QuestItemMode = false;
        private Action<GameItemInstanceUI> m_OnItemSelectedAction;

        private void Awake() {
            Instance = this;

            m_RectTransform = GetComponent<RectTransform>();

            gameObject.SetActive(false);

            openCloseToggle.onClick.AddListener(ToggleOpenCloseState);
        }

        private void ToggleOpenCloseState() {
            RightSideSwitch.Instance.ActivateObject(gameObject);
        }

        private void OnEnable() {
            RefreshInventory();
        }

        public void SetSellMode(bool b) {
            m_SellMode = b;

            //Refresh if it's already open
            if (gameObject.activeInHierarchy) {
                FillAvailableItems();
            }
        }

        public void SetQuestItemMode(bool b, Action<GameItemInstanceUI> OnSelected) {
            m_QuestItemMode = b;

            if (b) {
                m_OnItemSelectedAction = OnSelected;
            } else {
                m_OnItemSelectedAction = null;
            }

            //Refresh if it's already open
            if (gameObject.activeInHierarchy && b) {
                FillAvailableItems();
            }
        }

        public void RefreshInventory() {
            FillAvailableItems();
            FillReservedItems();
        }

        private void FillAvailableItems() {
            Action<GameItemInstanceUI> onCreateAction = null;
            if (m_SellMode) {
                onCreateAction = (item) => {
                    item.SetModeSell(BlacksmithWindow.Instance.AttemptSellItem);
                };
            }else if (m_QuestItemMode) {
                onCreateAction = (item) => {
                    item.SetModeQuestSelect(m_OnItemSelectedAction);
                };
            }
            AddItemsFromListToParent(InventoryManager.OwnedItems, availableItemsParent, onCreateAction);
        }

        private void FillReservedItems() {
            AddItemsFromListToParent(InventoryManager.ReservedItems, reservedItemsParent);
        }

        private void AddItemsFromListToParent(List<GameItem> itemList, RectTransform parent, Action<GameItemInstanceUI> OnCreate = null) {
            DeleteInstancesFromParent(parent);

            itemList = ItemUtility.SortByRarity(itemList);

            foreach (GameItem item in itemList) {
                GameItemInstanceUI newInstance = Instantiate(itemPrefab);
                newInstance.transform.SetParent(parent, false);
                newInstance.transform.localScale = Vector2.one;
                newInstance.SetItem(item);
                if (OnCreate != null) OnCreate(newInstance);
            }
        }

        private void DeleteInstancesFromParent(RectTransform parent) {
            bool skipFirst = true;
            foreach (RectTransform child in parent) {
                if (skipFirst) {
                    skipFirst = false;
                    continue;
                }

                Destroy(child.gameObject);
            }
        }
    }

}