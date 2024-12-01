using Databases;
using Gameplay.LevelItems;
using Infrastructure;
using Services.SaveService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayWindow
{
    public class ActorMenuPresenter : Presenter
    {
        [SerializeField] private RectTransform _transform;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private Button _sellButton;
        [SerializeField] private Button _background;

        private Platform _platform;
        private GridLogicService _gridLogicService;

        private void Awake() => _gridLogicService = ServiceLocator.Resolve<GridLogicService>();

        public void Show(Platform platform, Vector3 screenPoint, ActorConfig actorConfig)
        {
            _platform = platform;
            _transform.position = screenPoint;

            _cost.text = (actorConfig.Cost / 2).ToString();
            _sellButton.onClick.AddListener(Sell);
            _background.onClick.AddListener(Hide);
            gameObject.SetActive(true);
        }

        private void Sell()
        {
            _gridLogicService.SellUnitAt(_platform);
        }

        private void Hide()
        {
            _sellButton.onClick.RemoveListener(Sell);
            _background.onClick.RemoveListener(Hide);
            gameObject.SetActive(false);
        }
    }
}