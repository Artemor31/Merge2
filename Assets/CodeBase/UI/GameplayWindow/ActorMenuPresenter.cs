using Infrastructure;
using UnityEngine.UI;
using UnityEngine;
using Databases;
using Gameplay.Grid;
using Services.GridService;
using TMPro;

namespace UI.GameplayWindow
{
    public class ActorMenuPresenter : Presenter
    {
        [SerializeField] private RectTransform _transform;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private Button _sellButton;
        [SerializeField] private Button _background;

        private GridLogicService _gridLogicService;
        private Platform _platform;

        private void Awake()
        {
            _gridLogicService = ServiceLocator.Resolve<GridLogicService>();
            //_sellButton.onClick.AddListener(Sell);
            _background.onClick.AddListener(Hide);
        }

        public void Show(Platform platform, Vector3 screenPoint, ActorConfig actorConfig)
        {
            _platform = platform;
            _transform.position = screenPoint;

            float max = Mathf.Max(1, 10 * 0.7f);
            _cost.text = Mathf.RoundToInt(max).ToString();
            gameObject.SetActive(true);
        }

        public void Hide() => gameObject.SetActive(false);

        // private void Sell()
        // {
        //     _gridLogicService.SellUnitAt(_platform);
        //     Hide();
        // }
    }
}