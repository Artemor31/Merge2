using System.Collections.Generic;
using System.Linq;
using Gameplay.Units;
using Infrastructure;
using Services;
using Services.GridService;
using UnityEngine;

namespace Gameplay.Grid
{
    public class GridView : MonoBehaviour
    {
        [SerializeField] private List<Platform> _platforms;
        [SerializeField] private Platform _selectPlatform;
        [SerializeField] private List<GameObject> _gridDivs;

        private GridViewService _gridViewService;
        private GridDataService _gridDataService;
        private GameplayContainer _gameplayContainer;

        public void Init(Vector2Int size)
        {
            _gridViewService = ServiceLocator.Resolve<GridViewService>();
            _gridDataService = ServiceLocator.Resolve<GridDataService>();
            _gameplayContainer = ServiceLocator.Resolve<GameplayContainer>();
            _gridViewService.OnPlatformPressed += PlatformOnOnPressed;
            _gridViewService.OnPlatformHovered += PlatformOnOnHovered;
            _gridViewService.OnPlatformReleased += PlatformOnOnReleased;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    _platforms[x * size.y + y].gameObject.SetActive(true);
                }
                _gridDivs[x].gameObject.SetActive(true);
            }

            for (int y = 0; y < size.x; y++)
            {
            }
        }

        public Platform[,] GetPlatforms()
        {
            int gridSizeX = _gridDataService.GridSize.x;
            int gridSizeY = _gridDataService.GridSize.y;
            var array = new Platform[gridSizeX, gridSizeY];

            for (int i = 0; i < gridSizeX; i++)
            {
                
                for (int j = 0; j < gridSizeY; j++)
                {
                    array[i, j] = _platforms[i * gridSizeY + j];
                }
            }

            return array;
        }

        public Platform PlatformWith(Actor actor) => _platforms.FirstOrDefault(p => p.Actor == actor);
        private void PlatformOnOnReleased(Platform ended) => IsHighlighted(false);
        private void PlatformOnOnHovered(Platform gridData) => SetSelected(gridData.Index);

        private void OnDisable()
        {
            _gridViewService.OnPlatformPressed -= PlatformOnOnPressed;
            _gridViewService.OnPlatformHovered -= PlatformOnOnHovered;
            _gridViewService.OnPlatformReleased -= PlatformOnOnReleased;
        }

        private void PlatformOnOnPressed(Platform platform)
        {
            SetSelected(platform.Index);
            IsHighlighted(true);
        }
        
        private void IsHighlighted(bool active)
        {
            _selectPlatform.gameObject.SetActive(active);
            _gameplayContainer.Get<EnemyGrid>().Disable();
        }

        private void SetSelected(Vector2Int position)
        {
            int index = position.y + position.x * _gridDataService.GridSize.y;
            _selectPlatform.transform.position = _platforms[index].transform.position;
            _gameplayContainer.Get<EnemyGrid>().Highlinght(position.y);
        }
    }
}