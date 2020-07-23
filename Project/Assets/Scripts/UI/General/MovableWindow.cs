using Rondo.QuestSim.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.General {

	[RequireComponent(typeof(CanvasGroup))]
	public class MovableWindow : MonoBehaviour {

		public Button exitButton;
		public RectTransform dragArea;

		[Header("Resizing")]
		public RectTransform resizeDragArea;
		public float minHeight = 200;

		private RectTransform m_RectTransform;
		private Vector3 m_TotalDragDelta;

		private void Awake() {
			m_RectTransform = GetComponent<RectTransform>();

			MovableWindowManager.Instance.RegisterMovableWindow(this);
		}

		public void StartDrag() {
			m_TotalDragDelta = Vector3.zero;
		}

		public bool IsMouseInDragArea(Vector3 mousePosition) {
			Vector2 localMousePosition = dragArea.InverseTransformPoint(mousePosition);
			return IsMouseInArea(localMousePosition, dragArea);
		}

		public bool IsMouseInResizeArea(Vector3 mousePosition) {
			Vector2 localMousePosition = resizeDragArea.InverseTransformPoint(mousePosition);
			return IsMouseInArea(localMousePosition, resizeDragArea);
		}

		public bool IsMouseInArea(Vector2 localMousePosition, RectTransform rectTransform) {
			if (!gameObject.activeSelf) {
				return false;
			}

			if (Input.GetMouseButtonDown(0)) {
				if (rectTransform.rect.Contains(localMousePosition)) {
					return true;
				}
			}
			return false;
		}

		public void TranslateWindow(Vector3 delta) {
			m_TotalDragDelta += delta;

			m_RectTransform.Translate(delta);
		}

		public void ResizeWindow(Vector3 delta) {
			m_TotalDragDelta += delta;

			float currentHeight = m_RectTransform.sizeDelta.y;
			if(currentHeight - delta.y <= minHeight) {
				delta.y = currentHeight - minHeight;
			}

			m_RectTransform.sizeDelta -= new Vector2(0, delta.y);
			m_RectTransform.localPosition += new Vector3(0, delta.y * 0.5f, 0);
		}
	}
}