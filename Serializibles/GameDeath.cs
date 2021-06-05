using UnityEngine;

namespace Assets.Scripts.Serializibles
{
    [CreateAssetMenu(fileName = "New Test Death", menuName = "Test Death")]
    class GameDeath: GameEvent
    {
        public Sprite deathImage;
    }
}
