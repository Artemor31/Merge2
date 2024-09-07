using System.Linq;
using Databases;
using Gameplay.Units;
using Infrastructure;
using LevelData;
using Services;
using Services.SaveService;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayWindow
{
    public class ShopPresenter : Presenter
    {
        [SerializeField] private UnitCard _cardPrefab;
        [SerializeField] private Button _box1Button;
        [SerializeField] private Button _box2Button;
        [SerializeField] private Button _box3Button;
        private GameObserver _gameObserver;
        private UnitsDatabase _unitsDatabase;
        private GridDataService _gridDataService;
        private PlayerProgressService _playerService;
        private GameFactory _factory;

        public override void Init()
        {
            _cardPrefab = ServiceLocator.Resolve<AssetsProvider>().Load<UnitCard>(AssetsPath.UnitCard);
            _gameObserver = ServiceLocator.Resolve<GameObserver>();
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UnitsDatabase>();
            _gridDataService = ServiceLocator.Resolve<GridDataService>();
            _playerService = ServiceLocator.Resolve<PlayerProgressService>();
            _factory = ServiceLocator.Resolve<GameFactory>();
            
            _box1Button.onClick.AddListener(() => OnBoxClicked(1));
            _box2Button.onClick.AddListener(() => OnBoxClicked(2));
            _box3Button.onClick.AddListener(() => OnBoxClicked(3));
            ConstructRandomCarousel();
        }

        private void OnBoxClicked(int level)
        {
            ActorConfig actorConfig = _unitsDatabase.Units.Where(c => c.Level == level).Random();
            CreateUnit(actorConfig);
        }
        
        private void CreateUnit(ActorConfig config)
        {
            Platform platform = _gridDataService.GetFreePlatform();
            if (platform == null) return;

            if (!_playerService.TryBuy(config.Cost)) return;

            Actor actor = _factory.CreateActor(config);
            actor.transform.position = platform.transform.position;
            _gridDataService.AddPlayerUnit(actor, platform);
        }

        private void ConstructRandomCarousel()
        {
            
        }
    }
}