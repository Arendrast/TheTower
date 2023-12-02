using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float Damage;
    public float AttackSpeed;
    public float Radius;
    public float CriticalChance;
    public float CriticalFactor;
    public float DamageOnMeter;

    [SerializeField] private LayerMask _layerMaskOfEnemy;
    [SerializeField] private Rigidbody2D _ammoRb;
    [SerializeField] private bool _isDrawAffectedArea = true;

    private bool _isRotate;
    private List<GameObject> _enemyListInRadius = new List<GameObject>();
    private GameObject _goal;
    private GameObject _currentAmmo;
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
        for (var i = 0; i < _enemyListInRadius.Count; i++)
        {
            if (_enemyListInRadius[i] != gameObject)
            {
                var currentEnemy = _enemyListInRadius[i];
                
                var position = transform.position;

                if (Vector2.Distance(position, currentEnemy.transform.position) <=
                    Vector2.Distance(position, goal.transform.position))
                {
                    goal = currentEnemy;
                }
            }
        }
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
        var ammo = Instantiate(_ammoRb, position, _ammoRb.transform.rotation);
        ammo.AddForce((_goal.transform.position - position) * AttackSpeed);
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
