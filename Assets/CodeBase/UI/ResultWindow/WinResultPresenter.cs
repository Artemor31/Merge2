using Infrastructure;
using Services.StateMachine;

namespace UI.ResultWindow
{
    public class WinResultPresenter : BaseResultPresenter, IWindowDataReceiver<ResultData>
    {
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