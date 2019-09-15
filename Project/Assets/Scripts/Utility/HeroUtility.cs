
using System;
using UnityEngine;

namespace Rondo.Generic.Utility {

    public static class HeroUtility {

        private static float LEVEL_EXPONENT = 1.25f;
        private static int BASE_LEVEL_REQUIREMENT = 10;

        public static void CalculateHeroLevel(float totalExperience, out int level, out int expForNextLevel, out float normalizedProgress) {
            float totalExpLeft = totalExperience;
            int expRequired = BASE_LEVEL_REQUIREMENT;
            int lastExpRequired = 0;
            int currentLevel = 1;
            while (totalExpLeft != 0) {
                if (expRequired > totalExpLeft) break;
                totalExpLeft -= expRequired;
                currentLevel++;
                lastExpRequired = expRequired;
                expRequired = Mathf.RoundToInt(expRequired * LEVEL_EXPONENT);
            }

            level = currentLevel;
            expForNextLevel = expRequired;
            normalizedProgress = 1 - (((float)expRequired - totalExpLeft) / expRequired);
        }

        public static int GetTotalExperienceRequiredForLevel(int level) {
            int currentLevel = 1;
            int totalExp = 0;
            int expRequired = BASE_LEVEL_REQUIREMENT;
            while (currentLevel < level) {
                currentLevel++;
                totalExp += expRequired;
                expRequired = Mathf.RoundToInt(expRequired * LEVEL_EXPONENT);
            }
            return totalExp;
        }

    }

}