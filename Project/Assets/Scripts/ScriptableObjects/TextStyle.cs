using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.ScriptableObjects {

    [CreateAssetMenu]
    public class TextStyle : ScriptableObject {
        public int size = 16;
        public Color color = new Color(0.95f, 0.95f, 0.95f);
        public bool isBold = false;
        public bool isItalic = false;
    }

}
 
 
 
 