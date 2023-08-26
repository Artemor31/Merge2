using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Gameplay.Units
{
    public abstract class Damageable : MonoBehaviour
    {
        public float Health => _health;
        
        [SerializeField] private NavMeshAgent Agent;
        [SerializeField] private float _health;

        public void TakeDamage(float damage)
        {
            _health -= damage;
            if (Health <= 0)
                Die();
        }

        private void Die()
        {
            Agent.enabled = false;
        }
    }
}