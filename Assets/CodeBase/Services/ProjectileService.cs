using System.Collections.Generic;
using Databases;
using Gameplay.Units;
using Gameplay.Units.Healths;
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
        
        public void Create(ProjectileType type,  Vector3 position, Actor target, float damage, HealthContext context)
        {
            ProjectileData data = _database.Get(type);
            if (!_pools.ContainsKey(type))
            {
                _pools.Add(type, new Pool<Projectile>(25, 7, data.Prefab));
            }

            var projectile = _pools[type].Get(position);
            projectile.Init(target, damage, data, context);
            _active.Add(projectile);
        }

        public void ClearField()
        {
            foreach (Projectile projectile in _active)
            {   
                _pools[projectile.Data.Id].ToPool(projectile);
            }
            
            _active.Clear();
        }

        private void Tick()
        {
            for (int index = _active.Count - 1; index >= 0; index--)
            {
                if (_active.Count <= index) return;
                
                var projectile = _active[index];
                
                projectile.Tick();
                if (projectile.Hited)
                {
                    _active.Remove(projectile);
                    _pools[projectile.Data.Id].ToPool(projectile);
                }
            }
        }
    }
}