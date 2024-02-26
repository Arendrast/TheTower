using System;
using UnityEngine;

namespace General
{
    [Serializable]
    public class FieldUpgrade
    {
        [field: SerializeField] public float Multiplier { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public int RequiredLevel { get; private set; }
    }
}