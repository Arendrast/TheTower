using System;
using System.Collections.Generic;
using System.Linq;
using General;
using UnityEngine;

namespace Player
{
    public class Tower : MonoBehaviour, IObjectBeindInitialized
    {
        [Header("Parameters")]
        [Range(0, 1000)] [SerializeField] private float _damage;
        [Range(0, 1000)] [SerializeField] private float _attackSpeed;
        [Range(0, 1000)] [SerializeField] private float _radius;
        [Range(0, 100)] [SerializeField] private float _criticalDamageChance;
        [Range(0, 100)] [SerializeField] private float _criticalFactor;
        [Range(0, 100)] [SerializeField] private float _damageOnMeter;
        [Range(0, 1000)] [SerializeField] private float _multiplierDamageOnMeter;
        [Range(0, 100)] [SerializeField] private float _multiplierCriticalDamage;
        [Range(0, 100)] [SerializeField] private float _distanceAtWhichMultiplierIsActivatedOnCriticalHits = 1;
        [Space] [Header("Other")]
        [SerializeField] private LayerMask _layerMaskOfEnemy;
        [SerializeField] private Ammo _ammo;
        [SerializeField] private TowerHealth _health;
        [SerializeField] private Upgrades _upgrades;
        [SerializeField] private bool _isDrawAffectedArea = true;

        private bool _isRotate;
        private List<GameObject> _enemyListInRadius = new List<GameObject>();
        private GameObject _goal;
        private GameObject _currentAmmo;
        private bool _isActiveMultiplierOnCriticalHits;
        private bool _isActiveMultiplierDamageOnMeter;
        private float _currentSpeedAttack;

