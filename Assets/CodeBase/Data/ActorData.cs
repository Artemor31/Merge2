using Databases;
using System;

namespace Data
{
    [Serializable]
    public struct ActorData
    {
        public int Level;
        public Race Race;
        public Mastery Mastery;

        public ActorData(int level, Race race, Mastery mastery)
        {
            Level = level;
            Race = race;
            Mastery = mastery;
        }
        
        public static bool operator==(ActorData our,  ActorData other) => our.Level == other.Level && 
                                                                          our.Mastery == other.Mastery && 
                                                                          our.Race == other.Race;
        
        public static bool operator !=(ActorData our, ActorData other) => !(our == other);

        public static ActorData None => new(0, Race.None, Mastery.None);
        public override int GetHashCode() => HashCode.Combine((int)Race, (int)Mastery);
        public override bool Equals(object obj) => obj is ActorData data && Equals(data);
        private bool Equals(ActorData other) => Race == other.Race && Mastery == other.Mastery;
    }
}