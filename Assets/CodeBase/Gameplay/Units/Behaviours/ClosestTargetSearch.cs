using System.Collections.Generic;

namespace Gameplay.Units.Behaviours
{
    public class ClosestTargetSearch : TargetSearch
    {
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
    }
}