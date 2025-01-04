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
        public Dictionary<Mastery, bool> Masteries = new()
        {
            {Mastery.Warrior, true}, 
            {Mastery.Ranger, true}, 
            {Mastery.Mage, false}, 
            {Mastery.Assassin, false}
        };
        
        public Dictionary<Race, bool> Races = new()
        {
            {Race.Human, true},
            {Race.Orc, false},
            {Race.Demon, false},
            {Race.Undead, false},
        };

        public void Serialize()
        {
            List<string> ids = Masteries.Select(mastery => mastery.ToString()).ToList();
            ids.AddRange(Races.Select(race => race.ToString()));
        }
    }
}