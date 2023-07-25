using System.Collections.Generic;
using CodeBase.Units;
using UnityEngine;

namespace CodeBase.Databases
{
    [CreateAssetMenu(menuName = "Create PlayerUnitsDatabase", fileName = "PlayerUnitsDatabase", order = 0)]
    public class PlayerUnitsDatabase : ScriptableObject
    {
        public List<PlayerUnitData> Units;
    }
}