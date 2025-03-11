using UnityEngine;

namespace Gameplay.Units.Projectiles
{
    public class SimpleFollowProjectile : Projectile
    {
        [SerializeField] private TrailRenderer _trailRenderer;

        public override void Release()
        {
            if (_trailRenderer)
            {
                _trailRenderer.Clear();
            }
            
            base.Release();
        }

        public override void Tick()
        {
            if (Target == null) return;

            Vector3 center = Target.transform.position + Vector3.up;
            transform.LookAt(center);
            transform.Translate(Vector3.forward * Time.deltaTime * Data.MoveSpeed);

            if (Vector3.Distance(transform.position, center) < Data.DamageArea)
            {
                Hit();
            }
        }
    }
}