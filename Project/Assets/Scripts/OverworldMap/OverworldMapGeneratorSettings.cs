using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests.Sources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Overworld {

	[Serializable]
	public struct OverworldMapGeneratorSettings {

		[Header("Base texture settings")]
		public int resolutionWidth;
		public int resolutionHeight;

		[Header("Land settings")]
		public float landSpacing;
		public float landRadiusMin;
		public float landRadiusMax;
		public float landStrengthMin;
		public float landStrengthMax;

		[Header("Colors")]
		public Color waterColor;
		public Color coastColor;

		[Header("Zoom")]
		public float zoomMinX;
		public float zoomMaxX;
		public float zoomMinY;
		public float zoomMaxY;

		[Header("Debug")]
		public bool showPresenceGradients;
	}
}