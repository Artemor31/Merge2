using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public class Actor : MonoBehaviour
    {
        [field: SerializeField] public int Level { get; private set; }

        [SerializeField] private Health _health;
        [SerializeField] private Mover _mover;
        [SerializeField] private TargetSearch _targetSearch;
        [SerializeField] private Attacker _attacker;
        [SerializeField] private AnimatorScheduler _animator;

        public bool IsDead => _health.Current <= 0;
        private UnitState _state = UnitState.Idle;
        private List<Actor> _candidates;

        public Actor Initialize()
        {
            _state = UnitState.Idle;
            _health.Reset();
            _targetSearch.Disable();
            return this;
        }

        public void Unleash(List<Actor> candidates)
        {
            _state = UnitState.Fighting;
            _candidates = candidates;
            _targetSearch.SearchTarget(_candidates);
            _mover.MoveTo(_targetSearch.Target);
            _health.Died += OnDies;
        }

        public void TakeDamage(float damage) => _health.TakeDamage(damage);

        private void Update()
        {
            if (IsDead || _state == UnitState.Idle) return;

            _attacker.Tick();

            if (_targetSearch.NeedNewTarget())
            {
                _targetSearch.SearchTarget(_candidates);
            }

            if (_attacker.InRange(_targetSearch.Target.transform.position) == false)
            {
            }
        }

        private void OnDies()
        {
            _health.Died -= OnDies;
            _attacker.Disable();
            _mover.Reset();
            _targetSearch.Disable();
        }
    }
}