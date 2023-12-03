using System;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Upgrades : MonoBehaviour
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
    private const int IndexOfFirstLevelOfUpgrades = 1;
    private const int IndexOfValueText = 0;

    [Serializable]
    public class Upgrade
    {   
        [HideInInspector] public string Name;
        [SerializeField] private string _nameInInspector;
        [field: SerializeField] public string NameInNotification { get; private set; }
        [field: SerializeField] public TMP_Text TextOfPrice { get; private set; }
        [field: SerializeField] public TMP_Text TextOfRequiredLvl { get; private set; }
        [field: SerializeField] public int MaxLvl { get; private set; }
        [field: SerializeField] public Sprite SpriteOnClosed { get; private set; }
        [field: SerializeField] public Sprite DefaultSprite { get; private set; }
        [field: SerializeField] public Button UseButton { get; private set; }
        [field: SerializeField] public List<FieldUpgrade> ListOfParameters { get; private set; }
        [field: SerializeField] public List<FieldUpgrade> ListOfParametersOfSecondParameter { get; private set; }
        [field: SerializeField] public List<TMP_Text> ListOfTextsOfValues { get; private set; }
        [field: SerializeField] public bool IsStopGameFlashingObject { get; private set; }
        [field: SerializeField] public float InitialValue { get; private set; }
    }
    
    private readonly Dictionary<NamesOfUpgrades, Upgrade> _dictOfUpgrades = new Dictionary<NamesOfUpgrades, Upgrade>();
    private const int IndexOfElementOfFirstUpgrade = 1;

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

    private void Awake()
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
        
        _errorText.gameObject.SetActive(true);

        foreach (var upgrade in _dictOfUpgrades.Values)
        {
            upgrade.TextOfRequiredLvl.gameObject.SetActive(false);
            upgrade.UseButton.image.sprite = upgrade.DefaultSprite;
        }

        Initialize();
    }

    private void Initialize()
    {
        if (!PlayerPrefs.HasKey(NamesVariablesPlayerPrefs.MaxOpenLvl))
            PlayerPrefs.SetInt(NamesVariablesPlayerPrefs.MaxOpenLvl, 1);
        
        _maxOpenLevel = PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.MaxOpenLvl);
        _currentLvl = SceneManager.GetActiveScene().buildIndex;
        
        InitializeAndConfigureUpgrade(_dictOfUpgrades[NamesOfUpgrades.Damage], NamesVariablesPlayerPrefs.DamageMultiplier, NamesVariablesPlayerPrefs.DamageUpgradeLevel);
        InitializeAndConfigureUpgrade(_dictOfUpgrades[NamesOfUpgrades.AttackSpeed], NamesVariablesPlayerPrefs.AttackSpeedMultiplier, NamesVariablesPlayerPrefs.AttackSpeedUpgradeLevel);
        InitializeAndConfigureUpgrade(_dictOfUpgrades[NamesOfUpgrades.CriticalDamageChance], NamesVariablesPlayerPrefs.CriticalChanceMultiplier, NamesVariablesPlayerPrefs.CriticalChanceUpgradeLevel);
        InitializeAndConfigureUpgrade(_dictOfUpgrades[NamesOfUpgrades.CriticalFactor], NamesVariablesPlayerPrefs.CriticalFactorMultiplier, NamesVariablesPlayerPrefs.CriticalFactorUpgradeLevel);
        InitializeAndConfigureUpgrade(_dictOfUpgrades[NamesOfUpgrades.Radius], NamesVariablesPlayerPrefs.RadiusMultiplier, NamesVariablesPlayerPrefs.RadiusUpgradeLevel);
        InitializeAndConfigureUpgrade(_dictOfUpgrades[NamesOfUpgrades.Health], NamesVariablesPlayerPrefs.HealthMultiplier, NamesVariablesPlayerPrefs.HealthUpgradeLevel);
        InitializeAndConfigureUpgrade(_dictOfUpgrades[NamesOfUpgrades.HealthRegeneration], NamesVariablesPlayerPrefs.HealthRegenerationMultiplier, NamesVariablesPlayerPrefs.HealthRegenerationUpgradeLevel);
    }
    
   private IEnumerator UpdateStateOfButton(Upgrade upgrade, int indexLevel)
   {
       var requiredLvl = upgrade.ListOfParameters[indexLevel].RequiredLevel;
       
        if (_maxOpenLevel < requiredLvl && _currentLvl < requiredLvl)
        {
            upgrade.TextOfPrice.gameObject.SetActive(false);
            upgrade.TextOfRequiredLvl.gameObject.SetActive(true);

            upgrade.TextOfRequiredLvl.text = requiredLvl.ToString();
            upgrade.UseButton.enabled = false;
            upgrade.UseButton.image.sprite = upgrade.SpriteOnClosed;
        }
        else
        {
            if (upgrade.ListOfParameters[indexLevel].Price > _money.Number)
                yield return new WaitUntil(() => upgrade.ListOfParameters[indexLevel].Price <= _money.Number);
            
            upgrade.TextOfPrice.gameObject.SetActive(true);
            upgrade.TextOfRequiredLvl.gameObject.SetActive(false);

            upgrade.UseButton.enabled = true;
            upgrade.UseButton.image.sprite = upgrade.DefaultSprite;
        }
   }

   private void InitializeAndConfigureUpgrade(Upgrade upgrade, string namePlayerPref, string nameVarLvlPlayerPref)
   {
       var listOfParameters = upgrade.ListOfParameters;

       if (!PlayerPrefs.HasKey(namePlayerPref) || !PlayerPrefs.HasKey(nameVarLvlPlayerPref))
       {
           PlayerPrefs.SetFloat(namePlayerPref, upgrade.ListOfParameters[0].Multiplier);
           PlayerPrefs.SetInt(nameVarLvlPlayerPref, IndexOfFirstLevelOfUpgrades);
       }

       var index = PlayerPrefs.GetInt(nameVarLvlPlayerPref);

       var requiredLvl = listOfParameters[IndexOfElementOfFirstUpgrade].RequiredLevel;
       var isUpgradeOpened = requiredLvl <= _currentLvl || requiredLvl <= _maxOpenLevel;

       if (!isUpgradeOpened)
       {
           upgrade.TextOfPrice.gameObject.SetActive(false);
           upgrade.TextOfRequiredLvl.gameObject.SetActive(true);
           upgrade.UseButton.image.sprite = upgrade.SpriteOnClosed;
           upgrade.TextOfRequiredLvl.text = requiredLvl.ToString();
           upgrade.UseButton.enabled = false;
       }

       SetTexts(new List<TMP_Text> {upgrade.TextOfPrice, upgrade.ListOfTextsOfValues[IndexOfValueText]}, new List<string>
           {
               listOfParameters[index].Price == 0
                   ? "Free"
                   : listOfParameters[index].Price.ToString(),
               index.ToString()
           });
       
       StartCoroutine(UpdateStateOfButton(upgrade, PlayerPrefs.GetInt(namePlayerPref)));
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
           NamesVariablesPlayerPrefs.DamageMultiplier, NamesVariablesPlayerPrefs.DamageUpgradeLevel, ref _tower.Damage);

   public void ImproveAttackSpeed() => ImproveSometing(PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.AttackSpeedUpgradeLevel),
           _dictOfUpgrades[NamesOfUpgrades.AttackSpeed], NamesVariablesPlayerPrefs.AttackSpeedMultiplier, 
           NamesVariablesPlayerPrefs.AttackSpeedUpgradeLevel, ref _tower.AttackSpeed);
   
   public void ImproveCriticalChance() => ImproveSometing(PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.CriticalChanceUpgradeLevel),
           _dictOfUpgrades[NamesOfUpgrades.CriticalDamageChance], NamesVariablesPlayerPrefs.CriticalChanceMultiplier,
           NamesVariablesPlayerPrefs.CriticalChanceUpgradeLevel, ref _tower.CriticalDamageChance);
   
   public void ImproveCriticalFactor() => ImproveSometing(PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.CriticalFactorUpgradeLevel),
       _dictOfUpgrades[NamesOfUpgrades.CriticalFactor], NamesVariablesPlayerPrefs.CriticalFactorMultiplier,
       NamesVariablesPlayerPrefs.CriticalFactorUpgradeLevel, ref _tower.CriticalFactor);
   
   public void ImproveCriticalRadius() => ImproveSometing(PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.RadiusUpgradeLevel),
       _dictOfUpgrades[NamesOfUpgrades.Radius], NamesVariablesPlayerPrefs.RadiusMultiplier,
       NamesVariablesPlayerPrefs.RadiusUpgradeLevel, ref _tower.Radius);
   
   public void ImproveCriticalDamageOnMeter() => ImproveSometing(PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.DamageOnMeterUpgradeLevel),
       _dictOfUpgrades[NamesOfUpgrades.DamageOnMeter], NamesVariablesPlayerPrefs.DamageOnMeterMultiplier,
       NamesVariablesPlayerPrefs.DamageOnMeterUpgradeLevel, ref _tower.DamageOnMeter);

   public void ImproveHealth()
   {
       var currentLvl = PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.HealthUpgradeLevel);
       var ratio = _towerHealth.CurrentHealth / _towerHealth.MaxHealPoint; 
       ImproveSometing(PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.HealthUpgradeLevel),
           _dictOfUpgrades[NamesOfUpgrades.Health], NamesVariablesPlayerPrefs.HealthMultiplier,
           NamesVariablesPlayerPrefs.HealthUpgradeLevel, ref _towerHealth.MaxHealPoint);

       if (PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.HealthUpgradeLevel) > currentLvl)
        _towerHealth.CurrentHealPoint = ratio * _towerHealth.MaxHealPoint;
   } 
   
   public void ImproveHealthRegeneration() => ImproveSometing(PlayerPrefs.GetInt(NamesVariablesPlayerPrefs.HealthRegenerationUpgradeLevel),
       _dictOfUpgrades[NamesOfUpgrades.HealthRegeneration], NamesVariablesPlayerPrefs.HealthRegenerationMultiplier,
       NamesVariablesPlayerPrefs.HealthRegenerationUpgradeLevel, ref _towerHealth.unitOfHealthRegeneration);

   private void Improve(Upgrade upgrade, string nameValuePlayerPref, string nameVarLvlPlayerPref, bool isUseTwoListParameters = false)
   {
       var index = PlayerPrefs.GetInt(nameVarLvlPlayerPref);

       var listOfParameters = upgrade.ListOfParameters;

       if (isUseTwoListParameters)
           listOfParameters = upgrade.ListOfParametersOfSecondParameter;

       if (index < upgrade.MaxLvl)
       {
           if (_money.Number >= listOfParameters[index].Price)
           {
               SetTexts(new List<TMP_Text> {upgrade.TextOfPrice, upgrade.ListOfTextsOfValues[IndexOfValueText]},
                   new List<string>
                   {
                       listOfParameters[index + 1].Price == 0 ? "Free" : listOfParameters[index + 1].Price.ToString(),
                       (index + 1).ToString(), (index + 1).ToString() });
               PlayerPrefs.SetFloat(nameValuePlayerPref, listOfParameters[index].Multiplier);
               PlayerPrefs.SetInt(nameVarLvlPlayerPref, index + 1);
               _money.Number -= listOfParameters[index].Price;
               _money.SaveInPlayerPref();
               _notificationText.text = upgrade.NameInNotification + _contentOfNotificationAboutImproveOfUpgrade;
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
