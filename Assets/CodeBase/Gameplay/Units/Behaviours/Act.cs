using UnityEngine;

namespace Gameplay.Units.Behaviours
{
    public abstract class Act : MonoBehaviour
    {
        protected virtual bool CooldownUp => ActTimer <= 0;

        [SerializeField] protected float _damage;
        [SerializeField] protected float _range;
        [SerializeField] protected float _actCooldown;

        protected float ActTimer;
        protected AnimatorScheduler Animator;

        public virtual void Init(AnimatorScheduler animator)
        {
            Animator = animator;
            ActTimer = 0;
        }

        public abstract void PerformOn(Actor actor);

        public virtual void Tick()
        {
            if (CooldownUp == false)
                ActTimer -= Time.deltaTime;
        }

        protected void ResetCooldown() => ActTimer = _actCooldown;
        protected void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(transform.position, _range);
        public virtual bool CanAttack(Actor actor) => InRange(actor) && CooldownUp;
        public virtual bool InRange(Actor actor) => 
            Vector3.Distance(transform.position, actor.transform.position) <= _range;
    }
}