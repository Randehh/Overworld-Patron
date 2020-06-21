using Rondo.QuestSim.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rondo.QuestSim.UI.General {

	[ExecuteInEditMode]
	public class TextStyler : MonoBehaviour {
		private TextMeshProUGUI m_TextComponent;
		public TextStyle styleToApply;

		private void Awake() {
			m_TextComponent = GetComponent<TextMeshProUGUI>();

			if (m_TextComponent == null) {
				Debug.LogError("Text component not found, removing self");
				DestroyImmediate(this);
				return;
			}
		}

		private void OnValidate() {
			if (styleToApply == null || m_TextComponent == null) {
				return;
			}

			m_TextComponent.fontSize = styleToApply.size;
			m_TextComponent.color = styleToApply.color;

			FontStyles fontStyle = FontStyles.Normal;
			if (styleToApply.isBold && styleToApply.isItalic) {
				fontStyle = FontStyles.Bold | FontStyles.Italic;
			} else if (styleToApply.isBold) {
				fontStyle = FontStyles.Bold;
			} else if (styleToApply.isItalic) {
				fontStyle = FontStyles.Italic;
			}
			m_TextComponent.fontStyle = fontStyle;
		}
	}
}