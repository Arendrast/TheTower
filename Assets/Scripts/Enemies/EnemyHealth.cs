using Currencies;
using General;
using Player;
using UnityEngine;

namespace Enemies
{
    public class EnemyHealth : Health
    {
        [field: SerializeField] public float RewardForKill { get; private set; }
        private new void Start()
        {
            base.Start();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Ammo>(out var ammo))
            {
                TakeDamage(ammo.IsCriticalDamage() ? ammo.Damage * ammo.MultiplierCriticalDamage : ammo.Damage);   
                Destroy(other.gameObject);
            }
        }
    }
}
