using System;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class CardLevel
    {
        [field: SerializeField] public float Multiplier { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public int RequiredLevel { get; private set; }
    }
}