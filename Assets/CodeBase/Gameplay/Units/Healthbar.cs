using System;
using CodeBase.Infrastructure;
using CodeBase.LevelData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Gameplay.Units
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField] private Image _value;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private Health _health;
        [SerializeField] private Actor _actor;
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Canvas _canvas;

        private void OnEnable()
        {
            SetValue();
            _health.HealthChanged += SetValue;
            _level.text = _actor.Level.ToString();
            
            Camera currentCamera = ServiceLocator.Resolve<CameraService>().CurrentMainCamera();
            _canvas.worldCamera = currentCamera;
            transform.LookAt(-currentCamera.transform.position);
        }

        // separate spawning of actor and attaching healthbar to him
        private void Update() => transform.position = _target.position + _offset;
        private void OnDisable() => _health.HealthChanged -= SetValue;
        private void SetValue() => _value.fillAmount = _health.Ratio;
    }
}