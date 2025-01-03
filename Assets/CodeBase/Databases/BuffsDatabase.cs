using System;
using System.Collections.Generic;
using Infrastructure;
using Services.Buffs.Components;
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
            
            foreach (var data in BuffConfigs)
            {
                if (data.Race != Race.None)
                {
                    RaceData.Add(data.Race, data);
                }
                else
                {
                    MasteryData.Add(data.Mastery, data);
                }
            }
        }
    }
    
    [Serializable]
    public class BuffConfig
    {
        public string Name => Mastery == Mastery.None ? Race.ToString() : Mastery.ToString();
        public Sprite Icon;
        public Race Race;
        public Mastery Mastery;
        public string Description;
        public SerializableMonoScript<BuffComponent> Behaviour;
    }
}