using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Units.Behaviours
{
    public class DirectMover : Mover
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _speed;
        private AnimatorScheduler _animator;

        public override void Init(AnimatorScheduler animator)
        {
            _animator = animator;
            _agent.speed = _speed;
        }

        public override void MoveTo(Vector3 target)
        {
            _animator.Move(_speed);
            _agent.isStopped = false;
            _agent.destination = target;
        }

        public override void MoveTo(Actor target) => MoveTo(target.transform.position);
        public override void Dispose() => _agent.enabled = false;

        public override void Stop()
        {
            _animator.Move(0);
            _agent.isStopped = true;
        }
    }
}