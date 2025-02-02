using Databases;
using Databases.Data;
using Gameplay.Grid;
using Services.GridService;
using Services.Infrastructure;
using Services.Resources;

namespace Services
{
    public class MergeService : IService
    {
        private readonly GridLogicService _gridService;
        private readonly UnitsDatabase _unitsDatabase;
        public bool CanMerge { get; set; }

        public MergeService(DatabaseProvider databaseProvider, GridLogicService gridService)
        {
            _gridService = gridService;
            _unitsDatabase = databaseProvider.GetDatabase<UnitsDatabase>();
        }

        public void Merge(Platform started, Platform ended)
        {
            if (CanMerge == false) return;
            
            ActorData startData = started.Actor.Data;
            started.Clear();
            ended.Clear();

            startData.Level++;
            ActorConfig actorConfig = _unitsDatabase.ConfigFor(startData);
            _gridService.TryCreatePlayerUnitAt(actorConfig, ended);
        }
    }
}