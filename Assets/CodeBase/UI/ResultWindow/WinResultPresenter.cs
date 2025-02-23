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

        public void SetData(ResultData data)
        {
            Clear();
            ResultData = data;

            AddReward(Currency.Crown, ResultData.CrownsValue.ToString());
            AddReward(Currency.Coin, ResultData.CoinsValue.ToString());
            AddReward(Currency.Gem, ResultData.GemsValue.ToString());
        }

        protected override void OnNextLevelClicked()
        {
            base.OnNextLevelClicked();
            GameStateMachine.Enter<LoadLevelState>();
        }
    }
}