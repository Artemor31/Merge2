using System;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public abstract class Health : MonoBehaviour
    {
        public abstract event Action Died;
        public abstract float Current { get; protected set; }
        public abstract void TakeDamage(float damage);
        public abstract void Die();
        public abstract void Disable();
    }
}