using UnityEngine;

namespace Assets.Scripts.Serializibles
{
    [CreateAssetMenu(fileName = "New Deed", menuName = "Deed")]
    public class GameDeed : ScriptableObject
    {
        public string deedId, deedText;
    }
}
