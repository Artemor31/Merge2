using Infrastructure;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.WorldSpace
{
    public class CanvasHealthbar : MonoBehaviour
    {
        [SerializeField] private float Offset = 2.4f;
        [SerializeField] private Image _value;

        private const float HealthChangeSpeed = 0.05f;
        private CameraService _cameraService;
        private RectTransform _parentCanvas;
        private Transform _target;
        private float _targetRatio;

        public void Init(RectTransform canvas, Transform actor)
        {
            _cameraService = ServiceLocator.Resolve<CameraService>();
            _parentCanvas = canvas;
            _target = actor;
            ChangeHealth(1);
        }

        public void SetColor(bool isMy) => _value.color = isMy ? Color.green : Color.red;

        public void ChangeHealth(float ratio)
        {
            _targetRatio = ratio;
            _value.fillAmount = ratio;
        }

        private void Update()
        {
            if (_target == null) return;

            if (!Mathf.Approximately(_value.fillAmount, _targetRatio))
            {
                if (_value.fillAmount - _targetRatio > 0)
                {
                    _value.fillAmount -= HealthChangeSpeed;
                }
                else
                {
                    _value.fillAmount += HealthChangeSpeed;
                }
            }
            
            Vector3 offsetPos = _target.position + Vector3.up * Offset;
            Vector2 screenPoint = _cameraService.WorldToScreenPoint(offsetPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentCanvas, screenPoint, null, out var canvasPos);
            transform.localPosition = canvasPos;  
        }
    }
}