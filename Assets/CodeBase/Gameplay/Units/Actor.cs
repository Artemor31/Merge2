using System.Collections.Generic;
using Gameplay.Units.Behaviours;
using UnityEngine;
using Databases;
using Services;
using System;
using Data;

namespace Gameplay.Units
{
    public class Actor : MonoBehaviour
    {
        public event Action Died;
        public event Action<float, float> HealthChanged;
        public event Action Disposed;
        
        public bool IsDead => _health.Current <= 0;
        public ActorData Data { get; private set; }
        public ActorStats Stats { get; set; }

        [SerializeField] private Act _act;
        [SerializeField] private Mover _mover;
        [SerializeField] private Health _health;
        [SerializeField] private TargetSearch _targetSearch;
        [SerializeField] private AnimatorScheduler _animator;
        
        private UnitState _state = UnitState.Idle;
        private ICollection<Actor> _candidates;
        private IUpdateable _updateable;

        public void Initialize(GameObject view, IUpdateable updateable, ActorData data, ActorStats stats)
        {
            Data = data;
            Stats = stats;
            _state = UnitState.Idle;
            _updateable = updateable;

            _updateable.Tick += Tick;
            _health.HealthChanged += OnHealthChanged;
            
            _animator.Init(view.GetComponent<Animator>());
            _health.Init(_animator, stats);
            _mover.Init(_animator, stats);
            _act.Init(_animator, stats);
            
            view.transform.SetParent(transform);
            view.transform.position = Vector3.zero;
        }

        private void Tick()
        {
            if (IsDead || _state == UnitState.Idle) return;
            
            _act.Tick();

            if (_targetSearch.NeedNewTarget()) 
                _targetSearch.SearchTarget(_candidates);

            if (!_targetSearch.Target) return;

            if (_act.InRange(_targetSearch.Target))
            {
                _mover.Stop();
                if (_act.CanAttack(_targetSearch.Target)) 
                    _act.PerformOn(_targetSearch.Target);
            }
            else
            {
                _mover.MoveTo(_targetSearch.Target);
            }
        }

        public void Dispose()
        {
            _mover.Dispose();
            _targetSearch.Dispose();
            _updateable.Tick -= Tick;
            _health.HealthChanged -= OnHealthChanged;
            _animator.enabled = false;
            Disposed?.Invoke();
        }

        public void ChangeHealth(float value, HealthContext context) => _health.ChangeHealth(value, context);

        public void Unleash(ICollection<Actor> candidates)
        {
            _state = UnitState.Fighting;
            _candidates = candidates;
            _targetSearch.SearchTarget(_candidates);
        }

        private void OnHealthChanged(float current, float max)
        {
            HealthChanged?.Invoke(current, max);
            if (current > 0) return;
            
            Died?.Invoke();
            Dispose();
        }
    }
}