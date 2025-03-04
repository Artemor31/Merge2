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
        private RectTransform _parentCanvas;
        private Transform _target;

        public void Init(RectTransform canvas, Transform actor)
        {
            _cameraService = ServiceLocator.Resolve<CameraService>();
            _parentCanvas = canvas;
            _target = actor;
            ChangeHealth(1);
        }

        public void SetColor(bool isMy) => _value.color = isMy ? Color.green : Color.red;

        public void ChangeHealth(float ratio) => _value.fillAmount = ratio;
        
        private void Update()
        {
            if (_target == null) return;
            
            Vector3 offsetPos = _target.position + Vector3.up * Offset;
            Vector2 screenPoint = _cameraService.WorldToScreenPoint(offsetPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentCanvas, screenPoint, null, out var canvasPos);
            transform.localPosition = canvasPos;  
        }
    }
}