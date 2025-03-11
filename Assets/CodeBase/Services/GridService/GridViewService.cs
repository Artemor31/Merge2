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
        private int _selected;
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

        private int _hovered;
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
            if (Selling)
            {
                _gridLogicService.SellUnitAt(_selected);
            }
            else if(true)// (PointerUnderPlatform(out Platform platform))
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