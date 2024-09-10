using System.Linq;
using Data;
using Databases;
using Gameplay.Units;
using UnityEngine;

namespace Services
{
    public class MergeService : IService
    {
        private readonly GameFactory _gameFactory;
        private readonly UnitsDatabase _unitsDatabase;

        public MergeService(GameFactory gameFactory, DatabaseProvider databaseProvider)
        {
            _gameFactory = gameFactory;
            _unitsDatabase = databaseProvider.GetDatabase<UnitsDatabase>();
        }

        public bool TryMerge(GridRuntimeData started, GridRuntimeData ended)
        {
            if (started.Actor.Data.Level != ended.Actor.Data.Level) return false;

            var config = _unitsDatabase.Units.FirstOrDefault(u => u.Level == started.Actor.Data.Level + 1);
            if (config == null) return false;

            started.Actor.Dispose();
            Object.Destroy(started.Actor.gameObject);
            started.Actor = null;

            ended.Actor.Dispose();
            Object.Destroy(ended.Actor.gameObject);
            ended.Actor = null;

            Actor result = _gameFactory.CreateActor(config);
            result.transform.position = ended.Platform.transform.position;
            ended.Actor = result;

            return true;
        }
    }
}