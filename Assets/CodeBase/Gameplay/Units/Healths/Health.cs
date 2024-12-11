using System;
using Databases;
using UnityEngine;

namespace Gameplay.Units.Healths
{
    public abstract class Health : MonoBehaviour
    {
        public abstract event Action<float, float> HealthChanged;
        public abstract float Current { get; protected set; }
        public abstract float CurrentRatio { get; }
        public abstract void Init(AnimatorScheduler animator, ActorStats stats);
        public abstract void ChangeHealth(float value, HealthContext context);
    }
}