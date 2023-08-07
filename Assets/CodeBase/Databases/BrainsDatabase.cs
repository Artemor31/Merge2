using System;
using System.Collections.Generic;
using CodeBase.Gameplay.Units;

namespace CodeBase.Databases
{
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