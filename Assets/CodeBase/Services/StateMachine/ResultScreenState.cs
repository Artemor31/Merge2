using System.Collections;
using System.Linq;
using Gameplay.Units;
using Services.GridService;
using Services.Infrastructure;
using UI.GameplayWindow;
using UI.ResultWindow;
using UnityEngine;

namespace Services.StateMachine
{
    public struct ResultScreenData
    {
        public readonly bool IsWin;
        public readonly bool Force;
        
        public ResultScreenData(bool isWin, bool force)
        {
            IsWin = isWin;
            Force = force;
        }
    }
    
    public class ResultScreenState : IState<ResultScreenData>
    {
        private readonly WindowsService _windowsService;
        private readonly GridDataService _gridDataService;
        private readonly GameplayDataService _gameplayService;
        private readonly WaveBuilder _waveBuilder;
        private readonly PersistantDataService _persistantDataService;
        private readonly GridLogicService _gridLogicService;
        private readonly ICoroutineRunner _coroutineRunner;

        public ResultScreenState(WindowsService windowsService, 
                                 GridDataService gridDataService,
                                 GameplayDataService gameplayService,
                                 WaveBuilder waveBuilder,
                                 PersistantDataService persistantDataService,
                                 GridLogicService gridLogicService,
                                 ICoroutineRunner coroutineRunner)
        {
            _windowsService = windowsService;
            _gridDataService = gridDataService;
            _gameplayService = gameplayService;
            _waveBuilder = waveBuilder;
            _persistantDataService = persistantDataService;
            _gridLogicService = gridLogicService;
            _coroutineRunner = coroutineRunner;
        }

        public void Enter(ResultScreenData data)
        {
            _windowsService.Close<GameCanvas>();
            _gridLogicService.Dispose();
            _gridDataService.Save();
            _gridDataService.Dispose();
            
            _coroutineRunner.StartCoroutine(ShowEndWindow(data.IsWin, data.Force));
        }

        private IEnumerator ShowEndWindow(bool isWin, bool force)
        {
            yield return force ? null : new WaitForSeconds(1.5f);
            
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
                _windowsService.Show<WinResultPresenter, ResultData>(CollectRewards(true));
            }
            else
            {
                _gridDataService.Reset();
                _gameplayService.Reset();
                _windowsService.Show<LoseResultPresenter, ResultData>(CollectRewards(false));
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