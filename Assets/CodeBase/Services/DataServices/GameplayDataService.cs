using System;
using Infrastructure;
using Services.Infrastructure;
using Services.ProgressData;

namespace Services.DataServices
{
    public class GameplayDataService : IService
    {
        public const string StoryData = "StoryData";
        
        public int Wave => _progress.Wave;
        public ReactiveProperty<int> Crowns;
        private readonly SaveService _saveService;
        private GameplayProgress _progress;

        public GameplayDataService(SaveService saveService)
        {
            _saveService = saveService;
            _progress = _saveService.Restore<GameplayProgress>(StoryData);
            Crowns = new ReactiveProperty<int>(_progress.Crowns);
        }

        public void CompleteLevel()
        {
            _progress.Wave++;
            Save();
        }

        public void AddCrowns(int value)
        {
            _progress.Crowns += value;
            Save();
            Crowns.Value = _progress.Crowns;
        }

        public void DecreaseCrowns(int value)
        {
            _progress.Crowns -= value;
            Save();
            Crowns.Value = _progress.Crowns;
        }
        
        public int GetCostFor(int level) => level * 70;

        public bool TryBuy(int cost)
        {
            if (!(_progress.Crowns >= cost)) return false;
            
            _progress.Crowns -= cost;
            Save();
            Crowns.Value = _progress.Crowns;
            return true;
        }

        public void Reset()
        {
            _progress = new GameplayProgress();
            Save();
            Crowns.Value = _progress.Crowns;
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