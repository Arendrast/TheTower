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
using UnityEngine.UI;

namespace General
{
    public class Upgrades : MonoBehaviour, IObjectBeindInitialized
    {
        [SerializeField] private List<Upgrade> _listOfUpgrades;
    
        [SerializeField] private Money _money;
    
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

        [Serializable]
        public class Upgrade
        {   
            [HideInInspector] public string Name;
            [ReadOnlyInspector] [SerializeField] private string _nameInInspector;
            [field: SerializeField] public string NameInNotification { get; private set; }
            [field: SerializeField] public TMP_Text TextOfPrice { get; private set; }
            [field: SerializeField] public TMP_Text TextOfLvl { get; private set; }
            //[field: SerializeField] public TMP_Text TextOfRequiredLvl { get; private set; }
            [field: SerializeField] public int MaxLvl { get; private set; }
            [field: SerializeField] public Color ColorOnClosed { get; private set; }
            [field: SerializeField] public Color ColorOnOpened { get; private set; }
            [field: SerializeField] public Sprite SpriteOnClosed { get; private set; }
            [field: SerializeField] public Sprite DefaultSprite { get; private set; }
            [field: SerializeField] public Button UseButton { get; private set; }
            [field: SerializeField] public float InitialValue { get; private set; }
            [field: SerializeField] public List<FieldUpgrade> ListOfParameters { get; private set; }
            //[field: SerializeField] public List<FieldUpgrade> ListOfParametersOfSecondParameter { get; private set; }
        }

        private readonly Dictionary<NamesOfUpgrades, Upgrade> _dictOfUpgrades = new Dictionary<NamesOfUpgrades, Upgrade>();
        private const int IndexOfElementOfFirstUpgrade = 0;

        [Serializable]
        public class FieldUpgrade
        {
            [field: SerializeField] public float Multiplier { get; private set; }
            [field: SerializeField] public int Price { get; private set; }
            [field: SerializeField] public int RequiredLevel { get; private set; }
        }


        public enum NamesOfUpgrades
        {
            Damage,
            AttackSpeed,
            CriticalDamageChance,
            CriticalFactor,
            Radius,
            DamageOnMeter,
            Health,
            HealthRegeneration
        }

        public void Initialize()
        {
            var listOfNamesOfUpgrades = Enum.GetValues(typeof(NamesOfUpgrades)).Cast<NamesOfUpgrades>().ToList();
            for (var i = 0; i < listOfNamesOfUpgrades.Count; i++)
            {
                _listOfUpgrades[i].Name = listOfNamesOfUpgrades[i].ToString();
                _dictOfUpgrades.Add(listOfNamesOfUpgrades[i], _listOfUpgrades[i]);
            }
        
            if (!_money)
                _money = FindObjectOfType<Money>();
            if (!_tower)
                _tower = FindObjectOfType<Tower>();
        
            //_errorText.gameObject.SetActive(true);

            foreach (var upgrade in _dictOfUpgrades.Values)
            {
                //upgrade.TextOfRequiredLvl.gameObject.SetActive(false);
                upgrade.UseButton.image.sprite = upgrade.DefaultSprite;
            }

            InitializeUpgrades();
        }

        private void InitializeUpgrades()
        {
            if (!PlayerPrefs.HasKey(NamesVariablesPlayerPrefs.MaxOpenLvl))
                PlayerPrefs.SetInt(NamesVariablesPlayerPrefs.MaxOpenLvl, 1);
        
            _maxOpenLevel = PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.MaxOpenLvl);
            _currentLvl = SceneManager.GetActiveScene().buildIndex;
        
