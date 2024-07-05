using System;
using CodeBase.Infrastructure;
using CodeBase.LevelData;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Gameplay.Units
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField] private Image _value;
        [SerializeField] private Health _health;
        [SerializeField] private Canvas _canvas;
        private Transform _lookAt;

        private void OnEnable()
        {
            SetValue();
            _health.HealthChanged += SetValue;
            _canvas.worldCamera = ServiceLocator.Resolve<CameraService>().CurrentMainCamera();
            _lookAt = _canvas.worldCamera.transform;
        }

        private void Update() => transform.LookAt(_lookAt.position);
        private void OnDisable() => _health.HealthChanged -= SetValue;
        private void SetValue() => _value.fillAmount = _health.Ratio;
    }
}