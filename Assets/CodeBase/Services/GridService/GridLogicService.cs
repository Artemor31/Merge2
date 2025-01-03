﻿using System;
using Databases;
using Databases.Data;
using Gameplay.Grid;
using Services.Infrastructure;
using Services.Resources;
using UnityEngine;

namespace Services.GridService
{
    public class GridLogicService : IService
    {
        public event Action OnPlayerFieldChanged;
        
        private readonly Vector2Int _gridSize = new(3, 5);
        private readonly GridDataService _dataService;
        private readonly GameFactory _gameFactory;
        private readonly GameplayDataService _gameplayService;
        private readonly UnitsDatabase _unitsDatabase;


        public GridLogicService(GridDataService dataService,
                                GameFactory gameFactory,
                                DatabaseProvider databaseProvider, 
                                GameplayDataService gameplayService)
        {
            _dataService = dataService;
            _gameFactory = gameFactory;
            _gameplayService = gameplayService;
            _unitsDatabase = databaseProvider.GetDatabase<UnitsDatabase>();
        }

        public void CreatePlayerField()
        {
            var platforms = _gameFactory.CreatePlatforms(_gridSize);
            _gameFactory.CreateGridView();
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

        public bool TryCreatePlayerUnit(ActorConfig config)
        {
            if (_dataService.HasFreePlatform(out var platform))
            {
                _gameFactory.CreatePlayerActor(config.Data, platform);
                OnPlayerFieldChanged?.Invoke();
                return true;
            }

            return false;
        }

        public bool TryCreatePlayerUnitAt(ActorConfig config, Platform platform)
        {
            if (platform.Busy) return false;
            _gameFactory.CreatePlayerActor(config.Data, platform);
            OnPlayerFieldChanged?.Invoke();
            return true;
        }

        public void SellUnitAt(Platform platform)
        {
            ActorConfig actorConfig = _unitsDatabase.ConfigFor(platform.Actor.Data);
            _gameplayService.AddMoney(actorConfig.Cost / 2);
            platform.Clear();
        }
    }
}