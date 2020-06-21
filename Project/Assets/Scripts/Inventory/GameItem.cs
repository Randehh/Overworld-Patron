using Rondo.QuestSim.General;
using Rondo.QuestSim.Utility;
using UnityEngine;

namespace Rondo.QuestSim.Inventory {

    public class GameItem {

        public string DisplayName { get; private set; }
        public GameItemTypes ItemType { get; private set; }
        public GameItemRarity Rarity { get; private set; }
        public int Power { get { return Mathf.RoundToInt(BasePower) * (int)Rarity; } }

        public float BasePower { get; private set; }

        public int SellPrice { get { return Power / 2; } }
        public int BuyPrice { get { return Power; } }

        public GameItem(GameItemTypes type, GameItemRarity rarity) {
            ItemType = type;
            Rarity = rarity;

            DisplayName = NameDatabase.GetItemName(this);
            BasePower = Random.Range(12f, 15f);
        }

        public Sprite GetIcon() {
            return SpriteFetcher.Instance.itemIcons.GetSpriteForType(ItemType);
        }

        public Color GetItemColor() {
            return SpriteFetcher.Instance.itemIcons.GetColorForRarity(Rarity);
        }
    }

}