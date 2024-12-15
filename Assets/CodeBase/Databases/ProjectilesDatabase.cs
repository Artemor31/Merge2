using System.Collections.Generic;
using System.Linq;
using Gameplay.Units.Projectiles;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Create ProjectilesDatabase", fileName = "ProjectilesDatabase", order = 0)]
    public class ProjectilesDatabase : Database
    {
        [SerializeField] private List<ProjectileData> ProjectileDatas;
        public ProjectileData Get<T>() => ProjectileDatas.First(p => p.GetType() == typeof(T));
    }

    public class ProjectileData
    {
        public Projectile Prefab;
    }
}