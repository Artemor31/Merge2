using CodeBase.Gameplay;
using CodeBase.Infrastructure;
using CodeBase.LevelData;
using Object = UnityEngine.Object;

namespace CodeBase.Services.StateMachine
{
    public class LoadLevelState : IState, IExitableState
    {
        private const string GameplaySceneName = "Gameplay";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly WaveBuilder _waveBuilder;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, WaveBuilder waveBuilder)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _waveBuilder = waveBuilder;
        }

        public void Enter() => _sceneLoader.Load(GameplaySceneName, 
                 then: () => _gameStateMachine.Enter<GameLoopState>());

        public void Exit()
        {
            var progressService = ServiceLocator.Resolve<ProgressService>();
            progressService.StaticData = Object.FindObjectOfType<LevelStaticData>();
            progressService.StaticData.GridView.Init(ServiceLocator.Resolve<MergeService>());
            int currentWave = progressService.Progress.Wave;
            
            _waveBuilder.BuildEnemyWave(progressService.StaticData, progressService.GameplayModel, currentWave);
            _waveBuilder.BuildPlayerWave(progressService.StaticData, progressService.GameplayModel);
        }
    }
}