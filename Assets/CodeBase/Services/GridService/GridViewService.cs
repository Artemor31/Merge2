using System;
using System.Collections.Generic;
using Gameplay.Grid;
using Gameplay.Units;
using Services.Infrastructure;
using UnityEngine;
using UnityEngine.AI;

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
        private readonly LayerMask _uiMask;
        private readonly MergeService _mergeService;
        private readonly CameraService _cameraService;
        private readonly GridLogicService _gridLogicService;
        private readonly GridDataService _gridDataService;
        private Vector2Int _selected;
        private bool _dragging;

        public GridViewService(IUpdateable updateable,
                               GridDataService dataService,
                               MergeService mergeService,
                               CameraService cameraService,
                               GridLogicService gridLogicService, 
                               GridDataService gridDataService)
        {
            _updateable = updateable;
            _dataService = dataService;
            _mergeService = mergeService;
            _cameraService = cameraService;
            _gridLogicService = gridLogicService;
            _gridDataService = gridDataService;
            _platformMask = 1 << LayerMask.NameToLayer("Platform");
            _groundMask = 1 << LayerMask.NameToLayer("Ground");
            _uiMask = 1 << LayerMask.NameToLayer("UI");

            _updateable.Tick += OnTick;
        }

        public void OnClicked(Platform platform)
        {
            if (platform.Busy)
            {
                _dragging = true;
                _selected = platform.Index;
                platform.Actor.GetComponent<NavMeshAgent>().enabled = false;
                platform.Actor.GetComponent<BoxCollider>().enabled = false;
                OnPlatformPressed?.Invoke(platform);
            }
        }

        public void OnClicked(Actor actor)
        {
            if (_gridDataService.PlayerUnits.Contains(actor))
            {
                Platform platform = _gridLogicService.GetPlatformFor(actor);
                _dragging = true;
                _selected = platform.Index;
                actor.GetComponent<NavMeshAgent>().enabled = false;
                actor.GetComponent<BoxCollider>().enabled = false;
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
        
        public void OnReleased(Actor actor) => OnReleased(_gridLogicService.GetPlatformFor(actor));
        
        public void OnReleased(Platform started)
        {
            if (!_dragging) return;

            if (Selling)
            {
                _gridLogicService.SellUnitAt(_selected);
                _dragging = false;
                _selected = Vector2Int.zero;
                OnPlatformReleased?.Invoke(started);
                return;
            }
            
            Platform ended;
            if (RaycastPlatform(out Platform platform))
            {
                ended = _dataService.GetDataAt(platform.Index);

                if (started.Index == ended.Index)
                {
                    ResetActorPosition(started);
                }
                else if (ended.Free)
                {
                    MoveActor(started, ended);
                }
                else
                {
                    if (!_mergeService.TryMerge(started, ended))
                    {
                        ResetActorPosition(started);
                    }
                }
            }
            else
            {
                ResetActorPosition(started);
                ended = started;
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
            ended.Actor.GetComponent<NavMeshAgent>().enabled = true;
            ended.Actor.GetComponent<BoxCollider>().enabled = true;
        }

        private bool RaycastPlatform(out Platform platform)
        {
            var ray = _cameraService.TouchPointRay();
            foreach (RaycastHit hit in _cameraService.RayCast(ray, _platformMask))
            {
                if (hit.transform.TryGetComponent(out platform))
                {
                    return true;
                }
            }

            platform = null;
            return false;
        }

        private void ResetActorPosition(Platform data) =>
            data.Actor.transform.position = data.transform.position;
    }
}