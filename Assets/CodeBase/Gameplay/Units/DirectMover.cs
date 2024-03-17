using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Gameplay.Units
{
    public class DirectMover : Mover
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _speed;

        private void Awake() =>
            _agent.speed = _speed;

        public override void MoveTo(Vector3 target) =>
            _agent.destination = target;
        
        public override void MoveTo(Actor target) =>
            _agent.destination = target.transform.position;

        public override void Reset() => 
            _agent.destination = transform.position;
    }
}