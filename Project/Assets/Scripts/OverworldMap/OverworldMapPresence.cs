using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests.Sources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Overworld {

	public struct OverworldMapPresence {
		public QuestSourceFaction faction;
		public Color color;
		public float presence;

		public OverworldMapPresence(QuestSourceFaction faction, float presence) {
			this.faction = faction;
			this.presence = presence;

			if(faction != null) {
				color = faction.FactionColor;
			} else {
				color = new Color(1, 1, 1, 0);
			}
		}
	}
}