using Services.StateMachine;

namespace UI.ResultWindow
{
    public class WinResultPresenter : BaseResultPresenter
    {
        protected override void OnNextLevelClicked()
        {
            gameObject.SetActive(false);
            _gameStateMachine.Enter<LoadLevelState>();
        }
    }
}