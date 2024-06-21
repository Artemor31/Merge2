using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public class Actor : MonoBehaviour
    {
        public event Action<Actor> OnDied;
        
        [field: SerializeField] public int Level { get; private set; }
        [SerializeField] private Health _health;
        [SerializeField] private Mover _mover;
        [SerializeField] private TargetSearch _targetSearch;
        [SerializeField] private Attacker _attacker;
        [SerializeField] private AnimatorScheduler _animator;

        public bool IsDead => _health.Current <= 0;
        private UnitState _state = UnitState.Idle;
        private IReadOnlyList<Actor> _candidates;

        public void Initialize(int level)
        {
            Level = level;
            _state = UnitState.Idle;
            _health.Init(_animator);
            _mover.Init(_animator);
            _attacker.Init(_animator);
        }

        public void Unleash(IReadOnlyList<Actor> candidates)
        {
            _state = UnitState.Fighting;
            _candidates = candidates;
            _targetSearch.SearchTarget(_candidates);
            _health.Died += OnDies;
        }

        public void TakeDamage(float damage) => _health.TakeDamage(damage);

        private void Update()
        {
            if (IsDead || _state == UnitState.Idle) return;

            if (_targetSearch.NeedNewTarget())
            {
                _targetSearch.SearchTarget(_candidates);
            }
            
            if (_targetSearch.Target == null) return;
            
            _attacker.Tick();
            if (_attacker.InRange(_targetSearch.Target))
            {
                _mover.Stop();
                if (_attacker.CanAttack(_targetSearch.Target))
                {
                    _attacker.Attack(_targetSearch.Target);
                }
            }
            else
            {
                _mover.MoveTo(_targetSearch.Target);
            }
        }

        private void OnDies()
        {
            _health.Died -= OnDies;
            _mover.Stop();
            OnDied?.Invoke(this);
        }
    }
}