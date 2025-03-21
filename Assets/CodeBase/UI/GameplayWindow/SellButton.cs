using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayWindow
{
    public class SellButton : Presenter
    {
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private TextMeshProUGUI _costValue;
        [SerializeField] private Image _view;

        public void Show(int cost)
        {
            gameObject.SetActive(true);
            _cost.text = "Продать";
            _costValue.text = cost.ToString();
        }

        public void Hide() => gameObject.SetActive(false);
    }
}