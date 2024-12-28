using System.Collections.Generic;
using System.Linq;
using Databases;
using System;

namespace Services.SaveProgress
{
    [Serializable]
    public class PersistantProgress
    {
        public int Coins;
        public List<Mastery> Masteries;
        public List<Race> Races;

        public PersistantProgress()
        {
            Coins = 10;
            Masteries = new List<Mastery> {Mastery.Ranger, Mastery.Warrior};
            Races = new List<Race> {Race.Human};
        }

        public void Serialize()
        {
            List<string> ids = Masteries.Select(mastery => mastery.ToString()).ToList();
            ids.AddRange(Races.Select(race => race.ToString()));
        }
    }
}