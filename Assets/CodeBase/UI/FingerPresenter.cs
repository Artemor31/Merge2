using System;
using System.Collections;
using Infrastructure;
using Services;
using UnityEngine;

namespace UI
{
    public class FingerPresenter : Presenter
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _scaleFactor = 0.5f;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _speed = 1f;

        private CameraService _cameraService;
        private Vector2 _offset;
        private readonly WaitForEndOfFrame _waitForEndOfFrame = new();
        private readonly Vector3 _originalScale = Vector3.one;
        
        private bool _isScalingUp;
        private float _currentTime;
        private Coroutine _routine;
        private TutorView _v1;
        private TutorView _v2;

        public override void Init()
        {
            _cameraService = ServiceLocator.Resolve<CameraService>();
            _offset = new Vector2(_rectTransform.rect.width / 4, -_rectTransform.rect.height / 2);
        }

        public void Disable() => gameObject.SetActive(false);
        public void Active() => gameObject.SetActive(true);

        public void PointTo(TutorView currentView)
        {
            if (currentView == null)
            {
                throw new Exception("Not found item with id = " + currentView.Id);
            }
            
            Active();
            Vector2 screenCoordinate = currentView.Is2D
                ? currentView.RectTransform.TransformPoint(currentView.RectTransform.rect.center)
                : _cameraService.WorldToScreenPoint(currentView.Transform.position + Vector3.up);

            _rectTransform.position = screenCoordinate + _offset;
            _routine = StartCoroutine(Blink());
        }

        public void MoveBetween(TutorView v1, TutorView v2)
        {
            _v2 = v2;
            _v1 = v1;
            if (_routine != null)
            {
                StopCoroutine(_routine);
            }

            _routine = StartCoroutine(MoveBetween());
        }

        private IEnumerator MoveBetween()
        {
            while (true)
            {
                yield return _waitForEndOfFrame;
                
                if (_v1 != null && _v2 != null)
                {
                    Vector2 screenPosA = _cameraService.WorldToScreenPoint(_v1.transform.position);
                    Vector2 screenPosB = _cameraService.WorldToScreenPoint(_v2.transform.position);
                    float pong = Mathf.PingPong(Time.time * _speed, 1);
                    _rectTransform.position = Vector2.Lerp(screenPosA, screenPosB, pong) + _offset;
                }
            }    
        }

        private IEnumerator Blink()
        {
            while (true)
            {
                yield return _waitForEndOfFrame;

                _rectTransform.localScale = _isScalingUp
                    ? Vector3.Lerp(_originalScale * (1 - _scaleFactor), _originalScale, _currentTime / _duration)
                    : Vector3.Lerp(_originalScale, _originalScale * (1 - _scaleFactor), _currentTime / _duration);

                _currentTime += Time.deltaTime;

                if (_currentTime >= _duration)
                {
                    _currentTime = 0f;
                    _isScalingUp = !_isScalingUp;
                }
            }
        }
    }
}