using Data;
using Databases;
using Gameplay.LevelItems;
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

        public bool TryMerge(Platform started, Platform ended)
        {
            ActorData startData = started.Actor.Data;
            ActorData endedData = ended.Actor.Data;

            if (startData != endedData) return false;
            
            startData.Level++;
            var config = _unitsDatabase.ConfigFor(startData);
            if (config == null) return false;

            started.Actor.Dispose();
            Object.Destroy(started.Actor.gameObject);
            started.Actor = null;

            ended.Actor.Dispose();
            Object.Destroy(ended.Actor.gameObject);
            ended.Actor = null;

            Actor result = _gameFactory.CreateActor(config.Data, ended.transform.position);
            ended.Actor = result;

            return true;

        }
    }
}