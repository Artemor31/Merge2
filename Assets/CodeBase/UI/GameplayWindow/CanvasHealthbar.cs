using Infrastructure;
using Services;
using Services.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayWindow
{
    public class CanvasHealthbar : MonoBehaviour
    {
        [SerializeField] private float Offset = 2.4f;
        [SerializeField] private Image _value;

        private CameraService _cameraService;
        private IUpdateable _updateable;
        private RectTransform _parentCanvas;
        private Transform _target;

        public void Init(RectTransform canvas, Transform actor)
        {
            _cameraService = ServiceLocator.Resolve<CameraService>();
            _updateable = ServiceLocator.Resolve<IUpdateable>();
            _parentCanvas = canvas;
            _target = actor;
            _updateable.Tick += UpdateableOnTick;
            ChangeHealth(1);
        }

        private void UpdateableOnTick()
        {
            Vector3 offsetPos = _target.position + Vector3.up * Offset;
            Vector2 screenPoint = _cameraService.WorldToScreenPoint(offsetPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentCanvas, screenPoint, null, out var canvasPos);
            transform.localPosition = canvasPos;  
        }

        public void ChangeHealth(float ratio) => _value.fillAmount = ratio;
        public void UnInit()
        {
            _updateable.Tick -= UpdateableOnTick;
            _target = null;
        }
    }
}