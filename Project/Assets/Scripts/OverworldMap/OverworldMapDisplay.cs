using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Reputation;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.Overworld {

	[RequireComponent(typeof(RectTransform))]
	public class OverworldMapDisplay : MonoBehaviour {

		private RectTransform m_RectTransform;
		private List<GameObject> m_CurrentTags = new List<GameObject>();

		public Image mapImage;
		public RectTransform nameTagTemplate;
		public Vector2 tagOffset = new Vector2(10, -10);

		private void Awake() {
			m_RectTransform = GetComponent<RectTransform>();
		}

		public void SetMap(OverworldMap settings) {
			ClearTags();

			Texture2D mapTexture = settings.GetMapTexture();
			Sprite sprite = Sprite.Create(settings.GetMapTexture(), new Rect(0, 0, mapTexture.width, mapTexture.height), Vector2.zero);
			mapImage.sprite = sprite;

			foreach(KeyValuePair<QuestSourceFaction, Vector2> entry in settings.GetFactionCapitals()) {
				SetCapitalTag(entry.Key.DisplayName, entry.Value);
			}
		}

		private void SetCapitalTag(string name, Vector2 position) {
			GameObject tagClone = Instantiate(nameTagTemplate.gameObject, transform);
			RectTransform tagTransform = tagClone.GetComponent<RectTransform>();
			TextMeshProUGUI tagText = tagClone.GetComponent<TextMeshProUGUI>();

			tagText.text = name;

			Vector2 parentSize = m_RectTransform.sizeDelta;
			tagTransform.anchoredPosition = new Vector2((position.x) * parentSize.x, (1 - position.y) * -parentSize.y) + tagOffset;
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