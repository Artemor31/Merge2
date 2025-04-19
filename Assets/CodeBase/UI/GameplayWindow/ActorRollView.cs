using System;
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

        private BuffsDatabase _buffsDatabase;
        private GameplayDataService _gameplayService;
        private GridService _gridService;
        private ActorData? _actorData;
        private Actor _actor;

        public override void Init()
        {
            _buffsDatabase = ServiceLocator.Resolve<BuffsDatabase>();
            _gridService = ServiceLocator.Resolve<GridService>();
            _gameplayService = ServiceLocator.Resolve<GameplayDataService>();
            _buyButton.onClick.AddListener(TryBuyUnit);
        }

        public void SetData(ActorData actorData, Actor actor)
        {
            if (_actor != null)
            {
                Destroy(_actor.gameObject);
            }
            
            _actorData = actorData;
            SetUI(actorData);
            SetPrefab(actor);
        }

        private void TryBuyUnit()
        {
            int cost = _buffsDatabase.CostFor(_actorData.Value.Level);
            if (_gridService.CanAddUnit && _gameplayService.TryBuy(cost))
            {
                _gridService.TryCreatePlayerUnit();
                Hide();
            }
        }

        private void Hide()
        {
            _prefabRoot.gameObject.SetActive(false);
            _buyButton.interactable = false;
        }

        private void SetUI(ActorData actorData)
        {
            _levelText.text = actorData.Level.ToString();
            _cost.text = _buffsDatabase.CostFor(actorData.Level).ToString();
            _race.sprite = _buffsDatabase.IconFor(actorData.Race);
            _mastery.sprite = _buffsDatabase.IconFor(actorData.Mastery);
        }

        private void SetPrefab(Actor actor)
        {
            _actor = actor;
            actor.transform.SetParent(_prefabRoot, false);
            actor.transform.localPosition = Vector3.zero;
        }
    }
}