using UnityEngine;
using UnityEngine.AI;

namespace CodeBase
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent Agent;

        public void SetDestination(Transform target)
        {
            Agent.SetDestination(target.position);
        }
    }
}