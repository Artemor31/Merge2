using System.Collections.Generic;
using System.Linq;
using Gameplay.Units;
using Infrastructure;
using NaughtyAttributes.Core.DrawerAttributes_SpecialCase;
using Services;
using Services.GridService;
using UnityEngine;

namespace Gameplay.Grid
{
    public class GridView : MonoBehaviour
    {
        public List<Platform> Platforms => _platforms;
        [SerializeField] private List<Platform> _platforms;
        [SerializeField] private Platform _selectPlatform;
        [SerializeField] private List<GameObject> _gridDivs;
        [SerializeField] private Collider _collider;

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
        }
        
        public void Enable(bool enable)
        {
            _collider.enabled = enable;
            foreach (var platform in _platforms)
            {
                platform.Collider.enabled = enable;
            }
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

        private void SetSelected(int index)
        {
            _selectPlatform.transform.position = _platforms[index].transform.position;
            _gameplayContainer.Get<EnemyGrid>().Highlinght(index % _gridDataService.GridSize.y);
        }
    }
}