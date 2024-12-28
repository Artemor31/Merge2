using System.Collections.Generic;
using UnityEngine;
using System;

namespace Databases
{
    [CreateAssetMenu(menuName = "Database/UpgradesDatabase", fileName = "UpgradesDatabase", order = 0)]
    public class UpgradesDatabase : Database
    {
        public List<UpgradeData> Datas;
    }
    
    [Serializable]
    public class UpgradeData
    {
        public string Name => Mastery == Mastery.None ? Race.ToString() : Mastery.ToString();
        public Sprite Icon;
        public Mastery Mastery;
        public Race Race;
    }
}