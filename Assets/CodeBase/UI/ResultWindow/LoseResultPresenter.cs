using Infrastructure;
using Services.StateMachine;

namespace UI.ResultWindow
{
    public class LoseResultPresenter : BaseResultPresenter, IWindowDataReceiver<ResultData>
    {
        public void SetData(ResultData data)
        {
            Clear();
            ResultData = data;

            if (ResultData.CoinsValue > 0)
                AddReward(Currency.Coin, ResultData.CoinsValue.ToString());
            
            if (ResultData.GemsValue > 0)
                AddReward(Currency.Gem, ResultData.GemsValue.ToString());
        }
        
        protected override void OnNextLevelClicked()
        {
            base.OnNextLevelClicked();

            if (GridDataService.InStory)
            {
                GameStateMachine.Enter<LoadLevelState>();
            }
            else
            {
                GameStateMachine.Enter<MenuState>();
            }
        }
    }
}