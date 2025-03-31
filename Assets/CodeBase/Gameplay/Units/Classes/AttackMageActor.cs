using System.Collections;
using System.Linq;
using UnityEngine;

namespace Gameplay.Units.Classes
{
    public class AttackMageActor : Actor
    {
        [SerializeField] private ParticleSystem _Vfx;
        
        private void Update()
        {
            if (IsDead) return;

            TickActTimer();

            if (!CanFindTarget()) return;

            LookAtTarget();
            
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

        private void SpawnVFX(Vector3 point) => Instantiate(_Vfx, point, Quaternion.identity).Play();

        private IEnumerator PerformAct()
        {
            ResetCooldown();
            View.PerformAct();

            yield return new WaitForSeconds(0.5f);

            if (Target == null || IsDead) yield break;
            
            SpawnVFX(Target.transform.position + Vector3.up);
            Target.ChangeHealth(Stats.Damage, HealthContext.Heal);
            if (Stats.Vampirism > 0)
            {
                ChangeHealth(Stats.Damage * Stats.Vampirism, HealthContext.Heal);
            }
        }

        protected override bool NeedNewTarget() => true;

        protected override void SearchNewTarget()
        {
            Target = SearchTarget.For(this)
                                 .SelectTargets(Side.Ally)
                                 .FilterBy(Strategy.Closest)
                                 .FirstOrDefault();
        }
    }
}