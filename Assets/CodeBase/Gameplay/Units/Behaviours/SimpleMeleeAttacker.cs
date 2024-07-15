using UnityEngine;

namespace CodeBase.Gameplay.Units.Behaviours
{
    public class SimpleMeleeAttacker : Attacker
    {
        public override void Attack(Actor actor)
        {
            if (!CanAttack(actor)) return;

            transform.LookAt(actor.transform);
            Animator.Attack();
            actor.TakeDamage(_damage);
            AttackTimer = _attackCooldown;
        }
    }
}