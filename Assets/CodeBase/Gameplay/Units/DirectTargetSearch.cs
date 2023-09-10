using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public class DirectTargetSearch : TargetSearch
    {
        public override Unit Target { get; protected set; }
        private List<Unit> _candidates;

        public override void SetTargets(List<Unit> candidates)
        {
            _candidates = candidates;
            SearchTarget();
        }

        public override void Disable()
        {
            _candidates.Clear();
            Target = null;
            Debug.LogError($"Killing the {gameObject.name}");
        }

        private void SearchTarget()
        {
            if (_candidates.Count == 0) return;

            var targetIndex = 0;
            float targetDistance = DistanceTo(_candidates.First());

            for (var index = 1; index < _candidates.Count; index++)
            {
                var unit = _candidates[index];
                
                if (IsDead(unit) == false && DistanceTo(unit) < targetDistance)
                    targetIndex = index;
            }

            Target = _candidates[targetIndex];
            Target.Health.Died += OnTargetDies;
        }

        private void OnTargetDies()
        {
            Debug.LogError($"target dies for {gameObject.name}");
            Target.Health.Died -= OnTargetDies;
            Target = null;
            SearchTarget();
        }

        private bool IsDead(Unit unit) => 
            unit.Health.Current <= 0;

        private float DistanceTo(Unit target) => 
            Vector3.Distance(target.transform.position, transform.position);
    }
}