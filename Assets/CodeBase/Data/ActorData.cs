using System;
using Databases;

namespace Data
{
    [Serializable]
    public struct ActorData
    {
        public static ActorData None => new(0, Mastery.None, Race.None);

        public Mastery Mastery;
        public int Level;
        public Race Race;
        
        public ActorData(int level, Mastery mastery, Race race)
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