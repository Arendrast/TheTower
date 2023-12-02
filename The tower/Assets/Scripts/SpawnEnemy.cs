using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private float _radiusOfSpawnArea = 2;
    [SerializeField] private float _ratioRadiusOfDrawingCircleToReal = 57.5f;
    [SerializeField] private float _timeBeforeStartNextWave = 0.5f;
    [SerializeField] private float _timeBeforeStartOneWave = 1f;
    [SerializeField] private Transform _player;
    [SerializeField] private Slider _sliderWave;
    [SerializeField] private TMP_Text _textNumberWave;
    [SerializeField] private bool _isDrawSpawnZone = true;
    [SerializeField] private List<Wave> _waveList = new List<Wave>();

    private bool _isPlayerLoose;
    private int _currentWave;
    private int _currentNumberEnemyOnStage;
    private int _currentNumberEnemyOnWave;
    private const float FullAngle = 360;

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

    private void Start()
    {
        StartCoroutine(StartNewWave(_timeBeforeStartOneWave));
    }

    private IEnumerator StartNewWave(float timeToStart = 0)
    {
        yield return new WaitForSeconds(timeToStart);
        
        var listOfStages = _waveList[_currentWave].ListOfStages;
        
        foreach (var stage in listOfStages)
        {
            for (var enemyGroupIndex = 0; enemyGroupIndex < stage.EnemyList.Count; enemyGroupIndex++)
            {
                var enemyGroup = stage.EnemyList[enemyGroupIndex];
                if (enemyGroup.Enemy && enemyGroup.Number > 0)
                {
                    _currentNumberEnemyOnWave += enemyGroup.Number;
                }
                else
                {
                    stage.EnemyList.Remove(enemyGroup);
                }
            }
        }
        
        for (var stageIndex = 0; stageIndex < listOfStages.Count; stageIndex++)
        {
            var enemyList = listOfStages[stageIndex].EnemyList;
            var enemyListWithValidEnemyGroup = new List<GroupOfEnemies>();
            
            foreach (var enemyGroup in enemyList)
            {
                _currentNumberEnemyOnStage += enemyGroup.Number;   
                enemyListWithValidEnemyGroup.Add(enemyGroup);
            }

            if (_currentNumberEnemyOnStage == 0)
                break;

            //_numberEnemyOnWave = 0;

            //_sliderWave.value = 0;
            //_sliderWave.maxValue = _numberEnemyOnWave;

            var time = 0f;
            var quaterCircle = FullAngle / 4;
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
                            
                            var angle = Random.Range(sectorIndex == 1 ? 0 : (sectorIndex - 1) * quaterCircle, sectorIndex * quaterCircle);

                            var currentEnemy = Instantiate(group.Enemy, new Vector2(
                                    _radiusOfSpawnArea * Mathf.Deg2Rad * Mathf.Cos(angle),
                                    _radiusOfSpawnArea * Mathf.Deg2Rad * Mathf.Sin(angle)),
                                group.Enemy.transform.rotation);

                            currentEnemy.SpawnEnemy = this;
                            
                            listOfSectorIndexes.Remove(sectorIndex);
                            if (listOfSectorIndexes.Count == 0)
                                listOfSectorIndexes = new List<int> {1, 2, 3, 4};
                        }

                        enemyListWithValidEnemyGroup.Remove(group);
                    }
                }

                time += Time.deltaTime;
                yield return null;
            }
            
            yield return new WaitUntil(() => _currentNumberEnemyOnStage == 0);
        }
        
        yield return new WaitUntil(() => _currentNumberEnemyOnWave == 0);

        if (_currentWave + 1 != _waveList.Count)
        {
            yield return new WaitForSeconds(_timeBeforeStartNextWave);
            _currentWave++;
            StartCoroutine(StartNewWave());
        }
    }

    public void ReduceNumberEnemies()
    {
        _currentNumberEnemyOnStage--;
        _currentNumberEnemyOnWave--;
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