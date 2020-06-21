using Rondo.Generic.Utility;
using Rondo.QuestSim.Facilities;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.UI.General;
using Rondo.QuestSim.UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.Blacksmith {

    public class BlacksmithWindow : MonoBehaviourSingleton<BlacksmithWindow> {

        public BlacksmithItem blacksmithItemTemplate;
        public RectTransform buyItemsParent;
        public RectTransform contractsParent;

        private void Awake() {
            Instance = this;

            gameObject.SetActive(false);
        }

        private void OnEnable() {
            FillBuyItems();
            InventoryWindow.Instance.SetSellMode(true);
        }

        private void OnDisable() {
            InventoryWindow.Instance.SetSellMode(false);
        }

        public void AttemptBuyItem(GameItemInstanceUI item) {
            int price = item.Item.BuyPrice;
            if(price <= InventoryManager.Gold) {
                InventoryManager.Gold -= price;
                BlacksmithManager.RemoveItem(item.Item);
                InventoryManager.OwnedItems.Add(item.Item);

                InventoryWindow.Instance.RefreshInventory();
                FillBuyItems();
            }
        }

        public void AttemptSellItem(GameItemInstanceUI item) {
            int price = item.Item.SellPrice;
            InventoryManager.Gold += price;
            InventoryManager.OwnedItems.Remove(item.Item);

            InventoryWindow.Instance.RefreshInventory();
            FillBuyItems();
        }

        private void FillBuyItems() {
            DeleteInstancesFromParent(buyItemsParent);

            foreach (GameItem item in BlacksmithManager.ItemsOnSale) {
                BlacksmithItem newInstance = Instantiate(blacksmithItemTemplate);
                newInstance.transform.SetParent(buyItemsParent, false);
                newInstance.transform.localScale = Vector2.one;
                newInstance.gameObject.SetActive(true);
                newInstance.SetItem(item);
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