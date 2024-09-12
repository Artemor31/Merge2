using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Create WavesDatabase", fileName = "WavesDatabase", order = 0)]
    public class WavesDatabase : Database
    {
        public List<WaveData> WavesData;
    }
    
    [Serializable]
    public class WaveData
    {
        public List<EnemyAmount> Enemies;
    }
    
    [Serializable]
    public class EnemyAmount
    {
        public ActorData ActorData;
        public int Amount;
    } 
}