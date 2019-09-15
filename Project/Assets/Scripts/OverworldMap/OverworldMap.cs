using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests.Sources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Overworld {

	[Serializable]
	public class OverworldMap {

		private OverworldMapGeneratorSettings m_GeneratorSettings;

		private Texture2D m_MapTexture;
		private Dictionary<QuestSourceFaction, List<MapLocation>> m_FactionLocations = new Dictionary<QuestSourceFaction, List<MapLocation>>();
		private Dictionary<QuestSourceFaction, float[,]> m_FactionPresenceCache = new Dictionary<QuestSourceFaction, float[,]>();

		private Dictionary<QuestSourceFaction, Vector2> m_FactionCapitalLocations = new Dictionary<QuestSourceFaction, Vector2>();

		private QuestSourceFaction m_WaterFaction = new QuestSourceFaction(Reputation.ReputationBiases.GOVERNMENT);
		private QuestSourceFaction m_CoastFaction = new QuestSourceFaction(Reputation.ReputationBiases.GOVERNMENT);
		private QuestSourceFaction m_CapitalFaction = new QuestSourceFaction(Reputation.ReputationBiases.GOVERNMENT);


		public OverworldMap(OverworldMapGeneratorSettings settings) {
			m_GeneratorSettings = settings;

			m_WaterFaction.FactionColor = settings.waterColor;
			m_CoastFaction.FactionColor = settings.coastColor;
			m_CapitalFaction.FactionColor = new Color(0, 0, 0, 1);

			//Generate water locations around the edges of the map
			List<MapLocation> waterLocations = new List<MapLocation>();
			for (float x = 0; x <= 1.01f; x += 0.05f) {
				for (float y = 0; y <= 1.01f; y += 0.05f) {
					Vector2 position = new Vector2(x, y) + MathUtilities.GetRandomVector2(-0.015f, -0.015f, 0.015f, 0.015f);
					waterLocations.Add(new MapLocation(position, UnityEngine.Random.Range(0.015f, 0.1f), UnityEngine.Random.Range(1f, 10f)));
				}
			}
			m_FactionLocations[m_WaterFaction] = waterLocations;
		}

		public void SetFactionLocation(QuestSourceFaction faction, Vector2 position, float radius, float strength, bool isCapital = false) {
			if (!m_FactionLocations.ContainsKey(faction)) {
				m_FactionLocations[faction] = new List<MapLocation>();
			}
			m_FactionLocations[faction].Add(new MapLocation(position, radius, strength));

			if (isCapital == true) {
				m_FactionCapitalLocations[faction] = position;
			}


		}

		public void CalculateFactionPresences() {
			foreach (QuestSourceFaction faction in m_FactionLocations.Keys) {
				float[,] factionPresenceTable = new float[m_GeneratorSettings.resolutionWidth, m_GeneratorSettings.resolutionHeight];
				for (int x = 0; x < m_GeneratorSettings.resolutionWidth; x++) {
					for (int y = 0; y < m_GeneratorSettings.resolutionHeight; y++) {
						float totalMapPresence = 0;
						Vector2 normalizedMapLocation = GetZoomPosition(x, y);

						foreach (MapLocation mapLocation in m_FactionLocations[faction]) {
							float baseLocationPresence = Mathf.Clamp(mapLocation.radius - Vector2.Distance(mapLocation.location, normalizedMapLocation), 0, float.MaxValue) * mapLocation.strength;
							totalMapPresence += baseLocationPresence;
						}
						factionPresenceTable[x, y] = totalMapPresence;
					}
				}
				m_FactionPresenceCache[faction] = factionPresenceTable;
			}
		}

		public void SetMapTexture(Texture2D texture) {
			m_MapTexture = texture;
		}

		public Texture2D GetMapTexture() {
			return m_MapTexture;
		}

		public OverworldMapPresence GetStrongestFactionPresence(int x, int y) {
			Vector2 normalizedMapLocation = GetZoomPosition(x, y);

			foreach (QuestSourceFaction faction in m_FactionCapitalLocations.Keys) {
				Vector2 factionCapitalLocation = m_FactionCapitalLocations[faction];
				if (Vector2.Distance(factionCapitalLocation, normalizedMapLocation) <= 0.01f) {
					return new OverworldMapPresence(m_CapitalFaction, 1);
				}
			}

			QuestSourceFaction strongestFaction = null;
			QuestSourceFaction secondStrongestFaction = null;
			float highestPresence = 0;
			float presenceDifference = float.MaxValue;
			

			foreach (QuestSourceFaction faction in m_FactionLocations.Keys) {
				float factionPresence = m_FactionPresenceCache[faction][x, y];
				if (factionPresence > highestPresence) {
					presenceDifference = factionPresence - highestPresence;
					highestPresence = factionPresence;
					secondStrongestFaction = strongestFaction;
					strongestFaction = faction;
				}
			}

			float borderThreshold = 0.05f;
			float coastThreshold = 0.1f;
			if (secondStrongestFaction == m_WaterFaction && presenceDifference <= coastThreshold) {
				return new OverworldMapPresence(m_CoastFaction, 1);
			} else if(presenceDifference <= borderThreshold) {
				return new OverworldMapPresence(m_CapitalFaction, 1);
			}
			return new OverworldMapPresence(strongestFaction, highestPresence);
		}

		private Vector2 GetZoomPosition(int x, int y) {
			Vector2 normalizedMapLocation = new Vector2((float)x / m_GeneratorSettings.resolutionWidth, (float)y / m_GeneratorSettings.resolutionHeight);
			normalizedMapLocation.x = MathUtilities.RemapFloat(normalizedMapLocation.x, 0, 1, m_GeneratorSettings.zoomMinX, m_GeneratorSettings.zoomMaxX);
			normalizedMapLocation.y = MathUtilities.RemapFloat(normalizedMapLocation.y, 0, 1, m_GeneratorSettings.zoomMinY, m_GeneratorSettings.zoomMaxY);
			return normalizedMapLocation;
		}

		private class MapLocation {
			public Vector2 location;
			public float radius;
			public float strength;

			public MapLocation(Vector2 location, float radius, float strength) {
				this.location = location;
				this.radius = radius;
				this.strength = strength;
			}
		}
	}
}