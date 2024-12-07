using System;
using Databases;
using UnityEngine;

namespace Gameplay.Units.Behaviours
{
    public class SimpleHealth : Health
    {
        public override event Action<float, float> HealthChanged;
        public override float Current { get; protected set; }
        private float _maxHealth;

        private AnimatorScheduler _animator;

        public override void Init(AnimatorScheduler animator, ActorStats stats)
        {
            _animator = animator;
            Current = _maxHealth = stats.Health;
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