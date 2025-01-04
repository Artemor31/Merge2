using System;
using Services.Infrastructure;
using Services.SaveProgress;

namespace Services
{
    public class GameplayDataService : IService
    {
        public event Action<int> OnCrownsChanged;
        public int Wave => _progress.Wave;
        public int Crowns => _progress.Crowns;

        private const string PlayerData = "GameplayData";
        private readonly GameplayProgress _progress;
        private readonly SaveService _saveService;

        public GameplayDataService(SaveService saveService)
        {
            _saveService = saveService;
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

        private void Save() => _saveService.Save(PlayerData, _progress);
    }
}