using Rondo.Generic.Utility;
using Rondo.QuestSim.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.General {

	public class MovableWindowManager : MonoBehaviourSingleton<MovableWindowManager> {

		private enum DragMode {
			None = 0,
			Move = 1,
			Resize = 2,
		}

		public RectTransform windowParent;

		private Dictionary<Transform, MovableWindow> m_Windows = new Dictionary<Transform, MovableWindow>();
		private Vector3 m_LastMousePosition;
		private MovableWindow m_DraggingWindow;
		private DragMode m_DragMode = DragMode.None;

		private void Update() {
			Vector3 mousePosition = Input.mousePosition;
			if (Input.GetMouseButtonDown(0)) {
				for (int i = windowParent.childCount - 1; i >= 0; i--) {
					Transform windowTransform = windowParent.GetChild(i);
					MovableWindow window = m_Windows[windowTransform];

					DragMode nextDragMode = DragMode.None;
					if (window.IsMouseInDragArea(mousePosition)) {
						nextDragMode = DragMode.Move;
					} else if (window.IsMouseInResizeArea(mousePosition)) {
						nextDragMode = DragMode.Resize;
					}

					if(nextDragMode != DragMode.None) {
						m_DragMode = nextDragMode;
						m_DraggingWindow = window;
						m_LastMousePosition = mousePosition;
						windowTransform.SetSiblingIndex(windowParent.childCount - 1);
						m_DraggingWindow.StartDrag();
						break;
					}
				}
			} else if (Input.GetMouseButtonUp(0)) {
				m_DragMode = DragMode.None;
				m_DraggingWindow = null;
			}

			if (m_DraggingWindow != null) {
				Vector3 mouseDelta = mousePosition - m_LastMousePosition;

				switch (m_DragMode) {
					case DragMode.Move:
						m_DraggingWindow.TranslateWindow(mouseDelta);
						break;

					case DragMode.Resize:
						m_DraggingWindow.ResizeWindow(mouseDelta);
						break;

					default:
						Debug.LogError("Attempted to interact with drag window, but no valid interaction found for drag mode " + m_DragMode.ToString());
						break;
				}

				m_LastMousePosition = mousePosition;
			}
		}

		public void RegisterMovableWindow(MovableWindow window) {
			m_Windows.Add(window.transform, window);

			window.transform.SetParent(windowParent);
		}
	}
}