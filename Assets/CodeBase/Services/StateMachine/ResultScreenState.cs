using System.Linq;
using Gameplay.Units;
using Services.GridService;
using Services.Infrastructure;
using UI.GameplayWindow;
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
        private readonly GridLogicService _gridLogicService;

        public ResultScreenState(WindowsService windowsService, 
                                 GridDataService gridDataService,
                                 GameplayDataService gameplayService,
                                 WaveBuilder waveBuilder,
                                 PersistantDataService persistantDataService,
                                 GridLogicService gridLogicService)
        {
            _windowsService = windowsService;
            _gridDataService = gridDataService;
            _gameplayService = gameplayService;
            _waveBuilder = waveBuilder;
            _persistantDataService = persistantDataService;
            _gridLogicService = gridLogicService;
        }

        public void Enter(bool isWin)
        {
            _windowsService.Close<GameCanvas>();
            _gridLogicService.Dispose();
            _gridDataService.Save();
            
            foreach (Actor actor in _waveBuilder.EnemyUnits)
            {
                actor.gameObject.SetActive(false);
                actor.Dispose();
            }

            foreach (Actor actor in _gridDataService.PlayerUnits)
            {
                actor.gameObject.SetActive(false);
                actor.Dispose();
            }
            
            if (isWin)
            {
                _gameplayService.CompleteLevel();
                _windowsService.Show<WinResultPresenter, ResultData>(CollectRewards(isWin));
            }
            else
            {
                _gridDataService.Reset();
                _gameplayService.Reset();
                _windowsService.Show<LoseResultPresenter, ResultData>(CollectRewards(isWin));
            }
        }

        private ResultData CollectRewards(bool isWin)
        {
            int sumCoins = 0;
            int crownsValue = isWin ? Random.Range(8, 14) : 0; 
            int count = _waveBuilder.EnemyUnits.Count(u => u.IsDead);
            
            for (int i = 0; i < count; i++)
            {
                sumCoins += Random.Range(1, 4);
            }

            _persistantDataService.AddCoins(sumCoins);
            _persistantDataService.AddGems(count);
            _gameplayService.AddCrowns(crownsValue);

            return new ResultData
            {
                CrownsValue = crownsValue,
                GemsValue = count,
                CoinsValue = sumCoins
            };
        }

        public void Enter()
        {
            
        }
    }
}