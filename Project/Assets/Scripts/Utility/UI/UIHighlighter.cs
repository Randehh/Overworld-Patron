using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.Generic.Utility.UI {

    public class UIHighlighter : MonoBehaviourSingleton<UIHighlighter> {

        public float pulseSpeed = 3;
        public float lerpSpeed = 5;

        [Header("Default colors")]
        public Color greenHightlightColor;
        public Color redHighlightColor;

        private Dictionary<string, HighlightGroup> m_Groups = new Dictionary<string, HighlightGroup>();

        private void Update() {
            foreach(HighlightGroup hs in m_Groups.Values) {
                hs.Update(pulseSpeed, lerpSpeed);
            }
        }

        public HighlightGroup GetGroup(string id) {
            if (m_Groups.ContainsKey(id)) {
                return m_Groups[id];
            }else {
                HighlightGroup newGroup = new HighlightGroup(id);
                m_Groups.Add(id, newGroup);
                return newGroup;
            }
        }

        public void RemoveGroup(string id) {
            if (!m_Groups.ContainsKey(id)) return;
            m_Groups[id].RemoveAll();
            m_Groups.Remove(id);
        }

        public class HighlightGroup {

            private string m_GroupID;
            private Dictionary<MonoBehaviour, IHighlightSettings> m_HighlightObjects = new Dictionary<MonoBehaviour, IHighlightSettings>();

            public HighlightGroup(string id) {
                m_GroupID = id;
            }

            public void Update(float pulseSpeed, float lerpSpeed) {
                if (m_HighlightObjects.Count == 0) return;

                float value = Mathf.Sin(Time.time * pulseSpeed).Map(-1, 1, 0, 1);
                foreach (IHighlightSettings hs in m_HighlightObjects.Values) {
                    hs.Update(value, lerpSpeed);
                }
            }

            public void AddObjects(MonoBehaviour[] objects, Color hightlightColor, Color startColor) {
                return;
                foreach (MonoBehaviour i in objects) AddObject(i, hightlightColor, startColor);
            }

            public void AddObject(MonoBehaviour obj, Color hightlightColor, Color startColor) {
                if (m_HighlightObjects.ContainsKey(obj)) {
                    m_HighlightObjects[obj].SetHighlightColor(hightlightColor);
                } else {
                    if(obj is TextMeshProUGUI) {
                        m_HighlightObjects.Add(obj, new HighlightSettingsText(obj as TextMeshProUGUI, startColor, hightlightColor));
                    } else if(obj is Graphic) {
                        m_HighlightObjects.Add(obj, new HighlightSettingsGraphic(obj as Graphic, startColor, hightlightColor));
                    } else {
                        Debug.LogWarning("Attempted to add an object of type " + obj.GetType().Name + " as highlighted object, but it's not supported!");
                    }
                }
            }

            public void RemoveObjects(MonoBehaviour[] objects) {
                foreach (MonoBehaviour g in objects) RemoveObject(g);
            }

            public void RemoveObject(MonoBehaviour obj) {
                if (!m_HighlightObjects.ContainsKey(obj)) return;
                m_HighlightObjects[obj].Reset();
                m_HighlightObjects.Remove(obj);
            }

            public void RemoveAll() {
                List<MonoBehaviour> keys = new List<MonoBehaviour>(m_HighlightObjects.Keys);
                for (int i = keys.Count - 1; i >= 0; i--) {
                    RemoveObject(keys[i]);
                }
            }
        }

        public abstract class IHighlightSettings {

            protected Color m_StartColor;
            protected Color m_EndColor;
            protected Color m_EndColorTarget;

            protected Color m_CurrentColor;

            public IHighlightSettings(Color startColor, Color endColor) {
                m_StartColor = startColor;
                m_EndColor = endColor;
                m_EndColorTarget = endColor;

                m_CurrentColor = m_StartColor;
            }

            public virtual void Update(float value, float lerpSpeed) {
                m_EndColor = Color.Lerp(m_EndColor, m_EndColorTarget, lerpSpeed * Time.time);
                m_CurrentColor = Color.Lerp(m_StartColor, m_EndColor, value);
            }

            public void SetHighlightColor(Color c) {
                m_EndColorTarget = c;
            }

            public virtual void Reset() { }
        }

        private class HighlightSettingsGraphic : IHighlightSettings {

            private Graphic m_Object;

            public HighlightSettingsGraphic(Graphic image, Color startColor, Color endColor) : base(startColor, endColor) {
                m_Object = image;
            }

            public override void Update(float value, float lerpSpeed) {
                base.Update(value, lerpSpeed);
                m_Object.color = Color.Lerp(m_Object.color, m_CurrentColor, 1 * Time.time);
            }

            public override void Reset() {
                m_Object.color = m_StartColor;
            }
        }

        private class HighlightSettingsText : IHighlightSettings {

            private TextMeshProUGUI m_Object;

            public HighlightSettingsText(TextMeshProUGUI text, Color startColor, Color endColor) : base(startColor, endColor) {
                m_Object = text;
            }

            public override void Update(float value, float lerpSpeed) {
                base.Update(value, lerpSpeed);
                m_Object.color = Color.Lerp(m_Object.color, m_CurrentColor, 1 * Time.time);
            }

            public override void Reset() {
                m_Object.color = m_StartColor;
            }
        }
    }

}