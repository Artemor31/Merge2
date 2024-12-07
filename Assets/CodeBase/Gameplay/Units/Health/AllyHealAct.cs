using UnityEngine;

namespace Gameplay.Units.Health
{
    public class AllyHealAct : Act
    {
        [SerializeField] private ParticleSystem _healVfxPrefab;
        
        public override void PerformOn(Actor actor)
        {
            if (!CanAttack(actor)) return;

            transform.LookAt(actor.transform);
            Animator.PerformAct();
            SpawnVFX(actor.transform.position);
            actor.ChangeHealth(Stats.Damage, HealthContext.Heal);
            ActTimer = Stats.ActCooldown;
        }

        private void SpawnVFX(Vector3 point)
        {
            var vfx = Instantiate(_healVfxPrefab, point, Quaternion.identity);
            vfx.Play();
        }
    }
}