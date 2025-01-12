﻿using System;
using System.Linq;
using Databases;
using Services.Infrastructure;
using Services.SaveProgress;

namespace Services
{
    public class PersistantDataService : IService
    {
        private const string SavePath = "PersistantData";
        
        public event Action<int> OnCoinsChanged;
        public event Action<int> OnGemsChanged;
        
        public int Coins => _progress.Coins;
        public int Gems => _progress.Gems;

        private readonly PersistantProgress _progress;
        private readonly SaveService _saveService;

        public PersistantDataService(SaveService saveService)
        {
            _saveService = saveService;
            _progress = _saveService.Restore<PersistantProgress>(SavePath);
        }

        public void AddCoins(int value)
        {
            _progress.Coins += value;
            OnCoinsChanged?.Invoke(_progress.Coins);
            Save();
        }
        
        public void AddGems(int gems)
        {
            _progress.Gems += gems;
            OnGemsChanged?.Invoke(_progress.Gems);
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

        private void Save()
        {
            _progress.Serialize();
            _saveService.Save(SavePath, _progress);
        }

        public void SetOpened((Race, Mastery) data)
        {
            _progress.Opened.Add(data);
            Save();
        }

        public bool IsOpened(Mastery mastery) => _progress.Opened.Any(p => p.Item2 == mastery);
        public bool IsOpened(Race race) => _progress.Opened.Any(p => p.Item1 == race);
        public bool IsOpened(Mastery mastery, Race race) => _progress.Opened.Any(p => p.Item1 == race && p.Item2 == mastery);
    }
}