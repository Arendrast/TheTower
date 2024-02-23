using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace General
{
    public class UpgradesPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textGameSpeed;
        [Range(0, 10)] [SerializeField] private float _defulatGameSpeed = 1;
        [SerializeField] private Image _imageWhichCloseEconimicUpgradeScreenIfNotOpened;
        [SerializeField] private bool _isUnlockEconomicUpgradeScreen = true;
        [SerializeField] private List<Image> _listOfPanels = new List<Image>();
        
        public void Start()
        {
            if (!PlayerPrefs.HasKey(NamesVariablesPlayerPrefs.GameSpeed))
                PlayerPrefs.SetFloat(NamesVariablesPlayerPrefs.GameSpeed, _defulatGameSpeed);

            _textGameSpeed.text = PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.GameSpeed).ToString();
            
            if (!PlayerPrefs.HasKey(NamesVariablesPlayerPrefs.IsBuyEconomicUpgradeScreen))
                PlayerPrefs.SetInt(NamesVariablesPlayerPrefs.IsBuyEconomicUpgradeScreen, Convert.ToInt32(false));
            _imageWhichCloseEconimicUpgradeScreenIfNotOpened.gameObject.SetActive(/*!Convert.ToBoolean(PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.IsBuyEconomicUpgradeScreen)) ||*/ !_isUnlockEconomicUpgradeScreen);
        }
        public void SwitchPanel(Image requiredPanel)
        {
            foreach (var panel in _listOfPanels)
                panel.gameObject.SetActive(false);
            requiredPanel.gameObject.SetActive(true);
        }

        public void SetSpeedGameByStep(float step)
        {
            var value = PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.GameSpeed) + step;

            if (value < 0)
                return;
            
            PlayerPrefs.SetFloat(NamesVariablesPlayerPrefs.GameSpeed, value);
            _textGameSpeed.text = value.ToString();
        }

    }
}
