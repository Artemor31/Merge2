using CodeBase.Gameplay;
using CodeBase.LevelData;
using CodeBase.Services.SaveService;

namespace CodeBase.Services.StateMachine
{
    public class LoadLevelState : IState, IExitableState
    {
        private const string GameplaySceneName = "Gameplay";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly WaveBuilder _waveBuilder;
        private readonly RuntimeDataRepository _runtimeDataRepository;
        private readonly GameFactory _gameFactory;
        private readonly GridService _gridService;

        public LoadLevelState(GameStateMachine gameStateMachine,
                              SceneLoader sceneLoader,
                              WaveBuilder waveBuilder,
                              RuntimeDataRepository runtimeDataRepository,
                              GameFactory gameFactory,
                              GridService gridService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _waveBuilder = waveBuilder;
            _runtimeDataRepository = runtimeDataRepository;
            _gameFactory = gameFactory;
            _gridService = gridService;
        }

        public void Enter() => _sceneLoader.Load(GameplaySceneName,
            then: () => _gameStateMachine.Enter<GameLoopState>());

        public void Exit()
        {
            var platforms = _gameFactory.CreatePlatforms(_runtimeDataRepository.GridSize);
            _runtimeDataRepository.InitPlatforms(platforms);

            _gameFactory.CreateGridView(_gridService);
            _waveBuilder.BuildEnemyWave(_runtimeDataRepository);
            _waveBuilder.BuildPlayerWave(_runtimeDataRepository.GetPlayerUnits());
        }
    }
}