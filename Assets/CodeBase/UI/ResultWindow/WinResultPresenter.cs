using Gameplay;
using Infrastructure;
using Services;
using Services.StateMachine;

namespace UI.ResultWindow
{
    public class WinResultPresenter : BaseResultPresenter, IWindowDataReceiver<ResultData>
    {
        public override void OnShow()
        {
            base.OnShow();
            ServiceLocator.Resolve<GameplayContainer>().Get<Confetti>().Play();
        }

        protected override void OnNextLevelClicked()
        {
            base.OnNextLevelClicked();
            GameStateMachine.Enter<LoadLevelState>();
        }
    }
}