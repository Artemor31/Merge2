using System.Collections;
using Gameplay.Units;
using Gameplay.Units.Healths;
using UnityEngine;

namespace Services.Buffs.Components
{
    public class HealthRegenComponent : BuffComponent
    {
        private const float RegenValue = 0.05f / 3;
        private const float Seconds = 1f / 3;

        private WaitForSeconds _waitForSeconds;
        private Actor _actor;
        private int _regen;

        private void OnEnable()
        {
            _actor = GetComponent<Actor>();
            _waitForSeconds = new WaitForSeconds(Seconds);
            _regen = (int)(_actor.Stats.Health * RegenValue);
            StartCoroutine(HealthRegen());
        }

        private IEnumerator HealthRegen()
        {
            while (!_actor.IsDead)
            {
                _actor.ChangeHealth(_regen, HealthContext.Heal);
                yield return _waitForSeconds;
            }
        }
    }
}