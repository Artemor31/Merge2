using Data;
using Databases;
using Gameplay.LevelItems;
using Services.SaveService;
using UnityEngine;

namespace Services
{
    public class MergeService : IService
    {
        private readonly GridLogicService _gridService;
        private readonly UnitsDatabase _unitsDatabase;

        public MergeService(DatabaseProvider databaseProvider, GridLogicService gridService)
        {
            _gridService = gridService;
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

            _gridService.TryCreatePlayerUnitAt(config, ended);

            return true;

        }
    }
}