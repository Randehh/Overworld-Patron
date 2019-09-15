using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Gameplay {

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerController : MonoBehaviour {

        public float maxMoveSpeed = 5;
        public float acceleration = 0.5f;
        public List<InteractionZone> activeInteractionZones = new List<InteractionZone>();

        private Rigidbody m_Body;

        private void Awake() {
            m_Body = GetComponent<Rigidbody>();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                foreach(InteractionZone iZone in activeInteractionZones) {
                    iZone.Interact();
                }
            }
        }

        private void FixedUpdate() {
            Vector2 movement = new Vector2();
            if (Input.GetKey(KeyCode.A)) movement.x -= 1;
            if (Input.GetKey(KeyCode.D)) movement.x += 1;
            if (Input.GetKey(KeyCode.S)) movement.y -= 1;
            if (Input.GetKey(KeyCode.W)) movement.y += 1;
            movement *= acceleration;

            m_Body.AddForce(new Vector3(movement.x, 0, movement.y));
        }
    }

}