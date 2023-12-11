using System.Collections;
using General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class TowerHealth : Health, IObjectBeindInitialized
    {
        public float MaxHealPoint 
        {
            get => maxHealPoint;
            set
            {
                _healthSlider.maxValue = value;
                maxHealPoint = value;
            }
        }
        
        [Space]
        public float UnitOfHealthRegeneration;

        [SerializeField] private TMP_Text _healthCounter;
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private float _regenerationFrequencyInSec = 1;

        public void Initialize()
        {
            Start();
            StartCoroutine(Regenerate());
            _healthSlider.maxValue = MaxHealPoint;
            _healthSlider.value = MaxHealPoint;
            UpdateText();
        }

        private IEnumerator Regenerate()
        {
            yield return new WaitForSeconds(_regenerationFrequencyInSec);
            RestoreHealth(UnitOfHealthRegeneration);

            StartCoroutine(Regenerate());
        }
        
        public void UpdateValueOnSlider() => _healthSlider.value = CurrentHealthPoint;

        public void UpdateText() => _healthCounter.text = $"{CurrentHealthPoint}/{maxHealPoint}";
    }
}
