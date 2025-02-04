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
            
            float damage = Random.Range(0, 1f) <= Stats.CritChance
                ? Stats.Damage * (1 + Stats.CritValue)
                : Stats.Damage;

            Vector3 position = transform.position + Vector3.up;
            _service.Create(_projectileType, position, Target, damage);
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