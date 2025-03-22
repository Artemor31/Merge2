using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Units;
using Gameplay.Units.Projectiles;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Database/ProjectilesDatabase", fileName = "ProjectilesDatabase", order = 0)]
    public class ProjectilesDatabase : Database
    {
        [SerializeField] private List<ProjectileData> ProjectileDatas;
        
        public ProjectileData Get(ProjectileType type) => ProjectileDatas.First(p => p.Id == type);
    }

    [Serializable]
    public class ProjectileData
    {
        public ProjectileType Id;
        public float MoveSpeed;
        public float DamageArea;
        public Projectile Prefab;
        public ParticleSystem HitVFX;
        public HealthContext DamageContext;
    }

    public enum ProjectileType
    {
        None = 0,
        Arrow = 1,
        Fireball = 2,
    }
}