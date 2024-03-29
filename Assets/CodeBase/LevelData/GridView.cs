using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Units;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.LevelData
{
    public class GridView : MonoBehaviour
    {
        private static readonly int GridSizeX = Shader.PropertyToID("_GridSizeX");
        private static readonly int GridSizeY = Shader.PropertyToID("_GridSizeY");
        private static readonly int SelectCell = Shader.PropertyToID("_SelectCell");
        private static readonly int SelectedCellX = Shader.PropertyToID("_SelectedCellX");
        private static readonly int SelectedCellY = Shader.PropertyToID("_SelectedCellY");

        [SerializeField] private List<Platform> _platforms;
        [SerializeField] private LayerMask _platformMask;

        private Dictionary<Platform, Actor> _formation;
        private RaycastHit[] _hits;
        private Material _material;
        private Vector2 _size;
        private Camera _camera;
        private bool _dragging;
        private Actor _actor;
        private Platform _platform;
        private MergeService _mergeService;

        public void Init(MergeService mergeService)
        {
            _mergeService = mergeService;
        }

        private void OnEnable()
        {
            _material = GetComponent<MeshRenderer>().material;
            _material.SetFloat(SelectCell, 0);
            _formation = new Dictionary<Platform, Actor>();
            _hits = new RaycastHit[5];
            _camera = Camera.main;

            foreach (Platform platform in _platforms)
            {
                platform.OnClicked += PlatformOnOnClicked;
                platform.OnReleased += PlatformOnOnReleased;
                platform.OnHovered += PlatformOnOnHovered;
                _formation.Add(platform, null);
            }
        }

        private void Update()
        {
            if (!_dragging) return;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (RaycastPlatform(out Platform platform))
            {
                var plane = new Plane(Vector3.up, platform.transform.position);
                if (plane.Raycast(ray, out float distance))
                {
                    _actor.transform.position = ray.GetPoint(distance);
                }
            }
        }

        public Platform GetFreePlatform() => _formation.Keys.FirstOrDefault(platform => _formation[platform] == null);

        public void AddActor(Actor actor, Platform platform) => _formation[platform] = actor;

        private void PlatformOnOnClicked(Platform platform)
        {
            if (_formation[platform] != null)
            {
                _dragging = true;
                SetSelected(true);
                _actor = _formation[platform];
                _platform = platform;
                _actor.GetComponent<NavMeshAgent>().enabled = false;
            }
        }

        private void PlatformOnOnReleased(Platform platform)
        {
            if (RaycastPlatform(out Platform castedPlatform) && _actor != null)
            {
                // if new grid is empty
                if (_formation[castedPlatform] == null)
                {
                    _formation[castedPlatform] = _actor;
                    _actor.transform.position = castedPlatform.transform.position;

                    _actor.GetComponent<NavMeshAgent>().enabled = true;
                    _formation[_platform] = null;
                }
                else
                {
                    if (_mergeService.TryMerge(_formation[castedPlatform], _actor, out var newActor))
                    {
                        Destroy(_formation[castedPlatform].gameObject);
                        _formation[castedPlatform] = null;
                        
                        Destroy(_formation[platform].gameObject);
                        _formation[platform] = null;
                        
                        newActor.transform.position = castedPlatform.transform.position;
                        _formation[castedPlatform] = newActor;
                    }
                    else
                    {
                        _actor.transform.position = _platform.transform.position;
                    }
                }

                SetSelected(false);
                _dragging = false;
                _actor = null;
                _platform = null;
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