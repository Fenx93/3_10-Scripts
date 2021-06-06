using UnityEngine;

namespace Assets.Scripts.Serializibles
{
    public class GameEvent : ScriptableObject
    {
        public string questId, questText, leftOptionText, rightOptionText;

        [Header("Load more quests")]
        public bool loadLeft;
        public bool loadRight;
        public GameEvent[] loadedQuests;

        [Header("A deed is completed")]
        public GameDeed leftDeed;
        public GameDeed rightDeed;
    }
}
