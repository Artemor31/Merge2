using System;
using System.Collections.Generic;

namespace Services.SaveService
{
    public class RepositoryProvider : IService
    {
        private readonly Dictionary<Type, IRepository> _repos = new()
        {
            {typeof(GridRepository), new GridRepository()},
            {typeof(PlayerDataRepository), new PlayerDataRepository()},
        };

        public TRepo GetRepo<TRepo>() where TRepo : IRepository => _repos.ContainsKey(typeof(TRepo))
            ? (TRepo)_repos[typeof(TRepo)]
            : throw new KeyNotFoundException();
    }
}