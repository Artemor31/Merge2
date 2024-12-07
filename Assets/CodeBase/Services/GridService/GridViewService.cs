using System;
using Gameplay.Grid;
using Services.Infrastructure;
using UnityEngine;
using UnityEngine.AI;

namespace Services.GridService
{
    public class GridViewService : IService
    {
        private enum State
        {
            Idle = 0,
            ClickTimer = 1,
            Dragging = 2
        }
        
        private const float TimeToDrag = 0.5f;
        
        private readonly IUpdateable _updateable;
        private readonly GridDataService _dataService;
        private readonly LayerMask _platformMask;
        private readonly MergeService _mergeService;
        private readonly CameraService _cameraService;

        private State _state = State.Idle;
        private Vector2 _size;
        private Vector2Int _selected;
        private float _timer;
        private Platform _platform;

        public event Action<Platform> OnPlatformPressed;
        public event Action<Platform> OnPlatformReleased;
        public event Action<Platform> OnPlatformHovered;
        public event Action<Platform> OnPlatformClicked;

        public GridViewService(IUpdateable updateable,
                               GridDataService dataService,
                               MergeService mergeService,
                               CameraService cameraService)
        {
            _updateable = updateable;
            _dataService = dataService;
            _mergeService = mergeService;
            _cameraService = cameraService;
            _platformMask = 1 << LayerMask.NameToLayer("Platform");

            _updateable.Tick += OnTick;
        }

        public void OnClicked(Platform platform)
        {
            if (platform.Busy)
            {
                _timer = TimeToDrag;
                _platform = platform;
                _state = State.ClickTimer;
            }
        }

        public void OnHovered(Platform gridData)
        {
            if (_state == State.Dragging)
            {
                OnPlatformHovered?.Invoke(gridData);
            }
        }

        public void OnReleased(Platform started)
        {
            if (_state == State.Idle) return;
            
            if (_state == State.ClickTimer)
            {
                _state = State.Idle;
                OnPlatformClicked?.Invoke(started);
                _timer = 0;
                return;
            }

            if (_state == State.Dragging)
            {
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

                _state = State.Idle;
                _selected = Vector2Int.zero;
                OnPlatformReleased?.Invoke(ended);
            }
        }

        private void OnTick()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }

            if (_timer <= 0 && _state == State.ClickTimer)
            {
                _state = State.Dragging;
                _selected = _platform.Index;
                _platform.Actor.GetComponent<NavMeshAgent>().enabled = false;
                OnPlatformPressed?.Invoke(_platform);
            }

            if (_state == State.Idle || _state == State.ClickTimer) return;

            var ray = _cameraService.TouchPointRay();

            foreach (RaycastHit hit in _cameraService.RayCast(ray, _platformMask))
            {
                if (_cameraService.CastPlane(hit.transform, ray, out float distance))
                {
                    _dataService.GetDataAt(_selected).Actor.transform.position = ray.GetPoint(distance);
                    break;
                }
            }
        }

        private void MoveActor(Platform started, Platform ended)
        {
            ended.Actor = started.Actor;
            started.Actor = null;
            ResetActorPosition(ended);
            ended.Actor.GetComponent<NavMeshAgent>().enabled = true;
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