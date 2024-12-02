using System.Collections.Generic;
using Services;
using UnityEngine;

namespace Gameplay.LevelItems
{
    public class CameraService : IService
    {
        private const float MaxDistance = 1000;
        private readonly SceneLoader _sceneLoader;
        private readonly RaycastHit[] _hits;
        private Camera _camera;

        public CameraService(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            _camera = Camera.main;
            _hits = new RaycastHit[5];
            _sceneLoader.OnSceneChanged += SceneChangedHandler;
        }
        
        public Camera CurrentCamera() => _camera;
        public Ray TouchPointRay() => _camera.ScreenPointToRay(Input.mousePosition);
        public Vector3 WorldToScreenPoint(Vector3 vector3) => _camera.WorldToScreenPoint(vector3);
        public bool CastPlane(Transform transform, Ray ray, out float distance) => 
            new Plane(Vector3.up, transform.position).Raycast(ray, out distance);

        public IEnumerable<RaycastHit> RayCast(Ray ray, int layerMask)
        {
            int count = Physics.RaycastNonAlloc(ray, _hits, MaxDistance, layerMask);

            for (int i = 0; i < count; i++)
            {
                yield return _hits[i];
            }
        }

        private void SceneChangedHandler() => _camera = UnityEngine.Camera.main;
    }
}