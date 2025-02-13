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
            for (var i = 0; i < _rewards.Count; i++)
            {
                if (_rewards[i].WaveToGet >= maxWave && i != 0)
                {
                    return _rewards[i - 1];
                }
            }
            
            throw new Exception("No wave to reward");
        }

        public int NextWave(int maxWave)
        {
            for (int i = 0; i < _rewards.Count - 1; i++)
            {
                if (_rewards[i].WaveToGet >= maxWave)
                    return _rewards[i].WaveToGet;
            }

            return maxWave;
        }

        [Serializable]
        public class RewardData
        {
            public int WaveToGet;
            public Currency Type;
            public int Amount;
        }
    }
}