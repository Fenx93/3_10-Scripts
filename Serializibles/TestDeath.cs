using UnityEngine;

namespace Assets.Scripts.Serializibles
{
    [CreateAssetMenu(fileName = "New Test Death", menuName = "Test Death")]
    class TestDeath: TestEvent
    {
        public Sprite deathImage;
    }
}
