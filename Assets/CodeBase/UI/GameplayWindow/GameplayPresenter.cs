using System.Collections.Generic;
using System.Linq;
using CodeBase.Databases;
using CodeBase.Gameplay;
using CodeBase.Gameplay.Units;
using CodeBase.Infrastructure;
using CodeBase.LevelData;
using CodeBase.Models;
using CodeBase.Services;
using CodeBase.Services.StateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.GameplayWindow
{
    public class GameplayPresenter : Presenter
    {
        [SerializeField] public Transform UnitsParent;
        [SerializeField] public LayerMask Ground;
        [SerializeField] public Button StartWaveButton;

        private GameplayModel _model;
        private WindowsService _windowsService;
        private UnitsDatabase _unitsDatabase;
        private LevelStaticData _levelStaticData;
        private UnitCard _cardPrefab;
        private Dictionary<UnitCard, UnitId> _unitCards;
        private bool _refreshed;
        private GameFactory _factory;

        public override void Init()
        {
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>()
                                           .GetDatabase<UnitsDatabase>();

            var progressService = ServiceLocator.Resolve<ProgressService>();
            _model = progressService.GameplayModel;
            _levelStaticData = progressService.StaticData;
            _windowsService = ServiceLocator.Resolve<WindowsService>();
            _cardPrefab = ServiceLocator.Resolve<AssetsProvider>().Load<UnitCard>(AssetsPath.UnitCard);
            _factory = ServiceLocator.Resolve<GameFactory>();

            StartWaveButton.onClick.AddListener(StartWave);
            CreatePlayerCards();
        }

        private void CreatePlayerCards()
        {
            _unitCards = new Dictionary<UnitCard, UnitId>();
            foreach (var unitData in _unitsDatabase.Units)
            {
                UnitCard card = Instantiate(_cardPrefab, UnitsParent);

                card.SetIcon(unitData.Icon);
                card.SetTitle(unitData.Name);
                card.Button.onClick.AddListener(() => CardOnClicked(card));
                _unitCards.Add(card, unitData.Id);
            }
        }

        private void CardOnClicked(UnitCard card)
        {
            Actor actor = _factory.CreateUnit(_unitCards[card]);
            Platform platform = _levelStaticData.PlayerPositions.First(p => p.Actor == null);
            if (platform == null) return;
            
            Quaternion rotation = Quaternion.AngleAxis(180, Vector3.up);
            actor.transform.SetPositionAndRotation(platform.transform.position, rotation);
            _model.AddAlly(actor);
        }

        private void StartWave()
        {
            _model.State = GameState.Processing;
        }

        private void OpenShowWindow()
        {
            _windowsService.Show<ShopPresenter>();
        }

        private void UpdateHp()
        {
        }

        private void UpdateMoney()
        {
        }

        private void UpdateCards()
        {
        }
    }
}