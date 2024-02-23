using System.Collections;
using TMPro;
using UnityEngine;

namespace Currencies
{
    public abstract class Currency : MonoBehaviour
    {
        protected float SumAll { get; set; }

        [SerializeField] private TMP_Text _text;
        [SerializeField] private int _numberAfterResetToZero;
        [SerializeField] private float _timeOfAccumulationAllGettingCurrency = 2f;
        [SerializeField] private bool _isResetToZeroCurrentNumberOnStart;
        [SerializeField] private bool _isImageShiftingToLeft = true;
        protected string nameVariableInPlayerPref = "UnknownCurrency";
        private string _contentOnSpriteZeroNotNull;
    
        private float _number;
    
        public float Number
        {
            get => _number;
            set
            {
                if (_number > value)
                {
                    var difference = _number - value;
                    SumAll -= difference;   
                }

                _number = value;
                UpdateContentOnText();
            }
        }
        protected void Awake()
        {
            if (_text.spriteAsset)
                _contentOnSpriteZeroNotNull = "<sprite=0>";

            if (!PlayerPrefs.HasKey(nameVariableInPlayerPref) || _isResetToZeroCurrentNumberOnStart)
                PlayerPrefs.SetInt(nameVariableInPlayerPref, _numberAfterResetToZero);

            Number = PlayerPrefs.GetInt(nameVariableInPlayerPref);
            SumAll = Number;
        }
    
        public void SaveInPlayerPref() => PlayerPrefs.SetFloat(nameVariableInPlayerPref, _number);

        private void UpdateContentOnText() => _text.text = _isImageShiftingToLeft ? _contentOnSpriteZeroNotNull + _number : 
            _number + _contentOnSpriteZeroNotNull;
    
        public IEnumerator IncreaseCounter(int additionalCurrency)
        {
            var number = Number;

            if (Number != SumAll)
                Number = SumAll;
        
            SumAll = number + additionalCurrency;

            var time = 0f;
            while (_timeOfAccumulationAllGettingCurrency > time)
            {
                Number = (int)Mathf.Lerp(number, SumAll, time);

                time += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        public void Increase(int number) => Number += number;
        

        private IEnumerator OffObject(GameObject obj, float timeBeforeOff)
        {
            var offSetTime = 0.5f;
            yield return new WaitForSecondsRealtime(timeBeforeOff + offSetTime);
            obj.SetActive(false);   
        }

        public void GuaranteedSaveData()
        {
            StopAllCoroutines();
            if (Number < SumAll)
                Number = SumAll;
        
            SaveInPlayerPref();
        }
    
#if !UNITY_EDITOR
    private void OnApplicationFocus(bool isHasFocus)
    {
        if (SceneManager.GetActiveScene().buildIndex == _indexMainMenu && !isHasFocus)
            GuaranteedSaveData();
    }
#endif
    }
}
