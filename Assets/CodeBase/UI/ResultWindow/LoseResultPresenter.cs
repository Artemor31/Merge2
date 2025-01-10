using Infrastructure;
using Services.StateMachine;

namespace UI.ResultWindow
{
    public class LoseResultPresenter : BaseResultPresenter
    {
        public override void SetData<TData>(WindowData data)
        {
            Clear();
            ResultData = (ResultData)data;

            if (ResultData.CoinsValue > 0)
                AddReward(Currency.Coin, ResultData.CoinsValue.ToString());
            
            if (ResultData.GemsValue > 0)
                AddReward(Currency.Gem, ResultData.GemsValue.ToString());
        }
        
        protected override void OnNextLevelClicked()
        {
            base.OnNextLevelClicked();
            GameStateMachine.Enter<MenuState>();
        }
    }
}