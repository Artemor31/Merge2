using System;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public class SimpleHealth : Health
    {
        [SerializeField] private float _maxHealth;
        private AnimatorScheduler _animator;

        public override event Action Died;
        public override float Current { get; protected set; }

        public override void Init(AnimatorScheduler animator)
        {
            _animator = animator;
            Current = _maxHealth;
        }

        public override void TakeDamage(float damage)
        {
            Current -= damage;
            if (Current <= 0)
                Die();
        }

        private void Die()
        {
            Died?.Invoke();
            _animator.Die();
        }
    }
}