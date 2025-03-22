using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InfoWindow
{
    public class InfoActorPresenter : Presenter
    {
        [SerializeField] private Image _view;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Sprite _closedSprite;
        
        public void SetData(Sprite sprite, string text, string description)
        {
            _icon.sprite = sprite;
            _name.text = text;
            _description .text = description;
        }

        public void SetClosed()
        {
            _icon.sprite = _closedSprite;
            _name.text = _description .text = "";
            _view.color = Color.gray;
        }
    }
}