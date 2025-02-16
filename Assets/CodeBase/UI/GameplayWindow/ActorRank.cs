using Databases;
using Databases.Data;
using Infrastructure;
using Services;
using Services.Infrastructure;
using Services.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayWindow
{
    public class ActorRank : MonoBehaviour
    {
        [SerializeField] private float Offset = 2.4f;
        [SerializeField] private Image _raceImage;
        [SerializeField] private Image _masteryImage;
        [SerializeField] private TextMeshProUGUI _level;
        private Transform _target;
        private CameraService _cameraService;
        private RectTransform _parentCanvas;

        public void Init(RectTransform rectTransform, Transform target, ActorData data)
        {
            _parentCanvas = rectTransform;
            _target = target;
            _cameraService = ServiceLocator.Resolve<CameraService>();

            var buffsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<BuffsDatabase>();
            _raceImage.sprite = buffsDatabase.IconFor(data.Race);
            _masteryImage.sprite = buffsDatabase.IconFor(data.Mastery);
            _level.text = data.Level.ToString();
        }

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