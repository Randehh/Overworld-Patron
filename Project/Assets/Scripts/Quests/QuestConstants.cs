using Rondo.Generic.Utility;
using UnityEngine;

namespace Rondo.QuestSim.Quests {

    public static class QuestConstants {
        public static float BASE_POWER_LEVEL = 25f;
        public static float POWER_PER_LEVEL = 25f;
        public static float LEVELS_PER_STAR = 5;
        public static float POWER_PER_STAR = POWER_PER_LEVEL / LEVELS_PER_STAR;

        public static float GetPowerLevel(float stars) {
            return stars * POWER_PER_STAR;

        }
    }
}