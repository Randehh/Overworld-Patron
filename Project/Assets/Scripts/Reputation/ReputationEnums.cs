using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Reputation {

    public enum ReputationLevels : int {
        STRANGER = 0,
        FRIENDLY = 2,
        TRUSTED = 4,
        REVERED = 7,
        EXALTED = 10
    }

    public enum ReputationBiases {
        UNKNOWN = -1,
        VILLAGERS,
        GOVERNMENT,
        MONSTER_SLAYING,
        WAR_EFFORT
    }

    public enum ReputationNameConventions {
        UNKNOWN = -1,
        COMPOUND,               //Hellbreakers
        GROUP,                  //Hands of Fate
        POINT_OF_INTEREST,
        TERRITORY
    }

}