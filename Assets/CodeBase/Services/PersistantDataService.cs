﻿using System;
using Services.Infrastructure;

namespace Services
{
    public class PersistantDataService : IService
    {
        private const string SavePath = "PersistantData";
        
        public event Action<int> OnCoinsChanged;
        public int Coins => _progress.Coins;
        
        private readonly SaveService _saveService;
        private readonly PersistantData _progress;

        public PersistantDataService(SaveService saveService)
        {
            _saveService = saveService;
            _progress = _saveService.Restore<PersistantData>(SavePath);
        }

        public void AddCoins(int value)
        {
            _progress.Coins += value;
            OnCoinsChanged?.Invoke(_progress.Coins);
            Save();
        }

        public bool TryBuy(int cost)
        {
            if (_progress.Coins < cost) return false;

            _progress.Coins -= cost;
            OnCoinsChanged?.Invoke(_progress.Coins);
            Save();
            return true;
        }

        private void Save() => _saveService.Save(SavePath, _progress);
    }

    [Serializable]
    public class PersistantData
    {
        public int Coins = 10;
    }
}