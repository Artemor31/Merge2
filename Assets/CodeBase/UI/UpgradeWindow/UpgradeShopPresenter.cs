using System;
using UnityEngine;

namespace UI.UpgradeWindow
{

    [Serializable]
    public class UpgradeProgress
    {
        public UpgradeProgressPair[] UpgradesProgress;
    }
    
    [Serializable]
    public class UpgradeProgressPair
    {
        public string Id;
        public int Level;
    }

    public class UpgradeShopPresenter : Presenter
    {
        [SerializeField] private UpgradeItemPresenter _itemPresenter;
        [SerializeField] private RectTransform _parent;
    }
}