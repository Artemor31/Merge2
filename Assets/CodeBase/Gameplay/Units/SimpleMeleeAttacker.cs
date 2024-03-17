using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public class SimpleMeleeAttacker : Attacker
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _range;
        [SerializeField] private float _attackCooldown;

        private float _timer;
        private bool _available;

        public override bool CanAttack(Actor actor) => 
            InRange(actor) && CooldownUp()  && _available;

        public override void Attack(Actor actor)
        {
            _available = true;
            if (!CanAttack(actor)) return;
            
            actor.Health.TakeDamage(_damage);
            _timer = _attackCooldown;
        }

        public override void Reset() =>
            _available = false;

        public override bool InRange(Vector3 transformPosition)
        {
            return true;
        }

        private void Update()
        {
            if (CooldownUp() == false)
                _timer -= Time.deltaTime;
        }

        private bool CooldownUp() => 
            _timer <= 0;

        private bool InRange(Actor actor) => 
            Vector3.Distance(transform.position, actor.transform.position) <= _range;
    }
}