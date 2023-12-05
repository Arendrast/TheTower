using MyCustomEditor;
using UnityEngine;
using UnityEngine.Events;

namespace General
{
    public class Health : MonoBehaviour
    {
        public float CurrentHealthPoint
        {
            get => _currentHealthPoint;
            set => _currentHealthPoint = value;
        }

        public UnityEvent OnDie;
        public UnityEvent OnTakeDamage;
       
        public bool IsDie { get; private set; }

        [SerializeField] protected float maxHealPoint = 100;
        [ReadOnlyInspector] [SerializeField] protected float _currentHealthPoint;

        [SerializeField] private SpriteRenderer _spriteRenderer;


        protected void Start()
        {
            CurrentHealthPoint = maxHealPoint;
            if (!_spriteRenderer)
                _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void TakeDamage(float damage)
        {
            if (IsDie || damage <= 0)
                return;
            
            if (CurrentHealthPoint - damage > 0)
            {
                CurrentHealthPoint = _currentHealthPoint - damage;
                OnTakeDamage?.Invoke();
            }
            else
            {
                _currentHealthPoint = 0;
                IsDie = true;
                OnDie?.Invoke();
            }
        }

        public void RestoreHealth(float recoverySize) => CurrentHealthPoint = CurrentHealthPoint + recoverySize < maxHealPoint ? CurrentHealthPoint + recoverySize : maxHealPoint;
    

        public void Destroy() => Destroy(gameObject);

        public void BecomeTransparent()
        {
            var currentColor = _spriteRenderer.color;
            _spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0);
        }
    }
}
