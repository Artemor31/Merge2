using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public class Unit : MonoBehaviour
    {
        [field: SerializeField] public Health Health { get; set; }
        [field: SerializeField] public Mover Mover { get; set; }
        [field: SerializeField] public TargetSearch TargetSearch { get; set; }
        [field: SerializeField] public Attacker Attacker { get; set; }

        private UnitState _state = UnitState.Idle;
        private List<Unit> _candidates;

        public void SetFighting(List<Unit> candidates)
        {
            _state = UnitState.Fighting;
            _candidates = candidates;
            TargetSearch.SearchTarget(_candidates);
            Mover.MoveTo(TargetSearch.Target);
            Health.Died += OnDies;
        }

        public void SetIdle()
        {
            _state = UnitState.Idle;
            Health.Reset();
            TargetSearch.Reset();
        }

        private void Update()
        {
            if (Health.Current <= 0 || _state == UnitState.Idle) return;

            if (TargetSearch.Target == null || TargetSearch.Target.Health.Current <= 0)
            {
                TargetSearch.SearchTarget(_candidates);
            }

            if (Attacker.InRange(TargetSearch.Target.transform.position) == false)
            {
                
            }
        }

        private void OnDies()
        {
            Health.Died -= OnDies;
            Reset();
        }

        private void Reset()
        {
            Attacker.Reset();
            Mover.Reset();
            TargetSearch.Reset();
        }
    }
}