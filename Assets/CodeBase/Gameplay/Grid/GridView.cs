using System;
using System.Collections.Generic;
using System.Linq;
using Databases.Data;
using Gameplay.Units;
using Infrastructure;
using NaughtyAttributes.Core.DrawerAttributes_SpecialCase;
using Services;
using Services.GridServices;
using UnityEngine;

namespace Gameplay.Grid
{
    public class GridView : MonoBehaviour
    {
        public List<Platform> Platforms => _platforms;
        [SerializeField] private List<Platform> _platforms;
        [SerializeField] private Platform _selectPlatform;
        [SerializeField] private Collider _collider;
        [SerializeField] private List<Platform> _bufferPlatforms;

        private GridDataService _gridDataService;
        private GameplayContainer _gameplayContainer;

        public void Init(Vector2Int size)
        {
            _gridDataService = ServiceLocator.Resolve<GridDataService>();
            _gameplayContainer = ServiceLocator.Resolve<GameplayContainer>();
        }

        public void Enable(bool enable) => gameObject.SetActive(enable);

        public void SetState(ViewState state, Platform platform)
        {
            switch (state)
            {
                case ViewState.Normal:
                {
                    _gameplayContainer.Get<EnemyGrid>().Disable();
                    foreach (Platform item in _platforms)
                    {
                        item.SetViewState(ViewState.Normal);
                    }

                    break;
                }
                case ViewState.ShowSame:
                {
                    foreach (Platform item in _platforms)
                    {
                        if (item.Actor?.Data == platform.Actor?.Data && item != platform)
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