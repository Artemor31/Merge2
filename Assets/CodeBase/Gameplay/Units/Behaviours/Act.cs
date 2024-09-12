using Databases;
using UnityEngine;

namespace Gameplay.Units.Behaviours
{
    public abstract class Act : MonoBehaviour
    {
        protected AnimatorScheduler Animator;
        protected ActorStats Stats;
        protected float ActTimer;

        private bool CooldownUp => ActTimer <= 0;

        public virtual void Init(AnimatorScheduler animator, ActorStats stats)
        {
            Animator = animator;
            ActTimer = 0;
            Stats = stats;
        }

        public virtual void Tick()
        {
            if (CooldownUp == false)
                ActTimer -= Time.deltaTime;
        }

        public abstract void PerformOn(Actor actor);
        public bool CanAttack(Actor actor) => InRange(actor) && CooldownUp;

        public bool InRange(Actor actor) =>
            Vector3.Distance(transform.position, actor.transform.position) <= Stats.Range;

        protected void ResetCooldown() => ActTimer = Stats.ActCooldown;
        protected void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(transform.position, Stats.Range);
    }
}