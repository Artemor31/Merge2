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
        private readonly GridDataService _dataService;
        private readonly LayerMask _platformMask;
        private readonly RaycastHit[] _hits;
        private readonly MergeService _mergeService;
        private readonly CameraService _cameraService;

        private bool _dragging;
        private Vector2 _size;
        private Vector2Int _selected;

        public event Action<Platform> OnPlatformClicked;
        public event Action<Platform> OnPlatformReleased;
        public event Action<Platform> OnPlatformHovered;

        public GridViewService(IUpdateable updateable,
                               GridDataService dataService,
                               MergeService mergeService,
                               CameraService cameraService)
        {
            _updateable = updateable;
            _dataService = dataService;
            _mergeService = mergeService;
            _cameraService = cameraService;
            _hits = new RaycastHit[3];
            _platformMask = 1 << LayerMask.NameToLayer("Platform");

            _updateable.Tick += OnTick;
        }

        public void OnHovered(Platform gridData)
        {
            if (!_dragging) return;
            OnPlatformHovered?.Invoke(gridData);
        }

        public void OnReleased(Platform started)
        {
            if (!_dragging || !RaycastPlatform(out Platform platform)) return;

            Platform ended = _dataService.GetDataAt(platform.Index);

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

            _dragging = false;
            _selected = Vector2Int.zero;
            OnPlatformReleased?.Invoke(ended);
        }

        private void MoveActor(Platform started, Platform ended)
        {
            ended.Actor = started.Actor;
            started.Actor = null;
            ResetActorPosition(ended);
            ended.Actor.GetComponent<NavMeshAgent>().enabled = true;
        }

        public void OnClicked(Platform gridData)
        {
            if (gridData.Free) return;

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
                if (hit.transform.TryGetComponent(out Platform platform) &&
                    CastPlane(platform, ray, out float distance))
                {
                    Vector3 point = ray.GetPoint(distance);
                    _dataService.GetDataAt(_selected).Actor.transform.position = point;
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

        private void ResetActorPosition(Platform data) =>
            data.Actor.transform.position = data.transform.position;
    }
}