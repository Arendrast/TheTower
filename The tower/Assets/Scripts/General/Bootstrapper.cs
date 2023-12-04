using Enemies;
using Player;
using UnityEngine;

namespace General
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private SpawnEnemy _spawnEnemy;
        [SerializeField] private TowerHealth _towerHealth;
        private void Awake()
        {
            _towerHealth.Initialize();
            _spawnEnemy.Initialize();
        }
    }
}