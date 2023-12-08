using Enemies;
using Player;
using UnityEngine;

namespace General
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Upgrades _upgrades;
        [SerializeField] private SpawnEnemy _spawnEnemy;
        [SerializeField] private TowerHealth _towerHealth;
        [SerializeField] private Tower _tower;
        private void Awake()
        {
            _upgrades.Initialize();
            _towerHealth.Initialize();
            _spawnEnemy.Initialize();
            _tower.Initialize();
        }
    }
}