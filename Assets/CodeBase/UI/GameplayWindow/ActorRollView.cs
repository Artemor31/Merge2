using Databases;
using Gameplay.Units;
using Infrastructure;
using Services;
using Services.DataServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayWindow
{
    public class ActorRollView : Presenter
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private Image _race;
        [SerializeField] private Image _mastery;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Transform _prefabRoot;

        protected GridService GridService;
        private BuffsDatabase _buffsDatabase;
        private GameplayDataService _gameplayService;
        private Actor _actor;

        public override void Init()
        {
            GridService = ServiceLocator.Resolve<GridService>();
            _buffsDatabase = ServiceLocator.Resolve<BuffsDatabase>();
            _gameplayService = ServiceLocator.Resolve<GameplayDataService>();
            _buyButton.onClick.AddListener(TryBuyUnit);
        }

        public void SetData(Actor actor)
        {
            if (_actor != null)
            {
                Destroy(_actor.gameObject);
            }
            
            _actor = actor;
            _actor.Dispose();
            Transform transform1;
            (transform1 = _actor.transform).SetParent(_prefabRoot, false);
            transform1.localPosition = Vector3.zero;
            transform1.localScale = Vector3.one * 120;

            ActorData data = _actor.Data;
            _levelText.text = data.Level.ToString();
            _cost.text = _buffsDatabase.CostFor(data.Level).ToString();
            _race.sprite = _buffsDatabase.IconFor(data.Race);
            _mastery.sprite = _buffsDatabase.IconFor(data.Mastery);
        }

        public void Hide()
        {
            _prefabRoot.gameObject.SetActive(false);
            _buyButton.interactable = false;
        }

        protected virtual void TryBuyUnit()
        {
            int cost = _buffsDatabase.CostFor(_actor.Data.Level);
            if (GridService.CanAddUnit && _gameplayService.TryBuy(cost))
            {
                GridService.TryCreatePlayerUnit();
                Hide();
            }
        }
    }
}