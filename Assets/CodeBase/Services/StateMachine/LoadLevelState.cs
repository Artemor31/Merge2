using Gameplay;
using LevelData;
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
        private readonly GridService _gridService;

        public LoadLevelState(GameStateMachine gameStateMachine,
                              SceneLoader sceneLoader,
                              WaveBuilder waveBuilder,
                              GridDataService gridDataService,
                              GameFactory gameFactory,
                              GridService gridService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _waveBuilder = waveBuilder;
            _gridDataService = gridDataService;
            _gameFactory = gameFactory;
            _gridService = gridService;
        }

        public void Enter() => _sceneLoader.Load(GameplaySceneName,
            then: () => _gameStateMachine.Enter<GameLoopState>());

        public void Exit()
        {
            var platforms = _gameFactory.CreatePlatforms(_gridDataService.GridSize);
            _gridDataService.InitPlatforms(platforms);

            _gameFactory.CreateGridView(_gridService);
            _waveBuilder.BuildEnemyWave();
        }
    }
}