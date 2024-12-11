using UnityEngine;

namespace Gameplay.Units.Healths
{
    public class AllyHealAct : Act
    {
        [SerializeField] private ParticleSystem _healVfxPrefab;
        
        public override void PerformOn(Actor actor)
        {
            if (!CanPerformOn(actor)) return;

            transform.LookAt(actor.transform);
            Animator.PerformAct();
            SpawnVFX(actor.transform.position);
            actor.ChangeHealth(Stats.Damage, HealthContext.Heal);
            ActTimer = Stats.ActCooldown;
        }

        private void SpawnVFX(Vector3 point) => 
            Instantiate(_healVfxPrefab, point, Quaternion.identity).Play();
    }
}