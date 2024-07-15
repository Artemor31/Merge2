using System;
using UnityEngine;

namespace CodeBase.Gameplay.Units.Behaviours
{
    public abstract class Health : MonoBehaviour
    {
        public abstract event Action Died;
        public abstract event Action HealthChanged;
        public abstract float Current { get; protected set; }
        public abstract float Ratio { get; }
        public abstract void TakeDamage(float damage);
        public abstract void Init(AnimatorScheduler animator);
    }
}