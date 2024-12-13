using System;
using System.Collections.Generic;
using Services;
using Services.Infrastructure;

namespace Infrastructure
{
    public class ServiceLocator
    {
        private static ServiceLocator _container;

        private static ServiceLocator Container =>
            _container ??= new ServiceLocator
            {
                Binds = new Dictionary<Type, IService>()
            };

        private Dictionary<Type, IService> Binds;

        private ServiceLocator()
        {
        }

        public static void Bind<T>(T service) where T : IService
        {
            if (Container.Binds.ContainsKey(typeof(T)))
                throw new Exception();

            Container.Binds.Add(typeof(T), service);
        }

        public static T Resolve<T>() where T : class, IService
        {
            if (Container.Binds.TryGetValue(typeof(T), out var model))
                return (T)model;
            throw new Exception($"type {typeof(T).Name} not binded");
        }

        public static void Clear() =>
            _container = null;
    }
}