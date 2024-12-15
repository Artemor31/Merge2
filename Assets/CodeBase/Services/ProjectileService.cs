using System;
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
        private readonly Dictionary<Type, Pool<Projectile>> _pools = new();
        private readonly List<Projectile> _active = new();
        private readonly ProjectilesDatabase _database;

        public ProjectileService(IUpdateable updateable, DatabaseProvider provider)
        {
            updateable.Tick += Tick;
            _database = provider.GetDatabase<ProjectilesDatabase>();
        }
        
        public void Create<T>(Actor caster, Actor target, float damage) where T : Projectile
        {
            if (!_pools.ContainsKey(typeof(T)))
            {
                _pools.Add(typeof(T), new Pool<Projectile>(10, 3, _database.Get<T>().Prefab));
            }

            var projectile = _pools[typeof(T)].Get();
            projectile.Init(caster, target, damage);
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
                }
            }
        }
    }
}