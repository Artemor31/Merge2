using System.Collections;
using System.Linq;
using Gameplay.Units.Healths;
using UnityEngine;

namespace Gameplay.Units
{
    public class WarriorActor : Actor
    {
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
                if (CooldownUp)
                {
                    _mover.MoveTo(Target);
                }
            }
        }

        private IEnumerator PerformAct()
        {
            ResetCooldown();
            View.PerformAct();
            yield return new WaitForSeconds(0.5f);
            
            if (Target == null) yield break;
            
            bool isCrit = Random.Range(0, 1f) >= 1 - Stats.CritChance;
            float damage = isCrit ? Stats.Damage + Stats.Damage * Stats.CritValue : Stats.Damage;

            Target.ChangeHealth(damage, isCrit ? HealthContext.Crit : HealthContext.Damage);
            
            if (Stats.Vampirism > 0)
            {
                ChangeHealth(damage * Stats.Vampirism, HealthContext.Heal);
            }
        }

        protected override bool NeedNewTarget() => Target == null || Target.IsDead;

        protected override void SearchNewTarget() => Target = SearchTarget.For(this)
                                                                          .SelectTargets(Side.Enemy)
                                                                          .FilterBy(Strategy.OnSameLine)
                                                                          .FirstOrDefault();
    }
}