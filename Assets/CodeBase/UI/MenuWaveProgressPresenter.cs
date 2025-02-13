using TMPro;
using UnityEngine;

namespace UI
{
    public class MenuWaveProgressPresenter : Presenter
    {
        [SerializeField] private TextMeshProUGUI _value;

        public void SetValue(float current, float max)
        {
            _value.text = $"{Mathf.RoundToInt(current / max * 100)}%";
        }
    }
}