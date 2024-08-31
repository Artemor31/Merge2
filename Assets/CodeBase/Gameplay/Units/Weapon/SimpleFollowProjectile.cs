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
            transform.Translate(Time.deltaTime * _speed * -center);
            transform.LookAt(center);

            _cube.transform.position = Target.position;

            if (Vector3.Distance(transform.position, center) < _damageArea)
            {
                Hit();
            }
        }
    }
}