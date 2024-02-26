using System.Collections.Generic;
using UnityEngine;

namespace General
{
    [CreateAssetMenu(fileName = "Game/SongConfig", menuName = "SongConfig")]
    public class SongConfig : ScriptableObject
    {
        [field: SerializeField] public List<Song> Songs { get; private set; }
    }
}