using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests;

namespace Rondo.QuestSim.Heroes {

	public enum HeroClasses {
		WARRIOR,
		SORCERER,
		ROGUE,
		HUNTER
	}

	public enum HeroQuestAffinity {
		LOW,
		MEDIUM,
		HIGH
	}

	public static class HeroClassesUtility {

		public static float GetExperienceModifier(this HeroClasses heroClass, QuestTypes questType) {
			switch (heroClass.GetQuestTypeAffinity(questType)) {
				case HeroQuestAffinity.LOW: return 0.75f;
				case HeroQuestAffinity.MEDIUM: return 1f;
				case HeroQuestAffinity.HIGH: return 1.25f;

				default:
					throw new System.Exception("Quest modifier not defined for quest type " + questType.ToString());
			}
		}

		public static float GetSuccessRateModifier(this HeroClasses heroClass, QuestTypes questType) {
			switch (heroClass.GetQuestTypeAffinity(questType)) {
				case HeroQuestAffinity.LOW: return -5;
				case HeroQuestAffinity.MEDIUM: return 0;
				case HeroQuestAffinity.HIGH: return 5;

				default:
					throw new System.Exception("Quest modifier not defined for quest type " + questType.ToString());
			}
		}

		public static HeroQuestAffinity GetQuestTypeAffinity(this HeroClasses heroClass, QuestTypes questType) {
			switch (questType) {
				case QuestTypes.ASSASSINATION:
					switch (heroClass) {
						case HeroClasses.WARRIOR: return HeroQuestAffinity.MEDIUM;
						case HeroClasses.HUNTER: return HeroQuestAffinity.MEDIUM;
						case HeroClasses.SORCERER: return HeroQuestAffinity.LOW;
						case HeroClasses.ROGUE: return HeroQuestAffinity.HIGH;
						default: throw new System.Exception("Hero class not defined " + heroClass.ToString());
					}

				case QuestTypes.MERCENARY_WORK:
					switch (heroClass) {
						case HeroClasses.WARRIOR: return HeroQuestAffinity.HIGH;
						case HeroClasses.HUNTER: return HeroQuestAffinity.LOW;
						case HeroClasses.SORCERER: return HeroQuestAffinity.MEDIUM;
						case HeroClasses.ROGUE: return HeroQuestAffinity.MEDIUM;
						default: throw new System.Exception("Hero class not defined " + heroClass.ToString());
					}

				case QuestTypes.ITEM_GATHERING:
					switch (heroClass) {
						case HeroClasses.WARRIOR: return HeroQuestAffinity.LOW;
						case HeroClasses.HUNTER: return HeroQuestAffinity.MEDIUM;
						case HeroClasses.SORCERER: return HeroQuestAffinity.HIGH;
						case HeroClasses.ROGUE: return HeroQuestAffinity.MEDIUM;
						default: throw new System.Exception("Hero class not defined " + heroClass.ToString());
					}

				case QuestTypes.MONSTER_HUNTING:
					switch (heroClass) {
						case HeroClasses.WARRIOR: return HeroQuestAffinity.MEDIUM;
						case HeroClasses.HUNTER: return HeroQuestAffinity.HIGH;
						case HeroClasses.SORCERER: return HeroQuestAffinity.MEDIUM;
						case HeroClasses.ROGUE: return HeroQuestAffinity.LOW;
						default: throw new System.Exception("Hero class not defined " + heroClass.ToString());
					}

				default:
					throw new System.Exception("Quest modifier not defined for quest type " + questType.ToString());
			}
		}
	}
}