        public void Initialize()
        {
            _damage = PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.NamesOfUpgrades.Damage.ToString());
            _attackSpeed = PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.NamesOfUpgrades.AttackSpeed.ToString());
            _radius = PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.NamesOfUpgrades.Radius.ToString());
            _criticalDamageChance = PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.NamesOfUpgrades.CriticalDamageChance.ToString());
            _criticalFactor = PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.NamesOfUpgrades.CriticalFactor.ToString());
            _damageOnMeter = PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.NamesOfUpgrades.DamageOnMeter.ToString());
            _currentSpeedAttack = _attackSpeed * PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.GameSpeed);
            SetSubscribeToMethods(true);
        }

        private void OnDisable() => SetSubscribeToMethods(false);
        

        private void SetSubscribeToMethods(bool isSubscribe)
        {
            var upgradeDamage = NamesVariablesPlayerPrefs.NamesOfUpgrades.Damage;
            var upgradeAttackSpeed = NamesVariablesPlayerPrefs.NamesOfUpgrades.AttackSpeed;
            var upgradeRadius = NamesVariablesPlayerPrefs.NamesOfUpgrades.Radius;
            var upgradeCriticalDamageChance = NamesVariablesPlayerPrefs.NamesOfUpgrades.CriticalDamageChance;
            var upgradeCriticalFactor = NamesVariablesPlayerPrefs.NamesOfUpgrades.CriticalFactor;
            var upgradeDamageOnMeter = NamesVariablesPlayerPrefs.NamesOfUpgrades.DamageOnMeter;

            if (isSubscribe)
            {
                _upgrades.DictOfUpgrades[upgradeDamage].OnChangeValue += value => _damage = value;
                _upgrades.DictOfUpgrades[upgradeAttackSpeed].OnChangeValue += value => _attackSpeed = value;
                _upgrades.DictOfUpgrades[upgradeRadius].OnChangeValue += value => _radius = value;
                _upgrades.DictOfUpgrades[upgradeCriticalDamageChance].OnChangeValue += value => _criticalDamageChance = value;
                _upgrades.DictOfUpgrades[upgradeCriticalFactor].OnChangeValue += value => _criticalFactor = value;
                _upgrades.DictOfUpgrades[upgradeDamageOnMeter].OnChangeValue += value => _damageOnMeter = value;
            }
            else
            {
                _upgrades.DictOfUpgrades[upgradeDamage].OnChangeValue -= value => _damage = value;
                _upgrades.DictOfUpgrades[upgradeAttackSpeed].OnChangeValue -= value => _attackSpeed = value;
                _upgrades.DictOfUpgrades[upgradeRadius].OnChangeValue -= value => _radius = value;
                _upgrades.DictOfUpgrades[upgradeCriticalDamageChance].OnChangeValue -= value => _criticalDamageChance = value;
                _upgrades.DictOfUpgrades[upgradeCriticalFactor].OnChangeValue -= value => _criticalFactor = value;
                _upgrades.DictOfUpgrades[upgradeDamageOnMeter].OnChangeValue -= value => _damageOnMeter = value;
            }
        }


        private void FixedUpdate()
        {
            if (_health.IsDie)
                return;
            
            else if (_currentAmmo)
            {
                if (1 << _goal.layer != _layerMaskOfEnemy || Math.Abs(_currentSpeedAttack - _attackSpeed * PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.GameSpeed)) > Constants.Epsilon)
                    Destroy(_currentAmmo);
                else
                    return;
            }

            if (_goal && 1 << _goal.layer == _layerMaskOfEnemy)
                Attack();
        
            else if (IsEnemyFound())
            {
                _goal = GetNearestGoal();
                Attack();
            }
        }

        private GameObject GetNearestGoal()
        {
            var goal = _enemyListInRadius[0];
            var position = transform.position;
            for (var i = 0; i < _enemyListInRadius.Count; i++)
            {
                if (_enemyListInRadius[i] != gameObject)
                {
                    var currentEnemy = _enemyListInRadius[i];
                
                
                    if (Vector2.Distance(position, currentEnemy.transform.position) <=
                        Vector2.Distance(position, goal.transform.position))
                    {
                        goal = currentEnemy;
                    }
                }
            }

            var distance = Vector2.Distance(position, goal.transform.position);
            
            _isActiveMultiplierOnCriticalHits = distance <= _distanceAtWhichMultiplierIsActivatedOnCriticalHits;
            _isActiveMultiplierDamageOnMeter = distance <= _damageOnMeter;
            
            _enemyListInRadius.Clear();
            
            return goal;
        }

        private bool IsEnemyFound()
        {
            var position = transform.position;
            var colliderArray = Physics2D.OverlapCircleAll(position, _radius, _layerMaskOfEnemy);
        
            if (colliderArray.Length > 0)
            {
                foreach (var contact in colliderArray)
                {
                    _enemyListInRadius.Add(contact.gameObject);
                }

                _enemyListInRadius = _enemyListInRadius.Distinct().ToList();
            
                return true;
            }

            return false;
        }

        private void Attack()
        {
            var position = transform.position;
            var ammo = Instantiate(_ammo, position, _ammo.transform.rotation);
            ammo.Damage = _isActiveMultiplierOnCriticalHits ? _damage * _criticalFactor : _damage;
            ammo.CriticalDamageChance = _criticalDamageChance;
            var attackSpeed = _attackSpeed * PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.GameSpeed);
            _currentSpeedAttack = attackSpeed;
            ammo.RigidBody.AddForce((_goal.transform.position - position) * attackSpeed);
            ammo.MultiplierCriticalDamage = _multiplierCriticalDamage;
            _currentAmmo = ammo.gameObject;
        }
    
        /*private void Rotate()
    {
        var toRotation = Quaternion.LookRotation(_goal.transform.position, Vector2.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
    }*/
    
        private void OnDrawGizmos()
        {
            if (_isDrawAffectedArea)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, _radius);
            }
        }

        public void DestroyAmmo()
        {
          if (_currentAmmo) 
              Destroy(_currentAmmo);
        } 
    }
}
