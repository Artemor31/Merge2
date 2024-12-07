using System;
using System.Collections.Generic;
using Databases;

namespace Services
{
    public class DatabaseProvider : IService
    {
        private readonly Dictionary<Type, Database> _databases;

        public DatabaseProvider(AssetsProvider assetsProvider)
        {
            _databases = new();

            var databases = assetsProvider.LoadAll<Database>(AssetsPath.DatabasesFolder);
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