using System.Collections.Generic;
using CodeBase.Gameplay.Units;
using UnityEngine;

namespace CodeBase.Databases
{
    [CreateAssetMenu(menuName = "Create MeleeAttackerBrain", fileName = "MeleeAttackerBrain", order = 0)]
    public class MeleeAttackerBrain : Brain
    {
        public override IUnit BestTarget(IEnumerable<IUnit> candidates)
        {
            return null;
        }
    }
}