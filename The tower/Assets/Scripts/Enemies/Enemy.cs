using System;
using Player;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Vector3 _endPointOfMovement;
        [SerializeField] private EnemyHealth _enemyHealth;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _velocityMovement;
        [SerializeField] private float _damage = 10;
        private bool _isMove = true;

        private void Start()
        {
            if (!_rb)
                _rb = GetComponent<Rigidbody2D>();
            if (!_enemyHealth)
                _enemyHealth = GetComponent<EnemyHealth>();
        }

        private void FixedUpdate()
        {
            if (_enemyHealth.IsDie || !_isMove)
            {
                if (_rb.velocity.magnitude > 0)
                    _rb.velocity = Vector2.zero;
                return;
            }
            
            _rb.velocity = (_endPointOfMovement - transform.position).normalized * _velocityMovement;
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log(other.gameObject.name);
            if (other.gameObject.TryGetComponent<TowerHealth>(out var health))
            {
                health.TakeDamage(_damage);
                _isMove = false;
            }
        }
    }
}
