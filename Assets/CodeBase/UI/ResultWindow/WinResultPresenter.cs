using Gameplay;
using Infrastructure;
using Services;
using Services.StateMachine;
using YG;

namespace UI.ResultWindow
{
    public class WinResultPresenter : BaseResultPresenter, IWindowDataReceiver<ResultData>
    {
        public override void OnShow()
        {
            base.OnShow();
            ServiceLocator.Resolve<GameplayContainer>().Get<Confetti>().Play();
        }

        protected override string GetHeader(int level) => YG2.lang switch
        {
            "ru" => "Уровень пройден!",
            "en" => "Level complete!",
            "tr" => "Level complete!",
            _ => "Level complete!"
        };

        protected override void OnNextLevelClicked()
        {
            base.OnNextLevelClicked();
            GameStateMachine.Enter<LoadLevelState>();
        }
    }
}