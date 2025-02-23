using System.Collections.Generic;
using Databases;
using System;

namespace Services.SaveProgress
{
    [Serializable]
    public class PersistantProgress : SaveData
    {
        public int Coins;
        public int Gems;
        public int OpenedRows;
        public int BonusCrowns;
        public int MaxWave;
        public int[] Races;
        public int[] Masteries;

        public List<(Race, Mastery)> Opened;

        public PersistantProgress()
        {
            Coins = 1000;
            Gems = 200;
            OpenedRows = 1;
            Opened = new List<(Race, Mastery)>
            {
                (Race.Human, Mastery.Warrior), 
            };
            BonusCrowns = 0;
            MaxWave = 0;
        }

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