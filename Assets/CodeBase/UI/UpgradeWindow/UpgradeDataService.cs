using System;
using System.Collections.Generic;
using System.Linq;
using Databases;
using Infrastructure;
using Services;
using Services.Infrastructure;
using Services.SaveProgress;

namespace UI.UpgradeWindow
{
    public class UpgradeDataService : IService
    {
        private const string SavePath = "UpgradesData";
        
        private readonly GameplayDataService _gameplayDataService;
        private readonly SaveService _saveService;
        private readonly UpgradeProgress _progress;

        public UpgradeDataService(GameplayDataService gameplayDataService, SaveService saveService)
        {
            _gameplayDataService = gameplayDataService;
            _saveService = saveService;
            _progress = _saveService.Restore<UpgradeProgress>(SavePath);
        }

        public bool TryProgress(string id)
        {
            UpgradeProgressPair upgrade = _progress.UpgradesProgress.First(p => p.Id == id);
            int cost = GetUpgradeCost(upgrade.Level + 1);
            
            if (_gameplayDataService.TryBuy(cost))
            {
                upgrade.Level++;
                _saveService.Save(SavePath, _progress);
                return true;
            }
            
            return false;
        }
        
        public IEnumerable<Race> GetAllRaces() => Extensions.AsCollection<Race>().Where(race => race != Race.None);
        public IEnumerable<Mastery> GetAllMasteries() => Extensions.AsCollection<Mastery>().Where(race => race != Mastery.None);
        public int LevelOf(string id) => _progress.UpgradesProgress.First(p => p.Id == id).Level;
        public int GetUpgradeCost(string id) => GetUpgradeCost(_progress.UpgradesProgress.First(p => p.Id == id).Level);
        private int GetUpgradeCost(float x) => (int)(Math.Log10(x + 1) * 2 + x / 6);
    }
}