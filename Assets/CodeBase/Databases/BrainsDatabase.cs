using System;
using System.Collections.Generic;
using CodeBase.Gameplay.Units;
using UnityEngine;

namespace CodeBase.Databases
{
    [CreateAssetMenu(menuName = "Create BrainsDatabase", fileName = "BrainsDatabase", order = 0)]
    public class BrainsDatabase : Database  
    {
        public List<BrainPairs> Brains;
    }

    [Serializable]
    public class BrainPairs
    {
        public Brain Brain;
        public UnitId UnitId;
    }
}