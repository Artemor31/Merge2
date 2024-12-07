using System.Collections;
using System.Collections.Generic;
using Databases;
using Gameplay.Units.Weapon;
using Infrastructure;
using UnityEngine;

namespace Gameplay.Units.Behaviours
{
    public class RangeAttackAct : Act
    {
        [SerializeField] private Projectile _projectilePrefab;

        private Vector3 Center => transform.position + Vector3.up;
        private Pool<Projectile> _pool;
        private List<Projectile> _projectiles;

        public override void Init(AnimatorScheduler animator, ActorStats stats)
        {
            base.Init(animator, stats);
            _projectiles = new List<Projectile>();
            _pool = new Pool<Projectile>(5, 3, _projectilePrefab);
        }

        public override void PerformOn(Actor actor)
        {
            Animator.PerformAct();
            ResetCooldown();
            transform.LookAt(actor.transform);
            StartCoroutine(DoDamage(actor.transform));
        }

        public override void Tick()
        {
            base.Tick();
            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                Projectile projectile = _projectiles[i];
                projectile.Tick();
            }
        }

        private IEnumerator DoDamage(Transform target)
        {
            yield return new WaitForSeconds(2.5f);
            Projectile projectile = _pool.Get();
            projectile.transform.position = Center;
            projectile.Init(target, Stats.Damage, ProjectileOnHited);
            _projectiles.Add(projectile);
        }

        private void ProjectileOnHited(Projectile hited)
        {
            _projectiles.Remove(hited);
            _pool.Collect(hited);
        }
    }
}