using UnityEngine;

namespace Assets.Scripts.Serializibles
{
    [CreateAssetMenu(fileName = "New Test Quest", menuName = "Test Quest")]
    class TestQuest : TestEvent
    {
        public bool firstTimeMet = false;
        public GameObject character;

        [Header("Left stats")]
        public int healthLeft;
        public int moneyLeft;
        public int moodLeft;
        public int popularityLeft;

        [Header("Right stats")]
        public int healthRight;
        public int moneyRight;
        public int moodRight;
        public int popularityRight;

        [Header("Death specifics")]
        public bool deathLeft;
        public bool deathRight;

        public bool loadLeft;
        public bool loadRight;
        //public Combat combat;

        // public bool combatLeft;
        // public bool combatRight;

        [Header("Affects and loads")]
        public bool isLinkedLeft;
        public Quest linkedLeftQuest;

        public bool isLinkedRight;
        public Quest linkedRightQuest;
    }
}
