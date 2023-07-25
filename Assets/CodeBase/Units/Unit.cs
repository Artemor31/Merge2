using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Units
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent Agent;

        public void SetDestination(Transform target)
        {
            Agent.SetDestination(target.position);
        }

    }
}