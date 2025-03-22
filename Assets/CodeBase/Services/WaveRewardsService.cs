using System;
using Databases;
using Services.DataServices;
using Services.Infrastructure;
using Services.ProgressData;
using Services.StateMachine;
using UI.WaveSlider;

namespace Services
{
    public class WaveRewardsService : IService
    {
        private const string SavePath = "WaveRewardData";
        private readonly PersistantDataService _persistantDataService;
        private readonly WaveRewardsDatabase _waveRewardsDatabase;
        private readonly GameplayDataService _gameplayDataService;
        private readonly SaveService _saveService;
        private readonly WindowsService _windowsService;
        private int _lastCollected;

        public WaveRewardsService(PersistantDataService persistantDataService,
                                  DatabaseProvider databaseProvider,
                                  GameStateMachine gameStateMachine,
                                  GameplayDataService gameplayDataService,
                                  SaveService saveService, 
                                  WindowsService windowsService)
        {
            _waveRewardsDatabase = databaseProvider.GetDatabase<WaveRewardsDatabase>();
            _persistantDataService = persistantDataService;
            _gameplayDataService = gameplayDataService;
            _saveService = saveService;
            _windowsService = windowsService;
            gameStateMachine.OnStateChanged += HandleStateChanged;
            _lastCollected = _saveService.Restore<WaveRewardData>(SavePath).LastCollectedWave;
        }

        public (int CurrentWave, RewardData rewardData) GetWaveViewData()
        {
            int current = _gameplayDataService.Wave;
            RewardData rewardData = _waveRewardsDatabase.GetFor(current);
            return (current, rewardData);
        }

        private void HandleStateChanged(IState state)
        {
            if (state is not SetupLevelState) return;

            int wave = _gameplayDataService.Wave;
            if (wave == 0) return;
            
            RewardData rewardData = _waveRewardsDatabase.GetFor(wave, true);
            bool nowWaveToGetReward = wave == rewardData.WaveToGet;
            bool thisRewardNotYetCollected = _lastCollected != rewardData.WaveToGet;

            if (nowWaveToGetReward && thisRewardNotYetCollected)
            {
                _persistantDataService.AddCurrency(rewardData.Type, rewardData.Amount);
                _lastCollected = rewardData.WaveToGet;
                _saveService.Save(SavePath, new WaveRewardData(_lastCollected));
                _windowsService.Show<WaveProgressPopup, RewardData>(rewardData);
            }
        }
    }
    
    [Serializable]
    public class WaveRewardData : SaveData
    {
        public int LastCollectedWave;
        public WaveRewardData(int lastCollected) => LastCollectedWave = lastCollected;
        public WaveRewardData() => LastCollectedWave = 0;
    }
}