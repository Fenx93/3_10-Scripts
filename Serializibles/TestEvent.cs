using UnityEngine;

namespace Assets.Scripts.Serializibles
{
    public class TestEvent : ScriptableObject
    {
        public string questId, questText, leftOptionText, rightOptionText;

        // probably was planned to load chunks of quests on selection
        public Quest[] loadedQuests;
    }
}