            InitializeAndConfigureUpgrade(_dictOfUpgrades[NamesOfUpgrades.Damage], NamesVariablesPlayerPrefs.Damage, NamesVariablesPlayerPrefs.DamageUpgradeLevel);
            InitializeAndConfigureUpgrade(_dictOfUpgrades[NamesOfUpgrades.AttackSpeed], NamesVariablesPlayerPrefs.AttackSpeed, NamesVariablesPlayerPrefs.AttackSpeedUpgradeLevel);
            InitializeAndConfigureUpgrade(_dictOfUpgrades[NamesOfUpgrades.CriticalDamageChance], NamesVariablesPlayerPrefs.CriticalChance, NamesVariablesPlayerPrefs.CriticalChanceUpgradeLevel);
            InitializeAndConfigureUpgrade(_dictOfUpgrades[NamesOfUpgrades.CriticalFactor], NamesVariablesPlayerPrefs.CriticalFactor, NamesVariablesPlayerPrefs.CriticalFactorUpgradeLevel);
            InitializeAndConfigureUpgrade(_dictOfUpgrades[NamesOfUpgrades.Radius], NamesVariablesPlayerPrefs.Radius, NamesVariablesPlayerPrefs.RadiusUpgradeLevel);
            InitializeAndConfigureUpgrade(_dictOfUpgrades[NamesOfUpgrades.Health], NamesVariablesPlayerPrefs.Health, NamesVariablesPlayerPrefs.HealthUpgradeLevel);
            InitializeAndConfigureUpgrade(_dictOfUpgrades[NamesOfUpgrades.HealthRegeneration], NamesVariablesPlayerPrefs.HealthRegeneration, NamesVariablesPlayerPrefs.HealthRegenerationUpgradeLevel);
        }
    
        private IEnumerator UpdateStateOfButton(Upgrade upgrade, int indexLevel)
        {
            var requiredLvl = upgrade.ListOfParameters[indexLevel].RequiredLevel;

            if (_maxOpenLevel < requiredLvl && _currentLvl < requiredLvl || upgrade.MaxLvl <= indexLevel + 1)
                MakeClosedStateOfButton(upgrade);
            
            else
            {
                if (upgrade.ListOfParameters[indexLevel].Price > _money.Number)
                {
                    MakeClosedStateOfButton(upgrade);
                    yield return new WaitUntil(() => upgrade.ListOfParameters[indexLevel].Price <= _money.Number);
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

        private void InitializeAndConfigureUpgrade(Upgrade upgrade, string namePlayerPref, string nameVarLvlPlayerPref)
        {
            var listOfParameters = upgrade.ListOfParameters;

            if (!PlayerPrefs.HasKey(namePlayerPref) || !PlayerPrefs.HasKey(nameVarLvlPlayerPref))
            {
                PlayerPrefs.SetFloat(namePlayerPref, upgrade.InitialValue);
                PlayerPrefs.SetInt(nameVarLvlPlayerPref, IndexOfFirstLevelOfUpgrades);
            }

            var index = PlayerPrefs.GetInt(nameVarLvlPlayerPref);

            SetTexts(new List<TMP_Text> {upgrade.TextOfPrice, upgrade.TextOfLvl}, new List<string>
            {
                listOfParameters[index].Price == 0
                    ? "Free"
                    : listOfParameters[index].Price.ToString(),
                (index + 1).ToString()
            });

            StartCoroutine(UpdateStateOfButton(upgrade, PlayerPrefs.GetInt(nameVarLvlPlayerPref)));
        }
   

        public bool GetIsImprove(int upgradeIndex, Upgrade upgrade, string nameValuePlayerPref, string nameVarLvlPlayerPref)
        {
            if (_money.Number >= upgrade.ListOfParameters[upgradeIndex].Price)
            {
                Improve(upgrade, nameValuePlayerPref, nameVarLvlPlayerPref);
                return PlayerPrefs.GetInt(nameVarLvlPlayerPref) > upgradeIndex;
            }

            return false;
        }

        public void ImproveSometing(int upgradeIndex, Upgrade upgrade, string nameValuePlayerPref, string nameVarLvlPlayerPref, ref float assignmentVariable)
        {
            if (GetIsImprove(upgradeIndex, upgrade, nameValuePlayerPref, nameVarLvlPlayerPref))
                assignmentVariable = upgrade.InitialValue * PlayerPrefs.GetFloat(nameValuePlayerPref);
        }

        public void ImproveDamage() => ImproveSometing(PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.DamageUpgradeLevel), _dictOfUpgrades[NamesOfUpgrades.Damage], 
            NamesVariablesPlayerPrefs.Damage, NamesVariablesPlayerPrefs.DamageUpgradeLevel, ref _tower.Damage);

        public void ImproveAttackSpeed() => ImproveSometing(PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.AttackSpeedUpgradeLevel),
            _dictOfUpgrades[NamesOfUpgrades.AttackSpeed], NamesVariablesPlayerPrefs.AttackSpeed, 
            NamesVariablesPlayerPrefs.AttackSpeedUpgradeLevel, ref _tower.AttackSpeed);
   
        public void ImproveCriticalChance() => ImproveSometing(PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.CriticalChanceUpgradeLevel),
            _dictOfUpgrades[NamesOfUpgrades.CriticalDamageChance], NamesVariablesPlayerPrefs.CriticalChance,
            NamesVariablesPlayerPrefs.CriticalChanceUpgradeLevel, ref _tower.CriticalDamageChance);
   
