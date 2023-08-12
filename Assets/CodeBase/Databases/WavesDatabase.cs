using System;
using System.Collections.Generic;
using CodeBase.Gameplay.Units;
using UnityEngine;

namespace CodeBase.Databases
{
    [CreateAssetMenu(menuName = "Create WavesDatabase", fileName = "WavesDatabase", order = 0)]
    public class WavesDatabase : Database
    {
        public List<WaveData> WavesData;
    }
    
    [Serializable]
    public class WaveData
    {
        public int Wave;
        public List<EnemyAmount> Enemies;
    }
    
    [Serializable]
    public class EnemyAmount
    {
        public UnitId Unit;
        public int Amount;
    }

}