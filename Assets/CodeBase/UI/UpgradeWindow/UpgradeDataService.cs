using System.Collections.Generic;
using System.Linq;
using Databases;
using Gameplay.Units;
using Services;
using Services.Infrastructure;
using Services.SaveProgress;
using UnityEngine;

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
            int cost = CalculateCostForLevel(upgrade.Level + 1);
            
            if (_persistantDataService.TryBuy(cost))
            {
                upgrade.Level++;
                _saveService.Save(SavePath, _progress);
                return true;
            }
            
            return false;
        }
        
        public int LevelOf(string id) => _progress.Progress(id).Level;
        
        public int CalculateCostForLevel(int n) => Mathf.RoundToInt(0.5f * Mathf.Pow(n, 2) + 5);

        public void IncrementStats(List<Actor> units)
        {
            foreach (Actor unit in units)
            {
                ActorStats stats = unit.Stats;
                float coeff = 1.0f + 0.01f * LevelOf(unit.Data.Mastery.ToString())
                                   + 0.01f * LevelOf(unit.Data.Race.ToString());
                
                stats.Damage *= coeff;
                stats.Health *= coeff;
                unit.Stats = stats;
            }
        }
    }
}