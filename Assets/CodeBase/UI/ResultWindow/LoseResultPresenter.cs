using Services.StateMachine;

namespace UI.ResultWindow
{
    public class LoseResultPresenter : BaseResultPresenter
    {
        protected override void OnNextLevelClicked()
        {
            gameObject.SetActive(false);
            GameStateMachine.Enter<MenuState>();
        }
    }
}