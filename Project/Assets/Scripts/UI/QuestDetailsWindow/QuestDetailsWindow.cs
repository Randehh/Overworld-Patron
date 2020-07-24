using Rondo.Generic.Utility;
using Rondo.Generic.Utility.UI;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Quests.Rewards;
using Rondo.QuestSim.UI.ActiveQuests;
using Rondo.QuestSim.UI.General;
using Rondo.QuestSim.UI.Reputation;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.PostedQuests {

    public class QuestDetailsWindow : MonoBehaviourSingleton<QuestDetailsWindow> {

        public static string HIGHLIGHT_GROUP_ID = "quest_details_hero";

        public TextMeshProUGUI questTitle;
        public TextMeshProUGUI durationText;
        public TextMeshProUGUI difficultyText;
        public TextMeshProUGUI successText;
        public Button closeButton;
        public Button skipButton;
        public Button cancelButton;
        public Button acceptButton;
        public Button completeButton;
        public Button postButton;
        public QuestDetailsHero heroSectionTemplate;

        [Header("Handler rewards")]
        public TextMeshProUGUI handlerGoldReward;
        public GameItemInstanceUI handlerRewardItemInstance;
        public TextMeshProUGUI handlerAdditionalReward;
        public GameObject handlerItemRewardParent;
        public GameObject handlerAdditionalRewardParent;

        [Header("Parents")]
        public RectTransform parentHeroes;
		public RectTransform parentSuccess;

        public Action OnWindowClose = delegate { };

        public QuestInstance CurrentQuest { get; set; }
        public QuestDetailsWindowMode WindowMode { get; set; }
        public HeroInstance[] SelectedHeroes { get; set; }
        public int[] SelectedGoldRewards { get; set; }

        public bool[] PostButtonStatuses { get; set; }
        public bool[] AcceptButtonStatuses { get; set; }

        private QuestDetailsHero[] m_HeroSections;
        private List<FocusBox> m_FocusBoxes = new List<FocusBox>();

        void Awake() {
            Instance = this;

            m_HeroSections = new QuestDetailsHero[QuestInstance.MAX_HEROES_PER_QUEST];
            m_HeroSections[0] = heroSectionTemplate;
            for (int i = 1; i < QuestInstance.MAX_HEROES_PER_QUEST; i++) {
                QuestDetailsHero newInstance = Instantiate(heroSectionTemplate);
                newInstance.transform.SetParent(heroSectionTemplate.transform.parent, false);
                newInstance.transform.SetSiblingIndex(1 + i);

                m_HeroSections[i] = newInstance;
            }

            for(int i = 0; i < m_HeroSections.Length; i++) {
                QuestDetailsHero heroSection = m_HeroSections[i];
                heroSection.Initialize(i);
            }

            SelectedHeroes = new HeroInstance[QuestInstance.MAX_HEROES_PER_QUEST];
            PostButtonStatuses = new bool[QuestInstance.MAX_HEROES_PER_QUEST];
            AcceptButtonStatuses = new bool[QuestInstance.MAX_HEROES_PER_QUEST];
            SelectedGoldRewards = new int[QuestInstance.MAX_HEROES_PER_QUEST];

            for (int i = 0; i < SelectedGoldRewards.Length; i++) {
                SelectedGoldRewards[i] = 0;
            }

            WindowMode = QuestDetailsWindowMode.ACTIVE_REVIEW;
        }

        void Start() {
            closeButton.onClick.AddListener(() => {
                CurrentQuest.RefundQuestRewards(false, true);
                CloseWindow();
            });

            skipButton.onClick.AddListener(() => {
                CloseWindow();
            });

            cancelButton.onClick.AddListener(() => {
                CurrentQuest.RefundQuestRewards(true, true);
                QuestManager.PostedQuests.Remove(CurrentQuest);
                QuestManager.Requests.Add(CurrentQuest);
                QuestsWindow.Instance.Reload();
                CloseWindow();
            });

            acceptButton.onClick.AddListener(() => {
                foreach(HeroInstance hero in SelectedHeroes) {
                    if (hero == null) continue;
                    HeroManager.SetHeroToState(hero, HeroStates.ON_QUEST);
                }

                QuestManager.PostedQuests.Remove(CurrentQuest);
                QuestManager.ActiveQuests.Add(CurrentQuest, SelectedHeroes.Clone() as HeroInstance[]);
                QuestsWindow.Instance.Reload();
                CloseWindow();
            });

            completeButton.onClick.AddListener(() => {
                foreach (HeroInstance hero in QuestManager.ActiveQuests[CurrentQuest]) {
                    if (hero == null) continue;
                    HeroManager.SetHeroToState(hero, HeroStates.IDLE);
                }

                QuestManager.ActiveQuests.Remove(CurrentQuest);
                QuestsWindow.Instance.Reload();
                CloseWindow();
            });

            postButton.onClick.AddListener(() => {
                QuestManager.Requests.Remove(CurrentQuest);
                QuestManager.PostedQuests.Add(CurrentQuest);
                QuestsWindow.Instance.Reload();

                for (int i = 0; i < SelectedGoldRewards.Length; i++) {
                    CurrentQuest.GoldRewards[i] = new QuestRewardGold();
                    CurrentQuest.GoldRewards[i].GoldCount = SelectedGoldRewards[i];
                }

                InventoryManager.ModifyGold(-CurrentQuest.GetTotalGoldCount(), "Deposit money for quest");

                CloseWindow();
            });

            Instance = this;
            gameObject.SetActive(false);
        }

        public void CloseWindow() {
            CurrentQuest = null;

            gameObject.SetActive(false);
            ReputationUI.Instance.ResetAvailableHeroes();

            FocusBoxManager.Instance.ReleaseBoxes(m_FocusBoxes.ToArray());
            m_FocusBoxes.Clear();

            OnWindowClose();
        }

        public void OpenWindow(QuestInstance quest, QuestDetailsWindowMode mode) {
            if (CurrentQuest != null) CurrentQuest.RefundQuestRewards(false, true);

            WindowMode = mode;
            if (quest == CurrentQuest) {
                bool targetStatus = !gameObject.activeSelf;
                if (!targetStatus) {
                    CloseWindow();
                    return;
				} else {
                    gameObject.SetActive(true);
                }
            } else {
                gameObject.SetActive(true);
            }
            CurrentQuest = quest;

            Reset();

            switch (mode) {
                case QuestDetailsWindowMode.SETUP:
                    closeButton.gameObject.SetActive(true);
                    skipButton.gameObject.SetActive(false);
                    cancelButton.gameObject.SetActive(false);
                    acceptButton.gameObject.SetActive(false);
                    completeButton.gameObject.SetActive(false);
                    postButton.gameObject.SetActive(true);
					parentSuccess.gameObject.SetActive(false);

					RefreshItemRewardDropdown();
                    SetNoHero();
                    CheckPostButtonStatus();

                    foreach (QuestDetailsHero heroSection in m_HeroSections) {
                        if (!heroSection.gameObject.activeSelf) break;
                        m_FocusBoxes.Add(FocusBoxManager.Instance.SetFocusTarget(heroSection.rewardsGoldParent));
                        m_FocusBoxes.Add(FocusBoxManager.Instance.SetFocusTarget(heroSection.rewardsItemParent));
                    }
                    break;

                case QuestDetailsWindowMode.HERO_SELECT:
                    closeButton.gameObject.SetActive(false);
                    skipButton.gameObject.SetActive(true);
                    cancelButton.gameObject.SetActive(true);
                    acceptButton.gameObject.SetActive(true);
                    completeButton.gameObject.SetActive(false);
                    postButton.gameObject.SetActive(false);
					parentSuccess.gameObject.SetActive(true);

					foreach (QuestDetailsHero heroSection in m_HeroSections) {
                        if (!heroSection.gameObject.activeSelf) break;
                        m_FocusBoxes.Add(FocusBoxManager.Instance.SetFocusTarget(heroSection.heroParent));
                    }
                    break;

                case QuestDetailsWindowMode.POSTED_REVIEW:
                    closeButton.gameObject.SetActive(true);
                    skipButton.gameObject.SetActive(false);
                    cancelButton.gameObject.SetActive(true);
                    acceptButton.gameObject.SetActive(false);
                    completeButton.gameObject.SetActive(false);
                    postButton.gameObject.SetActive(false);
					parentSuccess.gameObject.SetActive(false);

					SetNoHero();
                    break;
                case QuestDetailsWindowMode.ACTIVE_REVIEW:
                    closeButton.gameObject.SetActive(true);
                    skipButton.gameObject.SetActive(false);
                    cancelButton.gameObject.SetActive(true);
                    acceptButton.gameObject.SetActive(false);
                    completeButton.gameObject.SetActive(false);
                    postButton.gameObject.SetActive(false);
					parentSuccess.gameObject.SetActive(true);

					FindActiveHero();
                    break;
                case QuestDetailsWindowMode.COMPLETED:
                    closeButton.gameObject.SetActive(false);
                    skipButton.gameObject.SetActive(false);
                    cancelButton.gameObject.SetActive(false);
                    acceptButton.gameObject.SetActive(false);
                    completeButton.gameObject.SetActive(true);
                    postButton.gameObject.SetActive(false);
					parentSuccess.gameObject.SetActive(true);

					FindActiveHero();
                    break;
            }

            questTitle.text = CurrentQuest.QuestTitle;
            durationText.text = GetDurationText();
            difficultyText.text = ""+ CurrentQuest.DifficultyLevel;

            handlerGoldReward.text = CurrentQuest.HandlerGoldRewardEstimate;

            handlerItemRewardParent.SetActive(CurrentQuest.HandlerItemReward != null);
            handlerRewardItemInstance.SetItem(CurrentQuest.HandlerItemReward != null ? CurrentQuest.HandlerItemReward.Item : (GameItem)null);

            handlerAdditionalRewardParent.SetActive(CurrentQuest.AdditionalReward != null);
            handlerAdditionalReward.text = CurrentQuest.AdditionalReward != null ? CurrentQuest.AdditionalReward.DisplayValue : "-";

            foreach(QuestDetailsHero heroSection in m_HeroSections) {
                heroSection.Reload();
            }
        }

        private void FindActiveHero() {
            foreach (QuestDetailsHero heroSection in m_HeroSections) {
                HeroInstance hero = heroSection.FindActiveHero();
                SetSelectedHero(hero, heroSection.HeroNumber);
            }
        }

        private void SetNoHero() {
            foreach (QuestDetailsHero heroSection in m_HeroSections) {
                heroSection.SetNoHero();
            }
        }

        public void SetSelectedHero(HeroInstance hero, int heroNumber) {
            SelectedHeroes[heroNumber] = hero;

            successText.text = GetSuccessRateForPercentage(CurrentQuest.GetTotalSuccessRate(SelectedHeroes));
        }

        public bool HasHeroSelected(HeroInstance hero) {
            foreach(HeroInstance h in SelectedHeroes) {
                if (h != null && h == hero) return true;
            }
            return false;
        }

        private string GetSuccessRateForPercentage(int percentage) {
            string s;
            if (percentage >= 95) s = "Extremely high";
            else if (percentage >= 85) s = "Very high";
            else if (percentage >= 70) s = "High";
            else if (percentage >= 50) s = "Average";
            else if (percentage >= 30) s = "Low";
            else if (percentage >= 15) s = "Very low";
            else s = "Extremely low";

            //If can see percentages
            if (true) s += " (" + percentage + "%)";
            return s;
        }

        private void RefreshItemRewardDropdown() {
            List<string> itemRewardNames = new List<string>() { "-" };
            foreach (GameItem item in InventoryManager.OwnedItems) {
                itemRewardNames.Add(item.DisplayName);
            }
        }

        public void CheckPostButtonStatus() {
            bool isPostable = true;

            for (int i = 0; i < CurrentQuest.PartySize; i++) {
                bool postStatus = PostButtonStatuses[i];
                if (postStatus == false) {
                    isPostable = false;
                    break;
                }
            }

            postButton.interactable = isPostable;
        }

        private void CheckAcceptButtonStatus() {
            bool isAcceptable = true;
            
            foreach(bool acceptStatus in AcceptButtonStatuses) {
                if(acceptStatus == false) {
                    isAcceptable = false;
                    break;
                }
            }

            acceptButton.interactable = isAcceptable;
        }

        public void Reset() {
            for(int i = 0; i < PostButtonStatuses.Length; i++) {
                PostButtonStatuses[i] = true;
            }

            for (int i = 0; i < AcceptButtonStatuses.Length; i++) {
                AcceptButtonStatuses[i] = true;
            }

            for (int i = 0; i < SelectedHeroes.Length; i++) {
                SelectedHeroes[i] = null;
            }

            for (int i = 0; i < SelectedGoldRewards.Length; i++) {
                SelectedGoldRewards[i] = 0;
            }

            for(int i = 0; i < m_HeroSections.Length; i++) {
                m_HeroSections[i].gameObject.SetActive(i <= CurrentQuest.PartySize - 1);
                m_HeroSections[i].SetNoHero();
            }

            ReputationUI.Instance.ResetAvailableHeroes();
            UIHighlighter.Instance.GetGroup(HIGHLIGHT_GROUP_ID).RemoveAll();

            FocusBoxManager.Instance.ReleaseBoxes(m_FocusBoxes.ToArray());
            m_FocusBoxes.Clear();
        }

        private string GetDurationText() {
            switch (WindowMode) {
                case QuestDetailsWindowMode.SETUP:
                case QuestDetailsWindowMode.HERO_SELECT:
                case QuestDetailsWindowMode.POSTED_REVIEW:
                    return "Quest length: " + CurrentQuest.DurationInDays + " day" + (CurrentQuest.DurationInDays == 1 ? "" : "s");

                case QuestDetailsWindowMode.COMPLETED:
                    return "Quest completed";

                case QuestDetailsWindowMode.ACTIVE_REVIEW:
                    return CurrentQuest.DaysLeftOnQuest + " day" + (CurrentQuest.DaysLeftOnQuest == 1 ? "" : "s") + " left";

                default:
                    return "Unknown duration";

            }
        }
    }

}