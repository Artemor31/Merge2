using System;
using System.Collections.Generic;
using Services.Buffs;
using Services.GridService;
using Services.Infrastructure;
using UI.UpgradeWindow;
using UnityEngine;

namespace Services.StateMachine
{
    public class GameStateMachine : IService
    {
        public event Action<IState> OnStateChanged;
        public IState Current { get; private set; }
        
        private readonly Dictionary<Type, IState> _states;

        public GameStateMachine(SceneLoader sceneLoader,
                                WindowsService windowsService,
                                WaveBuilder waveBuilder,
                                GridDataService gridDataService,
                                GridDataService service,
                                GameplayDataService gameplayData,
                                GridViewService gridLogicService,
                                BuffService buffService,
                                UpgradeDataService upgradeDataService,
                                PersistantDataService persistantDataService,
                                TutorialService tutorialService,
                                ICoroutineRunner coroutineRunner,
                                ProjectileService projectileService, GameplayContainer gameplayContainer)
        {
            _states = new Dictionary<Type, IState>
            {
                {typeof(BootstrapState), new BootstrapState(this, sceneLoader, windowsService)},
                {typeof(MenuState), new MenuState(tutorialService, windowsService)},
                {
                    typeof(LoadLevelState),
                    new LoadLevelState(this, sceneLoader, waveBuilder, gridLogicService, windowsService, gameplayData)
                },
                {typeof(SetupLevelState), new SetupLevelState(windowsService, gridLogicService, gameplayContainer)},
                {
                    typeof(GameLoopState),
                    new GameLoopState(this, gridDataService, gameplayData, waveBuilder, buffService, upgradeDataService,
                        windowsService)
                },
                {
                    typeof(ResultScreenState),
                    new ResultScreenState(windowsService, service, gameplayData, waveBuilder, persistantDataService,
                        gridLogicService, coroutineRunner, this, projectileService)
                },
            };
        }

        public void Enter<T>() where T : IState
        {
            if (Current?.GetType() == typeof(T)) return;

            if (Current is IExitableState state)
                state.Exit();

            Current = _states[typeof(T)];
            Current.Enter();
            OnStateChanged?.Invoke(Current);
        }

        public void Enter<T, TParam>(TParam param) where T : IState
        {
            if (Current?.GetType() == typeof(T))
            {
                Debug.LogError("Same state enter error");
            }

            if (Current is IExitableState state)
                state.Exit();

            Current = _states[typeof(T)];
            ((IState<TParam>)Current).Enter(param);
            OnStateChanged?.Invoke(Current);
        }
    }
}