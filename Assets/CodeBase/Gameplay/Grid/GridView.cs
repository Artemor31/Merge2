using System;
using System.Collections.Generic;
using Infrastructure;
using Services;
using Services.DataServices;
using UnityEngine;

namespace Gameplay.Grid
{
    public class GridView : MonoBehaviour
    {
        public List<Platform> Platforms => _platforms;
        public List<Platform> StashPlatforms => _bufferPlatforms;

        [SerializeField] private List<Platform> _platforms;
        [SerializeField] private Platform _selectPlatform;
        [SerializeField] private Collider _collider;
        [SerializeField] private GameObject _bufferParent;
        [SerializeField] private List<Platform> _bufferPlatforms;

        private GridDataService _gridDataService;
        private GameplayContainer _gameplayContainer;
        
        public void Enable(bool enable) => gameObject.SetActive(enable);

        public void Init(int count)
        {
            _gridDataService = ServiceLocator.Resolve<GridDataService>();
            _gameplayContainer = ServiceLocator.Resolve<GameplayContainer>();
            
            for (int i = 0; i < count; i++)
            {
                _platforms[i].gameObject.SetActive(true);
            }
        }
        
        public void SetState(ViewState state, Platform platform)
        {
            switch (state)
            {
                case ViewState.Normal:
                {
                    //_bufferParent.SetActive(false);
                    _gameplayContainer.Get<EnemyGrid>().Disable();
                    foreach (Platform item in _platforms)
                    {
                        item.SetViewState(ViewState.Normal);
                    }

                    break;
                }
                case ViewState.ShowSame:
                {
                    //_bufferParent.SetActive(true);
                    foreach (Platform item in _platforms)
                    {
                        if (item.Data == platform.Data && item != platform)
                        {
                            item.SetViewState(ViewState.ShowSame);
                        }
                    }

                    break;
                }
                case ViewState.ShowAttackLine:
                    _gameplayContainer.Get<EnemyGrid>().Highlinght(platform.Index % _gridDataService.GridSize.y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}