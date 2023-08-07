using System;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Gameplay.Units
{
    public class Unit : MonoBehaviour, IUnit
    {
        public event Action<IUnit> Died;
        public float Health { get; private set; }
        
        [field: SerializeField] public UnitId Id { get; private set; }
        [SerializeField] private NavMeshAgent Agent;

        public void SetDestination(Transform target)
        {
            Agent.SetDestination(target.position);
        }

        public void SetIdle()
        {
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health <= 0)
                Die();
        }

        private void Die()
        {
            Died?.Invoke(this);
        }

        public void SetTarget(IUnit bestTarget)
        {
            
        }
    }
}