using System;
using System.Collections.Generic;
using System.Linq;
using Databases;
using Gameplay.Grid;
using Gameplay.Units;
using Infrastructure;
using Services.DataServices;
using Services.Infrastructure;
using UI.GameplayWindow;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Services
{
    public class GridService : IService
    {
        public event Action OnPlayerFieldChanged;
        public event Action<Platform> OnPlatformPressed;
        public event Action<Platform> OnPlatformReleased;
        
        public GridView GridView { get; private set; }
        public List<Actor> PlayerUnits => _dataService.PlayerUnits;
        public bool CanAddUnit => _dataService.TryGetFreePlatform(out Platform _);

        private readonly IUpdateable _updateable;
        private readonly GridDataService _dataService;
        private readonly CameraService _cameraService;
        private readonly GameFactory _gameFactory;
        private readonly GameplayDataService _gameplayData;
        private readonly PersistantDataService _persistantData;
        private readonly UnitsDatabase _unitsDatabase;
        
        private int _selected;
        private bool _dragging;
        private int _hovered;

        public GridService(IUpdateable updateable, GridDataService dataService,
                               CameraService cameraService, GameFactory gameFactory,
                               DatabaseProvider databaseProvider, GameplayDataService gameplayData,
                               PersistantDataService persistantData)
        {
            _updateable = updateable;
            _dataService = dataService;
            _cameraService = cameraService;
            _gameFactory = gameFactory;
            _gameplayData = gameplayData;
            _persistantData = persistantData;
            _unitsDatabase = databaseProvider.GetDatabase<UnitsDatabase>();
            _updateable.Tick += OnTick;
        }
        
        public void CreatePlayerField()
        {
            GridView = _gameFactory.CreateGridView();
            int openedCount = _dataService.GridSize.x * _dataService.GridSize.y;
            _dataService.RestoreData(GridView.Platforms.GetRange(0, openedCount));
            for (int i = 0; i < openedCount; i++)
            {
                GridView.Platforms[i].Init(i);
                
                ActorData data = _dataService.ActorDataAt(i);
                if (!data.Equals(ActorData.None))
                {
                    _gameFactory.CreatePlayerActor(data, GridView.Platforms[i]);
                }
            }

            OnPlayerFieldChanged?.Invoke();
        }
        
        public void Dispose()
        {
            _dataService.Dispose();
            
            if (!GridView) return;
            Object.Destroy(GridView.gameObject);
            GridView = null;
        }

        public void TryCreatePlayerUnit(int tier)
        {
            ActorConfig actorConfig = _unitsDatabase.ConfigsFor(tier).Where(c => _persistantData.IsOpened(c.Data)).Random();
            TryCreatePlayerUnit(actorConfig);
        }

        public void TryCreatePlayerUnit(ActorConfig config)
        {
            _gameFactory.CreatePlayerActor(config.Data, _dataService.RandomPlatform());
            OnPlayerFieldChanged?.Invoke();
        }
        
        public void OnMouseDown(Platform platform)
        {
            if (platform.Busy)
            {
                _dragging = true;
                _selected = platform.Index;
                platform.Actor.Disable();
                GridView.SetState(ViewState.ShowSame, platform);
                GridView.SetState(ViewState.ShowAttackLine, platform);
                OnPlatformPressed?.Invoke(platform);
            }
        }

        public void OnMouseEnter(Platform platform)
        {
            if (_dragging)
            {
                _hovered = platform.Index;
                GridView.SetState(ViewState.ShowAttackLine, platform);
            }
        }

        public void OnMouseUp(Platform started)
        {
            if (!_dragging || started == null) return;
            
            
            Platform ended = started;
            if (_cameraService.PointerUnder<SellButton>())
            {
                Platform platform = GridView.Platforms[_selected];
                int costFor = _gameplayData.GetCostFor(platform.Actor.Data.Level);
                _gameplayData.AddCrowns(costFor);
                platform.Clear();
            }
            else
            {
                ended = _dataService.GetPlatform(_hovered);
                if (ended.Free)
                {
                    ended.Actor = started.Actor;
                    started.Actor = null;
                    ResetActorPosition(ended);
                }
                else if (started.Index != ended.Index && started.Actor.Data == ended.Actor.Data && started.Actor.Data.Level != 3)
                {
                    Merge(started, ended);
                }
                else
                {
                    ResetActorPosition(started);
                }
            }

            _hovered = 0;
            _selected = 0;
            _dragging = false;
            OnPlatformReleased?.Invoke(ended);
            GridView.SetState(ViewState.Normal, ended);
        }

        private void OnTick()
        {
            if (!_dragging) return;

            Vector3? pointOnGrid = _cameraService.GetPointOnGrid();
            if (pointOnGrid.HasValue)
            {
                _dataService.GetPlatform(_selected).Actor.transform.position = pointOnGrid.Value;
            }
        }

        private void Merge(Platform started, Platform ended)
        {
            ActorData startData = started.Actor.Data;
            startData.Level++;
            ActorConfig actorConfig = _unitsDatabase.ConfigFor(startData);
            
            started.Clear();
            ended.Clear();
            
            _gameFactory.CreatePlayerActor(actorConfig.Data, ended);
            _gameFactory.CreateMergeVFX(ended.transform.position);
            _dataService.Save();
            OnPlayerFieldChanged?.Invoke();
        }

        private void ResetActorPosition(Platform data)
        {
            data.Actor.transform.position = data.transform.position;
            data.Actor.Enable();
        }
    }
}