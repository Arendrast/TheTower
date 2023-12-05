using System;
using System.Collections;
using System.Collections.Generic;
using General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class SpawnEnemy : MonoBehaviour, IObjectBeindInitialized
    {
        [SerializeField] private float _radiusOfSpawnArea = 2;
        [SerializeField] private float _ratioRadiusOfDrawingCircleToReal = 57.5f;
        [SerializeField] private float _timeBeforeStartNextWave = 0.5f;
        [SerializeField] private float _timeBeforeStartOneWave = 1f;
        [SerializeField] private Transform _player;
        [SerializeField] private bool _isDrawSpawnZone = true;
        [SerializeField] private List<Wave> _waveList = new List<Wave>();

        [Space] [Header("Slider")] 
        [SerializeField] private float _speedMovementSliderWavePerSecond = 0.5f;
        [SerializeField] private Slider _sliderWave;
        [SerializeField] private TMP_Text _textNumberWave;

        private bool _isNeedToToTopUpSlider;
        private bool _isPlayerLoose;
        private int _currentWave;
        private int _numberEnemyOnStage;
        private int _numberEnemyOnWave;
        private int _currentNumberEnemyOnScene;
        private const float FullAngle = 360;
        private IEnumerator _currentSliderFillingCoroutine;
        private float _currentEndValueOfSlider;

        [Serializable]
        public class GroupOfEnemies
        {
            [field: SerializeField] public EnemyHealth Enemy { get; private set; }
            [field: SerializeField] public float TimeToSpawn { get; private set; } = 1;
            [field: SerializeField] public int Number { get; private set; } = 1;
        }

        [Serializable]
        public class Wave
        {
            [field: SerializeField] public List<Stage> ListOfStages { get; private set; }
        }

        [Serializable]
        public class Stage
        {
            [field: SerializeField] public List<GroupOfEnemies> EnemyList { get; private set; }
        }

        private IEnumerator FillSlider()
        {
            if (_currentSliderFillingCoroutine != null)
                StopCoroutine(_currentSliderFillingCoroutine);

            if (Math.Abs(_sliderWave.value - _currentEndValueOfSlider) > Constants.Epsilon)
                _sliderWave.value = _currentEndValueOfSlider;
            
            _currentEndValueOfSlider = _numberEnemyOnWave - _currentNumberEnemyOnScene;
            _currentSliderFillingCoroutine = FillSlider();
            
            while (Math.Abs(_sliderWave.value - _currentEndValueOfSlider) > Constants.Epsilon)
            {
                _sliderWave.value = Mathf.MoveTowards(
                    _sliderWave.value,
                    _numberEnemyOnWave - _currentNumberEnemyOnScene,
                    _speedMovementSliderWavePerSecond * Time.deltaTime);

                yield return null;
            }
        }

        public void Initialize()
        {
            StartCoroutine(StartNewWave(_timeBeforeStartOneWave));
            _textNumberWave.text = $"1/{_waveList.Count}";
            _sliderWave.value = 0;
        }

        private IEnumerator StartNewWave(float timeToStart = 0)
        {
            yield return new WaitForSeconds(timeToStart);
        
            var listOfStages = _waveList[_currentWave].ListOfStages;
            _textNumberWave.text = $"{_currentWave + 1}/{_waveList.Count}";

            _numberEnemyOnWave = 0;
            foreach (var stage in listOfStages)
            {
                for (var enemyGroupIndex = 0; enemyGroupIndex < stage.EnemyList.Count; enemyGroupIndex++)
                {
                    var enemyGroup = stage.EnemyList[enemyGroupIndex];
                    if (enemyGroup.Enemy && enemyGroup.Number > 0)
                    {
                        _numberEnemyOnWave += enemyGroup.Number;
                    }
                    else
                    {
                        stage.EnemyList.Remove(enemyGroup);
                    }
                }
            }

            _sliderWave.value = 0;
            _currentEndValueOfSlider = 0;
            StopCoroutine(FillSlider());
            _sliderWave.maxValue = _numberEnemyOnWave;
        
            for (var stageIndex = 0; stageIndex < listOfStages.Count; stageIndex++)
            {
                var enemyList = listOfStages[stageIndex].EnemyList;
                var enemyListWithValidEnemyGroup = new List<GroupOfEnemies>();
            
                foreach (var enemyGroup in enemyList)
                {
                    _numberEnemyOnStage += enemyGroup.Number;   
                    enemyListWithValidEnemyGroup.Add(enemyGroup);
                }

                if (_numberEnemyOnStage == 0)
                    break;

                var time = 0f;
                const float QuaterCircle = FullAngle / 4;
                while (enemyListWithValidEnemyGroup.Count > 0)
                {
                    for (var enemyIndex = 0; enemyIndex < enemyListWithValidEnemyGroup.Count; enemyIndex++)
                    {
                        var group = enemyListWithValidEnemyGroup[enemyIndex];
                        if (group.TimeToSpawn <= time)
                        {
                            var listOfSectorIndexes = new List<int> {1, 2, 3, 4};
                            for (var i = 0; i < group.Number; i++)
                            {
                                var sectorIndex = listOfSectorIndexes[Random.Range(0, listOfSectorIndexes.Count)];

                                var angle = Random.Range(sectorIndex == 1 ? 0 : (sectorIndex - 1) * QuaterCircle, sectorIndex * QuaterCircle);

                                var currentEnemy = Instantiate(group.Enemy, new Vector2(
                                        _radiusOfSpawnArea * Mathf.Deg2Rad * Mathf.Cos(angle),
                                        _radiusOfSpawnArea * Mathf.Deg2Rad * Mathf.Sin(angle)),
                                    group.Enemy.transform.rotation);
                                currentEnemy.OnDie.AddListener(ReduceNumberEnemies);

                                listOfSectorIndexes.Remove(sectorIndex);
                                if (listOfSectorIndexes.Count == 0)
                                    listOfSectorIndexes = new List<int> {1, 2, 3, 4};
                            }

                            _currentNumberEnemyOnScene += group.Number;
                            enemyListWithValidEnemyGroup.Remove(group);
                        }
                    }

                    time += Time.deltaTime;
                    yield return null;
                }
                
                yield return new WaitUntil(() => _currentNumberEnemyOnScene == 0);
            }

            if (_currentWave + 1 != _waveList.Count)
            {
                yield return new WaitForSeconds(_timeBeforeStartNextWave);
                _currentWave++;
                StartCoroutine(StartNewWave());
            }
        }

        private void ReduceNumberEnemies()
        {
            _currentNumberEnemyOnScene--;
            StartCoroutine(FillSlider());
        } 
        

        private void OnDrawGizmos()
        {
            if (_isDrawSpawnZone)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_player.position, _radiusOfSpawnArea / _ratioRadiusOfDrawingCircleToReal);
            }
        }
    }
}