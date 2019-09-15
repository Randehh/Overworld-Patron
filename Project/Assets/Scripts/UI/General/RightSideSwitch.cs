using Rondo.Generic.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.UI.General {

    public class RightSideSwitch : GameObjectSwitch {

        public static RightSideSwitch Instance;

        public override void Awake() {
            base.Awake();
            Instance = this;
        }

        public void ActivateObject(GameObject obj, bool disableIfSame = true) {
            if (m_CurrentObject == obj) {
                if (disableIfSame) {
                    m_CurrentObject.SetActive(false);
                    m_CurrentObject = null;
                }
            } else {
                base.ActivateObject(obj);
            }
        }
    }

}