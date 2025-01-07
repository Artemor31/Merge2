using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Units.Healths
{
    public class CanvasHealthbar : MonoBehaviour
    {
        [SerializeField] private float Offset = 2.4f;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private Image _value;

        private CameraService _cameraService;
        private RectTransform _parentCanvas;
        private Transform _target;

        public void Init(CameraService cameraService, RectTransform canvas, Transform actor, int level)
        {
            _cameraService = cameraService;
            _parentCanvas = canvas;
            _target = actor;
            _level.text = level.ToString();
            ChangeHealth(1);
        }

        public void UnInit() => _target = null;

        public void ChangeHealth(float ratio) => _value.fillAmount = ratio;

        private void Update()
        {
            var position = _target.position;
            Vector3 offsetPos = new(position.x, position.y + Offset, position.z);
            Vector2 screenPoint = _cameraService.WorldToScreenPoint(offsetPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentCanvas, screenPoint, null, out var canvasPos);
            transform.localPosition = canvasPos;
        }
    }
}