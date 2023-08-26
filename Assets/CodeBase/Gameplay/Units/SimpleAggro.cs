using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure;
using CodeBase.Models;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Gameplay.Units
{
    public interface ITargetSearch
    {
        Damageable Current { get; }
        void Construct(List<Damageable> candidates);
        void SearchTarget();
    }

    public class DirectTargetSearch : MonoBehaviour, ITargetSearch
    {
        public Damageable Current { get; private set; }
        private List<Damageable> _candidates;

        public void Construct(List<Damageable> candidates) => 
            _candidates = candidates;

        private void Update()
        {
            if (Current == null || Current.Health <= 0)
                SearchTarget();
        }
        
        public void SearchTarget()
        {
            if (_candidates == null || _candidates.Count == 0)
                throw new Exception();

            int index = 0;
            float targetDistance = Vector3.Distance(_candidates[0].transform.position, transform.position);

            for (var i = 1; i < _candidates.Count; i++)
            {
                float distance = Vector3.Distance(_candidates[i].transform.position, transform.position);
                if (distance < targetDistance) 
                    index = i;
            }

            Current = _candidates[index];
        }
    }

    public class Mover : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _speed;

        private void Awake() =>
            _agent.speed = _speed;

        public void MoveTo(Vector3 target) =>
            _agent.destination = target;

        public void Stop() =>
            _agent.destination = transform.position;
    }
}