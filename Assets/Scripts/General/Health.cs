using MyCustomEditor;
using UnityEngine;
using UnityEngine.Events;

namespace General
{
    public abstract class Health : MonoBehaviour
    {
        public float CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = value;
        }

        public UnityEvent OnDie;
        public UnityEvent OnTakeDamage;
       
        public bool IsDie { get; private set; }

        [SerializeField] protected float maxHealth = 100;
        [ReadOnlyInspector] [SerializeField] protected float _currentHealth;

        [SerializeField] private SpriteRenderer _spriteRenderer;


        protected void Start()
        {
            CurrentHealth = maxHealth;
            if (!_spriteRenderer)
                _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public virtual bool TakeDamage(float damage)
        {
            if (IsDie || damage <= 0)
                return false;
            
            if (CurrentHealth - damage > 0)
            {
                CurrentHealth = _currentHealth - damage;
                OnTakeDamage?.Invoke();
                return true;
            }
            else
            {
                _currentHealth = 0;
                IsDie = true;
                OnDie?.Invoke();
            }

            return false;
        }

        public void RestoreHealth(float recoverySize) => CurrentHealth = CurrentHealth + recoverySize < maxHealth ? CurrentHealth + recoverySize : maxHealth;
    

        public void Destroy() => Destroy(gameObject);

        public void BecomeTransparent()
        {
            var currentColor = _spriteRenderer.color;
            _spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0);
        }
    }
}
