using Services.GridService;
using Services.Infrastructure;
using UI.ResultWindow;
using UnityEngine;

namespace Services.StateMachine
{
    public class ResultScreenState : IState<bool>
    {
        private readonly WindowsService _windowsService;
        private readonly GridDataService _gridData;
        private readonly GameplayDataService _gameplayService;
        private readonly WaveBuilder _waveBuilder;
        private readonly PersistantDataService _persistantDataService;

        public ResultScreenState(WindowsService windowsService, 
                                 GridDataService gridData,
                                 GameplayDataService gameplayService,
                                 WaveBuilder waveBuilder,
                                 PersistantDataService persistantDataService)
        {
            _windowsService = windowsService;
            _gridData = gridData;
            _gameplayService = gameplayService;
            _waveBuilder = waveBuilder;
            _persistantDataService = persistantDataService;
        }

        public void Enter(bool isWin)
        {
            _gridData.Save();
            
            if (isWin)
            {
                _gameplayService.CompleteLevel();
                _windowsService.Show<WinResultPresenter, ResultData>(CollectRewards());
            }
            else
            {
                _gridData.Reset();
                _windowsService.Show<LoseResultPresenter>();
            }
        }

        private ResultData CollectRewards()
        {
            int sumCoins = 0;
            int sumGems = 0;
            for (int i = 0; i < _waveBuilder.EnemyUnits.Count; i++)
            {
                sumCoins += Random.Range(1, 4);
                sumGems += 1;
            }

            _persistantDataService.AddCoins(sumCoins);
            _persistantDataService.AddGems(sumGems);
            _gameplayService.AddCrowns(8);

            return new ResultData
            {
                CrownsValue = 8,
                GemsValue = sumGems,
                CoinsValue = sumCoins
            };
        }

        public void Enter()
        {
            
        }
    }
}