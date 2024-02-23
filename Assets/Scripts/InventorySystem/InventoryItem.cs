using System;
using System.Collections.Generic;
using General;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Card", menuName = "Inventory/Cards")]
    public class InventoryItem : ScriptableObject, IObjectBeindInitialized
    {
        public string NameInPlayerPrefs => _type.ToString();
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public bool IsAvailable { get; private set; }
        [SerializeField] private NamesVariablesPlayerPrefs.CardsTypes _type;
        [SerializeField] private List<CardLevel> _upgrades = new List<CardLevel>();
        [SerializeField] private float _initialValue;
        
        [Serializable]
        public class CardLevel
        {
            [field: SerializeField] public float Multiplier { get; private set; }
            [field: SerializeField] public int Price { get; private set; }
            [field: SerializeField] public int RequiredLevel { get; private set; }
        }

        public void Initialize()
        {
            var nameVariableOfLvl = _type + NamesVariablesPlayerPrefs.PostScriptCardLevel; 
            if (!PlayerPrefs.HasKey(nameVariableOfLvl))
                PlayerPrefs.SetInt(nameVariableOfLvl, 0);

            var value = PlayerPrefs.GetInt(nameVariableOfLvl);
            
            IsAvailable = value > 0 || IsAvailable;
            
            PlayerPrefs.SetFloat(NameInPlayerPrefs, value > 0 ? 
                _initialValue * _upgrades[PlayerPrefs.GetInt(nameVariableOfLvl) - 1].Multiplier
                : _initialValue);
        }
    }
}
