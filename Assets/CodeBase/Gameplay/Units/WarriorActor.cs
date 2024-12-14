using System.Linq;
using Gameplay.Units.Healths;

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
            Target.ChangeHealth(Stats.Damage, HealthContext.Damage);
            ResetCooldown();
        }

        protected override bool NeedNewTarget() => Target == null || Target.IsDead;

        protected override void SearchNewTarget() => Target = SearchTarget.For(this)
                                                                          .SelectTargets(Side.Enemy)
                                                                          .FilterBy(Strategy.OnSameLine)
                                                                          .FirstOrDefault();
    }
}