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
        
        private readonly PersistantDataService _persistantDataService;
        private readonly SaveService _saveService;
        private readonly UpgradeProgress _progress;

        public UpgradeDataService(PersistantDataService persistantDataService, SaveService saveService)
        {
            _persistantDataService = persistantDataService;
            _saveService = saveService;
            _progress = _saveService.Restore<UpgradeProgress>(SavePath);
        }

        public bool TryProgress(string id)
        {
            UpgradeProgressPair upgrade = _progress.Pairs.First(p => p.Id == id);
            int cost = GetUpgradeCost(upgrade.Level + 1);
            
            if (_persistantDataService.TryBuy(cost))
            {
                upgrade.Level++;
                _saveService.Save(SavePath, _progress);
                return true;
            }
            
            return false;
        }
        
        public IEnumerable<Race> GetAllRaces() => Extensions.AsCollection<Race>().Where(race => race != Race.None);
        public IEnumerable<Mastery> GetAllMasteries() => Extensions.AsCollection<Mastery>().Where(race => race != Mastery.None);
        public int LevelOf(string id) => _progress.Pairs.First(p => p.Id == id).Level;
        public int GetUpgradeLevel(string id) => GetUpgradeCost(_progress.Progress(id).Level);
        private int GetUpgradeCost(float x) => (int)(Math.Log10(x + 1) * 2 + x / 6);
    }
}