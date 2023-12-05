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
                _hpSlider.maxValue = value;
                maxHealPoint = value;
            }
        }
        
        [Space]
        public float UnitOfHealthRegeneration;

        [SerializeField] private TMP_Text _text;
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private float _regenerationFrequencyInSec = 1;

        public void Initialize()
        {
            Start();
            StartCoroutine(Regenerate());
            _hpSlider.maxValue = MaxHealPoint;
            _hpSlider.value = MaxHealPoint;
            UpdateText();
        }

        private IEnumerator Regenerate()
        {
            yield return new WaitForSeconds(_regenerationFrequencyInSec);
            RestoreHealth(UnitOfHealthRegeneration);

            StartCoroutine(Regenerate());
        }
        
        public void UpdateValueOnSlider() => _hpSlider.value = CurrentHealthPoint;

        public void UpdateText() => _text.text = $"{CurrentHealthPoint}/{maxHealPoint}";
    }
}
