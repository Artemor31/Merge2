using TMPro;
using UnityEngine;

namespace Infrastructure
{
    public class EasyFps : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private int _maxFps = 120;
    
        private int _frameCounter;
        private float _timeCounter;
        private float _lastFramerate;
        public float RefreshTime { get; set; } = 0.5f;

        private void Start() => QualitySettings.vSyncCount = 0;

        private void Update()
        {
            if (_timeCounter < RefreshTime)
            {
                _timeCounter += Time.deltaTime;
                _frameCounter++;
            }
            else
            {
                _lastFramerate = _frameCounter / _timeCounter;

                if (_lastFramerate <= _maxFps)
                {
                    _text.text = ((int)_lastFramerate).ToString();
                }
                else
                {
                    _text.text = _maxFps + "+";
                }

                _frameCounter = 0;
                _timeCounter = 0.0f;
            }
        }
    }
}