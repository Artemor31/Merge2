using System;
using UnityEngine;

namespace CodeBase.Services
{
    public class InputService : IService
    {
        public event Action<Vector3> LeftButtonDown;
        public event Action<Vector3> LeftButtonUp;
        public Vector3 MousePosition => Input.mousePosition;
        public Vector3 MousePositionOnPlane => PointerOnPlane();

        private readonly IUpdateable _updateable;
        private readonly LayerMask _layerMask;
        
        private Camera _camera;
        private bool _clicked;
        private Vector3 _lastPosition;

        public InputService(IUpdateable updateable, LayerMask layerMask)
        {
            _updateable = updateable;
            _layerMask = layerMask;
            //_updateable.Tick += UpdateableOnTick;
        }

        public void SetCamera(Camera camera) => _camera = camera;

        private Vector3 PointerOnPlane()
        {
            Ray ray = _camera.ScreenPointToRay(MousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, _layerMask))
            {
                _lastPosition = hit.point;
                return _lastPosition;
            }

            return _lastPosition;
        }

        private void UpdateableOnTick()
        {
            CheckClicks();
        }

        private void CheckClicks()
        {
            if (Input.GetMouseButtonDown(0) && _clicked == false)
            {
                _clicked = true;
                LeftButtonDown?.Invoke(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0) && _clicked)
            {
                _clicked = false;
                LeftButtonUp?.Invoke(Input.mousePosition);
            }
        }
    }
}