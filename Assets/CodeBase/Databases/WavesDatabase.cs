using System;
using System.Collections.Generic;
using NaughtyAttributes.Core.DrawerAttributes_SpecialCase;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Database/WavesDatabase", fileName = "WavesDatabase", order = 0)]
    public class WavesDatabase : Database
    {
        public List<WaveData> WavesData => _wavesData;
        [SerializeField] private List<WaveData> _wavesData;

        [Button]
        public void FillFields()
        {
            for (int i = 0; i < _wavesData.Count; i++)
            {
                if (_wavesData[i].Races.Length == 1 && _wavesData[i].Races[0] == Race.None)
                {
                    _wavesData[i].Races = Enum.GetValues(typeof(Race)) as Race[];
                }
                
                if (_wavesData[i].Masteries.Length == 1 && _wavesData[i].Masteries[0] == Mastery.None)
                {
                    _wavesData[i].Masteries = Enum.GetValues(typeof(Mastery)) as Mastery[];
                }
            }
        }
    }
    
    [Serializable]
    public class WaveData
    {
        public int PowerLimit;
        public int MaxLevel;
        public Mastery[] Masteries;
        public Race[] Races;
    }
}