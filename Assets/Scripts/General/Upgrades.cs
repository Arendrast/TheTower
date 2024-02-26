using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Currencies;
using MyCustomEditor;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General
{
    public class Upgrades : MonoBehaviour, IObjectBeindInitialized
    {
        public Dictionary<NamesVariablesPlayerPrefs.NamesOfUpgrades, Upgrade> DictOfUpgrades { get; private set; }
            = new Dictionary<NamesVariablesPlayerPrefs.NamesOfUpgrades, Upgrade>();

        [SerializeField] private List<Upgrade> _listOfUpgrades;

        [SerializeField] private MoneyCurrency _moneyCurrency;

        [SerializeField] private string _contentErrorNoEnoughCrystal = "No money!";
        [SerializeField] private string _contentErrorMaxLvl = "You have max lvl!";
        [SerializeField] private string _contentOfNotificationAboutImproveOfUpgrade = " upgraded";
        [SerializeField] private Tower _tower;
        [SerializeField] private TowerHealth _towerHealth;

        private TMP_Text _errorText, _notificationText;

        private Sprite _defaultSpriteOfImageOfUpgradeInitialLevelCat;

        private int _maxOpenLevel;
        private int _currentLvl;
        private const int IndexOfFirstLevelOfUpgrades = 0;

        private const int IndexOfElementOfFirstUpgrade = 0;

        public void Construct(Tower tower, TowerHealth towerHealth, MoneyCurrency moneyCurrency)
        {
            _tower = tower;
            _towerHealth = towerHealth;
            _moneyCurrency = moneyCurrency;
            Initialize();
        }

        public void Initialize()
        {
            if (!PlayerPrefs.HasKey(NamesVariablesPlayerPrefs.MaxOpenLvl))
                PlayerPrefs.SetInt(NamesVariablesPlayerPrefs.MaxOpenLvl, 1);

            _maxOpenLevel = PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.MaxOpenLvl);
            _currentLvl = SceneManager.GetActiveScene().buildIndex;
            
            var listOfNamesOfUpgrades = Enum.GetValues(typeof(NamesVariablesPlayerPrefs.NamesOfUpgrades)).Cast<NamesVariablesPlayerPrefs.NamesOfUpgrades>().ToList();
            for (var i = 0; i < _listOfUpgrades.Count; i++)
            {
                DictOfUpgrades.Add(listOfNamesOfUpgrades[i], _listOfUpgrades[i]);
                InitializeAndConfigureUpgrade(_listOfUpgrades[i]);
                _listOfUpgrades[i].UseButton.image.sprite = _listOfUpgrades[i].DefaultSprite;
            }

            if (!_moneyCurrency)
                _moneyCurrency = FindObjectOfType<MoneyCurrency>();
            if (!_tower)
                _tower = FindObjectOfType<Tower>();
        
            //_errorText.gameObject.SetActive(true);
        }

        private void OnValidate()
        {
            var listOfNamesOfUpgrades = Enum.GetValues(typeof(NamesVariablesPlayerPrefs.NamesOfUpgrades)).
                Cast<NamesVariablesPlayerPrefs.NamesOfUpgrades>().ToList();
           
            for (var i = 0; i < _listOfUpgrades.Count; i++)
                _listOfUpgrades[i].Name = listOfNamesOfUpgrades[i];
        }

        private void InitializeAndConfigureUpgrade(Upgrade upgrade)
        {
            var listOfParameters = upgrade.ListOfParameters;
            var nameOfLevelVariableInPlayerPrefs = upgrade.NameOfLevelVariableInPlayerPrefs;
            var nameOfValueVariableInPlayerPrefs = upgrade.NameOfValueVariableInPlayerPrefs;

            if (!PlayerPrefs.HasKey(nameOfLevelVariableInPlayerPrefs))
                PlayerPrefs.SetInt(nameOfLevelVariableInPlayerPrefs, IndexOfFirstLevelOfUpgrades);
            
            var index = PlayerPrefs.GetInt(nameOfLevelVariableInPlayerPrefs);

            SetTexts(new List<TMP_Text> {upgrade.TextOfPrice, upgrade.TextOfLvl}, new List<string>
            {
                listOfParameters[index].Price == 0
                    ? "Free"
                    : listOfParameters[index].Price.ToString(),
                (index + 1).ToString()
            });
            
            PlayerPrefs.SetFloat(nameOfValueVariableInPlayerPrefs, index <= 0 ? upgrade.InitialValue : upgrade.InitialValue * upgrade.ListOfParameters[index - 1].Multiplier);

            ChangeValueOnValueText(upgrade);

            StartCoroutine(UpdateStateOfButton(upgrade, PlayerPrefs.GetInt(nameOfLevelVariableInPlayerPrefs)));
        }
    
        private IEnumerator UpdateStateOfButton(Upgrade upgrade, int indexLevel)
        {
            var requiredLvl = upgrade.ListOfParameters[indexLevel].RequiredLevel;

            if (_maxOpenLevel < requiredLvl && _currentLvl < requiredLvl || upgrade.MaxLvl <= indexLevel + 1)
                MakeClosedStateOfButton(upgrade);
            
            else
            {
                if (upgrade.ListOfParameters[indexLevel].Price > _moneyCurrency.Number)
                {
                    MakeClosedStateOfButton(upgrade);
                    yield return new WaitUntil(() => upgrade.ListOfParameters[indexLevel].Price <= _moneyCurrency.Number);
                }

                upgrade.TextOfPrice.gameObject.SetActive(true);
                //upgrade.TextOfRequiredLvl.gameObject.SetActive(false);

                upgrade.UseButton.enabled = true;
                upgrade.UseButton.image.sprite = upgrade.DefaultSprite;
                if (upgrade.ColorOnOpened != Color.black)
                    upgrade.UseButton.image.color = upgrade.ColorOnOpened;
            }
        }

        private void MakeClosedStateOfButton(Upgrade upgrade)
        {
            //upgrade.TextOfRequiredLvl.gameObject.SetActive(true);
            //upgrade.TextOfRequiredLvl.text = requiredLvl.ToString();
            upgrade.UseButton.enabled = false;
            if (upgrade.SpriteOnClosed)
                upgrade.UseButton.image.sprite = upgrade.SpriteOnClosed;
            if (upgrade.ColorOnClosed != Color.black)
                upgrade.UseButton.image.color = upgrade.ColorOnClosed;
        }


        public bool GetIsImprove(Upgrade upgrade)
        {
            var upgradeIndex = PlayerPrefs.GetInt(upgrade.NameOfLevelVariableInPlayerPrefs);
            if (_moneyCurrency.Number >= upgrade.ListOfParameters[upgradeIndex].Price)
            {
                Improve(upgrade);
                return PlayerPrefs.GetInt(upgrade.NameOfLevelVariableInPlayerPrefs) > upgradeIndex;
            }

            return false;
        }

        private void ImproveHealth()
        {
            var ratio = _towerHealth.CurrentHealth / _towerHealth.MaxHealth;
            var upgrade = DictOfUpgrades[NamesVariablesPlayerPrefs.NamesOfUpgrades.Health];

            if (GetIsImprove(upgrade))
            {
                _towerHealth.MaxHealth = upgrade.InitialValue * PlayerPrefs.GetFloat(upgrade.NameOfValueVariableInPlayerPrefs);
                _towerHealth.CurrentHealth = ratio * _towerHealth.MaxHealth;
                _towerHealth.UpdateText();
                _towerHealth.UpdateValueOnSlider();
            }
        }
        
        
        [EnumAction(typeof(NamesVariablesPlayerPrefs.NamesOfUpgrades))]
        public void Improve(int upgradeEnum)
        {
            var upgrade = DictOfUpgrades[(NamesVariablesPlayerPrefs.NamesOfUpgrades) upgradeEnum];
            
            if (upgrade == DictOfUpgrades[NamesVariablesPlayerPrefs.NamesOfUpgrades.Health])
                ImproveHealth();
            else
                Improve(upgrade);
        }
        
        private void Improve(Upgrade upgrade)
        {
            var nameOfLevelVariableInPlayerPrefs = upgrade.NameOfLevelVariableInPlayerPrefs;
            var nameOfValueVariableInPlayerPrefs = upgrade.NameOfValueVariableInPlayerPrefs;
            var index = PlayerPrefs.GetInt(nameOfLevelVariableInPlayerPrefs);

            var listOfParameters = upgrade.ListOfParameters;

            //if (isUseTwoListParameters)
                //listOfParameters = upgrade.ListOfParametersOfSecondParameter;

            if (index + 1 < upgrade.MaxLvl)
            {
                if (_moneyCurrency.Number >= listOfParameters[index].Price)
                {
                    SetTexts(new List<TMP_Text> {upgrade.TextOfPrice, upgrade.TextOfLvl},
                        new List<string>
                        {
                            listOfParameters[index + 1].Price == 0 ? "Free" : listOfParameters[index + 1].Price.ToString(),
                            (index + 2).ToString(), (index + 1).ToString() });
                    var value = listOfParameters[index].Multiplier * upgrade.InitialValue;
                    PlayerPrefs.SetFloat(nameOfValueVariableInPlayerPrefs, value);
                    PlayerPrefs.SetInt(nameOfLevelVariableInPlayerPrefs, index + 1);
                    _moneyCurrency.Number -= listOfParameters[index].Price;
                    _moneyCurrency.SaveInPlayerPref();
                    //_notificationText.text = upgrade.NameInNotification + _contentOfNotificationAboutImproveOfUpgrade;
                    ChangeValueOnValueText(upgrade);
                    StartCoroutine(UpdateStateOfButton(upgrade, index + 1));
                    upgrade.OnChangeValue?.Invoke(value);
                }
            }
        }

        private void SetTexts(List<TMP_Text> listOfTexts, List<string> listOfContent)
        {
            for (var i = 0; i < listOfTexts.Count; i++)
                listOfTexts[i].text = listOfContent[i];
        }

        private void ChangeValueOnValueText(Upgrade upgrade)
        {
            if (upgrade.TextOfValue)
                upgrade.TextOfValue.text = PlayerPrefs.GetFloat(upgrade.NameOfValueVariableInPlayerPrefs).ToString();
        }
    }
}
