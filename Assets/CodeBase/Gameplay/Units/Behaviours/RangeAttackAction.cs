using System.Collections.Generic;
using CodeBase.Gameplay.Units.Weapon;
using UnityEngine;
using UnityEngine.Pool;

namespace CodeBase.Gameplay.Units.Behaviours
{
    public class RangeAttackAction : Action
    {
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private Transform _spawnPoint;

        private ObjectPool<Projectile> _pool;
        private List<Projectile> _projectiles;

        public override void Init(AnimatorScheduler animator)
        {
            base.Init(animator);
            _projectiles = new List<Projectile>();
            _pool = new ObjectPool<Projectile>(
                () => Instantiate(_projectilePrefab, _spawnPoint.position, Quaternion.identity),
                projectile => projectile.gameObject.SetActive(true),
                projectile => projectile.gameObject.SetActive(false));
        }


        public override void PerformOn(Actor actor)
        {
            Projectile projectile = _pool.Get();
            projectile.Init(actor.transform, _damage);
            projectile.OnHited += ProjectileOnOnHited;
            _projectiles.Add(projectile);
        }

        private void ProjectileOnOnHited(Projectile hited)
        {
            _projectiles.Remove(hited);
            _pool.Release(hited);
        }

        public override void Tick()
        {
            foreach (Projectile projectile in _projectiles)
            {
                projectile.Tick();
            }
        }
    }
}