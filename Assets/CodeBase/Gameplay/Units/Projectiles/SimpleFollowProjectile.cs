using UnityEngine;

namespace Gameplay.Units.Projectiles
{
    public class SimpleFollowProjectile : Projectile
    {
        public override void Tick()
        {
            Vector3 center = Target.transform.position + Vector3.up;
            transform.LookAt(center);
            transform.Translate(Vector3.forward * Time.deltaTime * _speed);

            if (Vector3.Distance(transform.position, center) < _damageArea)
            {
                Hit();
            }
        }
    }
}