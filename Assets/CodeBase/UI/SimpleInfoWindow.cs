using Infrastructure;
using Services.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SimpleInfoWindow : Presenter
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _right;

        public override void Init() => _right.onClick.AddListener(OnRightClick);
        private void OnRightClick() => ServiceLocator.Resolve<WindowsService>().Close<SimpleInfoWindow>();
    }
}