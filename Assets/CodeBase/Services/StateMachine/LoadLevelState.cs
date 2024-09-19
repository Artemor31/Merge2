using Services.SaveService;

namespace Services.StateMachine
{
    public class LoadLevelState : IExitableState
    {
        private const string GameplaySceneName = "Gameplay";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly WaveBuilder _waveBuilder;
        private readonly GridLogicService _gridLogicService;

        public LoadLevelState(GameStateMachine gameStateMachine,
                              SceneLoader sceneLoader,
                              WaveBuilder waveBuilder,
                              GridLogicService gridLogicService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _waveBuilder = waveBuilder;
            _gridLogicService = gridLogicService;
        }

        public void Enter() => _sceneLoader.Load(GameplaySceneName,
            then: () => _gameStateMachine.Enter<SetupLevelState>());

        public void Exit()
        {
            _gridLogicService.CreatePlayerField();
            _waveBuilder.BuildEnemyWave();
        }
    }
}