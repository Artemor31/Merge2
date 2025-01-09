using Infrastructure;
using Services.StateMachine;

namespace UI.ResultWindow
{
    public class WinResultPresenter : BaseResultPresenter
    {

        public override void SetData<TData>(WindowData data)
        {
            Clear();
            ResultData = (ResultData)data;

            AddReward(Currency.Crown, ResultData.CrownsValue.ToString());
            AddReward(Currency.Coin, ResultData.CoinsValue.ToString());
            AddReward(Currency.Gem, ResultData.GemsValue.ToString());
        }

        protected override void OnNextLevelClicked()
        {
            gameObject.SetActive(false);
            GameStateMachine.Enter<LoadLevelState>();
        }
    }
}