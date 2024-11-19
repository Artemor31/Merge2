using System.Collections.Generic;
using Databases;
using UnityEngine;

namespace Services.BuffService
{
    [CreateAssetMenu(menuName = "Create BuffsDatabase", fileName = "BuffsDatabase", order = 0)]
    public class BuffsDatabase : Database
    {
        public List<BuffConfig> BuffConfigs;
    }
}