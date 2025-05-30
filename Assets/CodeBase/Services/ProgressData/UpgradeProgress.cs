﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.ProgressData
{
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

    [Serializable]
    public class UpgradeProgress : SaveData
    {
        public List<UpgradeProgressPair> Pairs = new();

        public UpgradeProgressPair GetPair(string id)
        {
            UpgradeProgressPair firstOrDefault = Pairs.FirstOrDefault(p => p.Id == id);
            if (firstOrDefault == null)
            {
                UpgradeProgressPair pair = new(id, 1);
                Pairs.Add(pair);
                return pair;
            }

            return firstOrDefault;
        }
    }
}