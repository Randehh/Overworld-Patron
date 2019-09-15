using Rondo.Generic.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rondo.QuestSim.UI.General {

    public class FocusBoxManager : MonoBehaviourSingleton<FocusBoxManager> {

        public RectTransform boxParent;
        public FocusBox template;
        public int startPool = 5;

        private Dictionary<FocusBox, bool> m_FocusBoxes = new Dictionary<FocusBox, bool>();

        private void Awake() {
            for (int i = 0; i < startPool; i++) CreateFocusBox();
        }

        public List<FocusBox> SetFocusTargets(params RectTransform[] targets) {
            List<FocusBox> boxes = new List<FocusBox>();
            foreach (RectTransform target in targets) {
                boxes.Add(SetFocusTarget(target));
            }
            return boxes;
        }

        public FocusBox SetFocusTarget(RectTransform target) {
            FocusBox box = GetFreeBox();
            m_FocusBoxes[box] = true;
            box.SetFocusTarget(target);
            return box;
        }

        private FocusBox GetFreeBox() {
            FocusBox box = null;
            foreach(KeyValuePair<FocusBox, bool> pair in m_FocusBoxes) {
                bool inUse = pair.Value;
                if (!inUse) {
                    box = pair.Key;
                    break;
                }
            }

            if(box == null) {
                box = CreateFocusBox();
            }
            return box;
        }

        public void ReleaseBoxes(params FocusBox[] boxes) {
            foreach(FocusBox box in boxes) {
                ReleaseBox(box);
            }
        }

        private void ReleaseBox(FocusBox box) {
            if (!m_FocusBoxes.ContainsKey(box)) {
                Debug.LogError("Focusbox created outside the manager!", box.gameObject);
                return;
            }

            m_FocusBoxes[box] = false;
            box.Hide();
        }

        private FocusBox CreateFocusBox() {
            FocusBox newBox = Instantiate(template);
            newBox.GetComponent<RectTransform>().SetParent(boxParent);
            m_FocusBoxes.Add(newBox, false);
            newBox.gameObject.SetActive(true);
            return newBox;
        }
    }

}