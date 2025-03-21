using System.Collections.Generic;
using Services.Infrastructure;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Services
{
    public class CameraService : IService
    {
        private const float MaxDistance = 500;
        
        private readonly SceneLoader _sceneLoader;
        private readonly RaycastHit[] _hits;
        private readonly List<RaycastResult> _results = new();
        private readonly PointerEventData _pointerData = new(EventSystem.current);
        private readonly LayerMask _platformMask;
        private readonly LayerMask _groundMask;
        private Camera _camera;

        public CameraService(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            _camera = Camera.main;
            _hits = new RaycastHit[5];
            _platformMask = 1 << LayerMask.NameToLayer("Platform");
            _groundMask = 1 << LayerMask.NameToLayer("Ground");
            
            _sceneLoader.OnSceneChanged += SceneChangedHandler;
        }

        public bool PointerUnder<T>()
        {
            _pointerData.position = Input.mousePosition;
            EventSystem.current.RaycastAll(_pointerData, _results);

            foreach (RaycastResult result in _results)
            {
                if (result.gameObject.TryGetComponent<T>(out _))
                {
                    return true;
                }
            }

            return false;
        }

        public Vector3? GetPointOnGrid()
        {
            var ray = TouchPointRay();
            IEnumerable<RaycastHit> hits = RayCast(ray, _platformMask);
            foreach (RaycastHit hit in hits)
            {
                if (CastPlane(hit.transform, ray, out float distance))
                {
                    return ray.GetPoint(distance);
                }
            }

            hits = RayCast(ray, _groundMask);
            foreach (RaycastHit hit in hits)
            {
                if (CastPlane(hit.transform, ray, out float distance))
                {
                    return ray.GetPoint(distance);
                }
            }

            return null;
        }

        public Camera CurrentCamera() => _camera;
        public Ray TouchPointRay() => _camera.ScreenPointToRay(Input.mousePosition);
        public Vector3 WorldToScreenPoint(Vector3 vector3) => _camera.WorldToScreenPoint(vector3);
        public bool CastPlane(Transform transform, Ray ray, out float distance) => 
            new Plane(Vector3.up, transform.position).Raycast(ray, out distance);

        public RaycastHit[] RayCast(Ray ray, int layerMask)
        {
            Physics.RaycastNonAlloc(ray, _hits, MaxDistance, layerMask);
            return _hits;
        }

        private void SceneChangedHandler() => _camera = Camera.main;
    }
}