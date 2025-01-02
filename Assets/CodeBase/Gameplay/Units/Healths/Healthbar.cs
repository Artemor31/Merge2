using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Units.Healths
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private Image _value;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private List<Image> _arrows;
        private Transform _owner;

        public void Initialize(Camera camera, Transform owner, int level)
        {
            _owner = owner;
            _canvas.worldCamera = camera;
            _level.text = level.ToString();

            // for (int i = 1; i < level; i++)
            // {
            //     _arrows[i-1].gameObject.SetActive(true);
            // }
            
            transform.LookAt(-camera.transform.position);

            ChangeHealth(1);
        }

        public void ChangeHealth(float ratio) => _value.fillAmount = ratio;
        private void Update() => transform.position = _owner.transform.position + _offset;
    }
}