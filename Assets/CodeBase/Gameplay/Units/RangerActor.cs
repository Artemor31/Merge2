using System.Collections;
using System.Linq;
using Databases;
using Databases.Data;
using Gameplay.Units.Healths;
using Infrastructure;
using Services;
using UnityEngine;

namespace Gameplay.Units
{
    public class RangerActor : Actor
    {
        [SerializeField] private ProjectileType _projectileType; 
        private ProjectileService _service;

        public override void Initialize(ActorSkin view, ActorData data, ActorStats stats)
        {
            _service = ServiceLocator.Resolve<ProjectileService>();
            base.Initialize(view, data, stats);
        }

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
            float damage = isCrit ? Stats.Damage * Stats.CritValue : Stats.Damage;

            Vector3 position = transform.position + Vector3.up;
            HealthContext context = isCrit ? HealthContext.Crit : HealthContext.Damage;
            _service.Create(_projectileType, position, Target, damage, context);
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