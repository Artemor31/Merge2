using Gameplay;
using Gameplay.LevelItems;
using Services.SaveService;

namespace Services.StateMachine
{
    public class LoadLevelState : IState, IExitableState
    {
        private const string GameplaySceneName = "Gameplay";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly WaveBuilder _waveBuilder;
        private readonly GridDataService _gridDataService;
        private readonly GameFactory _gameFactory;
        private readonly GridViewService _gridViewService;

        public LoadLevelState(GameStateMachine gameStateMachine,
                              SceneLoader sceneLoader,
                              WaveBuilder waveBuilder,
                              GridDataService gridDataService,
                              GameFactory gameFactory,
                              GridViewService gridViewService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _waveBuilder = waveBuilder;
            _gridDataService = gridDataService;
            _gameFactory = gameFactory;
            _gridViewService = gridViewService;
        }

        public void Enter() => _sceneLoader.Load(GameplaySceneName,
            then: () => _gameStateMachine.Enter<GameLoopState>());

        public void Exit()
        {
            _gridDataService.SpawnPlatforms();
            _gameFactory.CreateGridView(_gridViewService);
            _waveBuilder.BuildEnemyWave();
        }
    }
}