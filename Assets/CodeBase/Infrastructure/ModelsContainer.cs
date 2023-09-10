using System;
using System.Collections.Generic;
using CodeBase.Models;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class ModelsContainer
    {
        private static ModelsContainer _container;

        private static ModelsContainer Container =>
            _container ??= new ModelsContainer
            {
                binds = new Dictionary<Type, IModel>()
            };

        private Dictionary<Type, IModel> binds;

        private ModelsContainer()
        {
        }

        public static void Bind<T>(T service) where T : IModel
        {
            if (Container.binds.ContainsKey(typeof(T)))
            {
                Container.binds[typeof(T)] = service;
                Debug.LogError("Changed instance for type = " + typeof(T).Name);
            }

            Container.binds.Add(typeof(T), service);
        }

        public static T Resolve<T>() where T : class, IModel, new()
        {
            if (Container.binds.TryGetValue(typeof(T), out var model))
                return (T)model;
            
                    // TODO
            // else try load from save file
            
            return new T();
        }

        public static void Clear() =>
            _container = null;
    }
}