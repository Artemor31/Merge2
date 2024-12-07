using System.Collections;
using Gameplay.Units;
using Gameplay.Units.Health;
using UnityEngine;

namespace Services.BuffService.Components
{
    public class HealthRegenComponent : BuffComponent
    {
        private int _regen;
        private Actor _actor;
        private WaitForSeconds _waitForSeconds;

        private void OnEnable()
        {
            _actor = GetComponent<Actor>();
            _waitForSeconds = new WaitForSeconds(1);
            _regen = (int)(_actor.Stats.Health * 0.05f);
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