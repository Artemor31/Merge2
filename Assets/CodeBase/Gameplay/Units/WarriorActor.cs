using System.Linq;
using Gameplay.Units.Healths;
using UnityEngine;

namespace Gameplay.Units
{
    public class WarriorActor : Actor
    {
        protected override void Tick()
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
                    PerformAct();
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

        private void PerformAct()
        {
            View.PerformAct();
            float damage = Random.Range(0, 1f) <= Stats.CritChance
                ? Stats.Damage * (1 + Stats.CritValue)
                : Stats.Damage;
            
            Target.ChangeHealth(damage, HealthContext.Damage);
            if (Stats.Vampirism > 0)
            {
                ChangeHealth(damage * Stats.Vampirism, HealthContext.Heal);
            }

            ResetCooldown();
        }

        protected override bool NeedNewTarget() => Target == null || Target.IsDead;

        protected override void SearchNewTarget() => Target = SearchTarget.For(this)
                                                                          .SelectTargets(Side.Enemy)
                                                                          .FilterBy(Strategy.OnSameLine)
                                                                          .FirstOrDefault();
    }
}