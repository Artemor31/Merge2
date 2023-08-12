using System.Collections.Generic;
using CodeBase.Gameplay.Units;
using UnityEngine;

namespace CodeBase.Databases
{
    [CreateAssetMenu(menuName = "Create UnitsDatabase", fileName = "UnitsDatabase", order = 0)]
    public class UnitsDatabase : Database
    {
        public List<UnitData> Units;
    }
}