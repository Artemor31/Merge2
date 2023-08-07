using System.Collections.Generic;
using CodeBase.Gameplay.Units;
using UnityEngine;

namespace CodeBase.Databases
{
    public abstract class Brain : ScriptableObject
    {
        public abstract IUnit BestTarget(IEnumerable<IUnit> candidates);
    }
}