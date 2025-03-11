using Databases;
using Gameplay.Units.Healths;
using Infrastructure;
using UnityEngine;

namespace Gameplay.Units.Projectiles
{
    public abstract class Projectile : MonoBehaviour, IPoolable
    {
        public bool Hited { get; private set; }
        public ProjectileData Data { get; private set; }
        protected Actor Target;
        protected float Damage;
        private HealthContext _context;

        public virtual void Init(Actor target, float damage, ProjectileData data, HealthContext context)
        {
            _context = context;
            Target = target;
            Damage = damage;
            Data = data;
            Hited = false;
        }

        protected virtual void Hit()
        {
            Hited = true;
            if (!Target.IsDead)
            {
                Target.ChangeHealth(Damage, _context);
                if (Data.HitVFX)
                    Instantiate(Data.HitVFX, transform.position, Quaternion.identity);
            }
        }

        public abstract void Tick();
        public virtual void Collect()
        {
            gameObject.SetActive(false);
            Hited = false;
        }

        public virtual void Release() => gameObject.SetActive(true);
    }
}