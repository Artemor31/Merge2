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

        public override bool CanAttack(Unit unit) => 
            InRange(unit) && CooldownUp()  && _available;

        public override void Attack(Unit unit)
        {
            _available = true;
            if (!CanAttack(unit)) return;
            
            unit.Health.TakeDamage(_damage);
            _timer = _attackCooldown;
        }

        public override void Reset() =>
            _available = false;

        private void Update()
        {
            if (CooldownUp() == false)
                _timer -= Time.deltaTime;
        }

        private bool CooldownUp() => 
            _timer <= 0;

        private bool InRange(Unit unit) => 
            Vector3.Distance(transform.position, unit.transform.position) <= _range;
    }
}