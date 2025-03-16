using System;
using System.Collections.Generic;
using System.Linq;
using Databases;
using Databases.Data;
using Gameplay.Grid;
using Gameplay.Units;
using Infrastructure;
using Services.Infrastructure;
using Services.Resources;
using Object = UnityEngine.Object;

namespace Services.GridService
{
    public class GridLogicService : IService
    {
        public event Action OnPlayerFieldChanged;

        public GridView GridView { get; private set; }
        public List<Actor> PlayerUnits => _dataService.PlayerUnits;

        private readonly GridDataService _dataService;
        private readonly GameFactory _gameFactory;
        private readonly GameplayDataService _gameplayService;
        private readonly PersistantDataService _persistantDataService;
        private readonly UnitsDatabase _unitsDatabase;

        public GridLogicService(GridDataService dataService,
                                GameFactory gameFactory,
                                DatabaseProvider databaseProvider,
                                GameplayDataService gameplayService,
                                PersistantDataService persistantDataService)
        {
            _dataService = dataService;
            _gameFactory = gameFactory;
            _gameplayService = gameplayService;
            _persistantDataService = persistantDataService;
            _unitsDatabase = databaseProvider.GetDatabase<UnitsDatabase>();
        }

        public void CreatePlayerField()
        {
            GridView = _gameFactory.CreateGridView(_dataService.GridSize);

            int openedCount = _dataService.GridSize.x * _dataService.GridSize.y;
            
            _dataService.RestoreData(GridView.Platforms.GetRange(0, openedCount));
            for (int i = 0; i < openedCount; i++)
            {
                GridView.Platforms[i].Init(i);
                GridView.Platforms[i].gameObject.SetActive(true);
                
                ActorData data = _dataService.ActorDataAt(i);
                if (!data.Equals(ActorData.None))
                {
                    _gameFactory.CreatePlayerActor(data, GridView.Platforms[i]);
                }
            }

            OnPlayerFieldChanged?.Invoke();
        }

        public bool CanAddUnit() => _dataService.TryGetFreePlatform(out Platform _);
        public Platform GetPlatformFor(Actor actor) => GridView.PlatformWith(actor);

        public void TryCreatePlayerUnit(int tier)
        {
            var platform = _dataService.RandomPlatform();
            IEnumerable<ActorConfig> actorConfigs = _unitsDatabase.ConfigsFor(tier);
            var configs = actorConfigs.Where(c => _persistantDataService.IsOpened(c.Data.Mastery, c.Data.Race));

            _gameFactory.CreatePlayerActor(configs.Random().Data, platform);
            OnPlayerFieldChanged?.Invoke();
        }

        public void TryCreatePlayerUnit(ActorConfig unitCard)
        {
            _dataService.TryGetFreePlatform(out Platform platform);
            _gameFactory.CreatePlayerActor(unitCard.Data, platform);
            OnPlayerFieldChanged?.Invoke();
        }

        public bool TryCreatePlayerUnitAt(ActorConfig config, Platform platform)
        {
            if (platform.Busy) return false;
            _gameFactory.CreatePlayerActor(config.Data, platform);
            _dataService.Save();
            OnPlayerFieldChanged?.Invoke();
            return true;
        }

        public int GetCostFor(int level)
        {
            if (level == 1) return 7;

            double value = Math.Pow(2, level - 1) * 7f;
            return (int)value;
        }

        public void SellUnitAt(int index)
        {
            Platform platform = GridView.Platforms[index];
            int costFor = GetCostFor(platform.Actor.Data.Level);
            _gameplayService.AddCrowns(costFor);
            platform.Clear();
        }

        public void Dispose()
        {
            _dataService.Dispose();
            
            if (!GridView) return;
            Object.Destroy(GridView.gameObject);
            GridView = null;
        }
    }
}