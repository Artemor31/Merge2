using System;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public class Unit : MonoBehaviour
    {
        public Health Health => _health;
        public Mover Mover => _mover;
        public TargetSearch TargetSearch => _targetSearch;
        public Attacker Attacker => _attacker;

        [SerializeField] private Health _health;
        [SerializeField] private Mover _mover;
        [SerializeField] private TargetSearch _targetSearch;
        [SerializeField] private Attacker _attacker;
    }

    public abstract class Attacker : MonoBehaviour
    {
        public abstract bool CanAttack { get; }
        public abstract void Attack(Unit unit);
    }

    public class SimpleMeleeAttacker : Attacker
    {
        public override bool CanAttack => _timer <= 0;

        [SerializeField] private float _damage;
        [SerializeField] private float _range;
        [SerializeField] private float _attackSpeed;
        
        private float _timer;

        public override void Attack(Unit unit)
        {
            
        }

        private void Update()
        {
            if (_timer > 0)
                _timer -= Time.deltaTime;
            
        }
    }
}