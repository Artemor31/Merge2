using System.Collections.Generic;
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
        private readonly RuntimeDataProvider _runtimeDataProvider;
        private readonly GameFactory _gameFactory;
        private readonly MergeService _mergeService;
        private readonly LevelDataProvider _levelDataProvider;

        public LoadLevelState(GameStateMachine gameStateMachine,
                              SceneLoader sceneLoader, 
                              WaveBuilder waveBuilder,
                              RuntimeDataProvider runtimeDataProvider, 
                              GameFactory gameFactory,
                              MergeService mergeService,
                              LevelDataProvider levelDataProvider)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _waveBuilder = waveBuilder;
            _runtimeDataProvider = runtimeDataProvider;
            _gameFactory = gameFactory;
            _mergeService = mergeService;
            _levelDataProvider = levelDataProvider;
        }

        public void Enter() => _sceneLoader.Load(GameplaySceneName,
            then: () => _gameStateMachine.Enter<GameLoopState>());

        public void Exit()
        {
            var platforms = _gameFactory.CreatePlatforms(_runtimeDataProvider.GridSize);
            _runtimeDataProvider.SetupPlatforms(platforms);
            
            EnemySpawner spawner = _gameFactory.CreateEnemySpawner();
            GridView grid = _gameFactory.CreateGridView(_mergeService);
            
            _levelDataProvider.AddItem(spawner);
            _levelDataProvider.AddItem(grid);
            
            _waveBuilder.BuildEnemyWave(spawner, _runtimeDataProvider);
            _waveBuilder.BuildPlayerWave(spawner, _runtimeDataProvider.GetPlayerUnits());
        }
    }
}