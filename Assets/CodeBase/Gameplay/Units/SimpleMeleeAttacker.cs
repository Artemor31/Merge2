using System;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public class SimpleMeleeAttacker : Attacker
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _range;
        [SerializeField] private float _attackCooldown;

        private bool CooldownUp => _timer <= 0;
        private float _timer;
        private AnimatorScheduler _animator;

        public override void Init(AnimatorScheduler animator)
        {
            _animator = animator;
            _timer = 0;
        }

        public override void Tick()
        {
            if (CooldownUp == false)
                _timer -= Time.deltaTime;
        }

        public override bool CanAttack(Actor actor) => 
            InRange(actor) && CooldownUp;

        public override void Attack(Actor actor)
        {
            if (!CanAttack(actor)) return;
            
            transform.LookAt(actor.transform);
            _animator.Attack();
            actor.TakeDamage(_damage);
            _timer = _attackCooldown;
        }

        public override bool InRange(Actor actor) => 
            Vector3.Distance(transform.position, actor.transform.position) <= _range;

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _range);
        }
    }
}