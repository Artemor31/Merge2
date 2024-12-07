using UnityEngine;

namespace Gameplay.Units.Weapon
{
    public class SimpleFollowProjectile : Projectile
    {
        private GameObject _cube;

        private void OnEnable()
        {
            _cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }

        public override void Tick()
        {
            Vector3 center = Target.transform.position + Vector3.up;
            Vector3 direction = center - transform.position + Vector3.up;

            if (direction.magnitude < _damageArea)
            {
                Hit();
                return;
            }

            transform.position += direction.normalized * Time.deltaTime * _speed;
            transform.LookAt(center);
        }
    }
}