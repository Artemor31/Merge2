using System.Collections;
using Gameplay.Units;
using UnityEngine;

namespace Services.BuffService.Components
{
    public class HealthRegenComponent : BuffComponent
    {
        private int _regen;
        private Actor _actor;
        private WaitForSeconds _waitForSeconds;

        public override void Apply()
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
                var stats = _actor.Stats;
                stats.Health += _regen;
                _actor.Stats = stats;
                yield return _waitForSeconds;
            }
        }
    }
}