        public void ImproveCriticalFactor() => ImproveSometing(PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.CriticalFactorUpgradeLevel),
            _dictOfUpgrades[NamesOfUpgrades.CriticalFactor], NamesVariablesPlayerPrefs.CriticalFactor,
            NamesVariablesPlayerPrefs.CriticalFactorUpgradeLevel, ref _tower.CriticalFactor);
   
        public void ImproveCriticalRadius() => ImproveSometing(PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.RadiusUpgradeLevel),
            _dictOfUpgrades[NamesOfUpgrades.Radius], NamesVariablesPlayerPrefs.Radius,
            NamesVariablesPlayerPrefs.RadiusUpgradeLevel, ref _tower.Radius);
   
        public void ImproveCriticalDamageOnMeter() => ImproveSometing(PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.DamageOnMeterUpgradeLevel),
            _dictOfUpgrades[NamesOfUpgrades.DamageOnMeter], NamesVariablesPlayerPrefs.DamageOnMeter,
            NamesVariablesPlayerPrefs.DamageOnMeterUpgradeLevel, ref _tower.DamageOnMeter);

        public void ImproveHealth()
        {
            var ratio = _towerHealth.CurrentHealthPoint / _towerHealth.MaxHealPoint;
            var upgrade = _dictOfUpgrades[NamesOfUpgrades.Health];
            var upgradeIndex = PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.HealthUpgradeLevel);
            var nameValuePlayerPref = NamesVariablesPlayerPrefs.Health;
            var nameVarLvlPlayerPref = NamesVariablesPlayerPrefs.HealthUpgradeLevel;
            
            if (GetIsImprove(upgradeIndex, upgrade, nameValuePlayerPref, nameVarLvlPlayerPref))
            {
                _towerHealth.MaxHealPoint = upgrade.InitialValue * PlayerPrefs.GetFloat(nameValuePlayerPref);
                _towerHealth.CurrentHealthPoint = ratio * _towerHealth.MaxHealPoint;
            }
        } 
   
        public void ImproveHealthRegeneration() => ImproveSometing(PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.HealthRegenerationUpgradeLevel),
            _dictOfUpgrades[NamesOfUpgrades.HealthRegeneration], NamesVariablesPlayerPrefs.HealthRegeneration,
            NamesVariablesPlayerPrefs.HealthRegenerationUpgradeLevel, ref _towerHealth.UnitOfHealthRegeneration);

        private void Improve(Upgrade upgrade, string nameValuePlayerPref, string nameVarLvlPlayerPref, bool isUseTwoListParameters = false)
        {
            var index = PlayerPrefs.GetInt(nameVarLvlPlayerPref);

            var listOfParameters = upgrade.ListOfParameters;

            //if (isUseTwoListParameters)
                //listOfParameters = upgrade.ListOfParametersOfSecondParameter;

            if (index + 1 < upgrade.MaxLvl)
            {
                if (_money.Number >= listOfParameters[index].Price)
                {
                    SetTexts(new List<TMP_Text> {upgrade.TextOfPrice, upgrade.TextOfLvl},
                        new List<string>
                        {
                            listOfParameters[index + 1].Price == 0 ? "Free" : listOfParameters[index + 1].Price.ToString(),
                            (index + 2).ToString(), (index + 1).ToString() });
                    PlayerPrefs.SetFloat(nameValuePlayerPref, listOfParameters[index].Multiplier * upgrade.InitialValue);
                    PlayerPrefs.SetInt(nameVarLvlPlayerPref,  index + 1);
                    _money.Number -= listOfParameters[index].Price;
                    _money.SaveInPlayerPref();
                    //_notificationText.text = upgrade.NameInNotification + _contentOfNotificationAboutImproveOfUpgrade;
                    StartCoroutine(UpdateStateOfButton(upgrade, index + 1));
                }
            }
        }

        private void SetTexts(List<TMP_Text> listOfTexts, List<string> listOfContent)
        {
            for (var i = 0; i < listOfTexts.Count; i++)
                listOfTexts[i].text = listOfContent[i];
        }
   
    }
}
