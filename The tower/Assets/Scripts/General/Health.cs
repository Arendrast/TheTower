using UnityEngine;
using UnityEngine.Events;

namespace General
{
    public class Health : MonoBehaviour
    {
        public bool IsDie { get; private set; }
        public float MaxHealPoint  = 100; 
        public float CurrentHealPoint  = 100;
        public UnityEvent OnDie;
        [field: SerializeField] public float CurrentHealth { get; protected set; }


        protected void Start()
        {
            CurrentHealPoint = MaxHealPoint;
        }

        public void TakeDamage(float damage)
        {
            if (IsDie)
                return;

            if (CurrentHealth - damage > 0)
            {
                CurrentHealth -= damage;
            }
            else
            {
                OnDie?.Invoke();
            }
        }

        public void RestoreHealth(float recoverySize) => CurrentHealth = CurrentHealth + recoverySize < MaxHealPoint ? CurrentHealth + recoverySize : MaxHealPoint;
    

        public void Destroy() => Destroy(gameObject);
    }
}
