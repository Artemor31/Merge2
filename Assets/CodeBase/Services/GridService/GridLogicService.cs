using System;
using System.Collections.Generic;
using System.Linq;
using Databases;
using Databases.Data;
using Gameplay.Grid;
using Infrastructure;
using Services.Infrastructure;
using Services.Resources;
using UnityEngine;

namespace Services.GridService
{
    public class GridLogicService : IService
    {
        public event Action OnPlayerFieldChanged;

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
            var gridView = _gameFactory.CreateGridView();
            var platforms = gridView.Platforms; 
            _dataService.InitPlatforms(platforms);

            for (int i = 0; i < platforms.GetLength(0); i++)
            {
                for (int j = 0; j < platforms.GetLength(1); j++)
                {
                    platforms[i, j].Init(i, j);
                    var data = _dataService.ActorDataAt(i, j);
                    if (!data.Equals(ActorData.None))
                    {
                        _gameFactory.CreatePlayerActor(data, platforms[i, j]);
                    }
                }
            }

            OnPlayerFieldChanged?.Invoke();
        }

        public bool CanAddUnit() => _dataService.HasFreePlatform(out Platform _);

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
            _dataService.HasFreePlatform(out Platform platform);
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

        public void SellUnitAt(Vector2Int selected)
        {
            var platform = _dataService.GetDataAt(selected);
            int level = platform.Actor.Data.Level;

            if (level == 1)
            {
                _gameplayService.AddCrowns(7);
            }
            else
            {
                double value = Math.Pow(2, level - 1) * 7f;
                _gameplayService.AddCrowns((int)value);
            }

            platform.Clear();
        }
    }
}