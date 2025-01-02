using Services.Infrastructure;

namespace Services.StateMachine
{
    public class BootstrapState : IState
    {
        private const string MenuSceneName = "Menu";
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly WindowsService _windowsService;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, WindowsService windowsService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _windowsService = windowsService;
        }

        public void Enter()
        {
            _sceneLoader.Load(MenuSceneName, () => _gameStateMachine.Enter<MenuState>());
            
            _windowsService.InitWindows();
        }
    }
}