using System;
using Gameplay.Units.Behaviours;
using UnityEngine;

namespace Gameplay.Units.Weapon
{
    public abstract class Projectile : MonoBehaviour
    {
        public event Action<Projectile> OnHited; 
        
        [SerializeField] protected float _speed;
        [SerializeField] protected float _damageArea;
        [SerializeField] protected ParticleSystem _hitVFX;

        protected Transform Target;
        protected float Damage;

        public virtual void Init(Transform target, float damage)
        {
            Target = target;
            Damage = damage;
        }

        public abstract void Tick();

        protected virtual void Hit()
        {
            if (_hitVFX)
            {
                _hitVFX.Play();
            }

            if (Target.TryGetComponent<Health>(out var health))
            {
                health.TakeDamage(Damage);
            }

            OnHited?.Invoke(this);
        }
    }
}