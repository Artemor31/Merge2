using System;
using System.Collections.Generic;
using CodeBase.Databases;

namespace CodeBase.Services
{
    public class DatabaseProvider : IService
    {
        private readonly Dictionary<Type, Database> _databases;
        private readonly AssetsProvider _assetsProvider;

        public DatabaseProvider(AssetsProvider assetsProvider)
        {
            _assetsProvider = assetsProvider;
            _databases = new();

            var databases = _assetsProvider.LoadAll<Database>(AssetsPath.DatabasesFolder);
            foreach (var database in databases) 
                _databases.Add(database.GetType(), database);
        }

        public T GetDatabase<T>() where T : Database
        {
            foreach (var pair in _databases)
            {
                if (pair.Key == typeof(T))
                    return (T)pair.Value;
            }

            throw new Exception("DB not found");
        }
    }
}