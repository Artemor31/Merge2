using CodeBase.Services;
using CodeBase.StateMachine;
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