using Databases.Data;
using Services;
using Services.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayWindow
{
    public class ActorHudInfo : MonoBehaviour
    {

        public virtual void  Init(CameraService camera, RectTransform canvas, Transform actor)
        {

        }
    }
    
    public class ActorRank : ActorHudInfo
    {
        
         
    }
    
    public class CanvasHealthbar : ActorHudInfo
    {
        [SerializeField] private float Offset = 2.4f;
        [SerializeField] private Image _value;

        private CameraService _cameraService;
        private IUpdateable _updateable;
        private RectTransform _parentCanvas;
        private Transform _target;

        public void Init(CameraService camera, 
                         IUpdateable updateable,
                         RectTransform canvas,
                         Transform actor)
        {
            _cameraService = camera;
            _updateable = updateable;
            _parentCanvas = canvas;
            _target = actor;
            _updateable.Tick += UpdateableOnTick;
            ChangeHealth(1);
        }

        private void UpdateableOnTick()
        {
                
        }

        public void UnInit() => _target = null;

        public void ChangeHealth(float ratio) => _value.fillAmount = ratio;

        private void Update()
        {
            Vector3 offsetPos = _target.position + Vector3.up * Offset;
            Vector2 screenPoint = _cameraService.WorldToScreenPoint(offsetPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentCanvas, screenPoint, null, out var canvasPos);
            transform.localPosition = canvasPos;
        }
    }
}