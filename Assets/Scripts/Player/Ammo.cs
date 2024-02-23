using MyCustomEditor;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ammo : MonoBehaviour
    { 
        [ReadOnlyInspector] public float Damage;
        [ReadOnlyInspector] public float CriticalDamageChance;
        [ReadOnlyInspector] public float MultiplierCriticalDamage;
        [field: SerializeField] public Rigidbody2D RigidBody { get; private set; }
        
        public bool IsCriticalDamage()
        {
            return CriticalDamageChance >= Random.Range(0, 101);
        }
    }
}
