using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Ammo>(out var ammo))
            TakeDamage(ammo.Damage);
    }
}
