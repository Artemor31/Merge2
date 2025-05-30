using System;
using Infrastructure;
using Services.Infrastructure;
using Services.ProgressData;

namespace Services.DataServices
{
    public class GameplayDataService : IService
    {
        public const string StoryData = "MetaData";
        
        public int Wave => _progress.Wave;
        public ReactiveProperty<int> Coins;
        private readonly SaveService _saveService;
        private GameplayProgress _progress;

        public GameplayDataService(SaveService saveService)
        {
            _saveService = saveService;
            _progress = _saveService.Restore<GameplayProgress>(StoryData);
            Coins = new ReactiveProperty<int>(_progress.Coins);
        }

        public void CompleteLevel()
        {
            _progress.Wave++;
            Save();
        }

        public void AddCrowns(int value)
        {
            _progress.Coins += value;
            Save();
            Coins.Value = _progress.Coins;
        }

        public int GetCostFor(int level) => level * 70;

        public bool TryBuy(int cost)
        {
            if (!(_progress.Coins >= cost)) return false;
            
            _progress.Coins -= cost;
            Save();
            Coins.Value = _progress.Coins;
            return true;
        }

        public void Reset()
        {
            _progress = new GameplayProgress();
            Save();
            Coins.Value = _progress.Coins;
        }

        private void Save() => _saveService.Save(StoryData, _progress);

        private int GetUnitCost(int level) => level switch
        {
            1 => 5,
            2 => 10,
            3 => 20,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}