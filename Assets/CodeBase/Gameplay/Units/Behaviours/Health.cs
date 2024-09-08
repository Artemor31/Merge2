using System;
using UnityEngine;

namespace Gameplay.Units.Behaviours
{
    public abstract class Health : MonoBehaviour
    {
        public abstract event Action<float, float> HealthChanged;
        public abstract float Current { get; protected set; }
        public abstract void Init(AnimatorScheduler animator);
        public abstract void ChangeHealth(float value, HealthContext context);
    }
}