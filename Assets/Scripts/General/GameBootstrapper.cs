using System.Collections.Generic;
using Enemies;
using Player;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace General
{
    public class GameBootstrapper : MonoBehaviour
    {
        [Space] [Header("Music")] [SerializeField]
        private List<AudioSource> _audioSourceList = new List<AudioSource>();

        [SerializeField] private AudioSource _audioSource;

        [Space] [Header("ScriptsForInitialize")] 
        [SerializeField] private Upgrades _upgrades;
        [SerializeField] private TowerHealth _towerHealth;
        [SerializeField] private Tower _tower;
        [SerializeField] private UnityEvent _onAwake;

        private void Awake()
        {
            _upgrades.Initialize();
            _towerHealth.Initialize();
            _tower.Initialize();

            _onAwake?.Invoke();
        }

        public void ChooseRandomAudioSource()
        {
            var audioSource = _audioSourceList[Random.Range(0, _audioSourceList.Count)];
            _audioSource.clip = audioSource.clip;
            _audioSource.pitch = _audioSource.pitch;
            _audioSource.priority = _audioSource.priority;
            _audioSource.volume = audioSource.volume;
            _audioSource.Play();
        }
    }
}