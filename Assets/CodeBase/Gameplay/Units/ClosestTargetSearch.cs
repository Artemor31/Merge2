using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public class ClosestTargetSearch : TargetSearch
    {
        public override Actor Target { get; protected set; }
        
        public override void SearchTarget(List<Actor> candidates)
        {
            if (candidates.Count == 0) return;

            var targetIndex = 0;
            float targetDistance = DistanceTo(candidates.First());

            for (var index = 1; index < candidates.Count; index++)
            {
                var unit = candidates[index];
                
                if (unit.IsDead == false && DistanceTo(unit) < targetDistance)
                    targetIndex = index;
            }

            Target = candidates[targetIndex];
        }

        public override void Disable() => 
            Target = null;

        public override bool NeedNewTarget() => 
            Target == null || Target.IsDead;

        private float DistanceTo(Actor target) => 
            Vector3.Distance(target.transform.position, transform.position);
    }
}