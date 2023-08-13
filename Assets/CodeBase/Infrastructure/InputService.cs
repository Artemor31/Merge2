using System;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class InputService : IService
    {
        public event Action<Vector3> LeftButtonDown;
        public event Action<Vector3> LeftButtonUp;
            
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