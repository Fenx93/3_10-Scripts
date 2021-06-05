using UnityEngine;

namespace Assets.Scripts.Serializibles
{
    public class GameEvent : ScriptableObject
    {
        public string questId, questText, leftOptionText, rightOptionText;

        public GameEvent[] loadedQuests;
    }
}
