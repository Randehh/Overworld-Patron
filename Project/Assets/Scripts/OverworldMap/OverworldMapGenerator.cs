using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Reputation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Overworld {
	public static class OverworldMapGenerator {

		public const int LAND_COUNT_MIN = 2;
		public const int LAND_COUNT_MAX = 5;

		private static int m_CurrentMapCount = 0;
		private static List<QuestSourceFaction> m_CurrentFactions;

		public static OverworldMap GenerateMap(OverworldMapGeneratorSettings settings) {
			OverworldMap map = new OverworldMap(settings);
			m_CurrentFactions = ReputationManager.GetAllFactions();

			GenerateCapitalCities(map, settings);
			map.CalculateFactionPresences();
			GenerateMapTexture(map, settings);

			map.GetMapTexture().name = "Map_" + m_CurrentMapCount;
			m_CurrentMapCount += 1;

			return map;
		}

		private static void GenerateCapitalCities(OverworldMap map, OverworldMapGeneratorSettings settings) {
			List<Vector2> capitalPositions = new List<Vector2>();
			float edgeSpacing = 0.3f;
			int squareCount = 4;
			float squareSize = (1 - (edgeSpacing * 2)) / squareCount;
			float halfSquareSize = squareSize * 0.5f;

			for (int x = 0; x < squareCount; x++) {
				for (int y = 0; y < squareCount; y++) {
					float offsetX = UnityEngine.Random.Range(-halfSquareSize, halfSquareSize);
					float offsetY = UnityEngine.Random.Range(-halfSquareSize, halfSquareSize);
					capitalPositions.Add(new Vector2(
						edgeSpacing + (squareSize * x) + offsetX,
						edgeSpacing + (squareSize * y) + offsetY
						));
				}
			}

			foreach (QuestSourceFaction faction in m_CurrentFactions) {
				Vector2 factionPosition = capitalPositions.GetRandom();
				capitalPositions.Remove(factionPosition);
				map.SetFactionLocation(faction, factionPosition, 0.2f, 20f, true);
				GenerateFactionLands(map, settings, faction, factionPosition);
			}
		}

		private static void GenerateFactionLands(OverworldMap map, OverworldMapGeneratorSettings settings, QuestSourceFaction faction, Vector2 centralPoint) {
			int landPointCount = UnityEngine.Random.Range(LAND_COUNT_MIN, LAND_COUNT_MAX);
			for (int i = 0; i <= landPointCount; i++) {
				Vector2 landPoint = centralPoint + MathUtilities.GetRandomVector2(-settings.landSpacing, -settings.landSpacing, settings.landSpacing, settings.landSpacing);
				float radius = UnityEngine.Random.Range(settings.landRadiusMin, settings.landRadiusMax);
				float strength = UnityEngine.Random.Range(settings.landStrengthMin, settings.landStrengthMax);
				map.SetFactionLocation(faction, landPoint, radius, strength);
			}
		}

		private static void GenerateMapTexture(OverworldMap map, OverworldMapGeneratorSettings settings) {
			Texture2D mapTexture = new Texture2D(settings.resolutionWidth, settings.resolutionHeight);
			OverworldMapPresence[,] presenceArray = new OverworldMapPresence[settings.resolutionWidth, settings.resolutionHeight];
			float highestPresence = 0;
			for (int x = 0; x < settings.resolutionWidth; x++) {
				for (int y = 0; y < settings.resolutionHeight; y++) {
					OverworldMapPresence presence = map.GetStrongestFactionPresence(x, y);
					presenceArray[x, y] = presence;

					if(presence.presence >= highestPresence) {
						highestPresence = presence.presence;
					}
				}
			}

			Color32[] colorArray = new Color32[settings.resolutionWidth * settings.resolutionHeight];
			for (int x = 0; x < settings.resolutionWidth; x++) {
				for (int y = 0; y < settings.resolutionHeight; y++) {
					OverworldMapPresence presence = presenceArray[x, y];
					Color c = presence.color;

					if (settings.showPresenceGradients) {
						int steps = 10;
						float perStepValue = 1f / steps;
						float stepValue = 0;
						float gradientValue = presence.presence / highestPresence;
						for (int i = 0; i <= steps; i++) {
							if(gradientValue >= stepValue && gradientValue < stepValue + perStepValue) {
								gradientValue = stepValue + (perStepValue * 2);
								break;
							}
							stepValue += perStepValue;
						}
						c.a *= gradientValue;
					}

					colorArray[x + (y * settings.resolutionWidth)] = c;
				}
			}

			mapTexture.SetPixels32(colorArray);
			mapTexture.Apply();
			map.SetMapTexture(mapTexture);
		}
	}
}