using System.Collections.Generic;
using Gameplay.Units.Behaviours;
using UnityEngine;
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

        [SerializeField] private Health _health;
        [SerializeField] private Mover _mover;
        [SerializeField] private TargetSearch _targetSearch;
        [SerializeField] private Act _act;
        [SerializeField] private AnimatorScheduler _animator;
        
        private UnitState _state = UnitState.Idle;
        private IReadOnlyList<Actor> _candidates;
        private IUpdateable _updateable;

        public void Initialize(GameObject view, IUpdateable updateable, ActorData data)
        {
            Data = data;
            _state = UnitState.Idle;
            _updateable = updateable;

            _updateable.Tick += Tick;
            _health.HealthChanged += OnHealthChanged;
            
            _animator.Init(view.GetComponent<Animator>());
            _health.Init(_animator);
            _mover.Init(_animator);
            _act.Init(_animator);
            
            view.transform.SetParent(transform);
            view.transform.position = Vector3.zero;
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

        private void Tick()
        {
            if (IsDead || _state == UnitState.Idle) return;
            
            _act.Tick();

            TrySearchTarget();
            
            if (_targetSearch.Target == null) return;

            if (_act.InRange(_targetSearch.Target))
            {
                _mover.Stop();
                TryPerform();
            }
            else
            {
                _mover.MoveTo(_targetSearch.Target);
            }
        }

        public void ChangeHealth(float value, HealthContext context) => 
            _health.ChangeHealth(value, context);

        public void Unleash(IReadOnlyList<Actor> candidates)
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

        private void TryPerform()
        {
            if (_act.CanAttack(_targetSearch.Target)) 
                _act.PerformOn(_targetSearch.Target);
        }

        private void TrySearchTarget()
        {
            if (_targetSearch.NeedNewTarget()) 
                _targetSearch.SearchTarget(_candidates);
        }
    }
}