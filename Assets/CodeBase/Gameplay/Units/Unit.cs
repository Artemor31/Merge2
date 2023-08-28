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
}