using System.Collections;
using System.Linq;
using UnityEngine;

namespace Gameplay.Units.Classes
{
    public class HealerActor : Actor
    {
        [SerializeField] private ParticleSystem _healVfxPrefab;
        private readonly WaitForSeconds _waitForSeconds = new(0.7f);

        private void Update()
        {
            if (IsDead) return;

            TickActTimer();

            if (!CanFindTarget()) return;

            transform.LookAt(Target.transform);

            if (InRange())
            {
                _mover.Stop();

                if (CooldownUp)
                {
                    StartCoroutine(PerformAct());
                }
            }
            else
            {
                _mover.MoveTo(Target);
            }
        }

        private void SpawnVFX(Vector3 point) => Instantiate(_healVfxPrefab, point, Quaternion.identity).Play();

        private IEnumerator PerformAct()
        {
            ResetCooldown();
            View.PerformAct();
            SpawnVFX(Target.transform.position + Vector3.up);
            Target.ChangeHealth(Stats.Damage, HealthContext.Heal);
            yield return _waitForSeconds;
        }

        protected override bool NeedNewTarget() => true;

        protected override void SearchNewTarget()
        {
            Target = SearchTarget.For(this)
                                 .SelectTargets(Side.Ally)
                                 .FilterBy(Strategy.MostDamaged)
                                 .FirstOrDefault();
        }
    }
}