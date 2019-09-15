using Rondo.Generic.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.General {

    public class NightFadeUI : MonoBehaviourSingleton<NightFadeUI> {

        public float enabledValue = 0.75f;

        private Image m_Image;
        private Coroutine m_Routine;

        private void Awake() {
            m_Image = GetComponent<Image>();
            Instance = this;
        }

        public void Enable(Action OnCompleted) {
            SetAlpha(enabledValue, OnCompleted);
        }

        public void Disable(Action OnCompleted) {
            SetAlpha(0, OnCompleted);
        }
        
        private void SetAlpha(float alpha, Action OnCompleted) {
            if (m_Routine != null) StopCoroutine(m_Routine);
            m_Routine = StartCoroutine(FadeToAlpha(alpha, OnCompleted));
        }

        private IEnumerator FadeToAlpha(float alpha, Action OnCompleted) {
            m_Image.raycastTarget = true;

            Color endColor = m_Image.color;
            endColor.a = alpha;
            while (Mathf.Abs(m_Image.color.a - alpha) >= 0.01f) {
                m_Image.color = Color.Lerp(m_Image.color, endColor, 0.1f);
                yield return null;
            }
            m_Image.color = endColor;

            m_Image.raycastTarget = alpha != 0;

            OnCompleted();
        }

    }

}