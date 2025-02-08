using System;
using Gameplay.Units;
using UnityEngine;

namespace Databases.BuffConfigs
{
    public abstract class BuffConfig : ScriptableObject
    {
        public string Name => GetBuffName();
        public Sprite Icon;
        public Race Race;
        public Mastery Mastery;
        public string Description;
        public bool ForAllies;
        public abstract void ApplyTo(Actor actor, int level);

        public string GetBuffName()
        {
            if (Race != Race.None)
            {
                return Race switch
                {
                    Race.Human => "Люди",
                    Race.Orc => "Орк",
                    Race.Undead => "Нежить",
                    Race.Demon => "Демон",
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            return Mastery switch
            {
                Mastery.Warrior => "Воин",
                Mastery.Ranger => "Стрелок",
                Mastery.Mage => "Маг",
                Mastery.Assassin => "Убийца",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}