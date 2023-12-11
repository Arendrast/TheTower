using Enemies;
using Player;
using UnityEngine;

namespace General
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Upgrades _upgrades;
        [SerializeField] private UpgradesPanel _upgradesPanel;
        [SerializeField] private SpawnEnemy _spawnEnemy;
        [SerializeField] private TowerHealth _towerHealth;
        [SerializeField] private Tower _tower;
        private void Awake()
        {
            _upgrades.Initialize();
            _upgradesPanel.Initialize();
            _towerHealth.Initialize();
            _spawnEnemy.Initialize();
            _tower.Initialize();
        }
    }
}