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
        private readonly GridViewService _gridLogicService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly GameStateMachine _gameStateMachine;
        private readonly ProjectileService _projectileService;

        public ResultScreenState(WindowsService windowsService, 
                                 GridDataService gridDataService,
                                 GameplayDataService gameplayService,
                                 WaveBuilder waveBuilder,
                                 PersistantDataService persistantDataService,
                                 GridViewService gridLogicService,
                                 ICoroutineRunner coroutineRunner,
                                 GameStateMachine gameStateMachine,
                                 ProjectileService projectileService)
        {
            _windowsService = windowsService;
            _gridDataService = gridDataService;
            _gameplayService = gameplayService;
            _waveBuilder = waveBuilder;
            _persistantDataService = persistantDataService;
            _gridLogicService = gridLogicService;
            _coroutineRunner = coroutineRunner;
            _gameStateMachine = gameStateMachine;
            _projectileService = projectileService;
        }

        public void Enter(ResultScreenData data)
        {
            _windowsService.Close<GameCanvas>();
            _windowsService.Close<GameplayPresenter>();
            _windowsService.Close<ShopPresenter>();
            _gridDataService.Save();
            _gridLogicService.Dispose();
            _persistantDataService.TrySetMaxWave(_gameplayService.Wave);
            _projectileService.ClearField();
            _coroutineRunner.StartCoroutine(ShowEndWindow(data.IsWin, data.Force));
        }

        private IEnumerator ShowEndWindow(bool isWin, bool force)
        {
            yield return force ? null : new WaitForSeconds(1.2f);
            DisposeActors();

            if (force)
            {
                _gameStateMachine.Enter<MenuState>();
                yield break; 
            }
            
            if (isWin)
            {
                _gameplayService.CompleteLevel();
                _windowsService.Show<WinResultPresenter, ResultData>(CollectRewards(true));
            }
            else
            {
                _windowsService.Show<LoseResultPresenter, ResultData>(CollectRewards(false));
            }
        }

        private void DisposeActors()
        {
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
        }

        private ResultData CollectRewards(bool isWin)
        {
            int sumCoins = 0;
            int count = _waveBuilder.EnemyUnits.Count(u => u.IsDead);
            int levelsSum = _waveBuilder.EnemyUnits.Sum(u => u.Data.Level);

            int crownsValue;
            if (true)
            {
                if (isWin)
                {
                    if (_gameplayService.Wave < 20)
                    {
                        // old approach
                        crownsValue = Random.Range(7, 12) * 2;
                    }
                    else
                    {
                        crownsValue = levelsSum + count / 2;
                    }
                }
                else
                {
                    crownsValue = 2 * count + levelsSum / 10;
                }
            }
            
            for (int i = 0; i < count; i++)
            {
                sumCoins += Random.Range(1, 4);
            }

            var gems = count > 0 ? Random.Range(1, 6) : 0;

            crownsValue += _persistantDataService.Crowns;
            
            _persistantDataService.AddCoins(sumCoins);
            _persistantDataService.AddGems(gems);
            _gameplayService.AddCrowns(crownsValue);

            return new ResultData
            {
                CrownsValue = crownsValue,
                GemsValue = gems,
                CoinsValue = sumCoins   
            };
        }

        public void Enter()
        {
            
        }
    }
}