using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Databases
{
    [CreateAssetMenu(menuName = "Create UnitsDatabase", fileName = "UnitsDatabase", order = 0)]
    public class UnitsDatabase : Database
    {
        public List<ActorConfig> Units;
    }

    public enum UnitId
    {
        None = 0,
        HumanWarrior1 = 1,
        HumanWarrior2 = 2,
        HumanWarrior3 = 3,
    }
}