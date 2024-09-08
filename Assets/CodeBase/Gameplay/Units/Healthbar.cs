using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Units
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField] private Image _value;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Canvas _canvas;
        
        private IUpdateable _updateable;
        private Actor _owner;

        public void Initialize(Camera camera, Actor actor, IUpdateable updateable)
        {
            _updateable = updateable;
            _owner = actor;
            _level.text = actor.Level.ToString();
            ChangeHealth(1,1);
            _canvas.worldCamera = camera;
            transform.LookAt(-camera.transform.position);
            
            actor.HealthChanged += ChangeHealth;
            actor.Died += OnDied;
            _updateable.Tick += UpdateableOnTick;
        }

        private void UpdateableOnTick() => transform.position = _owner.transform.position + _offset;
        private void ChangeHealth(float current, float max) => _value.fillAmount = current / max;
        
        private void OnDied()
        {
            _owner.HealthChanged -= ChangeHealth;
            _owner.Died -= OnDied;
            _updateable.Tick -= UpdateableOnTick;
            Destroy(gameObject);
        }
    }
}