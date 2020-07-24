using Rondo.QuestSim.UI.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Inventory {

    public static class InventoryManager {

        public static int Gold { get; private set; }
        public static int Stars { get { return m_Stars; } set { m_Stars = value; OnStarsChange(value); } }
        public static List<GameItem> OwnedItems { get; set; }
        public static List<GameItem> ReservedItems { get; set; }

        public static Action<int, int, string> OnGoldChange = delegate { };
        public static Action<int> OnStarsChange = delegate { };

        private static int m_Stars = 0;

        public static void Initialize() {
            Gold = 0;
            OwnedItems = new List<GameItem>();
            ReservedItems = new List<GameItem>();

            for (int i = 0; i < 3; i++) {
                OwnedItems.Add(GameItemGenerator.GenerateItem(GameItemTypes.UNKNOWN, GameItemRarity.COMMON));
            }
            for (int i = 0; i < 2; i++) {
                OwnedItems.Add(GameItemGenerator.GenerateItem(GameItemTypes.UNKNOWN, GameItemRarity.UNCOMMON));
            }
		}

        public static void ModifyGold(int goldChange, string reason) {
            Gold += goldChange;
            OnGoldChange(Gold, goldChange, reason);
		}

        public static void MoveItemToReserved(GameItem item) {
            if (!OwnedItems.Contains(item)) return;
            OwnedItems.Remove(item);
            ReservedItems.Add(item);

            RefreshInventoryWindow();
        }

        public static void MoveItemToOwned(GameItem item) {
            if (!ReservedItems.Contains(item)) return;
            ReservedItems.Remove(item);
            OwnedItems.Add(item);

            RefreshInventoryWindow();
        }

        private static void RefreshInventoryWindow() {
            InventoryWindow window = InventoryWindow.Instance;
            if (window == null || !window.gameObject.activeInHierarchy) return;
            window.RefreshInventory();
        }
    }

}