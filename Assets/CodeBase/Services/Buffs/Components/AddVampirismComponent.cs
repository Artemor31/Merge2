using Databases;
using Gameplay.Units;

namespace Services.Buffs.Components
{
    public class AddVampirismComponent : BuffComponent
    {
        private const float Value = 0.1f;
        
        private void OnEnable()
        {
            Actor actor = GetComponent<Actor>();
            ActorStats stats = actor.Stats;
            stats.Vampirism += Value;
            actor.Stats = stats;
        }
    }
}