using System;
using System.Collections.Generic;
using CodeBase.Units;

namespace CodeBase
{
    [Serializable]
    public class WaveData
    {
        public int Wave;
        public List<EnemyAmount> Enemies;
    }
}