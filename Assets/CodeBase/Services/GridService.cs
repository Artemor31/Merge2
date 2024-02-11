using UnityEngine;

namespace CodeBase.Services
{
    public class GridService : IService
    {
        private readonly InputService _inputService;
        private readonly IUpdateable _updateable;
        private GameObject _marker;
        private bool _follow;

        public GridService(IUpdateable updateable, InputService inputService)
        {
            _updateable = updateable;
            _inputService = inputService;
            _updateable.Tick += UpdateableOnTick;
        }

        public void Init()
        {
            _marker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            _follow = true;
        }
        
        private void UpdateableOnTick()
        {
            if (_follow == false) return;
            _marker.transform.position = _inputService.MousePositionOnPlane;
        }
    }
}