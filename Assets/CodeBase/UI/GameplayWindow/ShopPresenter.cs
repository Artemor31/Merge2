using System.Linq;
using Databases;
using Infrastructure;
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
        private UnitsDatabase _unitsDatabase;
        private GridDataService _gridDataService;
        private PlayerProgressService _playerService;

        public override void Init()
        {
            _cardPrefab = ServiceLocator.Resolve<AssetsProvider>().Load<UnitCard>(AssetsPath.UnitCard);
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UnitsDatabase>();
            _gridDataService = ServiceLocator.Resolve<GridDataService>();
            _playerService = ServiceLocator.Resolve<PlayerProgressService>();
            
            _box1Button.onClick.AddListener(() => OnBoxClicked(1, 10));
            _box2Button.onClick.AddListener(() => OnBoxClicked(2, 19));
            _box3Button.onClick.AddListener(() => OnBoxClicked(3, 26));
            ConstructRandomCarousel();
        }

        private void OnBoxClicked(int level, int cost)
        {
            ActorConfig actorConfig = _unitsDatabase.Units.Random(c => c.Data.Level == level);
            CreateUnit(actorConfig, cost);
        }
        
        private void CreateUnit(ActorConfig config, int cost)
        {
            if (_gridDataService.HasFreePlatform() == false) return;
            if (!_playerService.TryBuy(cost)) return;

            _gridDataService.AddPlayerUnit(config);
        }

        private void ConstructRandomCarousel()
        {
            // TODO
        }
    }
}