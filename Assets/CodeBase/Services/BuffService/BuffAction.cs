using Databases;

namespace Services.BuffService
{
    public abstract class BuffAction
    {
        public abstract Race Race { get; }
        public abstract Mastery Mastery { get; }

        public abstract string Description { get; }
        public abstract void ApplyBuff();
    }
}