using System.Linq;
using Gameplay.Units.Healths;
using UnityEngine;

namespace Gameplay.Units
{
    public class AttackMageActor : Actor
    {
        [SerializeField] private ParticleSystem _Vfx;
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
                _mover.MoveTo(Target);
            }
        }

        private void SpawnVFX(Vector3 point) => Instantiate(_Vfx, point, Quaternion.identity).Play();

        private void PerformAct()
        {
            SpawnVFX(Target.transform.position + Vector3.up);
            Target.ChangeHealth(Stats.Damage, HealthContext.Heal);
            View.PerformAct();
            ResetCooldown();
        }

        protected override bool NeedNewTarget() => true;

        protected override void SearchNewTarget()
        {
            Target = SearchTarget.For(this)
                                 .SelectTargets(Side.Ally)
                                 .FilterBy(Strategy.OnSameLine)
                                 .FirstOrDefault();
        }
    }
}