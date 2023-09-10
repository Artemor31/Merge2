using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public class Unit : MonoBehaviour
    {
        [field: SerializeField] public Health Health { get; set; }
        [field: SerializeField] public Mover Mover { get; set; }
        [field: SerializeField] public TargetSearch TargetSearch { get; set; }
        [field: SerializeField] public Attacker Attacker { get; set; }

        private void OnEnable() => 
            Health.Died += OnDies;
        
        private void OnDies()
        {
            Health.Died -= OnDies;
            Attacker.Disable();
            Health.Disable();
            Mover.Disable();
            TargetSearch.Disable();
        }
    }
}