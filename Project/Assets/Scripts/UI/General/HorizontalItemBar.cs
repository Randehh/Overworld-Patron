using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.UI.General {

    public class HorizontalItemBar : MonoBehaviour {

        public GameObject templateIcon;
        public RectTransform arrowIndicator;

        private Dictionary<GameObject, BarItem> m_Items = new Dictionary<GameObject, BarItem>();
        private BarItem m_Selected = null;
        private BarItem m_Rollover = null;

        private Vector2 m_ScaleDefault = new Vector2(0.75f, 0.75f);
        private Vector2 m_ScaleRollover = new Vector2(1.25f, 1.25f);
        private Vector2 m_ScaleSelected = new Vector2(1.75f, 1.75f);

        private void Awake() {
            templateIcon.SetActive(false);
        }

        private void Update() {
            foreach(BarItem item in m_Items.Values) {
                RectTransform t = item.icon;
                Vector2 targetScale = m_ScaleDefault;
                if (m_Selected == item) targetScale = m_ScaleSelected;
                else if (m_Rollover == item) targetScale = m_ScaleRollover;
                t.localScale = Vector2.Lerp(t.localScale, targetScale, 10 * Time.deltaTime);
            }

            if(m_Selected != null) {
                Vector3 targetPos = arrowIndicator.parent.InverseTransformPoint(m_Selected.hitbox.transform.position);
                //targetPos.y = -20;
                Vector3 currentPos = arrowIndicator.localPosition;
                arrowIndicator.localPosition = Vector3.Lerp(currentPos, targetPos, 10 * Time.deltaTime);
            }
            
        }

        public void SetItemCount(int count) {
            if (m_Items.Count > count) {
                for (int i = m_Items.Count - 1; i > count; i--) RemoveItem(i);
            } else if(m_Items.Count < count) {
                for(int i = m_Items.Count; i <= count; i++) CreateItem();
            }
            Reset();
        }

        public void Reset() {
            if (m_Items.Count == 0) return;
            Dictionary<GameObject, BarItem>.Enumerator enumerator = m_Items.GetEnumerator();
            enumerator.MoveNext();
            m_Selected = enumerator.Current.Value;
        }
        
        private void RemoveItem(int index) {
            List<GameObject> keys = new List<GameObject>(m_Items.Keys);
            RemoveItem(keys[index]);
        }

        private void RemoveItem(GameObject obj) {
            m_Items.Remove(obj);
            DestroyImmediate(obj);
        }

        private GameObject CreateItem() {
            GameObject newObj = Instantiate(templateIcon);
            newObj.SetActive(true);
            newObj.transform.SetParent(templateIcon.transform.parent);
            newObj.transform.localScale = Vector2.one;
            m_Items.Add(newObj, new BarItem(newObj, newObj.transform.GetChild(0).GetComponentInChildren<RectTransform>()));
            return newObj;
        }

        public void SetSelected(int index) {
            index = Mathf.Clamp(index, 0, m_Items.Count - 1);
            m_Selected = new List<BarItem>(m_Items.Values)[index];
        }

        /*
         * Calls from the Event Trigger
         */
        public void OnPointerEnter(GameObject obj) {
            m_Rollover = m_Items[obj];
        }

        public void OnPointerExit(GameObject obj) {
            m_Rollover = null;
        }

        public void OnPointerUp(GameObject obj) {
            m_Selected = m_Items[obj];
        }

        [ContextMenu("Test")]
        private void Test() {
            SetItemCount(Random.Range(2, 10));
        }


        private class BarItem {
            public GameObject hitbox;
            public RectTransform icon;

            public BarItem(GameObject hitbox, RectTransform icon) {
                this.hitbox = hitbox;
                this.icon = icon;
            }
        }
    }

}