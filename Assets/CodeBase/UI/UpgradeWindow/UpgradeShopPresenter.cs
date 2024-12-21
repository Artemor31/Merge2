using Infrastructure;
using Services.Resources;
using UnityEngine;

namespace UI.UpgradeWindow
{
    public class UpgradeShopPresenter : Presenter
    {
        [SerializeField] private UpgradeItemPresenter _prefab;
        [SerializeField] private RectTransform _parent;
        private AssetsProvider _assetsProvider;

        public override void Init()
        {
            _assetsProvider = ServiceLocator.Resolve<AssetsProvider>();
        }
    }
}