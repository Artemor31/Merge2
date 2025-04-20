using Services.Infrastructure;
using YG;

namespace UI.GameplayWindow
{
    public class AdsActorRollView : ActorRollView
    {
        protected override void TryBuyUnit()
        {
            if (GridService.CanAddUnit == false) return;
            YG2.RewardedAdvShow(AdsId.GetUnit, GetUnitForAds);
        }

        public override void Hide() => gameObject.SetActive(false);

        protected override void Reset() => gameObject.SetActive(true);

        private void GetUnitForAds()
        {
            GridService.TryCreatePlayerUnit();
            Hide();
        }
    }
}