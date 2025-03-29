using System.Collections;
using System.Linq;
using Gameplay.Units;
using Services.DataServices;
using Services.Infrastructure;
using UI.GameplayWindow;
using UI.ResultWindow;
using UI.WorldSpace;
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
        private readonly GridService _gridLogicService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly GameStateMachine _gameStateMachine;
        private readonly ProjectileService _projectileService;

        public ResultScreenState(WindowsService windowsService,
                                 GridDataService gridDataService,
                                 GameplayDataService gameplayService,
                                 WaveBuilder waveBuilder,
                                 PersistantDataService persistantDataService,
                                 GridService gridLogicService,
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
            CloseUI();
            ClearField();
            _persistantDataService.TrySetMaxWave(_gameplayService.Wave);
            _coroutineRunner.StartCoroutine(ShowEndWindow(data.IsWin, data.Force));
        }

        private void ClearField()
        {
            _gridDataService.Save();
            _gridLogicService.Dispose();
            _projectileService.ClearField();
        }

        private void CloseUI()
        {
            _windowsService.Close<GameCanvas>();
            _windowsService.Close<GameplayPresenter>();
            _windowsService.Close<ShopPresenter>();
        }

        private IEnumerator ShowEndWindow(bool isWin, bool force)
        {
            yield return force ? null : new WaitForSeconds(0.7f);
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
            int count = _waveBuilder.EnemyUnits.Count(u => u.IsDead);

            int sumCoins = 0;
            int crownsValue = 0;
            int gems = 0;
            if (isWin)
            {
                crownsValue = Random.Range(150, 450) * (_persistantDataService.Crowns / 100);
                sumCoins = Random.Range(40, 100);
                gems = (int)(count * 0.7f);
            }
            else if (count > 0)
            {
                crownsValue = Random.Range(100, 200) * (_persistantDataService.Crowns / 100);
                sumCoins = count * 5;
                gems = count;
            }

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