using UnityEngine;

namespace Gameplay.Units.Healths
{
    public class Health
    {
        public float CurrentRatio => _current / _maxHealth;
        public bool IsDead => _current <= 0;
        
        private readonly float _maxHealth;
        private float _current;

        public Health(float maxHealth) => _current = _maxHealth = maxHealth;

        public void ChangeHealth(float value, HealthContext contexts)
        {
            switch (contexts)
            {
                case HealthContext.None: break;
                case HealthContext.Damage:
                    _current -= value;
                    break;
                case HealthContext.Heal:
                    _current = Mathf.Min(_maxHealth, _current + value);
                    break;
                case HealthContext.PureDamage:
                    break;
            }
        }
    }
}