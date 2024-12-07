using System;

namespace UI.ShopWindow
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
}