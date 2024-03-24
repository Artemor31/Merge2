using System.Collections.Generic;
using CodeBase.Databases;
using CodeBase.Gameplay.Units;
using CodeBase.Infrastructure;
using CodeBase.LevelData;
using CodeBase.Services;
using CodeBase.Services.StateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.GameplayWindow
{
    public class GameplayPresenter : Presenter
    {
        [SerializeField] public Transform UnitsParent;
        [SerializeField] public Button StartWaveButton;

        private WindowsService _windowsService;
        private UnitsDatabase _unitsDatabase;
        private UnitCard _cardPrefab;
        private Dictionary<UnitCard, UnitId> _unitCards;
        private bool _refreshed;
        private GameFactory _factory;
        private ProgressService _progressService;

        public override void Init()
        {
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>()
                                           .GetDatabase<UnitsDatabase>();

            _progressService = ServiceLocator.Resolve<ProgressService>();
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
            Platform platform = _progressService.StaticData.GridView.GetFreePlatform();
            if (platform == null) return;
            
            Quaternion rotation = Quaternion.AngleAxis(180, Vector3.up);
            actor.transform.SetPositionAndRotation(platform.transform.position, rotation);
            _progressService.StaticData.GridView.AddActor(actor, platform);
            _progressService.GameplayModel.AddAlly(actor);
        }

        private void StartWave()
        {
            _progressService.GameplayModel.State = GameState.Processing;
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