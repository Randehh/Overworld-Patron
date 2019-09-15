using Rondo.QuestSim.Heroes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.ScriptableObjects {

    [CreateAssetMenu]
    public class PlayerIconDatabase : ScriptableObject {
        public Sprite deathIcon;
        public Sprite questingIcon;
        public Sprite idleIcon;
        public Sprite unknownIcon;
        public Sprite woundedIcon;

        public Sprite GetSpriteForStatus(HeroStates state) {
            switch (state) {
                case HeroStates.UNDISCOVERED:
                    return unknownIcon;
                case HeroStates.IDLE:
                    return idleIcon;
                case HeroStates.ON_QUEST:
                    return questingIcon;
                case HeroStates.DEAD:
                    return deathIcon;
                case HeroStates.WOUNDED:
                    return woundedIcon;
                default:
                    return unknownIcon;
            }
        }
    }

}