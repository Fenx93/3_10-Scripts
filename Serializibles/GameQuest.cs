using UnityEngine;

namespace Assets.Scripts.Serializibles
{
    [CreateAssetMenu(fileName = "New Test Quest", menuName = "Test Quest")]
    class GameQuest : GameEvent
    {
        public bool firstTimeMet = false;
        public GameObject character;

        [Header("Left stats")]
        public int healthLeft;
        public int moneyLeft, moodLeft, popularityLeft;

        [Header("Right stats")]
        public int healthRight; 
        public int moneyRight, moodRight, popularityRight;

        [Header("If an option leads to an unique death - use these")]
        public bool deathLeft;
        public bool deathRight;
        //public Combat combat;

        // public bool combatLeft;
        // public bool combatRight;

        [Header("This quest is continued")]
        public GameEvent linkedLeftQuest;
        public GameEvent linkedRightQuest;
    }
}
