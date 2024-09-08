using UnityEngine;

namespace Gameplay.Units.Behaviours
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
            actor.ChangeHealth(_damage, HealthContext.Heal);
            ActTimer = _actCooldown;
        }

        private void SpawnVFX(Vector3 point)
        {
            var vfx = Instantiate(_healVfxPrefab, point, Quaternion.identity);
            vfx.Play();
        }
    }
}