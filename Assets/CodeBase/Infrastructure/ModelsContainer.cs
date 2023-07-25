using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure
{
    public class ModelsContainer
    {
        private static ModelsContainer _container;

        private static ModelsContainer Container =>
            _container ??= new ModelsContainer
            {
                Binds = new Dictionary<Type, IModel>()
            };

        private Dictionary<Type, IModel> Binds;

        private ModelsContainer()
        {
        }

        public static void Bind<T>(T service) where T : IModel
        {
            if (Container.Binds.ContainsKey(typeof(T)))
                throw new Exception();

            Container.Binds.Add(typeof(T), service);
        }

        public static T Resolve<T>() where T : class, IModel
        {
            if (Container.Binds.TryGetValue(typeof(T), out var model))
                return (T)model;
            return null;
        }

        public static void Clear() =>
            _container = null;
    }
}