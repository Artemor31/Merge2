using System;
using Databases;

namespace Services.SaveService
{
    [Serializable]
    public struct ActorData
    {
        public static ActorData None => new(Race.None, Mastery.None, 0);
        
        public Race Race;
        public Mastery Mastery;
        public int Level;

        public ActorData(Race race, Mastery mastery, int level)
        {
            Race = race;
            Mastery = mastery;
            Level = level;
        }

        public override bool Equals(object obj) => obj is ActorData data && Equals(data);
        private bool Equals(ActorData other) => Race == other.Race && Mastery == other.Mastery;
        public override int GetHashCode() => HashCode.Combine((int)Race, (int)Mastery);
    }
}