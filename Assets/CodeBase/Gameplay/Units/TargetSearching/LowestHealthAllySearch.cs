using System.Collections.Generic;
using System.Linq;

namespace Gameplay.Units.TargetSearching
{
    class LowestHealthAllySearch : TargetSearch
    {
        public override void SearchTarget(ICollection<Actor> enemies, ICollection<Actor> allies)
        {
            Actor target = allies.First();
            foreach (Actor actor in allies)
            {
                if (actor.Health < target.Health)
                {
                    target = actor;
                }
            }

            Target = target;
        }
    }
}