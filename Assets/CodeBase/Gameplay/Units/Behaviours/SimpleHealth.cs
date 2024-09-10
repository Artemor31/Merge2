using System;
using UnityEngine;

namespace Gameplay.Units.Behaviours
{
    public class SimpleHealth : Health
    {
        [SerializeField] private float _maxHealth;
        public override event Action<float, float> HealthChanged;
        public override float Current { get; protected set; }
        
        private AnimatorScheduler _animator;

        public override void Init(AnimatorScheduler animator)
        {
            _animator = animator;
            Current = _maxHealth;
        }

        public override void ChangeHealth(float value, HealthContext contexts)
        {
            switch (contexts)
            {
                case HealthContext.None: break;
                case HealthContext.Damage:
                    Current -= value;
                    if (Current <= 0)
                        Die();
                    break;
                case HealthContext.Heal:
                    Current = Mathf.Min(_maxHealth, Current + value);
                    break;
                case HealthContext.PureDamage:
                    break;
            }
            
            HealthChanged?.Invoke(Current, _maxHealth);
        }

        private void Die() => _animator.Die();
    }
}