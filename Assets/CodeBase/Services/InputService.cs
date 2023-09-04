using System;
using UnityEngine;

namespace CodeBase.Services
{
    public class InputService : IService
    {
        public event Action<Vector3> LeftButtonDown;
        public event Action<Vector3> LeftButtonUp;

        public Vector3 MousePosition => Input.mousePosition;
        private readonly IUpdateable _updateable;
        private bool _clicked;

        public InputService(IUpdateable updateable)
        {
            _updateable = updateable;
            _updateable.Tick += UpdateableOnTick;
        }

        private void UpdateableOnTick()
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