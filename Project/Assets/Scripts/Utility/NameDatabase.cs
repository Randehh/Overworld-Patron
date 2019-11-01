using Rondo.Generic.Utility;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Utility {

    public static class NameDatabase {

        public static string GetCompoundName() {
            string[] partOne = new string[] {
                "Stone",
                "Grass",
                "Frost",
                "Onyx",
                "Ender",
                "Earth",
                "Bronze",
                "Wind",
                "Steel",
                "Wood",
                "Light",
                "Pearl",
                "Ivory",
                "Spring",
                "Gold",
                "Silver",
                "Solar",
                "Sun",
                "Moon",
                "Bright",
                "Curse",
                "Dark",
                "Red",
                "Char",
                "Night",
                "Chaos",
                "Hollow",
                "Doom"
            };

            string[] partTwo = new string[] {
                "blade",
                "drifter",
                "weaver",
                "spell",
                "seeker",
                "fall",
                "throne",
                "hide",
                "tusk",
                "scale",
                "face",
                "hill",
                "song",
                "blossom",
                "talon",
                "wolf",
                "stream",
                "bringer",
                "stride",
                "dreamer",
                "shield",
                "mace",
                "caller",
                "reach",
                "ford",
                "hand",
                "peak",
                "song",
                "hunter",
                "splitter",
                "seeker",
                "slayer",
                "shade",
                "stalker",
                "caller",
                "blood",
                "dagger",
                "mark",
                "breaker",
                "shatter"
            };

            return partOne[Random.Range(0, partOne.Length)] + partTwo[Random.Range(0, partTwo.Length)];
        }

        public static string GetTerritoryName() {
            int type = Random.Range(0, 1);
            if (type == 0) {
                string[] partOne = new string[] {
                    "Swan",
                    "Rock",
                    "Bear",
                    "Frey",
                    "Honey",
                    "Last",
                    "Wolf",
                    "Silk",
                    "Wind",
                    "Sly",
                    "Bone",
                    "East",
                    "North",
                    "West",
                    "South",
                    "Snow",
                    "Rose"
                };

                string[] partTwo = new string[] {
                    "burgh",
                    "rock",
                    "town",
                    "brook",
                    "well",
                    "bury",
                    "mond",
                    "gulch",
                    "fall",
                    "point",
                    "hall",
                    "hollow",
                    "wood",
                    "bay",
                    "more",
                    "valley"
                };

                return partOne[Random.Range(0, partOne.Length)] + partTwo[Random.Range(0, partTwo.Length)];
            } else if (type == 1) {

            }

            return "";
        }

        public static string GetPointOfInterestName() {
            string[] partOne = new string[] {
                "Stone",
                "Grass",
                "Frost",
                "Onyx",
                "Ender",
                "Cunning",
                "Earth",
                "Bronze",
                "Wind",
                "Steel",
                "Wood",
                "Nameless"
             };

            string[] partTwo = new string[] {
                "Estate",
                "Mansion",
                "Residence",
                "Chateau",
                "Manor",
                "Plains",
                "Lands",
                "Wastes",
                "Fields",
                "Range",
                "Wilds",
                "Vault",
                "Haven"
            };
            return "The " + partOne[Random.Range(0, partOne.Length)] + " " + partTwo[Random.Range(0, partTwo.Length)];
        }

        public static string GetGroupName() {
            int type = Random.Range(0, 2);
            if (type == 0) {                        //The Something Somethings
                string[] partOne = new string[] {
                    "Serpent",
                    "Forsaken",
                    "Violet",
                    "Cobalt",
                    "Wild",
                    "Last",
                    "Spider",
                    "Demon",
                    "Emerald",
                    "Flame",
                    "Royal",
                    "Faceless",
                    "United",
                    "Voiceless",
                    "Shadow",
                    "Raven",
                    "White",
                    "Nameless",
                    "Shattered",
                    "Unseen",
                    "Infinite",
                    "Eternal"
                };

                string[] partTwo = new string[] {
                    "Crows",
                    "Whisperers",
                    "Tribe",
                    "Clan",
                    "Brotherhood",
                    "Crew",
                    "Hands",
                    "Dreamers",
                    "Company",
                    "Syndicate",
                    "Lions",
                    "Ones",
                    "Angels",
                    "Stalkers",
                    "Swords",
                    "Hawks",
                    "Shields",
                    "Scars",
                    "Shapers",
                    "Alliance",
                    "Brigade"
                };

                return "The " + partOne[Random.Range(0, partOne.Length)] + " " + partTwo[Random.Range(0, partTwo.Length)];
            } else if (type == 1) {                     //The Somethings of Something
                string[] partOne = new string[] {
                    "Serpents",
                    "Seekers",
                    "Voices",
                    "Rangers",
                    "Outsiders",
                    "Shades",
                    "Shields",
                    "Spears",
                    "Swords",
                    "Hands",
                    "Eyes",
                    "Visions",
                    "Hammers",
                    "Hunters",
                    "Sentinels",
                    "Ravens",
                    "Guardians",
                    "Striders",
                    "Vanguard",
                    "Phantoms",
                    "Order",
                    "Roar",
                    "Dirge",
                    "Resurgence",
                    "Men",
                    "Tempest",
                    "Refugees",
                    "Bandits",
                    "Scourge"
                };

                string[] partTwo = new string[] {
                    "of Virtue",
                    "of Fate",
                    "of the Light",
                    "of the Weak",
                    "of Strength",
                    "of Promise",
                    "of the Lost Age",
                    "of the Void",
                    "of the Hawk",
                    "of the Boar",
                    "of the Stag",
                    "of the Lost",
                    "of Dawn",
                    "of Fury",
                    "of the Serene",
                    "of the World",
                    "of the Hopeful"
                };

                return partOne[Random.Range(0, partOne.Length)] + " " + partTwo[Random.Range(0, partTwo.Length)];
            }

            return "";
        }

        public static string GetItemName(GameItem item) {
            GameItemRarity rarity = item.Rarity;
            GameItemTypes type = item.ItemType;

            string typeString = type.ToString().ToCamelCase();

            switch (rarity) {
                default:
                case GameItemRarity.COMMON:
                    return "Trainee's " + typeString;
                case GameItemRarity.UNCOMMON:
                    return "Steel " + typeString;
                case GameItemRarity.RARE:
                    return "Enchanted " + typeString;
                case GameItemRarity.EPIC:
                    return "Masterwork " + typeString;
                case GameItemRarity.LEGENDARY:
					bool useAdjective = Random.Range(0, 2) == 0;
					string adjective = useAdjective ? GetAdjective() + " " : "";

					return GetCompoundName() + ", " + adjective + typeString + " of " + GetTerritoryName();
            }
        }

        private static WeightedRandom<int> m_HeroNameFirstNameLenghts = new WeightedRandom<int>(
            new int[] { 1, 2, 3, 4 },
            new int[] { 1, 8, 3, 1 });
        private static WeightedRandom<int> m_HeroNameSurNameLenghts = new WeightedRandom<int>(
            new int[] { 2, 3 },
            new int[] { 3, 1 });
        public static string GetHeroName() {
            string[] firstNameSyllables = new string[] {
                    "gar",
                    "lal",
                    "has",
                    "fo",
                    "sin",
                    "nor",
                    "dar",
                    "khe",
                    "gro",
                    "bun",
                    "dag",
                    "kan",
                    "orn",
                    "gog",
                    "san",
                    "ug",
                    "don",
                    "aen",
                    "an",
                    "wil",
                    "hel",
                    "oli",
                    "rae",
                    "lyn",
                    "tha",
                    "aga",
                    "gen",
                    "ne",
                    "ai",
                    "ka",
                    "in",
                    "jar",
                    "ym",
                    "lus",
                    "yl",
                    "cid",
                    "fed",
                    "va",
                    "mo",
                    "ka",
                    "ys",
                    "ne",
                    "ro",
                    "dan",
                    "te",
                    "cho",
                    "cha",
                    "en",
                    "gal",
                    "ill",
                    "id",
                    "art",
                    "has",
                    "tryn",
                    "myr",
                    "mae",
                    "as",
                    "te",
                    "ra",
                    "es",
                    "ta",
                    "he",
                    "to",
                    "sko",
                    "ke",
                    "na",
                    "sa",
                    "se",
                    "po",
                    "pa",
                    "mar",
                    "ia",
                    "amy",
                    "eb",
                    "rie",
                    "tas",
                    "dju",
                    "ra",
                    "gas",
                    "coi",
                    "gne",
                    "lau",
                    "ren",
                    "ce",
                    "lud",
                    "wig",
                    "lo",
                    "ga",
                    "mi",
                    "co",
                    "lash",
                    "ame",
                    "lia",
                    "lea"
                };

            string[] surNameSyllables = new string[] {
                    "sein",
                    "lath",
                    "runn",
                    "than",
                    "frun",
                    "stam",
                    "tarl",
                    "rugg",
                    "glath",
                    "sten",
                    "naeg",
                    "sith",
                    "joth",
                    "thus",
                    "hago",
                    "osho",
                    "gohe",
                    "thel",
                    "ebli",
                    "anoz",
                    "kiz",
                    "gomi",
                    "gehr",
                    "lush",
                    "ota",
                    "olda",
                    "vol"
                };

            string[] surNameEndings = new string[] {
                    "man",
                    "son",
                    "holm",
                    "hall",
                    "ham",
                    "ston",
                    "sey",
                    "ford",
                    "ton",
                    "ley",
                    "don",
                    "den",
                    "say",
                    "lowe",
                    "cett",
                    "lin",
                    "tun",
                    "lyn",
                    "dra",
                    "dulm",
                    "dil",
                    "mor",
                    "ian",
                    "zin",
                    "dral",
                    "nar",
                    "er",
                    "ysh",
                    "shel",
                    "rus",
                    "lix",
                    "eth",
                    "dras"
                };

            string firstName = "";
            string surName = "";
            int firstNameSyllableCount = m_HeroNameFirstNameLenghts.GetRandomValue();
            for (int i = 0; i < firstNameSyllableCount; i++) {
                firstName += firstNameSyllables[Random.Range(0, firstNameSyllables.Length)];
                if (Random.Range(0, 15) == 0 && i != firstNameSyllableCount - 1) {
                    if (Random.Range(0, 2) == 0) firstName += "'";
                    else firstName += "-";
                }
            }

            int surNameSyllableCount = m_HeroNameSurNameLenghts.GetRandomValue();
            for (int i = 0; i < surNameSyllableCount; i++) {
                if (i == surNameSyllableCount - 1) surName += surNameEndings[Random.Range(0, surNameEndings.Length)];
                else surName += surNameSyllables[Random.Range(0, surNameSyllables.Length)];
                if (Random.Range(0, 15) == 0 && i != surNameSyllableCount - 1) surName += "'";
            }

            Random.InitState((int)Time.time + firstName.GetHashCode() + surName.GetHashCode());

            return firstName.ToCamelCase() + " " + surName.ToCamelCase();
        }

		public static string GetAdjective() {
			string[] adjective = new string[] {
				"Stone",
				"Grass",
				"Frost",
				"Onyx",
				"Ender",
				"Cunning",
				"Earth",
				"Bronze",
				"Wind",
				"Steel",
				"Wood",
				"Nameless"
			 };
			return adjective[Random.Range(0, adjective.Length)];
		}
	}
}