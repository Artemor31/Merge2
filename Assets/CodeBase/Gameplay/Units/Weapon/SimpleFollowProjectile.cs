using UnityEngine;

namespace Gameplay.Units.Weapon
{
    public class SimpleFollowProjectile : Projectile
    {
        public override void Tick()
        {
            var center = Target.transform.position + Vector3.up;
            transform.Translate(Time.deltaTime * _speed * center);

            if (Vector3.Distance(transform.position, center) < _damageArea)
                Hit();
        }
    }
}