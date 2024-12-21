using System;

namespace Services.SaveProgress
{
    [Serializable]
    public class UpgradeProgress
    {
        public UpgradeProgressPair[] UpgradesProgress;
        public UpgradeProgress() => UpgradesProgress = new UpgradeProgressPair[0];
    }
    
    [Serializable]
    public class UpgradeProgressPair
    {
        public string Id;
        public int Level;
    }
}