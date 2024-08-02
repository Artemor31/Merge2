using System;
using System.Collections.Generic;
using CodeBase.Databases;
using CodeBase.Gameplay.Units.Behaviours;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public class Actor : MonoBehaviour
    {
        public event Action<Actor> OnDied;
        
        public bool IsDead => _health.Current <= 0;
        public Health Health => _health;
        public int Level { get; private set; }
        public Race Race { get; private set; }
        public Mastery Mastery { get; private set; }
        
        [SerializeField] private Health _health;
        [SerializeField] private Mover _mover;
        [SerializeField] private TargetSearch _targetSearch;
        [SerializeField] private Attacker _attacker;
        [SerializeField] private AnimatorScheduler _animator;
        
        private UnitState _state = UnitState.Idle;
        private IReadOnlyList<Actor> _candidates;
        private IUpdateable _updateable;
        
        public void Initialize(int level, GameObject view, IUpdateable updateable, Race race, Mastery mastery)
        {
            Mastery = mastery;
            Race = race;
            Level = level;
            _state = UnitState.Idle;
            _updateable = updateable;
            _updateable.Tick += Tick;

            _animator.Init(view.GetComponent<Animator>());
            _health.Init(_animator);
            _mover.Init(_animator);
            _attacker.Init(_animator);
            
            view.transform.SetParent(transform);
            view.transform.position = Vector3.zero;
        }

        private void Tick()
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

        public void Unleash(IReadOnlyList<Actor> candidates)
        {
            _state = UnitState.Fighting;
            _candidates = candidates;
            _targetSearch.SearchTarget(_candidates);
            _health.Died += OnDies;
        }

        public void TakeDamage(float damage) => _health.TakeDamage(damage);

        private void OnDies()
        {
            _health.Died -= OnDies;
            _updateable.Tick -= Tick;
            _mover.Stop();
            OnDied?.Invoke(this);
        }
    }
}