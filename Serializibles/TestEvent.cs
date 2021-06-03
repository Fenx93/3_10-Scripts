using UnityEngine;

namespace Assets.Scripts.Serializibles
{
    public class TestEvent : ScriptableObject
    {
        public string questName;
        public string questText;
        public string leftOptionText;
        public string rightOptionText;

        // probably was planned to load chunks of quests on selection
        public Quest[] loadedQuests;
    }
}
