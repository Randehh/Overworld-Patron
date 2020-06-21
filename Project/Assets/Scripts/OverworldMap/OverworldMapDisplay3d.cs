using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Reputation;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.Overworld {

	public class OverworldMapDisplay3d : MonoBehaviour {

		private List<GameObject> m_CurrentTags = new List<GameObject>();

		public Material mapMaterial;

		public Transform tagParent;
		public Transform nameTagTemplate;
		public Vector3 tagOffset = new Vector3(0.01f, 0, 0.01f);

		public void SetMap(OverworldMap settings) {
			ClearTags();

			Texture2D mapTexture = settings.GetMapTexture();
			mapMaterial.mainTexture = mapTexture;

			foreach(KeyValuePair<QuestSourceFaction, Vector2> entry in settings.GetFactionCapitals()) {
				SetCapitalTag(entry.Key.DisplayName, entry.Value);
			}
		}

		private void SetCapitalTag(string name, Vector2 position) {
			GameObject tagClone = Instantiate(nameTagTemplate.gameObject, tagParent, true);
			Transform tagTransform = tagClone.GetComponent<Transform>();
			TextMeshPro tagText = tagClone.GetComponent<TextMeshPro>();

			tagText.text = name;

			BoxCollider parentBoxCollider = tagParent.GetComponent<BoxCollider>();
			Vector3 parentSize = parentBoxCollider.size;
			tagTransform.localPosition = new Vector3((position.x * parentSize.x) - (parentSize.x * 0.5f), 0, (position.y * -parentSize.z) + (parentSize.z * 0.5f)) + tagOffset;
			tagClone.SetActive(true);

			m_CurrentTags.Add(tagClone);
		}

		private void ClearTags() {
			foreach(GameObject tag in m_CurrentTags) {
				Destroy(tag);
			}
			m_CurrentTags.Clear();
		}
	}
}