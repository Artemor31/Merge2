using CodeBase.Databases;
using UnityEngine;

namespace CodeBase.Gameplay.Units.Behaviours
{
    public abstract class Action : MonoBehaviour
    {
        protected virtual bool CooldownUp => AttackTimer <= 0;

        [SerializeField] protected float _damage;
        [SerializeField] protected float _range;
        [SerializeField] protected float _attackCooldown;

        protected float AttackTimer;
        protected AnimatorScheduler Animator;

        public virtual void Init(AnimatorScheduler animator)
        {
            Animator = animator;
            AttackTimer = 0;
        }

        public abstract void PerformOn(Actor actor);

        public virtual void Tick()
        {
            if (CooldownUp == false)
                AttackTimer -= Time.deltaTime;
        }

        protected void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(transform.position, _range);
        public virtual bool CanAttack(Actor actor) => InRange(actor) && CooldownUp;
        public virtual bool InRange(Actor actor) =>
            Vector3.Distance(transform.position, actor.transform.position) <= _range;
    }
}