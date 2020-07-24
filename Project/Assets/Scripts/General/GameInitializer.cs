using Rondo.Generic.Utility;
using Rondo.QuestSim.Facilities;
using Rondo.QuestSim.Gameplay;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Overworld;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Reputation;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.General {

    public class GameInitializer : MonoBehaviourSingleton<GameInitializer> {

		public OverworldMapDisplay mapDisplay;
		public OverworldMapDisplay3d mapDisplay3d;
		public OverworldMapGeneratorSettings generatorSettings;

		[Header("Map generating tests")]
		public int mapGenerationCount = 1;
		public Image mapTestImage;

        private void Awake() {
            HeroManager.Initialize();
            ReputationManager.Initialize();
			OverworldMapManager.Initialize(generatorSettings);
            QuestManager.Initialize();
            InventoryManager.Initialize();
            BlacksmithManager.Initialize();

			mapDisplay.SetMap(OverworldMapManager.mainMap);
			mapDisplay3d.SetMap(OverworldMapManager.mainMap);
		}

        private void Start() {
			InventoryManager.ModifyGold(100, "Starting funds");
            InventoryManager.Stars = 0;
        }

		private void Update() {
			if (Input.GetKeyDown(KeyCode.M)) {
				for(int i = 0; i < mapGenerationCount; i++) {
					GenerateMap(true);
				}
			}
		}

		private void GenerateMap(bool saveMap) {
			OverworldMapManager.Initialize(generatorSettings);
			mapDisplay.SetMap(OverworldMapManager.mainMap);
			mapDisplay3d.SetMap(OverworldMapManager.mainMap);
		}

		private void  SaveTextureToFile(Texture2D texture) {
			byte[] texBytes = texture.EncodeToPNG();
			File.WriteAllBytes(Application.dataPath + "/Maps/" + texture.name + ".png", texBytes);
		}
	}

}