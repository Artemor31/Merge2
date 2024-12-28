using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.SaveProgress
{
    [Serializable]
    public class UpgradeProgress
    {
        public List<UpgradeProgressPair> Pairs = new();

        public UpgradeProgressPair Progress(string id)
        {
            UpgradeProgressPair firstOrDefault = Pairs.FirstOrDefault(p => p.Id == id);
            if (firstOrDefault != null) return firstOrDefault;
            
            UpgradeProgressPair pair = new(id, 1);
            Pairs.Add(pair);
            return pair;
        }
    }

    [Serializable]
    public class UpgradeProgressPair
    {
        public string Id;
        public int Level;

        public UpgradeProgressPair(string id, int i)
        {
            Id = id;
            Level = i;
        }
    }
}