using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rondo.QuestSim.Gameplay {

    [RequireComponent(typeof(BoxCollider))]
    public class InteractionZone : MonoBehaviour {

        public UnityEvent onInteract;
        public UnityEvent onExit;
        public Transform alertSign;

        private Vector3 m_AlertSignStartPos;
        private bool m_CallExitFunction = false;

        private void Start() {
            m_AlertSignStartPos = alertSign.localPosition;
        }

        private void Update() {
            alertSign.localPosition = m_AlertSignStartPos + new Vector3(0, Mathf.Sin(Time.time * 3) * 0.1f, 0);
        }

        void OnTriggerEnter(Collider other) {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player == null) return;
            player.activeInteractionZones.Add(this);
        }

        void OnTriggerExit(Collider other) {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player == null) return;
            if(m_CallExitFunction) onExit.Invoke();
            player.activeInteractionZones.Remove(this);

            m_CallExitFunction = false;
        }

        public void Interact() {
            m_CallExitFunction = true;
            onInteract.Invoke();
        }

    }

}