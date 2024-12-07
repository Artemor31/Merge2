using System;
using Gameplay.Units.Health;
using Infrastructure;
using UnityEngine;

namespace Gameplay.Units.Fighting
{
    public abstract class Projectile : MonoBehaviour, IPoolable
    {
        public event Action<Projectile> OnHited; 
        
        [SerializeField] protected float _speed;
        [SerializeField] protected float _damageArea;
        [SerializeField] protected ParticleSystem _hitVFX;

        protected Transform Target;
        private float _damage;
        private Action<Projectile> _hitedAction;

        public virtual void Init(Transform target, float damage, Action<Projectile> hitedAction)
        {
            Target = target;
            _damage = damage;
            _hitedAction = hitedAction;
            OnHited += _hitedAction;
        }

        public abstract void Tick();

        protected virtual void Hit()
        {
            if (_hitVFX)
            {
                _hitVFX.Play();
            }

            if (Target.TryGetComponent<Health.Health>(out var health))
            {
                health.ChangeHealth(_damage, HealthContext.Damage);
            }

            OnHited?.Invoke(this);
            OnHited -= _hitedAction;
        }

        public void Collect() => gameObject.SetActive(false);

        public void Release() => gameObject.SetActive(true);
    }
}