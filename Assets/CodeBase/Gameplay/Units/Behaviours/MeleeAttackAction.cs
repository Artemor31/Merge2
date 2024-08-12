namespace Gameplay.Units.Behaviours
{
    public class MeleeAttackAction : Action
    {
        public override void PerformOn(Actor actor)
        {
            if (!CanAttack(actor)) return;

            transform.LookAt(actor.transform);
            Animator.Attack();
            actor.TakeDamage(_damage);
            AttackTimer = _attackCooldown;
        }
    }
}