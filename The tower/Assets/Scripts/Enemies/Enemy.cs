using System;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Vector3 _endPointOfMovement;
        [SerializeField] private float _velocityMovement;
        [SerializeField] private Rigidbody2D _rb;

        private void Awake() => _rb = GetComponent<Rigidbody2D>();

        private void FixedUpdate()
        {
            _rb.velocity = (_endPointOfMovement - transform.position).normalized * _velocityMovement;
        }
    }
}
