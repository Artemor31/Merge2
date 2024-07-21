using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Databases
{
    [CreateAssetMenu(menuName = "Create WavesDatabase", fileName = "WavesDatabase", order = 0)]
    public class WavesDatabase : Database
    {
        public List<WaveData> WavesData;
        
        [Serializable]
        public class WaveData
        {
            public int Wave;
            public List<EnemyAmount> Enemies;
        }
    
        [Serializable]
        public class EnemyAmount
        {
            public Race Race;
            public Mastery Mastery;
            public int Amount;
        } 
    }
}