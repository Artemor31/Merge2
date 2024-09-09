using System;
using Gameplay.LevelItems;
using Services.SaveService;
using UnityEngine;
using UnityEngine.AI;

namespace Services
{
    public class GridViewService : IService
    {
        private readonly IUpdateable _updateable;
        private readonly GridDataService _service;
        private readonly LayerMask _platformMask;
        private readonly RaycastHit[] _hits;
        private readonly MergeService _mergeService;
        private readonly CameraService _cameraService;

        private bool _dragging;
        private Vector2 _size;
        private Vector2Int _selected;

        public event Action<GridRuntimeData> OnPlatformClicked;
        public event Action<GridRuntimeData> OnPlatformReleased;
        public event Action<GridRuntimeData> OnPlatformHovered;

        public GridViewService(IUpdateable updateable, 
                           GridDataService service, 
                           MergeService mergeService,
                           CameraService cameraService)
        {
            _updateable = updateable;
            _service = service;
            _mergeService = mergeService;
            _cameraService = cameraService;
            _hits = new RaycastHit[3];
            _platformMask = 1 << LayerMask.NameToLayer("Platform");

            _updateable.Tick += OnTick;
            _service.OnPlatformClicked += OnClicked;
            _service.OnPlatformHovered += OnHovered;
            _service.OnPlatformReleased += OnReleased;
        }

        private void OnHovered(GridRuntimeData gridData)
        {
            if (!_dragging) return;
            OnPlatformHovered?.Invoke(gridData);
        }

        private void OnReleased(GridRuntimeData started)
        {
            if (!_dragging || !RaycastPlatform(out Platform platform)) return;

            GridRuntimeData ended = _service.GetDataAt(platform.Index);
            
            if (started.Index == ended.Index)
            {
                ResetActorPosition(started);
            }
            else if (ended.Busy == false)
            {
                MoveActor(started, ended);
            }
            else
            {
                TryMerge(started, ended);
            }

            _dragging = false;
            _selected = Vector2Int.zero;
            OnPlatformReleased?.Invoke(ended);
        }

        private void TryMerge( GridRuntimeData started, GridRuntimeData ended)
        {
            if (_mergeService.TryMerge(started.Actor, ended.Actor, out var newActor))
            {
     
            }
            else
            {
                ResetActorPosition(started);
            }
        }

        private void MoveActor(GridRuntimeData started, GridRuntimeData ended)
        {
            ended.Actor = started.Actor;
            started.Actor = null;
            ResetActorPosition(ended);
            ended.Actor.GetComponent<NavMeshAgent>().enabled = true;
        }

        private void OnClicked(GridRuntimeData gridData)
        {
            if (gridData.Busy == false) return;

            _dragging = true;
            _selected = gridData.Index;
            gridData.Actor.GetComponent<NavMeshAgent>().enabled = false;
            OnPlatformClicked?.Invoke(gridData);
        }

        private void OnTick()
        {
            if (!_dragging) return;

            Ray ray = _cameraService.TouchPointRay();
            Physics.RaycastNonAlloc(ray, _hits, 1000, _platformMask);

            foreach (RaycastHit hit in _hits)
            {
                if (hit.transform.TryGetComponent(out Platform platform) && CastPlane(platform, ray, out var distance))
                {
                    Vector3 point = ray.GetPoint(distance);
                    _service.GetDataAt(_selected).Actor.transform.position = point;
                    break;
                }
            }
        }
        
        private bool RaycastPlatform(out Platform platform)
        {
            Physics.RaycastNonAlloc(_cameraService.TouchPointRay(), _hits, 1000, _platformMask);
            foreach (RaycastHit hit in _hits)
            {
                if (hit.transform.TryGetComponent(out platform))
                {
                    return true;
                }
            }

            platform = null;
            return false;
        }

        private bool CastPlane(Platform platform, Ray ray, out float distance) =>
            new Plane(Vector3.up, platform.transform.position).Raycast(ray, out distance);

        private void ResetActorPosition(GridRuntimeData data)
        {
            
            data.Actor.transform.position = data.Platform.transform.position;
        }
    }
}