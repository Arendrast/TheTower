using System;
using UnityEngine;

namespace General
{
    [Serializable]
    public class Song
    {
        [field: SerializeField] public AudioClip Clip { get; private set; }
        [field: SerializeField, Range(0, 3)] public float Pitch { get; private set; }
        [field: SerializeField, Range(0, 256)] public int Priority { get; private set; }
        [field: SerializeField, Range(0, 1)] public float Volume { get; private set; } = 1;
    }
}