using System;
using System.Collections;
using Enemies;
using General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class TowerHealth : Health, IObjectBeindInitialized
    {
        public string NameLastDamager { get; private set; }
        public float MaxHealth 
        {
            get => maxHealth;
            set
            {
                _healthSlider.maxValue = value;
                maxHealth = value;
            }
        }

        [Space] 
        [SerializeField] private SpawnEnemy _spawnEnemy;
        [SerializeField] private Upgrades _upgrades;
        [SerializeField] private TMP_Text _healthCounter;
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private float _unitOfHealthRegeneration;
        [SerializeField] private float _regenerationFrequencyInSec = 1;

        public void Initialize()
        {
            MaxHealth = PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.NamesOfUpgrades.Health.ToString());
            _unitOfHealthRegeneration = PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.NamesOfUpgrades.HealthRegeneration.ToString());
            
            Start();
            
            StartCoroutine(Regenerate());
            
            _healthSlider.maxValue = MaxHealth;
            _healthSlider.value = MaxHealth;
            
            SetSubscribeToMethods(true);
            
            UpdateText();
        }

        private void OnDisable() => SetSubscribeToMethods(true);

        public void TakeDamage(float damage, string damagerName)
        {
            if (base.TakeDamage(damage))
                NameLastDamager = damagerName;
        }

        public override bool TakeDamage(float damage)
        {
            Debug.LogError("Вызов перегрузки этого метода не возможен. Используйте другую перегрузку.");
            return false;
        }

        private void SetSubscribeToMethods(bool isSubscribe)
        {
            var healthRegeneration = NamesVariablesPlayerPrefs.NamesOfUpgrades.HealthRegeneration;

            if (isSubscribe)
            {
                _upgrades.DictOfUpgrades[healthRegeneration].OnChangeValue += value => _unitOfHealthRegeneration = value;   
            }

            else
            {
                _upgrades.DictOfUpgrades[healthRegeneration].OnChangeValue -= value => _unitOfHealthRegeneration = value;   
            }
        }

        private IEnumerator Regenerate()
        {
            yield return new WaitForSeconds(_regenerationFrequencyInSec);
            RestoreHealth(_unitOfHealthRegeneration);

            StartCoroutine(Regenerate());
        }
        
        public void UpdateValueOnSlider() => _healthSlider.value = CurrentHealth;

        public void UpdateText() => _healthCounter.text = $"{CurrentHealth}/{maxHealth}";
    }
}
