using System.Collections.Generic;
using System.Linq;
using MyCustomEditor;
using UnityEngine;

namespace Player
{
    public class Tower : MonoBehaviour
    {
        public float Damage;
        public float AttackSpeed;
        public float Radius;
        public float CriticalDamageChance;
        public float CriticalFactor;
        public float DamageOnMeter;
        [SerializeField] private float _multiplierDamageOnMeter;
        [SerializeField] private float _multiplierCriticalDamage;
        [SerializeField] private float _distanceAtWhichMultiplierIsActivatedOnCriticalHits = 1;
        [SerializeField] private LayerMask _layerMaskOfEnemy;
        [SerializeField] private Ammo _ammo;
        [SerializeField] private bool _isDrawAffectedArea = true;

        private bool _isRotate;
        private List<GameObject> _enemyListInRadius = new List<GameObject>();
        private GameObject _goal;
        private GameObject _currentAmmo;
        private bool _isActiveMultiplierOnCriticalHits;
        private bool _isActiveMultiplierDamageOnMeter;
        private void FixedUpdate()
        {
            if (_currentAmmo)
                return;
        
            if (_goal)
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
            _isActiveMultiplierDamageOnMeter = distance <= DamageOnMeter;
            
            _enemyListInRadius.Clear();
            
            return goal;
        }

        private bool IsEnemyFound()
        {
            var position = transform.position;
            var colliderArray = Physics2D.OverlapCircleAll(position, Radius, _layerMaskOfEnemy);
        
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
            ammo.Damage = _isActiveMultiplierOnCriticalHits ? Damage * CriticalFactor : Damage;
            ammo.CriticalDamageChance = CriticalDamageChance;
            ammo.RigidBody.AddForce((_goal.transform.position - position) * AttackSpeed);
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
                Gizmos.DrawWireSphere(transform.position, Radius);
            }
        }
    
    
    }
}
