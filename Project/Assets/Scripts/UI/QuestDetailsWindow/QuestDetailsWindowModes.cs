using Rondo.Generic.Utility;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Quests.Rewards;
using Rondo.QuestSim.Reputation;
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


    public enum QuestDetailsWindowMode {
        SETUP,
        HERO_SELECT,
        POSTED_REVIEW,
        ACTIVE_REVIEW,
        COMPLETED
    }

}