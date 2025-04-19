using System;
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

        public string NameFor(Race race) => RaceData[race].GetBuffName();
        public string NameFor(Mastery mastery) => MasteryData[mastery].GetBuffName();
        public string NameFor(Race race, Mastery mastery) => $"{RaceData[race].GetBuffName()} {MasteryData[mastery].GetBuffName()}";
        public Sprite IconFor(Race race) => RaceData[race].Icon;
        public Sprite IconFor(Mastery mastery) => MasteryData[mastery].Icon;
        public int CostFor(int level)
        {
            return level switch
            {
                1 => 3,
                2 => 5,
                3 => 8,
                _ => throw new Exception("cost not found")
            };
        }
        
        /*
Редкость	1★ (начальный)	2★ (апгрейд)	3★ (макс.)
Обычный	3 золота	6 золота	12 золота
Редкий	5 золота	10 золота	20 золота
Эпический	8 золота	16 золота	32 золота
Легендарный	15 золота	30 золота	60 золота
         */
    }
}