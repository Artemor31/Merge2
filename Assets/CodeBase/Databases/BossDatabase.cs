using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Database/BossDatabase", fileName = "BossDatabase", order = 0)]
    public class BossDatabase : Database
    {
        [SerializeField] private List<BossData> _bosses;

        public string GetBossFor(Race race, Mastery mastery)
        {
            BossData data = _bosses.FirstOrDefault(b => b.Race == race && b.Mastery == mastery);
            return data != null ? data.PrefabPath : _bosses[0].PrefabPath;
        }
    }
    
    [Serializable]
    public class BossData
    {
        public string PrefabPath;
        public Race Race;
        public Mastery Mastery;
    }

}