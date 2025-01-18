using System.Collections.Generic;
using System.Linq;
using Gameplay.Units;
using Infrastructure;
using Services.GridService;
using UnityEngine;

namespace Gameplay.Grid
{
    public class GridView : MonoBehaviour
    {
        [SerializeField] private List<Platform> _platforms;
        [SerializeField] private Platform _selectPlatform;

        private GridViewService _gridViewService;
        private GridDataService _gridDataService;

        public Platform[,] GetPlatforms()
        {
            int gridSizeX = _gridDataService.GridSize.x;
            int gridSizeY = _gridDataService.GridSize.y;
            Platform[,] array = new Platform[gridSizeX, gridSizeY];

            for (int i = 0; i < gridSizeX; i++)
            {
                for (int j = 0; j < gridSizeY; j++)
                {
                    array[i, j] = _platforms[i * gridSizeY + j];
                }
            }

            return array;
        }

        public void Init()
        {
            _gridViewService = ServiceLocator.Resolve<GridViewService>();
            _gridDataService = ServiceLocator.Resolve<GridDataService>();
            _gridViewService.OnPlatformPressed += PlatformOnOnPressed;
            _gridViewService.OnPlatformHovered += PlatformOnOnHovered;
            _gridViewService.OnPlatformReleased += PlatformOnOnReleased;
        }
        
        public Platform PlatformWith(Actor actor) => _platforms.First(p => p.Actor == actor);
        
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

        private void PlatformOnOnReleased(Platform ended) => IsHighlighted(false);
        private void PlatformOnOnHovered(Platform gridData) => SetSelected(gridData.Index);
        private void IsHighlighted(bool active) => _selectPlatform.gameObject.SetActive(active);

        private void SetSelected(Vector2Int position)
        {
            int index = position.y + position.x * _gridDataService.GridSize.y;
            _selectPlatform.transform.position = _platforms[index].transform.position;
        }
    }
}