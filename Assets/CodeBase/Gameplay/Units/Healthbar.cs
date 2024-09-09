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
            _owner = actor;
            _updateable = updateable;
            _level.text = actor.Data.Level.ToString();
            _canvas.worldCamera = camera;
            transform.LookAt(-camera.transform.position);

            ChangeHealth(1,1);
            actor.HealthChanged += ChangeHealth;
            actor.Died += Dispose;
            _updateable.Tick += Tick;
        }

        private void Tick() => transform.position = _owner.transform.position + _offset;
        private void ChangeHealth(float current, float max) => _value.fillAmount = current / max;
        
        private void Dispose()
        {
            _owner.HealthChanged -= ChangeHealth;
            _owner.Died -= Dispose;
            _updateable.Tick -= Tick;
           // Destroy(this);
           // Destroy(gameObject);
        }
    }
}