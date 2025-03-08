using Databases;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Units
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        private ActorSkin _animator;
        private float _speed;

        public void Init(ActorSkin animator, ActorStats stats)
        {
            _animator = animator;
            _agent.speed = _speed = stats.MoveSpeed;
            Stop();
        }

        public void MoveTo(Actor target) => MoveTo(target.transform.position);
        public void Dispose() => _agent.enabled = false;

        public void Stop()
        {
            _animator.Move(0);
            if (_agent.isOnNavMesh)
            {
                _agent.isStopped = true;
            }
        }

        public void MoveTo(Vector3 target)
        {
            if (!_agent.isOnNavMesh) return;
            
            _animator.Move(_speed);
            _agent.isStopped = false;
            _agent.destination = target;
        }
    }
}