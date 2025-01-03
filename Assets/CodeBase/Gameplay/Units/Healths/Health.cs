using UnityEngine;

namespace Gameplay.Units.Healths
{
    public struct Health
    {
        public float CurrentRatio => _current / _maxHealth;
        public bool IsDead => _current <= 0;
        
        private readonly float _maxHealth;
        private readonly float _defence;
        private float _current;

        public Health(float maxHealth, float defence)
        {
            _current = _maxHealth = maxHealth;
            _defence = defence;
        }

        public void ChangeHealth(float value, HealthContext contexts)
        {
            switch (contexts)
            {
                case HealthContext.None: break;
                case HealthContext.Damage:
                    value *= 1f - _defence;
                    _current -= value;
                    break;
                case HealthContext.Heal:
                    _current = Mathf.Min(_maxHealth, _current + value);
                    break;
                case HealthContext.PureDamage:
                    _current -= value;
                    break;
            }
        }
    }
}