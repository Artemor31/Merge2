using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Units.TargetSearching
{
    public class LineTargetSearch : TargetSearch
    {
        public override void SearchTarget(ICollection<Actor> enemies, ICollection<Actor> allies)
        {
            Target = null;
            
            int tries = 1;
            float positionX = transform.position.x;
            var actualTargets = new List<Actor>();
            while (actualTargets.Count < 1 && tries < 9)
            {
                foreach (var candidate in enemies)
                {
                    if (!candidate.IsDead)
                    {
                        var abs = Mathf.Abs(candidate.transform.position.x - positionX);
                        if (abs < tries)
                        {
                            actualTargets.Add(candidate);
                        }
                    }
                }
                
                if (actualTargets.Count > 0) break;
                tries *= 2;
            }
            
            if (tries >= 9) return;
            
            float currentDistance = float.MaxValue;
            foreach (Actor actor in actualTargets)
            {
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