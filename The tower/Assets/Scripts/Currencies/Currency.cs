using System.Collections;
using TMPro;
using UnityEngine;

namespace Currencies
{
    public class Currency : MonoBehaviour
    {
        public int SumAll { get; private set; }

        [SerializeField] private TMP_Text _text;
        [SerializeField] private int _numberAfterResetToZero;
        [SerializeField] private float _timeOfAccumulationAllGettingCurrency = 2f;
        [SerializeField] private bool _isResetToZeroCurrentNumberOnStart;
        [SerializeField] private bool _isImageShiftingToLeft = true;
        protected string nameVariableInPlayerPref = "UnknownCurrency";
        private string _contentOnSpriteZeroNotNull;
    
        protected int number;
    
        public int Number
        {
            get => number;
            set
            {
                if (number > value)
                {
                    var difference = number - value;
                    SumAll -= difference;   
                }

                number = value;
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
    
        public void SaveInPlayerPref() => PlayerPrefs.SetInt(nameVariableInPlayerPref, number);

        private void UpdateContentOnText() => _text.text = _isImageShiftingToLeft ? _contentOnSpriteZeroNotNull + number : 
            number + _contentOnSpriteZeroNotNull;
    
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
