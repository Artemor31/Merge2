using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CurrencyElement : Presenter
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _value;

        public void SetData(Sprite icon, string value)
        {
            _icon.sprite = icon;
            _value.text = value;
        }

        public void SetData(string value) => _value.text = value;
    }
}