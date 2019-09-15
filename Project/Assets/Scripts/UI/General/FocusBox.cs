using Rondo.Generic.Utility;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.General {

    public class FocusBox : MonoBehaviour {

        public AnimationCurve scaleCurve;
        public AnimationCurve alphaCurve;
        public float duration = 1.5f;

        private RectTransform m_RectTransform;
        private Image m_Image;
        private Coroutine m_QuestStatusRoutine;

        private RectTransform m_NextFocus;

        private void Awake() {
            m_RectTransform = GetComponent<RectTransform>();
            m_Image = GetComponent<Image>();
        }

        private void Start() {
            SetAlpha(0);
        }

        public void Hide() {
            StopStatusRoutine();
        }

        public void SetFocusTarget(RectTransform focus) {
            m_NextFocus = focus;
            StopStatusRoutine();
            m_QuestStatusRoutine = StartCoroutine(StatusRoutine());
        }

        private IEnumerator StatusRoutine() {
            yield return null;

            m_RectTransform.position = m_NextFocus.position;
            Vector2 sizeTarget = m_NextFocus.rect.size;
            Vector2 sizeStart = sizeTarget + new Vector2(100, 100);

            float startTime = Time.time;
            float endTime = startTime + duration;

            while (Time.time < endTime) {
                float scaler = TimeUtilities.GetNormalizedTime(startTime, duration, Time.time);
                m_RectTransform.sizeDelta = Vector2.Lerp(sizeStart, sizeTarget, scaleCurve.Evaluate(scaler));
                SetAlpha(alphaCurve.Evaluate(scaler));
                yield return null;
            }

            startTime = Time.time;
            while (true) {
                //SetAlpha(Mathf.Lerp(m_Image.color.a, Mathf.Sin((startTime + Time.time) * 3).Map(-1, 1, 0.5f, 1), Time.deltaTime * 5));
                SetAlpha(1);
                yield return null;
            }
        }

        private void StopStatusRoutine() {
            if (m_QuestStatusRoutine != null) {
                StopCoroutine(m_QuestStatusRoutine);
                m_QuestStatusRoutine = null;
            }
            SetAlpha(0);
        }

        private void SetAlpha(float a) {
            Color c = Color.Lerp(Color.white, Color.black, Mathf.Sin(Time.time * 5).Map(-1, 1, 0, 1));
            c.a = a;
            m_Image.color = c;
        }
    }

}