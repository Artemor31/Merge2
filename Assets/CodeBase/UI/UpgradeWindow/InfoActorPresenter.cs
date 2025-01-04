using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UpgradeWindow
{
    public class InfoActorPresenter : Presenter
    {
        [SerializeField] private Image _view;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Sprite _closedSprite;


        public void SetData(Sprite sprite, string text)
        {
            _icon.sprite = sprite;
            _name.text = text;
            _view.color = Color.white;
        }

        public void SetClosed()
        {
            _icon.sprite = _closedSprite;
            _name.text = "???";
            _view.color = Color.gray;
        }
    }
}