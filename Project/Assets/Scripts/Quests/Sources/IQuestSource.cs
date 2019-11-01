using Rondo.QuestSim.Reputation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests {

    public interface IQuestSource {
		string DisplayName { get; set; }
		string RequestTitle { get; }

		int QuestDifficulty { get; }

		void GenerateSettings();
    }

}