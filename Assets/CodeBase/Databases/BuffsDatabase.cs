using System.Collections.Generic;
using Databases.BuffConfigs;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Database/BuffsDatabase", fileName = "BuffsDatabase", order = 0)]
    public class BuffsDatabase : Database
    {
        public List<BuffConfig> BuffConfigs;
        public Dictionary<Race, BuffConfig> RaceData;
        public Dictionary<Mastery, BuffConfig> MasteryData;

        public override void Cache()
        {
            RaceData = new Dictionary<Race, BuffConfig>();
            MasteryData = new Dictionary<Mastery, BuffConfig>();
            
            foreach (BuffConfig data in BuffConfigs)
            {
                if (data.Race != Race.None)
                {
                    RaceData.TryAdd(data.Race, data);
                }
                else
                {
                    MasteryData.TryAdd(data.Mastery, data);
                }
            }
        }

        public Sprite IconFor(Race race) => RaceData[race].Icon;
        public Sprite IconFor(Mastery mastery) => MasteryData[mastery].Icon;
    }
}