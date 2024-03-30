using System.Collections.Generic;
using CodeBase.Gameplay.Units;
using CodeBase.Services;
using CodeBase.Services.SaveService;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.LevelData
{
    public class GridView : LevelItem
    {
        private static readonly int SelectCell = Shader.PropertyToID("_SelectCell");
        private static readonly int SelectedCellX = Shader.PropertyToID("_SelectedCellX");
        private static readonly int SelectedCellY = Shader.PropertyToID("_SelectedCellY");

        [SerializeField] private LayerMask _platformMask;
        [SerializeField] private Material _material;

        private IUpdateable _updateable;
        private RuntimeDataProvider _dataProvider;

        private RaycastHit[] _hits;
        private Vector2 _size;
        private Camera _camera;
        private bool _dragging;
        private Vector2Int _selected;
        private MergeService _mergeService;
        private GridRuntimeData[,] _data;

        public void Init(IUpdateable updateable, RuntimeDataProvider dataProvider, MergeService mergeService)
        {
            _dataProvider = dataProvider;
            _updateable = updateable;
            _mergeService = mergeService;
            _updateable.Tick += OnTick;

            _material.SetFloat(SelectCell, 0);
            _hits = new RaycastHit[5];
            _camera = Camera.main;


            _data = _dataProvider.GridData;

            for (int i = 0; i < _data.GetLength(0); i++)
            {
                for (int j = 0; j < _data.GetLength(1); j++)
                {
                    _data[i, j].Platform.OnClicked += PlatformOnOnClicked;
                    _data[i, j].Platform.OnReleased += PlatformOnOnReleased;
                    _data[i, j].Platform.OnHovered += PlatformOnOnHovered;

                    //_formation.Add(_data[i,j].Platform, _data[i,j].Actor);
                }
            }
        }

        private void OnTick()
        {
            if (!_dragging) return;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (RaycastPlatform(out Platform platform))
            {
                var plane = new Plane(Vector3.up, platform.transform.position);
                if (plane.Raycast(ray, out float distance))
                {
                    _dataProvider[_selected].Actor.transform.position = ray.GetPoint(distance);
                }
            }
        }

        private void PlatformOnOnClicked(Platform platform)
        {
            GridRuntimeData gridRuntimeData = _data[platform.Index.x, platform.Index.y];
            if (gridRuntimeData.Busy == false) return;
            
            _dragging = true;
            _selected = platform.Index;
            SetSelected(true);
            gridRuntimeData.Actor.GetComponent<NavMeshAgent>().enabled = false;
        }

        private void PlatformOnOnReleased(Platform platform)
        {
            if (RaycastPlatform(out Platform castedPlatform) && _dragging)
            {
                // if new grid is empty
                var casted = _dataProvider[castedPlatform.Index];
                var selected = _dataProvider[_selected];
                
                if (casted.Busy == false)
                {
                    casted.Actor = selected.Actor;
                    casted.Actor.transform.position = castedPlatform.transform.position;
                    casted.Actor.GetComponent<NavMeshAgent>().enabled = true;
                    selected.Actor = null;
                }
                else
                {
                    // can merge
                    Actor actor = _dataProvider[castedPlatform.Index].Actor;
                    Actor actor2 = selected.Actor;
                    
                    if (_mergeService.TryMerge(actor, actor2, out var newActor))
                    {
                        Destroy(actor.gameObject);
                        _dataProvider[castedPlatform.Index].Actor = null;

                        Destroy(_dataProvider[platform.Index].Actor.gameObject);
                        _dataProvider[platform.Index].Actor = null;

                        newActor.transform.position = castedPlatform.transform.position;
                        _dataProvider[castedPlatform.Index].Actor = newActor;
                    }
                    else
                    {
                        selected.Actor.transform.position = selected.Platform.transform.position;
                    }
                }

                SetSelected(false);
                _dragging = false;
                _selected = Vector2Int.zero;
            }
        }

        private void PlatformOnOnHovered(Vector2Int position)
        {
            SetSelected(position);
        }

        private bool RaycastPlatform(out Platform platform)
        {
            platform = null;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            Physics.RaycastNonAlloc(ray, _hits, 1000, _platformMask);
            foreach (RaycastHit hit in _hits)
            {
                platform = hit.transform.GetComponent<Platform>();
                if (platform != null)
                {
                    return true;
                }
            }

            return false;
        }

        private void SetSelected(Vector2Int position)
        {
            _material.SetFloat(SelectedCellX, position.y);
            _material.SetFloat(SelectedCellY, position.x);
        }

        private void SetSelected(bool highlight)
        {
            _material.SetFloat(SelectCell, highlight ? 1 : 0);
        }
    }
}