using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public SpawnEnemy SpawnEnemy;
    [SerializeField] private Vector3 _endPointOfMovement;
    [SerializeField] private float _velocityMovement;
    [SerializeField] private Rigidbody2D _rb;
    
    private void FixedUpdate()
    {
        _rb.velocity = (_endPointOfMovement - transform.position).normalized * _velocityMovement;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Ammo>(out var ammo))
        {
            Destroy(gameObject);
            SpawnEnemy.ReduceNumberEnemies();
            Destroy(other.gameObject);   
        }
    }
}
