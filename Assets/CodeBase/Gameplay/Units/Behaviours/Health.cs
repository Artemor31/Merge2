using UnityEngine;

namespace Gameplay.Units.Behaviours
{
    public abstract class Health : MonoBehaviour
    {
        public abstract event System.Action Died;
        public abstract event System.Action HealthChanged;
        public abstract float Current { get; protected set; }
        public abstract float Ratio { get; }
        public abstract void TakeDamage(float damage);
        public abstract void Init(AnimatorScheduler animator);
    }
}