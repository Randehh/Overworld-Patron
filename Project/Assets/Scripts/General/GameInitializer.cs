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

			Sprite sprite = Sprite.Create(OverworldMapManager.mainMap.GetMapTexture(), new Rect(0, 0, generatorSettings.resolutionWidth, generatorSettings.resolutionHeight), Vector2.zero);
			mapTestImage.sprite = sprite;
        }

        private void Start() {
            InventoryManager.Gold = 100;
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
			Texture2D mapTexture = OverworldMapManager.mainMap.GetMapTexture();
			Sprite sprite = Sprite.Create(mapTexture, new Rect(0, 0, generatorSettings.resolutionWidth, generatorSettings.resolutionHeight), Vector2.zero);
			sprite.name = mapTexture.name;
			mapTestImage.sprite = sprite;

			if (saveMap) {
				SaveTextureToFile(mapTexture);
			}
		}

		private void  SaveTextureToFile(Texture2D texture) {
			byte[] texBytes = texture.EncodeToPNG();
			File.WriteAllBytes(Application.dataPath + "/Maps/" + texture.name + ".png", texBytes);
		}
	}

}