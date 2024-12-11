using System.Collections.Generic;

namespace Gameplay.Units.TargetSearching
{
    public class ClosestEnemyTargetSearch : TargetSearch
    {
        public override void SearchTarget(ICollection<Actor> enemies, ICollection<Actor> allies)
        {
            Target = null;
            float currentDistance = float.MaxValue;
            foreach (Actor actor in enemies)
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