using System.Collections;
using System.Collections.Generic;
using Gameplay.Units.Fighting;
using Infrastructure;
using UnityEngine;

namespace Gameplay.Units
{
    public class RangerActor : Actor
    {
        [SerializeField] private Projectile _projectilePrefab;
        private Vector3 Center => transform.position + Vector3.up;
        private Pool<Projectile> _pool;
        private List<Projectile> _projectiles;
        private Actor _target;

        private void OnEnable()
        {
            
            _projectiles = new List<Projectile>();
            _pool = new Pool<Projectile>(5, 3, _projectilePrefab);
        }

        protected override void Tick()
        {
            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                _projectiles[i].Tick();
            }
            
            if (IsDead) return;

            TickActTimer();

            if (!CanFindTarget()) return;

            transform.LookAt(Target.transform);

            if (InRange())
            {
                _mover.Stop();

                if (CooldownUp)
                {
                    PerformAct();
                }
            }
            else
            {
                if (CooldownUp)
                {
                    _mover.MoveTo(Target);
                }
            }
        }

        private void PerformAct()
        {
            
        }

        protected override bool NeedNewTarget() => Target == null || Target.IsDead;

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