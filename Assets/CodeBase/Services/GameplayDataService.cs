﻿using System;
using Services.Infrastructure;
using Services.SaveProgress;

namespace Services
{
    public class GameplayDataService : IService
    {
        public event Action<int> OnMoneyChanged;
        public int Wave => _progress.Wave;
        public int Gold => _progress.Money;

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

        public void AddMoney(int value)
        {
            _progress.Money += value;
            OnMoneyChanged?.Invoke(Gold);
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
}