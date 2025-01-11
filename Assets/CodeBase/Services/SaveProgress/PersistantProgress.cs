using System.Collections.Generic;
using Databases;
using System;

namespace Services.SaveProgress
{
    [Serializable]
    public class PersistantProgress : SaveData
    {
        public int Coins = 1000;
        public int Gems = 5;
        public int[] Races;
        public int[] Masteries;

        public List<(Race, Mastery)> Opened = new()
        {
            (Race.Human, Mastery.Warrior), 
            (Race.Human, Mastery.Ranger)
        };

        public override void Serialize()
        {
            Races = new int[Opened.Count];
            Masteries = new int[Opened.Count];

            for (int i = 0; i < Opened.Count; i++)
            {
                Races[i] = (int)Opened[i].Item1;
                Masteries[i] = (int)Opened[i].Item2;
            }
        }

        public override void Deserialize()
        {
            for (int i = 0; i < Races.Length; i++)
            {
                Opened[i] = ((Race)Races[i], (Mastery)Masteries[i]);
            }
        }
    }
}