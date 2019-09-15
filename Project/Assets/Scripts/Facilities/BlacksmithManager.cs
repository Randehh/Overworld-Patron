using Rondo.Generic.Utility;
using Rondo.QuestSim.Gameplay;
using Rondo.QuestSim.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Facilities {

    public static class BlacksmithManager {

        public static List<GameItem> ItemsOnSale { get { return GetSortedList(); } }
        public static GameItemRarity BestQualityInStore { get; set; }
        public static int ItemsPerDay { get; set; }

        private static Dictionary<GameItem, int> m_Items = new Dictionary<GameItem, int>();

        public static void Initialize() {
            m_Items = new Dictionary<GameItem, int>();
            BestQualityInStore = GameItemRarity.COMMON;
            ItemsPerDay = 1;

            DayManager.Instance.OnNextDay += RefreshItemsOnSale;

            RefreshItemsOnSale();
        }

        public static void RemoveItem(GameItem item) {
            if (!m_Items.ContainsKey(item)) return;
            m_Items.Remove(item);
        }

        public static void RefreshItemsOnSale() {
            List<GameItem> keys = new List<GameItem>(m_Items.Keys);
            for(int i = keys.Count - 1; i>= 0; i--) {
                GameItem item = keys[i];
                int currentLife = m_Items[item] - 1;

                if(currentLife <= 0) {
                    m_Items.Remove(item);
                }else {
                    m_Items[item] = currentLife;
                }
            }

            for (int i = 0; i < ItemsPerDay; i++) {
                GameItem item = GameItemGenerator.GenerateItem(GameItemTypes.UNKNOWN, EnumUtility.GetRandomEnumValue<GameItemRarity>(1, (int)BestQualityInStore), UnityEngine.Random.Range(0f, 1f));
                int shelfLife = UnityEngine.Random.Range(2, 5);
                m_Items.Add(item, shelfLife);
            }
        }

        public static List<GameItem> GetSortedList() {
            return ItemUtility.SortByRarity(new List<GameItem>(m_Items.Keys));
        }
    }

}