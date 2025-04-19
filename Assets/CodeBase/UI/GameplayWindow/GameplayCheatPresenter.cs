using System.Collections.Generic;
using Databases;
using Infrastructure;
using Services;
using Services.DataServices;
using Services.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayWindow
{
#if UNITY_EDITOR
    public class GameplayCheatPresenter : Presenter
    {
        [SerializeField] public Button GreedButton;
        [SerializeField] public Transform UnitsParent;
        
        private GameplayDataService _gameplayService;
        private GridService _gridService;
        private UnitsDatabase _unitsDatabase;
        private Dictionary<UnitCard, ActorConfig> _unitCards;
        private UnitCard _cardPrefab;

        public override void Init()
        {
            _gameplayService = ServiceLocator.Resolve<GameplayDataService>();
            _gridService = ServiceLocator.Resolve<GridService>();
            
            GreedButton.onClick.AddListener(() => _gameplayService.AddCrowns(50));
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UnitsDatabase>();
            _cardPrefab = ServiceLocator.Resolve<AssetsProvider>().Load<UnitCard>(AssetsPath.UnitCard);
            CreatePlayerCards();
        }
        
        private void CreatePlayerCards()
        {
            _unitCards = new Dictionary<UnitCard, ActorConfig>();
            foreach (ActorConfig actorConfig in _unitsDatabase.Units)
            {
                UnitCard card = Instantiate(_cardPrefab, UnitsParent);

                card.Setup(actorConfig);
                card.Button.onClick.AddListener(() => CardClicked(card));
                _unitCards.Add(card, actorConfig);
            }
        }
        
        private void CardClicked(UnitCard card)
        {
            if (_gridService.CanAddUnit)
            {
                _gridService.TryCreatePlayerUnit(_unitCards[card]);
            }
        }
    }
#endif
}