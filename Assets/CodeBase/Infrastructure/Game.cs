using CodeBase.Services;
using CodeBase.Services.StateMachine;
using CodeBase.UI;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public Game(SceneLoader sceneLoader, WindowsService windowsService)
        {
            var stateMachine = new GameStateMachine(sceneLoader, windowsService);
            ServiceLocator.Bind(stateMachine);
            
            stateMachine.Enter<BootstrapState>();
        }
    }
}