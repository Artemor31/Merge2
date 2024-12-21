using System;
using Databases;

namespace Services.SaveProgress
{
    [Serializable]
    public class PersistantProgress
    {
        public int Coins;
        public string[] OpenIds;

        public PersistantProgress()
        {
            Coins = 10;
            OpenIds = new[]
            {
                $"{Race.Human.ToString()}{Mastery.Warrior}", 
                $"{Race.Human.ToString()}{Mastery.Ranger}"
            };
        }
    }
}