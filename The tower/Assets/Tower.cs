using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private LayerMask _layerMaskOfEnemy;
    private bool _isRotate;
    private List<GameObject> _enemyListInRadius = new List<GameObject>();
    private GameObject _goal;

    private void FixedUpdate()
    {
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

        return goal;
    }

    private bool IsEnemyFound()
    {
        var position = transform.position;
        var colliderArray = new Collider2D[0];
        var size = Physics2D.OverlapCircleNonAlloc(position, _radius, colliderArray, _layerMaskOfEnemy);

        if (size > 0)
        {
            foreach (var contact in colliderArray)
                _enemyListInRadius.Add(contact.gameObject);

            _enemyListInRadius = _enemyListInRadius.Distinct().ToList();
            
            return true;
        }

        return false;
    }

    private void Attack()
    {
        
    }

    private void Update()
    {
        if (_goal)
            Rotate();
    }

    private void Rotate()
    {
        var toRotation = Quaternion.LookRotation(_goal.transform.position, Vector2.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
    }
    
    
}
