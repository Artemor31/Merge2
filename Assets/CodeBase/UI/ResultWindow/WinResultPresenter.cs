using Infrastructure;
using Services.StateMachine;

namespace UI.ResultWindow
{
    public class WinResultPresenter : BaseResultPresenter
    {
        public override void SetData<TData>(WindowData data)
        {
            Clear();
            ResultData resultData = (ResultData)data;

            AddReward(Currency.Crown, resultData.CrownsValue.ToString());
            AddReward(Currency.Coin, resultData.CoinsValue.ToString());
            AddReward(Currency.Gem, resultData.GemsValue.ToString());
        }

        protected override void OnNextLevelClicked()
        {
            gameObject.SetActive(false);
            _gameStateMachine.Enter<LoadLevelState>();
        }
    }
}