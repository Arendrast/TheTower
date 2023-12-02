using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector3 _endPointOfMovement;
    [SerializeField] private float _velocityMovement;
    [SerializeField] private Rigidbody2D _rb;
    
    private void FixedUpdate()
    {
        _rb.velocity = (_endPointOfMovement - transform.position).normalized * _velocityMovement;
    }
}
