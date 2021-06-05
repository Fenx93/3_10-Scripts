using UnityEngine;

namespace Assets.Scripts.Serializibles
{
    [CreateAssetMenu(fileName = "New Deed", menuName = "Deed")]
    class GameDeed : ScriptableObject
    {
        public string deedId, deedText;
    }
}
