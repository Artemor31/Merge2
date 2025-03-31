using System.Collections;
using System.Linq;
using Databases;
using Infrastructure;
using Services;
using UnityEngine;

namespace Gameplay.Units.Classes
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
            
            if (Target == null || !InRange())
            {
                View.GoIdle(true);
                yield break;
            }

            if (IsDead)
            {
                yield break;
            }


            bool isCrit = Random.Range(0, 1f) >= 1 - Stats.CritChance;
            float damage = isCrit ? Stats.Damage * Stats.CritValue : Stats.Damage;

            Vector3 position = transform.position + Vector3.up;
            HealthContext context = isCrit ? HealthContext.Crit : HealthContext.Damage;
            _service.Create(_projectileType, position, Target, damage, context);
        }

        protected override bool NeedNewTarget() => Target == null || Target.IsDead;

        protected override void SearchNewTarget() => Target = SearchTarget.For(this)
                                                                          .SelectTargets(Side.Enemy)
                                                                          .FilterBy(Strategy.Closest)
                                                                          .FirstOrDefault();
    }
}