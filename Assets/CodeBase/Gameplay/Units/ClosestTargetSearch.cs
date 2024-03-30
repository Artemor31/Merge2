using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public class ClosestTargetSearch : TargetSearch
    {
        public override Actor Target { get; protected set; }
        public override Vector3 TargetPoint => Target.transform.position;
        
        public override void SearchTarget(IReadOnlyList<Actor> candidates)
        {
            Target = null;
            float currentDistance = float.MaxValue;
            foreach (Actor actor in candidates)
            {
                if (actor.IsDead) continue;
                float distance = DistanceTo(actor);
                if (distance < currentDistance)
                {
                    Target = actor;
                    currentDistance = distance;
                }
            }
        }

        public override bool NeedNewTarget() => 
            Target == null || Target.IsDead;

        private float DistanceTo(Actor target) => 
            Vector3.Distance(target.transform.position, transform.position);
    }
}