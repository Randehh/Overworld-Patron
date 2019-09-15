using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests;

namespace Rondo.QuestSim.Heroes
{

	public enum HeroClasses {
        WARRIOR,
        SORCERER,
        ROGUE,
        HUNTER
    }

    public static class HeroClassesUtility {

        public static float GetQuestModifier(this HeroClasses heroClass, QuestTypes questType) {
            float modifier = 0.5f;
            switch (questType) {
                case QuestTypes.ASSASSINATION:
                    if (heroClass == HeroClasses.WARRIOR) modifier = 0.4f;
                    if (heroClass == HeroClasses.HUNTER) modifier = 0.75f;
                    if (heroClass == HeroClasses.SORCERER) modifier = 0.2f;
                    if (heroClass == HeroClasses.ROGUE) modifier = 1f;
                    break;

                case QuestTypes.ITEM_DELIVERY:
                    if (heroClass == HeroClasses.WARRIOR) modifier = 0.4f;
                    if (heroClass == HeroClasses.HUNTER) modifier = 0.75f;
                    if (heroClass == HeroClasses.SORCERER) modifier = 0.5f;
                    if (heroClass == HeroClasses.ROGUE) modifier = 0.5f;
                    break;

                case QuestTypes.ITEM_GATHERING:
                    if (heroClass == HeroClasses.WARRIOR) modifier = 0.1f;
                    if (heroClass == HeroClasses.HUNTER) modifier = 1f;
                    if (heroClass == HeroClasses.SORCERER) modifier = 0.8f;
                    if (heroClass == HeroClasses.ROGUE) modifier = 0.25f;
                    break;

                case QuestTypes.MONSTER_HUNTING:
                    if (heroClass == HeroClasses.WARRIOR) modifier = 1;
                    if (heroClass == HeroClasses.HUNTER) modifier = 0.5f;
                    if (heroClass == HeroClasses.SORCERER) modifier = 0.75f;
                    if (heroClass == HeroClasses.ROGUE) modifier = 0.25f;
                    break;

                default:
                    throw new System.Exception("Quest modifier not defined for quest type " + questType.ToString());
            }

            return modifier.Map(0, 1, 0.8f, 1.2f);
        }

    }

}