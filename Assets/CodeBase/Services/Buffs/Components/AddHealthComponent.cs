using Databases;
using Gameplay.Units;
using Gameplay.Units.Healths;

namespace Services.Buffs.Components
{
    public class AddHealthComponent : BuffComponent
    {
        private const float Value = 0.05f;
        
        private void OnEnable()
        {
            Actor actor = GetComponent<Actor>();
            ActorStats stats = actor.Stats;
            float bonus = stats.Health * Value;
            stats.Health += bonus;
            actor.Stats = stats;
            actor.ChangeHealth(bonus, HealthContext.Heal);
        }
    }
}