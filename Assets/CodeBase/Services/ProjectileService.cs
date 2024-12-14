using System;
using System.Collections.Generic;
using Gameplay.Units.Projectile;
using Infrastructure;
using Services.Infrastructure;
using UnityEngine;

namespace Services
{
    public class ProjectileService : IService
    {
        private readonly IUpdateable _updateable;
        private Dictionary<Type, Pool<Projectile>> _pools;

        public ProjectileService(IUpdateable updateable)
        {
            _updateable = updateable;
            _pools = new Dictionary<Type, Pool<Projectile>>();
        }
        
        public void Create<T>(Vector3 startPoint, Vector3 to) where T : Projectile
        {
            if (_pools.ContainsKey(typeof(T)))
            {
                //_pools[typeof(T)].Get()
            }
        }
    }
}