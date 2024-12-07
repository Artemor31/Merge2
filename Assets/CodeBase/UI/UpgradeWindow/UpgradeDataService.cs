using System;
using System.Collections.Generic;
using System.Linq;
using Databases;
using Services;
using Services.SaveService;

namespace UI.ShopWindow
{
    public class UpgradeDataService : IService
    {
        private const string SavePath = "UpgradesData";
        
        private readonly GameplayDataService _gameplayDataService;
        private readonly SaveService _saveService;
        private readonly UpgradeProgress _progress;
        private readonly UnitsDatabase _unitsDatabase;

        public UpgradeDataService(GameplayDataService gameplayDataService,
                                  SaveService saveService,
                                  DatabaseProvider databaseProvider)
        {
            _gameplayDataService = gameplayDataService;
            _saveService = saveService;
            _progress = _saveService.Restore<UpgradeProgress>(SavePath);
            _unitsDatabase = databaseProvider.GetDatabase<UnitsDatabase>();
        }

        public IEnumerable<ActorConfig> CurrentConfigs() => _unitsDatabase.Units.Where(config => config.Data.Level == 1);

        public bool TryProgress(string id)
        {
            UpgradeProgressPair upgrade = _progress.UpgradesProgress.First(p => p.Id == id);
            var cost = GetUpgradeCost(upgrade.Level + 1);
            
            if (_gameplayDataService.TryBuy(cost))
            {
                upgrade.Level++;
                _saveService.Save(SavePath, _progress);
                return true;
            }
            return false;
        }

        public int LevelOf(string id) => _progress.UpgradesProgress.First(p => p.Id == id).Level;
        public int GetUpgradeCost(string id) => GetUpgradeCost(_progress.UpgradesProgress.First(p => p.Id == id).Level);
        private int GetUpgradeCost(float x) => (int)(Math.Log10(x + 1) * 2 + x / 6);
    }
}