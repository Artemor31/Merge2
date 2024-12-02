using Data;
using Databases;
using Gameplay.LevelItems;
using Services.SaveService;

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
            if (startData == ended.Actor.Data)
            {
                startData.Level++;

                var config = _unitsDatabase.ConfigFor(startData);
                if (config != null)
                {
                    started.Clear();
                    ended.Clear();

                    _gridService.TryCreatePlayerUnitAt(config, ended);
                    return true;
                }
            }

            return false;
        }
    }
}