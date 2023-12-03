using UnityEngine;

public class Ammo : MonoBehaviour
{
    public float Damage;
    public float CriticalDamageChance;
    [field: SerializeField] public Rigidbody2D RigidBody { get; private set; }
    
}
