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

        [Header("Death specifics")]
        public bool deathLeft;
        public bool deathRight;
        public bool loadLeft, loadRight;
        //public Combat combat;

        // public bool combatLeft;
        // public bool combatRight;

        [Header("Affects and loads")]
        public bool isLinkedLeft;
        public GameEvent linkedLeftQuest;

        public bool isLinkedRight;
        public GameEvent linkedRightQuest;
    }
}
