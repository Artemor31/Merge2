using Gameplay.Units.Healths;
using Infrastructure;
using UnityEngine;

namespace Gameplay.Units.Projectiles
{
    public abstract class Projectile : MonoBehaviour, IPoolable
    {
        public bool Hited;

        [SerializeField] protected float _speed;
        [SerializeField] protected float _damageArea;
        [SerializeField] protected ParticleSystem _hitVFX;

        protected Actor Target;
        private float _damage;

        public virtual void Init(Actor caster, Actor target, float damage)
        {
            transform.position = caster.transform.position;
            Hited = false;
            Target = target;
            _damage = damage;
        }

        public abstract void Tick();

        protected virtual void Hit()
        {
            if (_hitVFX)
            {
                _hitVFX.Play();
            }

            Hited = true;
            Target.ChangeHealth(_damage, HealthContext.Damage);
        }

        public void Collect() => gameObject.SetActive(false);
        public void Release() => gameObject.SetActive(true);
    }
}