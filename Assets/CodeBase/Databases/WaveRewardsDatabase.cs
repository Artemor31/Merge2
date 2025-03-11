using System;
using System.Collections.Generic;
using Infrastructure;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Database/WaveRewards", fileName = "WaveRewards", order = 0)]
    public class WaveRewardsDatabase : Database
    {
        [SerializeField] private List<RewardData> _rewards;

        public RewardData GetFor(int maxWave)
        {
            foreach (RewardData data in _rewards)
            {
                if (maxWave >= data.StartWave && maxWave < data.WaveToGet)
                {
                    return data;
                }
            }

            return _rewards[^1];
        }
    }
    
    [Serializable]
    public class RewardData
    {
        public int WaveToGet;
        public int StartWave;
        public Currency Type;
        public int Amount;
    }
}