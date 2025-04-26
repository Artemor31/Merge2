using Services.StateMachine;
using YG;

namespace UI.ResultWindow
{
    public class LoseResultPresenter : BaseResultPresenter, IWindowDataReceiver<ResultData>
    {
        protected override string GetHeader(int level) => YG2.lang switch
        {
            "ru" => $"Уровень {level}",
            "en" => $"Level {level}",
            "tr" => $"Level {level}",
            _ => $"Level {level}"
        };

        protected override void OnNextLevelClicked()
        {
            base.OnNextLevelClicked();
            GameStateMachine.Enter<LoadLevelState>();
        }
    }
}