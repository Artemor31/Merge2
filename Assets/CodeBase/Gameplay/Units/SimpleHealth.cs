using System;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public class SimpleHealth : Health
    {
        public override event Action Died;
        public override float Current { get; protected set; }
        [SerializeField] private float _maxHealth;

        public override void TakeDamage(float damage)
        {
            Current -= damage;
            if (Current <= 0)
                Die();
        }

        public override void Reset() => 
            Current = _maxHealth;

        private void Die()
        {
            Debug.LogError($"{gameObject.name} dies :(");
            Died?.Invoke();
        }
    }
}