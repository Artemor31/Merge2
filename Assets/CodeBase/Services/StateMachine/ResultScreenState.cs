using System.Linq;
using Services.GridService;
using Services.Infrastructure;
using UI.ResultWindow;
using UnityEngine;

namespace Services.StateMachine
{
    public class ResultScreenState : IState<bool>
    {
        private readonly WindowsService _windowsService;
        private readonly GridDataService _gridDataService;
        private readonly GameplayDataService _gameplayService;
        private readonly WaveBuilder _waveBuilder;
        private readonly PersistantDataService _persistantDataService;

        public ResultScreenState(WindowsService windowsService, 
                                 GridDataService gridDataService,
                                 GameplayDataService gameplayService,
                                 WaveBuilder waveBuilder,
                                 PersistantDataService persistantDataService)
        {
            _windowsService = windowsService;
            _gridDataService = gridDataService;
            _gameplayService = gameplayService;
            _waveBuilder = waveBuilder;
            _persistantDataService = persistantDataService;
        }

        public void Enter(bool isWin)
        {
            _gridDataService.Save();
            
            if (isWin)
            {
                _gameplayService.CompleteLevel();
                _windowsService.Show<WinResultPresenter, ResultData>(CollectRewards());
            }
            else
            {
                _gridDataService.Reset();
                _gameplayService.Reset();
                _windowsService.Show<LoseResultPresenter>();
            }
        }

        private ResultData CollectRewards()
        {
            int sumCoins = 0;
            int value = Random.Range(8, 14);
            int count = _waveBuilder.EnemyUnits.Count(u => u.IsDead);
            for (int i = 0; i < count; i++)
            {
                sumCoins += Random.Range(1, 4);
            }

            _persistantDataService.AddCoins(sumCoins);
            _persistantDataService.AddGems(count);
            _gameplayService.AddCrowns(value);

            return new ResultData
            {
                CrownsValue = value,
                GemsValue = count,
                CoinsValue = sumCoins
            };
        }

        public void Enter()
        {
            
        }
    }
}