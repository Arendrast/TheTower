using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Card", menuName = "Inventory/Cards")]
    public class InventoryItem : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public bool IsAvailable { get; set; }
        public bool IsEquipped { get; set; }
    }
}
