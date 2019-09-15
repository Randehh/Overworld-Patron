using System.Collections.Generic;
using UnityEngine;

namespace Rondo.Generic.Utility {

    public class GameObjectSwitch : MonoBehaviour {

        public List<GameObject> overrideObjectList;
        public List<GameObject> toIgnore;

        protected List<GameObject> m_Objects = new List<GameObject>();
        protected GameObject m_CurrentObject = null;

        public virtual void Awake() {
            if(overrideObjectList.Count != 0) {
                m_Objects = overrideObjectList;
            } else {
                foreach (Transform child in transform) {
                    if (toIgnore.Contains(child.gameObject)) continue;
                    m_Objects.Add(child.gameObject);
                }
            }
        }

        public virtual void ActivateObject(GameObject obj) {
            if (obj != null && !m_Objects.Contains(obj)) {
                Debug.Log("Tried activating " + obj.name + ", but it's not included in this switch.", gameObject);
                return;
            }

            if(m_CurrentObject != null) {
                m_CurrentObject.SetActive(false);
                m_CurrentObject = null;
            }

            if(obj != null) {
                obj.SetActive(true);
                m_CurrentObject = obj;
            }

        }

    }
}
