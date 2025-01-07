using UnityEngine;

namespace Gameplay.Units.Projectiles
{
    public class GroundedFollowProjectile : Projectile
    {
        public override void Tick()
        {
            if (Target == null) return;

            transform.LookAt(Target.transform.position);
            transform.Translate(Vector3.forward * Time.deltaTime * Data.MoveSpeed);

            if (Vector3.Distance(transform.position, Target.transform.position) < Data.DamageArea)
            {
                Hit();
            }
        }
    }
}