using System;

namespace Services.SaveService
{
    public class PlayerDataService : IService
    {
        public event Action<int> OnMoneyChanged;
        public int Wave => _progress.Wave;
        public int Money => _progress.Money;

        private const string PlayerData = "PlayerData";
        private readonly PlayerData _progress;
        private readonly SaveService _saveService;

        public PlayerDataService(SaveService saveService)
        {
            _saveService = saveService;
            _progress = _saveService.Restore<PlayerData>(PlayerData);
        }

        public void CompleteLevel()
        {
            _progress.Wave++;
            Save();
        }

        public void AddMoney(int value)
        {
            _progress.Money += value;
            OnMoneyChanged?.Invoke(Money);
            Save();
        }

        public bool TryBuy(int cost)
        {
            if (!(_progress.Money >= cost)) return false;
            
            _progress.Money -= cost;
            OnMoneyChanged?.Invoke(_progress.Money);
            Save();
            return true;
        }

        private void Save() => _saveService.Save(PlayerData, _progress);
    }
    
    [Serializable]
    public class PlayerData
    {
        public int Money = 100;
        public int Wave;
        public int Coins;
    }
}