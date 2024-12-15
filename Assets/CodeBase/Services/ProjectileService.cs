using System.Collections.Generic;
using Databases;
using Gameplay.Units;
using Gameplay.Units.Projectiles;
using Infrastructure;
using Services.Infrastructure;
using Services.Resources;
using UnityEngine;

namespace Services
{
    public class ProjectileService : IService
    {
        private readonly Dictionary<ProjectileType, Pool<Projectile>> _pools = new();
        private readonly List<Projectile> _active = new();
        private readonly ProjectilesDatabase _database;

        public ProjectileService(IUpdateable updateable, DatabaseProvider provider)
        {
            updateable.Tick += Tick;
            _database = provider.GetDatabase<ProjectilesDatabase>();
        }
        
        public void Create(ProjectileType type,  Actor caster, Actor target, float damage)
        {
            ProjectileData data = _database.Get(type);
            if (!_pools.ContainsKey(type))
            {
                _pools.Add(type, new Pool<Projectile>(10, 3, data.Prefab));
            }

            var projectile = _pools[type].Get(caster.transform.position + Vector3.up);
            projectile.Init(target, damage, data);
            _active.Add(projectile);
        }

        private void Tick()
        {
            for (int index = _active.Count - 1; index >= 0; index--)
            {
                var projectile = _active[index];
                
                projectile.Tick();
                if (projectile.Hited)
                {
                    _active.Remove(projectile);
                    _pools[projectile.Data.Id].Collect(projectile);
                }
            }
        }
    }
}