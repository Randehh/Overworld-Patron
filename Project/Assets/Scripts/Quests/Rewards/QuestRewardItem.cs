using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Heroes;

namespace Rondo.QuestSim.Quests.Rewards {

    public class QuestRewardItem : IQuestReward {

        public GameItem Item { get; private set; }
        public float RewardValue { get { return Item.Power * ((int)Item.Rarity * 0.5f) + ((int)Item.Rarity * 10); } }
        public string DisplayValue { get { return Item.DisplayName; } }

        public QuestRewardItem(GameItem item) {
            Item = item;
        }

        public void ApplyReward(HeroInstance hero) {
            hero.EquipmentLevel += Item.Power;
            InventoryManager.ReservedItems.Remove(Item);
        }
    }

}