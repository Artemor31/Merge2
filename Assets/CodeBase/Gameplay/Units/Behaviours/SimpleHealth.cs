using UnityEngine;

namespace Gameplay.Units.Behaviours
{
    public class SimpleHealth : Health
    {
        [SerializeField] private float _maxHealth;
        private AnimatorScheduler _animator;

        public override event System.Action Died;
        public override event System.Action HealthChanged;
        public override float Current { get; protected set; }
        public override float Ratio => Current / _maxHealth;

        public override void Init(AnimatorScheduler animator)
        {
            _animator = animator;
            Current = _maxHealth;
            HealthChanged?.Invoke();
        }

        public override void TakeDamage(float damage)
        {
            Current -= damage;
            HealthChanged?.Invoke();
            if (Current <= 0)
                Die();
        }

        public override void Heal(float damage)
        {
            Current = Mathf.Min(Current += damage, _maxHealth);
            HealthChanged?.Invoke();
        }

        private void Die()
        {
            Died?.Invoke();
            _animator.Die();
        }
    }
}