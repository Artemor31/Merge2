using Data;
using Databases;
using Gameplay.LevelItems;
using UnityEngine;

namespace Services.SaveService
{
    public class GridLogicService : IService
    {
        private readonly Vector2Int GridSize = new(3, 5);
        private readonly GridDataService _dataService;
        private readonly GridViewService _viewService;
        private readonly GameFactory _gameFactory;

        public GridLogicService(GridDataService dataService,
                                GridViewService viewService,
                                GameFactory gameFactory)
        {
            _dataService = dataService;
            _viewService = viewService;
            _gameFactory = gameFactory;
        }

        public void CreatePlayerField()
        {
            _gameFactory.CreateGridView();
            var platforms = _gameFactory.CreatePlatforms(GridSize);
            _dataService.InitPlatforms(platforms);

            for (int i = 0; i < platforms.GetLength(0); i++)
            {
                for (int j = 0; j < platforms.GetLength(1); j++)
                {
                    platforms[i, j].Init(_viewService, i, j);
                    var data = _dataService.ActorDataAt(i, j);
                    if (!data.Equals(ActorData.None))
                    {
                        _gameFactory.CreateActorAt(data, platforms[i, j]);
                    }
                }
            }
        }

        public bool CanAddUnit() => _dataService.HasFreePlatform(out Platform _);

        public bool TryCreatePlayerUnit(ActorConfig config)
        {
            if (_dataService.HasFreePlatform(out var platform))
            {
                _gameFactory.CreateActorAt(config.Data, platform);
                return true;
            }

            return false;
        }
    }
}