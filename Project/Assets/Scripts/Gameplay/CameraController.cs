using Rondo.Generic.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Gameplay {

    public class CameraController : MonoBehaviourSingleton<CameraController> {

        public Transform target;
        public float followSpeed = 0.1f;
        public Vector3 offset = new Vector3(0, 15, -15);

        private Camera m_Camera;
        private Vector3 m_LookatTarget;

        private void Awake() {
            m_Camera = GetComponent<Camera>();
            m_LookatTarget = target.position;
        }

        private void Start() {
            transform.position = target.position + offset;
        }

        void FixedUpdate() {
            Vector3 targetPos = target.position;
            transform.position = Vector3.Lerp(transform.position, targetPos + offset, followSpeed);

            m_LookatTarget = Vector3.Lerp(m_LookatTarget, targetPos, followSpeed * 1.5f);
            transform.LookAt(m_LookatTarget);
        }
    }

}