using System;
using System.Linq;
using Databases;
using Infrastructure;
using Services.Infrastructure;
using Services.Resources;
using Services.SaveProgress;
using UnityEngine;
using YG;

namespace Services
{
    public class PersistantDataService : IService
    {
        private const string SavePath = "PersistantData";
        
        public event Action<int> OnCoinsChanged;
        public event Action<int> OnGemsChanged;
        public event Action<PersistantProgress> OnProgressChanged;

        public int Coins => _progress.Coins;
        public int Gems => _progress.Gems;
        public int Rows => _progress.OpenedRows;
        public bool RowsAtMax => _progress.OpenedRows == 4;
        public int Crowns => _progress.BonusCrowns;
        public bool CrownsAtMax => _progress.BonusCrowns == 30;
        public int MaxWave => _progress.MaxWave;
        public int NextWave => _rewardsDatabase.NextWave(MaxWave);

        private readonly PersistantProgress _progress;
        private readonly SaveService _saveService;
        private readonly WaveRewardsDatabase _rewardsDatabase;

        public PersistantDataService(SaveService saveService, DatabaseProvider databaseProvider)
        {
            _saveService = saveService;
            _progress = _saveService.Restore<PersistantProgress>(SavePath);
            _rewardsDatabase = databaseProvider.GetDatabase<WaveRewardsDatabase>();
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
            if (_progress.MaxWave < wave)
            {
                _progress.MaxWave = wave;
                YG2.SetLeaderboard("BestWave", _progress.MaxWave);
                var reward = _rewardsDatabase.GetFor(_progress.MaxWave);
                
                switch (reward.Type)
                {
                    case Currency.Coin:
                        AddCoins(reward.Amount);
                        break;
                    case Currency.Gem:
                        AddGems(reward.Amount);
                        break;
                }                
                
                Save();
            }
        }

        public (Currency Currency, int Amount) RewardCurrentReward()
        {
            // TODO do multy type reward, not only currency
            var reward = _rewardsDatabase.GetFor(_progress.MaxWave);
            return (reward.Type, reward.Amount);
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

        private void Save()
        {
            _progress.Serialize();
            _saveService.Save(SavePath, _progress);
        }

        public void SetOpened((Race, Mastery) data)
        {
            _progress.Opened.Add(data);
            Save();
            OnProgressChanged?.Invoke(_progress);
        }

        public bool IsOpened(Mastery mastery) => _progress.Opened.Any(p => p.Item2 == mastery);
        public bool IsOpened(Race race) => _progress.Opened.Any(p => p.Item1 == race);
        public bool IsOpened(Mastery mastery, Race race) => _progress.Opened.Any(p => p.Item1 == race && p.Item2 == mastery);
        public bool IsOpened(Race race, Mastery mastery) => _progress.Opened.Any(p => p.Item1 == race && p.Item2 == mastery);

        public bool TryUpRows()
        {
            if (Rows < 4)
            {
                _progress.OpenedRows++;
                Save();
                OnProgressChanged?.Invoke(_progress);
                return true;
            }

            return false;
        }

        public bool TryUpCrowns()
        {
            if (_progress.BonusCrowns < 30)
            {
                _progress.BonusCrowns += 5;
                Save();
                OnProgressChanged?.Invoke(_progress);
                return true;
            }
            
            return false;
        }
    }
}