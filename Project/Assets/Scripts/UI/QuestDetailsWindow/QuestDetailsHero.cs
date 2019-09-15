using Rondo.Generic.Utility.UI;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.UI.General;
using Rondo.QuestSim.UI.Inventory;
using Rondo.QuestSim.UI.Reputation;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.PostedQuests {

    public class QuestDetailsHero : MonoBehaviour {

        [Header("Heroes")]
        public ReputationHeroInstanceUI heroSelectedInstance;
        public Button heroSelectButton;

        [Header("Item rewards")]
        public GameItemInstanceUI heroRewardItemInstance;
        public Button heroRewardItemRemove;

        [Header("Gold rewards")]
        public TMP_InputField heroGoldRewardInput;
        public TextMeshProUGUI heroGoldRewardText;

        [Header("Highlight lists")]
        public Graphic[] highlightsRewards;
        public Graphic[] highlightsHeroSelect;

        [Header("Parents")]
        public RectTransform rewardsGoldParent;
        public RectTransform rewardsItemParent;
        public RectTransform heroParent;

        public List<HeroInstance> AvailableHeroes { get; set; }

        private QuestInstance CurrentQuest { get { return QuestDetailsWindow.Instance.CurrentQuest; } }
        private QuestDetailsWindowMode WindowMode { get { return QuestDetailsWindow.Instance.WindowMode; } }

        private HeroInstance m_SelectedHero = null;
        private int m_HeroNumber = 0;

        private void Awake() {
            AvailableHeroes = new List<HeroInstance>();
        }

        private void OnDisable() {
            InventoryWindow.Instance.SetQuestItemMode(false, null);
        }

        void Start() {
			return;
            heroGoldRewardInput.onValueChanged.AddListener((value) => {
                if (string.IsNullOrEmpty(value)) {
                    value = "0";
                }
                int goldValue = int.Parse(value);
                QuestDetailsWindow.Instance.SelectedGoldRewards[m_HeroNumber] = goldValue;

                CheckPostButtonStatus();
            });

            heroGoldRewardInput.onDeselect.AddListener((value) => {
                if (string.IsNullOrEmpty(value)) {
                    heroGoldRewardInput.text = "0";
                }
            });

            heroRewardItemInstance.GetComponent<Button>().onClick.AddListener(() => {
                InventoryWindow.Instance.SetQuestItemMode(true, SetSelectedItem);
                RightSideSwitch.Instance.ActivateObject(InventoryWindow.Instance.gameObject, false);
            });

            heroRewardItemRemove.onClick.AddListener(() => {
                Quests.Rewards.QuestRewardItem previousItem = CurrentQuest.ItemRewards[m_HeroNumber];
                if (previousItem != null) InventoryManager.MoveItemToOwned(previousItem.Item);
                previousItem = null;

                CurrentQuest.ItemRewards[m_HeroNumber] = null;

                heroRewardItemInstance.SetItem((GameItem)null);
                heroRewardItemRemove.interactable = false;

                CheckPostButtonStatus();
            });

            heroSelectButton.onClick.AddListener(() => {
                AvailableHeroes.Clear();
                foreach (HeroInstance hero in HeroManager.GetAvailableHeroes()) {
                    if (CurrentQuest.WouldHeroAccept(hero, m_HeroNumber) &&
                        !QuestDetailsWindow.Instance.HasHeroSelected(hero)) {
                        AvailableHeroes.Add(hero);
                    }
                }

                RightSideSwitch.Instance.ActivateObject(ReputationUI.Instance.gameObject, false);
                ReputationUI.Instance.SetAvailableHeroes(AvailableHeroes, SetSelectedHero);
            });
        }

        public void Initialize(int heroNumber) {
            m_HeroNumber = heroNumber;
        }

        public void Reload() {
            switch (WindowMode) {
                case QuestDetailsWindowMode.SETUP:
                    SetNoHero();
                    UIHighlighter.Instance.GetGroup(QuestDetailsWindow.HIGHLIGHT_GROUP_ID).AddObjects(highlightsRewards, UIHighlighter.Instance.redHighlightColor, highlightsRewards[0].color);
                    break;
                case QuestDetailsWindowMode.HERO_SELECT:
                    UIHighlighter.Instance.GetGroup(QuestDetailsWindow.HIGHLIGHT_GROUP_ID).AddObjects(highlightsHeroSelect, UIHighlighter.Instance.redHighlightColor, highlightsHeroSelect[0].color);
                    break;
                case QuestDetailsWindowMode.POSTED_REVIEW:
                    SetNoHero();
                    break;
                case QuestDetailsWindowMode.ACTIVE_REVIEW:
                    FindActiveHero();
                    break;
                case QuestDetailsWindowMode.COMPLETED:
                    FindActiveHero();
                    break;
            }

            heroRewardItemInstance.GetComponent<Button>().enabled = WindowMode == QuestDetailsWindowMode.SETUP;

            heroGoldRewardInput.gameObject.SetActive(WindowMode == QuestDetailsWindowMode.SETUP);
            heroGoldRewardText.gameObject.SetActive(WindowMode != QuestDetailsWindowMode.SETUP);

            heroSelectButton.enabled = WindowMode == QuestDetailsWindowMode.HERO_SELECT;

            heroGoldRewardText.text = ""+CurrentQuest.GoldRewards[m_HeroNumber].GoldCount;
            heroRewardItemInstance.SetItem(CurrentQuest.ItemRewards[m_HeroNumber]);
            if(heroGoldRewardInput.gameObject.activeSelf) heroGoldRewardInput.text = "0";

            CheckAcceptButtonStatus();
            CheckPostButtonStatus();
        }

        public void SetSelectedHero(HeroInstance hero) {
            heroSelectedInstance.ApplyHero(hero);
            m_SelectedHero = hero;

            QuestDetailsWindow.Instance.SetSelectedHero(hero, m_HeroNumber);

            UIHighlighter.Instance.GetGroup(QuestDetailsWindow.HIGHLIGHT_GROUP_ID).RemoveObjects(highlightsHeroSelect);
        }

        public void SetSelectedItem(GameItemInstanceUI item) {
            Quests.Rewards.QuestRewardItem previousItem = CurrentQuest.ItemRewards[m_HeroNumber];

            heroRewardItemInstance.SetItem(item.Item);
            Quests.Rewards.QuestRewardItem nextItem = CurrentQuest.ItemRewards[m_HeroNumber] = new Quests.Rewards.QuestRewardItem(item.Item);

            if (previousItem != null) InventoryManager.MoveItemToOwned(previousItem.Item);
            InventoryManager.MoveItemToReserved(item.Item);
            previousItem = nextItem;

            heroRewardItemRemove.interactable = true;

            CheckPostButtonStatus();
        }

        public void FindActiveHero() {
            HeroInstance hero = QuestManager.ActiveQuests[CurrentQuest][m_HeroNumber];
            heroSelectedInstance.ApplyHero(hero);
        }

        public void SetNoHero() {
            heroSelectedInstance.ApplyHero(null);
        }

        private void CheckPostButtonStatus() {
            bool isPostable = true;
            if ((InventoryManager.Gold < CurrentQuest.GetTotalGoldCount() && InventoryManager.Gold >= 0) ||
                (QuestDetailsWindow.Instance.SelectedGoldRewards[m_HeroNumber] == 0 && CurrentQuest.ItemRewards[m_HeroNumber] == null)) isPostable = false;

            QuestDetailsWindow.Instance.PostButtonStatuses[m_HeroNumber] = isPostable;

            if (isPostable) {
                UIHighlighter.Instance.GetGroup(QuestDetailsWindow.HIGHLIGHT_GROUP_ID).RemoveObjects(highlightsRewards);
            } else {
                UIHighlighter.Instance.GetGroup(QuestDetailsWindow.HIGHLIGHT_GROUP_ID).AddObjects(highlightsRewards, UIHighlighter.Instance.redHighlightColor, highlightsRewards[0].color);
            }

            QuestDetailsWindow.Instance.CheckPostButtonStatus();
        }

        private void CheckAcceptButtonStatus() {
            bool isAcceptable = true;
            if (m_SelectedHero == null) isAcceptable = false;

            QuestDetailsWindow.Instance.AcceptButtonStatuses[m_HeroNumber] = isAcceptable;
        }

    }

}