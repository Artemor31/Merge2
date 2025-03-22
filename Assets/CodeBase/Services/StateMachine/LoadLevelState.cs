using Services.DataServices;
using Services.Infrastructure;
using UI;
using UnityEngine.SceneManagement;

namespace Services.StateMachine
{
    public class LoadLevelState : IExitableState
    {
        private const string GameplaySceneName = "Gameplay";
        private const string MainHallSceneName = "MainHall";
        private const string EntranceSceneName = "Entrance";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly WaveBuilder _waveBuilder;
        private readonly GridService _gridLogicService;
        private readonly WindowsService _windowsService;
        private readonly GameplayDataService _gameplayDataService;

        public LoadLevelState(GameStateMachine gameStateMachine,
                              SceneLoader sceneLoader,
                              WaveBuilder waveBuilder,
                              GridService gridLogicService,
                              WindowsService windowsService,
                              GameplayDataService gameplayDataService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _waveBuilder = waveBuilder;
            _gridLogicService = gridLogicService;
            _windowsService = windowsService;
            _gameplayDataService = gameplayDataService;
        }

        public void Enter()
        {
            _windowsService.Show<LoadingScreen>();
            _sceneLoader.Load(GameplaySceneName, then: Then);
        }

        private void Then()
        {
            var scene = _gameplayDataService.Wave is > 20 and < 40 ? EntranceSceneName : MainHallSceneName;
            _sceneLoader.Load(scene, LoadSceneMode.Additive, then: () => _gameStateMachine.Enter<SetupLevelState>());
        }

        public void Exit()
        {
            _gridLogicService.CreatePlayerField();
            _waveBuilder.BuildEnemyWave(_gameplayDataService.Wave);
            _windowsService.Close<LoadingScreen>();
        }
    }
}