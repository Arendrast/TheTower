using System;
using System.Collections.Generic;
using MyCustomEditor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace General
{
    [Serializable]
    public class Upgrade
    {
        [ReadOnlyInspector] public NamesVariablesPlayerPrefs.NamesOfUpgrades Name;
        public string NameOfValueVariableInPlayerPrefs => Name.ToString();
        public string NameOfLevelVariableInPlayerPrefs => Name + NamesVariablesPlayerPrefs.PostScriptUpgradeLevel;
        public int MaxLvl => ListOfParameters.Count;
            
        [field: SerializeField] public string NameInNotification { get; private set; }
        [field: SerializeField] public TMP_Text TextOfPrice { get; private set; }
        [field: SerializeField] public TMP_Text TextOfLvl { get; private set; }
        [field: SerializeField] public TMP_Text TextOfValue { get; private set; }
        //[field: SerializeField] public TMP_Text TextOfRequiredLvl { get; private set; }
        [field: SerializeField] public Color ColorOnClosed { get; private set; }
        [field: SerializeField] public Color ColorOnOpened { get; private set; }
        [field: SerializeField] public Sprite SpriteOnClosed { get; private set; }
        [field: SerializeField] public Sprite DefaultSprite { get; private set; }
        [field: SerializeField] public Button UseButton { get; private set; }
        [field: SerializeField] public float InitialValue { get; private set; }
        [field: SerializeField] public List<FieldUpgrade> ListOfParameters { get; private set; }

        public Action<float> OnChangeValue;
        //[field: SerializeField] public List<FieldUpgrade> ListOfParametersOfSecondParameter { get; private set; }
    }
}