﻿namespace Gameplay.Units.Behaviours
{
    public class MeleeAttackAct : Act
    {
        public override void PerformOn(Actor actor)
        {
            if (!CanAttack(actor)) return;

            transform.LookAt(actor.transform);
            Animator.PerformAct();
            actor.ChangeHealth(Stats.Damage, HealthContext.Damage);
            ResetCooldown();
        }
    }
}