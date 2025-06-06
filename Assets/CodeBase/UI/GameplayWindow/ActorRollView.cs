using Databases;
using Gameplay.Units;
using Infrastructure;
using Services;
using Services.DataServices;
using Services.Infrastructure;
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
        private RewardsService _rewardsService;

        public override void Init()
        {
            GridService = ServiceLocator.Resolve<GridService>();
            _buffsDatabase = ServiceLocator.Resolve<BuffsDatabase>();
            _gameplayService = ServiceLocator.Resolve<GameplayDataService>();
            _rewardsService = ServiceLocator.Resolve<RewardsService>();
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
            _race.sprite = _buffsDatabase.IconFor(data.Race);
            _mastery.sprite = _buffsDatabase.IconFor(data.Mastery);

            if (_cost)
            {
                _cost.text = _rewardsService.CostForUnit(data.Level).ToString();
            }

            Reset();
        }

        protected virtual void Reset()
        {
            _prefabRoot.gameObject.SetActive(true);
            _buyButton.interactable = true;
        }

        public virtual void Hide()
        {
            _prefabRoot.gameObject.SetActive(false);
            _buyButton.interactable = false;
            
            if (_actor)
            {
                Destroy(_actor.gameObject);
            }
        }

        protected virtual void TryBuyUnit()
        {
            int cost = _rewardsService.CostForUnit(_actor.Data.Level);
            if (GridService.CanAddUnit && _gameplayService.TryBuy(cost))
            {
                GridService.TryCreatePlayerUnit();
                Hide();
            }
        }
    }
}