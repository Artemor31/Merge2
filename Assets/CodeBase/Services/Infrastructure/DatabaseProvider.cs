using System;
using System.Collections.Generic;
using Databases;

namespace Services.Infrastructure
{
    public class DatabaseProvider : IService
    {
        private readonly Dictionary<Type, Database> _databases;

        public DatabaseProvider(List<Database> databases)
        {
            _databases = new Dictionary<Type, Database>();
            foreach (var database in databases)
            {
                database.Cache();
                _databases.Add(database.GetType(), database);
            }
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