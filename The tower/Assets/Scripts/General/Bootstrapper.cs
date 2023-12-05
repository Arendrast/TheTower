using Enemies;
using Player;
using UnityEngine;

namespace General
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private SpawnEnemy _spawnEnemy;
        [SerializeField] private TowerHealth _towerHealth;
        [SerializeField] private Tower _tower;
        private void Awake()
        {
            _towerHealth.Initialize();
            _spawnEnemy.Initialize();
            //_tower.Initialize();
        }
    }
}