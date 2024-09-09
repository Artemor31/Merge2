using Services;
using UnityEngine;

namespace Gameplay.LevelItems
{
    public class CameraService : IService
    {
        private readonly SceneLoader _sceneLoader;
        private Camera _camera;

        public CameraService(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            _camera = Camera.main;
            _sceneLoader.OnSceneChanged += SceneChangedHandler;
        }
        
        public Camera CurrentMainCamera() => _camera;
        public Ray TouchPointRay() => _camera.ScreenPointToRay(Input.mousePosition);
        private void SceneChangedHandler() => _camera = Camera.main;
    }
}