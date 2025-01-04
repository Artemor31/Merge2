using System.Collections.Generic;
using System.Linq;
using Databases;
using System;

namespace Services.SaveProgress
{
    [Serializable]
    public class PersistantProgress
    {
        public int Coins = 10;
        public List<Mastery> Masteries = new() {Mastery.Ranger, Mastery.Warrior};
        public List<Race> Races = new() {Race.Human};

        public void Serialize()
        {
            List<string> ids = Masteries.Select(mastery => mastery.ToString()).ToList();
            ids.AddRange(Races.Select(race => race.ToString()));
        }
    }
}