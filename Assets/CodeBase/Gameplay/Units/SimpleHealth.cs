using System;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Gameplay.Units
{
    public class SimpleHealth : Health
    {
        public override event Action Died;
        public override float Current { get; protected set; }

        [SerializeField] private NavMeshAgent Agent;
        [SerializeField] private float _maxHealth;

        private void Awake()
        {
            Current = _maxHealth;
        }

        public override void TakeDamage(float damage)
        {
            Current -= damage;
            if (Current <= 0)
                Die();
        }

        public override void Die()
        {
            Agent.enabled = false;
            Died?.Invoke();
        }
    }
}