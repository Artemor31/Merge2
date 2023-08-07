using System;

namespace CodeBase.Gameplay.Units
{
    public interface IUnit
    {
        event Action<IUnit> Died;
        UnitId Id { get; }
        float Health { get; }
        void TakeDamage(float damage);
    }
}