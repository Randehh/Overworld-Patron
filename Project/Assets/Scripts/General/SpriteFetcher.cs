using Rondo.Generic.Utility;
using Rondo.QuestSim.Gameplay;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Reputation;
using Rondo.QuestSim.ScriptableObjects;
using UnityEngine;

namespace Rondo.QuestSim.General {

    public class SpriteFetcher : MonoBehaviourSingleton<SpriteFetcher> {

        public static PlayerIconDatabase PlayerIcons { get { return Instance.playerIcons; } }
        public static ItemIconDatabase ItemIcons { get { return Instance.itemIcons; } }

        public PlayerIconDatabase playerIcons;
        public ItemIconDatabase itemIcons;

        private void Awake() {
            Instance = this;
        }
    }


}