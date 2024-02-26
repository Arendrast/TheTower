using System;
using System.Collections;
using System.Collections.Generic;
using Currencies;
using General;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class SpawnEnemy : MonoBehaviour
    {
        public int CurrentWave { get; private set; }
        [Range(0, 1000)] [SerializeField] private float _radiusOfSpawnArea = 2;
        [Range(0, 100)] [SerializeField] private float _ratioRadiusOfDrawingCircleToReal = 57.5f;
        [Range(0, 10)] [SerializeField] private float _timeBeforeStartNextWave = 0.5f;
        [Range(0, 100)] [SerializeField] private float _timeBeforeStartOneWave = 1f;
        [SerializeField] private UnityEvent _onWin;
        private Transform Player => _towerHealth.transform;
        [SerializeField] private TowerHealth _towerHealth;
        [SerializeField] private MoneyCurrency _moneyCurrency;
        [SerializeField] private bool _isDrawSpawnZone = true;

        [Space] [Header("Slider")] [Range(0, 10)] [SerializeField]
        private float _speedMovementSliderWavePerSecond = 0.5f;

        [SerializeField] private Slider _sliderWave;
        [SerializeField] private TMP_Text _textNumberWave;

        [Space] [SerializeField] private List<Wave> _waveList = new List<Wave>();

        private bool _isNeedToToTopUpSlider;
        private bool _isPlayerLoose;
        private int _numberEnemyOnWave;
        private int _numberOfRemainingEnemiesOnWave;
        private int _numberOfRemainingEnemiesOnStage;

        private const float FullAngle = 360;
        private IEnumerator _currentSliderFillingCoroutine;
        private float _currentEndValueOfSlider;

        [Serializable]
        public class GroupOfEnemies
        {
            [field: SerializeField] public Enemy Enemy { get; private set; }
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
            
            _currentEndValueOfSlider = _numberEnemyOnWave - _numberOfRemainingEnemiesOnWave;
            _currentSliderFillingCoroutine = FillSlider();
            
            while (Math.Abs(_sliderWave.value - _currentEndValueOfSlider) > Constants.Epsilon)
            {
                _sliderWave.value = Mathf.MoveTowards(
                    _sliderWave.value,
                    _numberEnemyOnWave - _numberOfRemainingEnemiesOnWave,
                    _speedMovementSliderWavePerSecond * PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.GameSpeed) * Time.deltaTime);

                yield return null;
            }
        }

        public void Start()
        {
            StartCoroutine(StartNewWave(_timeBeforeStartOneWave));
            _textNumberWave.text = $"1/{_waveList.Count}";
            _towerHealth.OnDie.AddListener(StopAllCoroutines);
            _sliderWave.value = 0;
        }

        private IEnumerator StartNewWave(float timeToStart = 0)
        {
            yield return new WaitForSeconds(timeToStart);
        
            var listOfStages = _waveList[CurrentWave].ListOfStages;
            _textNumberWave.text = $"{CurrentWave + 1}/{_waveList.Count}";

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

            _numberOfRemainingEnemiesOnWave = _numberEnemyOnWave;

            _sliderWave.value = 0;
            _currentEndValueOfSlider = 0;
            StopCoroutine(FillSlider());
            _sliderWave.maxValue = _numberEnemyOnWave;

            for (var stageIndex = 0; stageIndex < listOfStages.Count; stageIndex++)
            {
                var enemyList = listOfStages[stageIndex].EnemyList;
                var enemyListWithValidEnemyGroup = new List<GroupOfEnemies>();

                _numberOfRemainingEnemiesOnStage = 0;
                foreach (var enemyGroup in enemyList)
                {
                    _numberOfRemainingEnemiesOnStage += enemyGroup.Number;
                    enemyListWithValidEnemyGroup.Add(enemyGroup);
                }
                
                if (_numberOfRemainingEnemiesOnStage == 0)
                    continue;

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

                                var enemyHealth = currentEnemy.EnemyHealth;
                                enemyHealth.OnDie.AddListener(ReduceNumberEnemies);
                                RotateEnemy(currentEnemy.transform);
                                currentEnemy.EndPointOfMovement = Player.transform.position;
                                enemyHealth.OnDie.AddListener(() => _moneyCurrency.Number += enemyHealth.RewardForKill * PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.NamesOfUpgrades.CashBonus.ToString()));
                                _towerHealth.OnDie.AddListener(() => { if (currentEnemy) currentEnemy.StopMove(); });

                                listOfSectorIndexes.Remove(sectorIndex);
                                if (listOfSectorIndexes.Count == 0)
                                    listOfSectorIndexes = new List<int> {1, 2, 3, 4};
                            }
                            enemyListWithValidEnemyGroup.Remove(group);
                        }
                    }

                    time += Time.deltaTime * PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.GameSpeed);
                    yield return null;
                }
                
                yield return new WaitUntil(() => _numberOfRemainingEnemiesOnStage <= 0);
            }

            _moneyCurrency.Number += PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.NamesOfUpgrades.CashBonusForWave.ToString());

            if (CurrentWave + 1 != _waveList.Count && !_towerHealth.IsDie)
            {
                yield return new WaitForSeconds(_timeBeforeStartNextWave);
                CurrentWave++;
                StartCoroutine(StartNewWave());
            }
            else
                _onWin?.Invoke();
        }

        public void RotateEnemy(Transform enemy)
        {
            var direction = Player.transform.position - enemy.transform.position;
            var angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            var offSet = 90f;
            enemy.transform.rotation = Quaternion.Euler(0, 0, angle + offSet);

        }

        private void ReduceNumberEnemies()
        {
            _numberOfRemainingEnemiesOnWave--;
            _numberOfRemainingEnemiesOnStage--;
            StartCoroutine(FillSlider());
        } 
        

        private void OnDrawGizmos()
        {
            if (_isDrawSpawnZone)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(Player.position, _radiusOfSpawnArea / _ratioRadiusOfDrawingCircleToReal);
            }
        }
    }
}