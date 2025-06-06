﻿using System;
using System.Linq;
using Databases;
using Infrastructure;
using Services.Infrastructure;
using Services.ProgressData;
using YG;

namespace Services.DataServices
{
    public class PersistantDataService : IService
    {
        private const string SavePath = "PersistantData";
        
        public event Action<int> OnCoinsChanged;
        public event Action<int> OnGemsChanged;
        public event Action<int> OnKeysChanged;
        public event Action<PersistantProgress> OnProgressChanged;

        public int Coins => _progress.Coins;
        public int Gems => _progress.Gems;
        public int MaxWave => _progress.MaxWave;

        private readonly PersistantProgress _progress;
        private readonly SaveService _saveService;

        public PersistantDataService(SaveService saveService)
        {
            _saveService = saveService;
            _progress = _saveService.Restore<PersistantProgress>(SavePath);
        }

        public void AddCurrency(Currency currency, int amount)
        {
            switch (currency)
            {
                case Currency.Coin:
                    AddCoins(amount);
                    break;
                case Currency.Gem:
                    AddGems(amount);
                    break;
            }
        }

        public void AddCoins(int value)
        {
            _progress.Coins += value;
            Save();
            OnCoinsChanged?.Invoke(_progress.Coins);
        }
        
        public void AddGems(int gems)
        {
            _progress.Gems += gems;
            Save();
            OnGemsChanged?.Invoke(_progress.Gems);
        }

        public void TrySetMaxWave(int wave)
        {
            if (_progress.MaxWave >= wave) return;
            
            _progress.MaxWave = wave;
            YG2.SetLeaderboard("BestWave", _progress.MaxWave);
            Save();
        }

        public bool TryBuyCoins(int cost)
        {
            if (_progress.Coins < cost) return false;

            _progress.Coins -= cost;
            Save();
            OnCoinsChanged?.Invoke(_progress.Coins);
            return true;
        }

        public bool TryBuyGems(int cost)
        {
            if (_progress.Gems < cost) return false;

            _progress.Gems -= cost;
            Save();
            OnGemsChanged?.Invoke(_progress.Gems);
            return true;
        }
        
        public bool TryBuyKeys(int i)
        {
            if (_progress.Keys < i) return false;

            _progress.Keys -= i;
            Save();
            OnKeysChanged?.Invoke(_progress.Keys);
            return true;
        }

        private void Save()
        {
            _saveService.Save(SavePath, _progress);
        }

        public void SetOpened((Race, Mastery) data)
        {
            _progress.Opened.Add(data);
            Save();
            OnProgressChanged?.Invoke(_progress);
        }

        public bool IsOpened(ActorData data) => IsOpened(data.Mastery, data.Race);
        public bool IsOpened(Race race) => _progress.Opened.Any(p => p.Item1 == race);
        public bool IsOpened(Mastery mastery) => _progress.Opened.Any(p => p.Item2 == mastery);
        public bool IsOpened(Race race, Mastery mastery) => _progress.Opened.Any(p => p.Item1 == race && p.Item2 == mastery);
        public bool IsOpened(Mastery mastery, Race race) => _progress.Opened.Any(p => p.Item1 == race && p.Item2 == mastery);

        // public bool TryUpRows()
        // {
        //     if (Rows < 4)
        //     {
        //         _progress.OpenedRows++;
        //         Save();
        //         OnProgressChanged?.Invoke(_progress);
        //         return true;
        //     }
        //
        //     return false;
        // }
        //
        // public bool TryUpCrowns()
        // {
        //     if (_progress.BonusCrowns < 20)
        //     {
        //         _progress.BonusCrowns += 4;
        //         Save();
        //         OnProgressChanged?.Invoke(_progress);
        //         return true;
        //     }
        //     

        //     return false;

        // }
    }
}