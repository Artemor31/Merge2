using System;
using Data;
using Databases;
using Gameplay.LevelItems;
using UnityEngine;

namespace Services.SaveService
{
    public class GridLogicService : IService
    {
        public event Action OnPlayerFieldChanged;
        
        private readonly Vector2Int _gridSize = new(3, 5);
        private readonly GridDataService _dataService;
        private readonly GameFactory _gameFactory;


        public GridLogicService(GridDataService dataService, GameFactory gameFactory)
        {
            _dataService = dataService;
            _gameFactory = gameFactory;
        }

        public void CreatePlayerField()
        {
            _gameFactory.CreateGridView();
            var platforms = _gameFactory.CreatePlatforms(_gridSize);
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
        }

        public bool CanAddUnit() => _dataService.HasFreePlatform(out Platform _);

        public bool TryCreatePlayerUnit(ActorConfig config)
        {
            if (_dataService.HasFreePlatform(out var platform))
            {
                _gameFactory.CreatePlayerActor(config.Data, platform);
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
    }
}