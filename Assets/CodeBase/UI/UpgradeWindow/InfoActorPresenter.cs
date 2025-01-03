using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UpgradeWindow
{
    public class InfoActorPresenter : Presenter
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _name;


        public void SetData(Sprite sprite, string text)
        {
            _icon.sprite = sprite;
            _name.text = text;
        }
    }
}