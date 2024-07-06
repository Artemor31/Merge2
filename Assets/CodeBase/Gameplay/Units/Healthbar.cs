using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Gameplay.Units
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField] private Image _value;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Canvas _canvas;
        private Health _health;
        private Transform _target;

        public void Initialize(Camera camera, Actor actor, Health health)
        {
            _health = health;
            _target = actor.transform;
            health.HealthChanged += SetHealth;
            _level.text = actor.Level.ToString();
            SetHealth();
            _canvas.worldCamera = camera;
            transform.LookAt(-camera.transform.position);
        }

        private void Update() => transform.position = _target.position + _offset;
        private void OnDisable() => _health.HealthChanged -= SetHealth;
        private void SetHealth() => _value.fillAmount = _health.Ratio;
    }
}