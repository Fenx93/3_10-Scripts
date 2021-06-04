using UnityEngine;

namespace Assets.Scripts.Serializibles
{
    [CreateAssetMenu(fileName = "New Test Quest", menuName = "Test Quest")]
    class TestQuest : TestEvent
    {
        public bool firstTimeMet = false;
        public GameObject character;

        [Header("Left stats")]
        public int healthLeft, moneyLeft, moodLeft, popularityLeft;

        [Header("Right stats")]
        public int healthRight, moneyRight, moodRight, popularityRight;

        [Header("Death specifics")]
        public bool deathLeft,  deathRight;

        public bool loadLeft, loadRight;
        //public Combat combat;

        // public bool combatLeft;
        // public bool combatRight;

        [Header("Affects and loads")]
        public bool isLinkedLeft;
        public TestEvent linkedLeftQuest;

        public bool isLinkedRight;
        public TestEvent linkedRightQuest;
    }
}
