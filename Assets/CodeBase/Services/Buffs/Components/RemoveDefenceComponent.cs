using Databases;
using Gameplay.Units;

namespace Services.Buffs.Components
{
    public class RemoveDefenceComponent : BuffComponent
    {
        private const float Value = 0.05f;
        
        private void OnEnable()
        {
            Actor actor = GetComponent<Actor>();
            ActorStats stats = actor.Stats;
            stats.Defence -= Value;
            actor.Stats = stats;
        }
    }
}