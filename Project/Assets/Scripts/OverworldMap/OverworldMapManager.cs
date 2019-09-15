using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Overworld {
	public static class OverworldMapManager {

		public static OverworldMap mainMap;

		public static void Initialize(OverworldMapGeneratorSettings settings) {
			mainMap = OverworldMapGenerator.GenerateMap(settings);
		}
	}
}