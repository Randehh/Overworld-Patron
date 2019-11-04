using Rondo.Generic.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Gameplay {

    public class CameraController : MonoBehaviourSingleton<CameraController> {

        public Transform target;
		public Transform targetLookAt;
        public float followSpeed = 0.1f;

		private float m_CurrentFollowSpeed = 0;

        private void Start() {
			m_CurrentFollowSpeed = followSpeed;

			transform.position = target.position;
        }

		public void SetTarget(Transform t) {
			target = t;
		}
		
		public void SetFollowSpeed(float speed) {
			followSpeed = speed;
		}

        void FixedUpdate() {
			m_CurrentFollowSpeed = Mathf.Lerp(m_CurrentFollowSpeed, followSpeed, 0.5f);

            transform.position = Vector3.Lerp(transform.position, target.position, m_CurrentFollowSpeed);

			if (targetLookAt != null) {
				transform.LookAt(targetLookAt);
			} else {
				transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, m_CurrentFollowSpeed);
			}
        }
    }

}