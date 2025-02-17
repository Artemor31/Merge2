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
        private readonly GameFactory _gameFactory;
        private readonly UnitsDatabase _unitsDatabase;

        public MergeService(DatabaseProvider databaseProvider, GridLogicService gridService, GameFactory gameFactory)
        {
            _gridService = gridService;
            _gameFactory = gameFactory;
            _unitsDatabase = databaseProvider.GetDatabase<UnitsDatabase>();
        }

        public void Merge(Platform started, Platform ended)
        {
            ActorData startData = started.Actor.Data;
            started.Clear();
            ended.Clear();

            startData.Level++;
            ActorConfig actorConfig = _unitsDatabase.ConfigFor(startData);
            _gridService.TryCreatePlayerUnitAt(actorConfig, ended);
            _gameFactory.CreateMergeVFX(ended.transform.position);
        }
    }
}