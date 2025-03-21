using Services.StateMachine;

namespace UI.ResultWindow
{
    public class LoseResultPresenter : BaseResultPresenter, IWindowDataReceiver<ResultData>
    {
        protected override void OnNextLevelClicked()
        {
            base.OnNextLevelClicked();
            GameStateMachine.Enter<LoadLevelState>();
        }
    }
}