using System;
using System.Collections.Generic;
using System.Linq;
using Databases;
using Databases.Data;
using Gameplay.Grid;
using Gameplay.Units;
using Infrastructure;
using Services.Infrastructure;
using Services.Resources;
using UI.GameplayWindow;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Services.GridService
{
    public class GridViewService : IService
    {
        public event Action OnPlayerFieldChanged;
        public event Action<Platform> OnPlatformPressed;
        public event Action<Platform> OnPlatformReleased;
        public event Action<Platform> OnPlatformHovered;
        
        public GridView GridView { get; private set; }
        public List<Actor> PlayerUnits => _dataService.PlayerUnits;

        private readonly IUpdateable _updateable;
        private readonly GridDataService _dataService;
        private readonly LayerMask _platformMask;
        private readonly LayerMask _groundMask;
        private readonly CameraService _cameraService;
        private readonly GameFactory _gameFactory;
        private readonly GameplayDataService _gameplayService;
        private readonly PersistantDataService _persistantDataService;
        private readonly UnitsDatabase _unitsDatabase;
        
        
        private readonly List<RaycastResult> _results = new();
        private readonly PointerEventData _pointerData = new(EventSystem.current);
        private int _selected;
        private bool _dragging;
        private int _hovered;

        public GridViewService(IUpdateable updateable,
                               GridDataService dataService,
                               CameraService cameraService,
                               GameFactory gameFactory,
                               DatabaseProvider databaseProvider,
                               GameplayDataService gameplayService,
                               PersistantDataService persistantDataService)
        {
            _updateable = updateable;
            _dataService = dataService;
            _cameraService = cameraService;
            _gameFactory = gameFactory;
            _gameplayService = gameplayService;
            _persistantDataService = persistantDataService;
            _unitsDatabase = databaseProvider.GetDatabase<UnitsDatabase>();
            
            _platformMask = 1 << LayerMask.NameToLayer("Platform");
            _groundMask = 1 << LayerMask.NameToLayer("Ground");

            _updateable.Tick += OnTick;
        }
        
        public void CreatePlayerField()
        {
            GridView = _gameFactory.CreateGridView(_dataService.GridSize);

            int openedCount = _dataService.GridSize.x * _dataService.GridSize.y;
            
            _dataService.RestoreData(GridView.Platforms.GetRange(0, openedCount));
            for (int i = 0; i < openedCount; i++)
            {
                GridView.Platforms[i].Init(i);
                GridView.Platforms[i].gameObject.SetActive(true);
                
                ActorData data = _dataService.ActorDataAt(i);
                if (!data.Equals(ActorData.None))
                {
                    _gameFactory.CreatePlayerActor(data, GridView.Platforms[i]);
                }
            }

            OnPlayerFieldChanged?.Invoke();
        }

        public bool CanAddUnit() => _dataService.TryGetFreePlatform(out Platform _);
        public Platform GetPlatformFor(Actor actor) => GridView.PlatformWith(actor);

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
            _dataService.TryGetFreePlatform(out Platform platform);
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

        public int GetCostFor(int level)
        {
            if (level == 1) return 7;

            double value = Math.Pow(2, level - 1) * 7f;
            return (int)value;
        }

        public void SellUnitAt(int index)
        {
            Platform platform = GridView.Platforms[index];
            int costFor = GetCostFor(platform.Actor.Data.Level);
            _gameplayService.AddCrowns(costFor);
            platform.Clear();
        }

        public void Dispose()
        {
            _dataService.Dispose();
            
            if (!GridView) return;
            Object.Destroy(GridView.gameObject);
            GridView = null;
        }

        public void OnClicked(Platform platform)
        {
            if (platform.Busy)
            {
                _dragging = true;
                _selected = platform.Index;
                platform.Actor.Disable();
                OnPlatformPressed?.Invoke(platform);
                GridView.HighlightSame(platform);
            }
        }

        public void OnHovered(Platform gridData)
        {
            if (_dragging)
            {
                _hovered = gridData.Index;
                OnPlatformHovered?.Invoke(gridData);
            }
        }

        public void OnReleased(Platform started)
        {
            if (!_dragging) return;
            if (started == null) return;
            
            Platform ended = started;
            
            if (PointerUnderSellButton())
            {
                SellUnitAt(_selected);
            }
            else
            {
                ended = _dataService.GetPlatform(_hovered);

                if (ended.Free)
                {
                    MoveActor(started, ended);
                }
                else if (started.Index != ended.Index && 
                         started.Actor.Data == ended.Actor.Data &&
                         started.Actor.Data.Level != 3)
                {
                    Merge(started, ended);
                }
                else
                {
                    ResetActorPosition(started);
                }
            }

            _hovered = 0;
            _dragging = false;
            _selected = 0;
            OnPlatformReleased?.Invoke(ended);
        }

        private void OnTick()
        {
            if (!_dragging) return;
            
            var ray = _cameraService.TouchPointRay();
            IEnumerable<RaycastHit> hits = _cameraService.RayCast(ray, _platformMask);
            foreach (RaycastHit hit in hits)
            {
                if (_cameraService.CastPlane(hit.transform, ray, out float distance))
                {
                    _dataService.GetPlatform(_selected).Actor.transform.position = ray.GetPoint(distance);
                    return;
                }
            }

            hits = _cameraService.RayCast(ray, _groundMask);
            foreach (RaycastHit hit in hits)
            {
                if (_cameraService.CastPlane(hit.transform, ray, out float distance))
                {
                    _dataService.GetPlatform(_selected).Actor.transform.position = ray.GetPoint(distance);
                    return;
                }
            }
        }

        private void MoveActor(Platform started, Platform ended)
        {
            ended.Actor = started.Actor;
            started.Actor = null;
            ResetActorPosition(ended);
        }

        private bool PointerUnderSellButton()
        {
            _pointerData.position = Input.mousePosition;
            EventSystem.current.RaycastAll(_pointerData, _results);

            foreach (RaycastResult result in _results)
            {
                if (result.gameObject.TryGetComponent<SellButton>(out _))
                {
                    return true;
                }
            }

            return false;
        }

        private bool PointerUnderPlatform(out Platform platform)
        {
            var ray = _cameraService.TouchPointRay();

            foreach (RaycastHit hit in _cameraService.RayCast(ray, _platformMask))
            {
                if (hit.transform.TryGetComponent(out platform))
                {
                    return true;
                }

                if (hit.transform.TryGetComponent(out Actor actor))
                {
                    platform = GetPlatformFor(actor);
                    return true;
                }
            }
            
            platform = null;
            return false;
        }
        
        public void Merge(Platform started, Platform ended)
        {
            ActorData startData = started.Actor.Data;
            started.Clear();
            ended.Clear();

            startData.Level++;
            ActorConfig actorConfig = _unitsDatabase.ConfigFor(startData);
            TryCreatePlayerUnitAt(actorConfig, ended);
            _gameFactory.CreateMergeVFX(ended.transform.position);
        }

        private void ResetActorPosition(Platform data)
        {
            data.Actor.transform.position = data.transform.position;
            data.Actor.Enable();
        }
    }
}