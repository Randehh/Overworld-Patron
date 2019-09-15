
using Rondo.QuestSim.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rondo.Generic.Utility {

    public static class ItemUtility {

        public static List<GameItem> SortByRarity(List<GameItem> items) {
            items = items.OrderBy((x) => x.Rarity).ThenBy((x) => x.DisplayName).ToList();
            return items;
        }

    }

}