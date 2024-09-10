using UnityEngine;

namespace Gameplay.Units.Behaviours
{
    public abstract class Act : MonoBehaviour
    {
        [SerializeField] protected float _damage;
        [SerializeField] protected float _range;
        [SerializeField] protected float _actCooldown;

        private bool CooldownUp => ActTimer <= 0;
        protected AnimatorScheduler Animator;
        protected float ActTimer;

        public virtual void Init(AnimatorScheduler animator)
        {
            Animator = animator;
            ActTimer = 0;
        }

        public virtual void Tick()
        {
            if (CooldownUp == false)
                ActTimer -= Time.deltaTime;
        }
        
        public abstract void PerformOn(Actor actor);
        public bool CanAttack(Actor actor) => InRange(actor) && CooldownUp;
        public bool InRange(Actor actor) => Vector3.Distance(transform.position, actor.transform.position) <= _range;
        protected void ResetCooldown() => ActTimer = _actCooldown;
        protected void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(transform.position, _range);
    }
}