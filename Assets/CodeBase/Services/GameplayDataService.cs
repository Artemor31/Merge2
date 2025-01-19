using System;
using Services.Infrastructure;
using Services.SaveProgress;

namespace Services
{
    public class GameplayDataService : IService
    {
        private const string PlayerData = "GameplayData";
        
        public event Action<int> OnCrownsChanged;
        public int Wave => _progress.Wave;
        public int Crowns => _progress.Crowns;
        private readonly SaveService _saveService;
        private readonly PersistantDataService _persistantDataService;
        private GameplayProgress _progress;

        public GameplayDataService(SaveService saveService, PersistantDataService persistantDataService)
        {
            _saveService = saveService;
            _persistantDataService = persistantDataService;
            _progress = _saveService.Restore<GameplayProgress>(PlayerData);
        }

        public void CompleteLevel()
        {
            _progress.Wave++;
            Save();
        }

        public void AddCrowns(int value)
        {
            _progress.Crowns += value;
            OnCrownsChanged?.Invoke(Crowns);
            Save();
        }

        public bool TryBuy(int cost)
        {
            if (!(_progress.Crowns >= cost)) return false;
            
            _progress.Crowns -= cost;
            OnCrownsChanged?.Invoke(_progress.Crowns);
            Save();
            return true;
        }

        public void Reset()
        {
            _progress = new GameplayProgress();
            _progress.Crowns += _persistantDataService.Crowns; 
            Save();
            OnCrownsChanged?.Invoke(_progress.Crowns);
        }

        private void Save() => _saveService.Save(PlayerData, _progress);
    }
}