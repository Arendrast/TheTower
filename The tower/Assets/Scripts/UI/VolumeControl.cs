using System;
using General;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class VolumeСontrol : MonoBehaviour, IObjectBeindInitialized
    {
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private Slider _slider;
        [SerializeField] private Toggle _toggle;
        [SerializeField] private string _nameVarSliderPlayerPrefs;
        [SerializeField] private string _nameVarTogglePlayerPrefs;

        [SerializeField] private float _volumeMultiplier = 20;
    
        private float _volume = 1;
        private bool _isOn = true;

        public void Initialize()
        {
            if (!PlayerPrefs.HasKey(_nameVarSliderPlayerPrefs))
                SaveValueSliderInPlayerPref();   
            if (!PlayerPrefs.HasKey(_nameVarTogglePlayerPrefs))
                SaveValueToggleInPlayerPref();
            
            _slider.value = PlayerPrefs.GetFloat(_nameVarSliderPlayerPrefs);
            SetVolume();
            _toggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt(_nameVarTogglePlayerPrefs));
        }

        public void SetVolume()
        {
            _volume = _slider.value;
            if (_isOn)
                _mixer.SetFloat("volume", Mathf.Log10(_volume) * _volumeMultiplier);
            SaveValueSliderInPlayerPref();
        }

        private void SaveValueSliderInPlayerPref() => PlayerPrefs.SetFloat(_nameVarSliderPlayerPrefs, _volume);
        

        public void SaveValueToggleInPlayerPref()
        {
            _isOn = _toggle.isOn;
            PlayerPrefs.SetInt(_nameVarTogglePlayerPrefs, Convert.ToInt32(_isOn));
            
                _mixer.SetFloat("volume", Mathf.Log10(!_isOn 
                    ? _slider.minValue 
                    : _volume) * _volumeMultiplier);
        }
    }
}