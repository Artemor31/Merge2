using System;
using System.Collections.Generic;
using Infrastructure;
using UI;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Database/WaveRewards", fileName = "WaveRewards", order = 0)]
    public class WaveRewardsDatabase : Database
    {
        [SerializeField] private List<RewardData> _rewards;

        public RewardData GetFor(int currentWave, bool leftSide = false)
        {
            if (leftSide)
            {
                foreach (RewardData data in _rewards)
                {
                    if (currentWave > data.StartWave && currentWave <= data.WaveToGet)
                    {
                        return data;
                    }
                }
            }
            else
            {
                foreach (RewardData data in _rewards)
                {
                    if (currentWave >= data.StartWave && currentWave < data.WaveToGet)
                    {
                        return data;
                    }
                }
            }

            return _rewards[^1];
        }
    }

    [Serializable]
    public class RewardData : WindowData
    {
        public int WaveToGet;
        public int StartWave;
        public Currency Type;
        public int Amount;
    }
}