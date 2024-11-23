using Gameplay.Units;

namespace Services.BuffService.Components
{
    public class AddRangeComponent : BuffComponent
    {
        private void OnEnable()
        {
            var actor = GetComponent<Actor>();
            var stats = actor.Stats;
            
            if (stats.Range < 3) return;
            
            stats.Range += 1;
            actor.Stats = stats;
        }
    }
}