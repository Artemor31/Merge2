using Databases;
using Gameplay.Units;

namespace Services.Buffs.Components
{
    public class AddDamageComponent : BuffComponent
    {
        private const float Value = 0.05f;
        
        private void OnEnable()
        {
            Actor actor = GetComponent<Actor>();
            ActorStats stats = actor.Stats;
            stats.Damage *= Value;
            actor.Stats = stats;
        }
    }
}