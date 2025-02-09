using System;
using System.Collections.Generic;
using Gameplay.Grid;
using Gameplay.Units;
using Services.Infrastructure;
using UnityEngine;

namespace Services.GridService
{
    public class GridViewService : IService
    {
        public bool Selling { get; set; }

        public event Action<Platform> OnPlatformPressed;
        public event Action<Platform> OnPlatformReleased;
        public event Action<Platform> OnPlatformHovered;

        private readonly IUpdateable _updateable;
        private readonly GridDataService _dataService;
        private readonly LayerMask _platformMask;
        private readonly LayerMask _groundMask;
        private readonly MergeService _mergeService;
        private readonly CameraService _cameraService;
        private readonly GridLogicService _gridLogicService;
        private Vector2Int _selected;
        private bool _dragging;

        public GridViewService(IUpdateable updateable,
                               GridDataService dataService,
                               MergeService mergeService,
                               CameraService cameraService,
                               GridLogicService gridLogicService)
        {
            _updateable = updateable;
            _dataService = dataService;
            _mergeService = mergeService;
            _cameraService = cameraService;
            _gridLogicService = gridLogicService;
            _platformMask = 1 << LayerMask.NameToLayer("Platform");
            _groundMask = 1 << LayerMask.NameToLayer("Ground");

            _updateable.Tick += OnTick;
        }

        public void OnClicked(Actor actor)
        {
            if (_dataService.PlayerUnits.Contains(actor)) 
                OnClicked(_gridLogicService.GetPlatformFor(actor));
        }

        public void OnClicked(Platform platform)
        {
            if (platform.Busy)
            {
                _dragging = true;
                _selected = platform.Index;
                platform.Actor.Disable();
                OnPlatformPressed?.Invoke(platform);
            }
        }

        public void OnHovered(Platform gridData)
        {
            if (_dragging)
            {
                OnPlatformHovered?.Invoke(gridData);
            }
        }

        public void OnHovered(Actor actor)
        {
            if (_dataService.PlayerUnits.Contains(actor))
                OnHovered(_gridLogicService.GetPlatformFor(actor));
        }

        public void OnReleased(Actor actor)
        {
            if (_dataService.PlayerUnits.Contains(actor))
                OnReleased(_gridLogicService.GetPlatformFor(actor));
        }

        public void OnReleased(Platform started)
        {
            if (!_dragging) return;
            if (started == null) return;

            Platform ended = started;
            if (Selling)
            {
                _gridLogicService.SellUnitAt(_selected);
            }
            else if (PointerUnderPlatform(out Platform platform))
            {
                ended = _dataService.GetDataAt(platform.Index);

                if (ended.Free)
                {
                    MoveActor(started, ended);
                }
                else if (started.Index != ended.Index && 
                         started.Actor.Data == ended.Actor.Data &&
                         started.Actor.Data.Level != 3)
                {
                    _mergeService.Merge(started, ended);
                }
                else
                {
                    ResetActorPosition(started);
                }
            }
            else
            {
                ResetActorPosition(started);
            }

            _dragging = false;
            _selected = Vector2Int.zero;
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
                    _dataService.GetDataAt(_selected).Actor.transform.position = ray.GetPoint(distance);
                    return;
                }
            }

            hits = _cameraService.RayCast(ray, _groundMask);
            foreach (RaycastHit hit in hits)
            {
                if (_cameraService.CastPlane(hit.transform, ray, out float distance))
                {
                    _dataService.GetDataAt(_selected).Actor.transform.position = ray.GetPoint(distance);
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
                    platform = _gridLogicService.GetPlatformFor(actor);
                    return true;
                }
            }
            
            platform = null;
            return false;
        }

        private void ResetActorPosition(Platform data)
        {
            data.Actor.transform.position = data.transform.position;
            data.Actor.Enable();
        }
    }
}