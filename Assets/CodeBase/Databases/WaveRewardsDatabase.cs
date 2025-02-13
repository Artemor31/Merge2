using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Database/WaveRewards", fileName = "WaveRewards", order = 0)]
    public class WaveRewardsDatabase : Database
    {
        [SerializeField] private List<ProjectileData> ProjectileDatas;
        
        public ProjectileData Get(ProjectileType type) => ProjectileDatas.First(p => p.Id == type);
    }
}