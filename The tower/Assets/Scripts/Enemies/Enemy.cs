using General;
using Player;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(EnemyHealth))]
    public class Enemy : MonoBehaviour
    {
        [HideInInspector] public Vector3 EndPointOfMovement { get; set; }
        [field: SerializeField] public EnemyHealth EnemyHealth { get; private set; }
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _velocityMovement;
        [SerializeField] private float _damage = 10;
        private bool _isMove = true;

        private void Start()
        {
            if (!_rb)
                _rb = GetComponent<Rigidbody2D>();
            if (!EnemyHealth)
                EnemyHealth = GetComponent<EnemyHealth>();
        }

        private void FixedUpdate()
        {
            if (EnemyHealth.IsDie || !_isMove)
            {
                if (_rb.velocity.magnitude > 0)
                    _rb.velocity = Vector2.zero;
                return;
            }
            
            _rb.velocity = (EndPointOfMovement - transform.position).normalized * (_velocityMovement * PlayerPrefs.GetFloat(NamesVariablesPlayerPrefs.GameSpeed));
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent<TowerHealth>(out var health))
            {
                health.TakeDamage(_damage);
                _isMove = false;
            }
        }
    }
}
