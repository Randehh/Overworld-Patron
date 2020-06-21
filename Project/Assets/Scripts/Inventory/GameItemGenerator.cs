using Rondo.Generic.Utility;
using Rondo.QuestSim.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Inventory {

    public static class GameItemGenerator {

        public static GameItem GenerateItem(
            GameItemTypes type = GameItemTypes.UNKNOWN,
            GameItemRarity rarity = GameItemRarity.UNKNOWN,
            float quality = 0.5f) {

            if (type == GameItemTypes.UNKNOWN) type = EnumUtility.GetRandomEnumValue<GameItemTypes>(1);
            if (rarity == GameItemRarity.UNKNOWN) rarity = EnumUtility.GetRandomEnumValue<GameItemRarity>(1);

            GameItem newItem = new GameItem(type, rarity, quality);
            return newItem;
        }

    }

}