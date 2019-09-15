using Rondo.Generic.Utility;
using Rondo.QuestSim.Gameplay;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.UI.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Heroes {

    public static class HeroManager {

        private static Dictionary<HeroInstance, QuestSourceFaction> m_Heroes = new Dictionary<HeroInstance, QuestSourceFaction>();

        public static void Initialize() {
            DayManager.Instance.OnNextDay += UpdateWoundedHeroes;
        }

        public static void AddHero(HeroInstance instance, QuestSourceFaction faction) {
            if (m_Heroes.ContainsKey(instance)) return;
            m_Heroes.Add(instance, faction);
            ReputationUI.Instance.CreateHeroInstance(instance);
        }

        public static HeroInstance GetRandomHero() {
            return GetAllHeroes().GetRandom();
        }

        public static List<HeroInstance> GetAllHeroes() {
            return new List<HeroInstance>(m_Heroes.Keys);
        }

        public static List<HeroInstance> GetAvailableHeroes() {
            List<HeroInstance> heroes = new List<HeroInstance>();
            List<HeroInstance> allHeroes = GetAllHeroes();

            foreach (HeroInstance hero in allHeroes) {
                if (hero.HeroState != HeroStates.IDLE) {
                    continue;
                }

                heroes.Add(hero);
            }
            
            return heroes;
        }

        public static int GetHeroCount() {
            return m_Heroes.Count;
        }

        public static QuestSourceFaction GetHeroFaction(HeroInstance hero) {
            if (!m_Heroes.ContainsKey(hero)) return null;
            return m_Heroes[hero];
        }

        public static void SetHeroToState(HeroInstance hero, HeroStates state, bool force = false) {
            if (force) {
                hero.HeroState = state;
                return;
            }

            switch (state) {
                case HeroStates.IDLE:
                case HeroStates.ON_QUEST:
                    if (hero.HeroState == HeroStates.DEAD ||
                        hero.HeroState == HeroStates.WOUNDED) {
                        return;
                    }
                    hero.HeroState = state;
                    break;

                case HeroStates.WOUNDED:
                case HeroStates.DEAD:
                case HeroStates.UNDISCOVERED:
                    hero.HeroState = state;
                    break;
            }
        }

        public static void UpdateWoundedHeroes() {
            foreach (HeroInstance hero in m_Heroes.Keys) {
                if (hero.HeroState != HeroStates.WOUNDED) continue;
                hero.WoundedDays--;
                if(hero.WoundedDays == 0) {
                    hero.HeroState = HeroStates.IDLE;
                }
            }
        }

    }

}