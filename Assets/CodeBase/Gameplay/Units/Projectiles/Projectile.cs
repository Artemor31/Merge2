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

        public virtual void Init(Actor target, float damage, ProjectileData data)
        {
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
                Target.ChangeHealth(Damage, Data.DamageContext);
                Instantiate(Data.HitVFX, transform.position, Quaternion.identity);
            }
        }

        public abstract void Tick();
        public void Collect() => gameObject.SetActive(false);
        public void Release() => gameObject.SetActive(true);
    }
}