using System;

namespace CodeBase.Services.SaveService
{
    public class PlayerProgressService : IService
    {
        public event Action<int> OnMoneyChanged;
        private readonly PlayerDataRepository _progress;
        
        public int Wave => _progress.Wave;
        public int Money => _progress.Money;
        
        public PlayerProgressService(RepositoryProvider repositoryProvider) => _progress = repositoryProvider.GetRepo<PlayerDataRepository>();
        public void CompleteLevel() => _progress.NextWave();
        
        public bool TryBuy(int cost)
        {
            if (!(_progress.Money >= cost)) return false;
            
            _progress.Money -= cost;
            OnMoneyChanged?.Invoke(_progress.Money);
            return true;
        }
    }
}