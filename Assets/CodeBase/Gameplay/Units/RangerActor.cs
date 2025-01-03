using System.Linq;
using Databases;
using Databases.Data;
using Gameplay.Units.Healths;
using Gameplay.Units.Projectiles;
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
            base.Initialize(view, data, stats);
            _service = ServiceLocator.Resolve<ProjectileService>();
        }

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
            _service.Create(_projectileType, this, Target, damage);
            
            ResetCooldown();
        }

        protected override bool NeedNewTarget() => Target == null || Target.IsDead;
        
        protected override void SearchNewTarget() => Target = SearchTarget.For(this)
                                                                          .SelectTargets(Side.Enemy)
                                                                          .FilterBy(Strategy.Closest)
                                                                          .FirstOrDefault();
    }
}