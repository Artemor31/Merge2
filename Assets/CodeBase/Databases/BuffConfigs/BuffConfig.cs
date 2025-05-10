using Gameplay.Units;
using UnityEngine;
using YG;

namespace Databases.BuffConfigs
{
    public abstract class BuffConfig : ScriptableObject
    {
        public string Name => GetBuffName();
        public Sprite Icon;
        public Race Race;
        public Mastery Mastery;
        public string Description;
        public string DescriptionEn;
        public string DescriptionTr;
        public bool ForAllies;
        public abstract void ApplyTo(Actor actor, int buffLevel);

        public string GetDescription() => YG2.lang switch
        {
            "ru" => Description,
            "tr" => DescriptionTr,
            _ => DescriptionEn
        };

        public string GetBuffName()
        {
            string lang = YG2.lang;
            
            if (Race != Race.None)
            {
                if (lang == "ru")
                {
                    return Race switch
                    {
                        Race.Human => "Люди",
                        Race.Orc => "Орк",
                        Race.Undead => "Нежить",
                        Race.Demon => "Демон",
                        _ => string.Empty
                    };
                }

                if (lang == "tr")
                {
                    return Race switch
                    {
                        Race.Human => "İnsan",
                        Race.Orc => "Ortlar",
                        Race.Undead => "Ölümsüz",
                        Race.Demon => "Şeytan",
                        _ => string.Empty
                    };
                }

                return Race switch
                {
                    Race.Human => "Human",
                    Race.Orc => "Ork",
                    Race.Undead => "Undead",
                    Race.Demon => "Demon",
                    _ => string.Empty
                };
            }

            if (lang == "ru")
            {
                return Mastery switch
                {
                    Mastery.Warrior => "Воин",
                    Mastery.Ranger => "Стрелок",
                    Mastery.Mage => "Маг",
                    Mastery.Assassin => "Убийца",
                    _ => string.Empty
                };
            }

            if (lang == "tr")
            {
                return Mastery switch
                {
                    Mastery.Warrior => "Savaşçı",
                    Mastery.Ranger => "Korucu",
                    Mastery.Mage => "Büyücü",
                    Mastery.Assassin => "Suikastçı",
                    _ => string.Empty
                };
            }
            
            return Mastery switch
            {
                Mastery.Warrior => "Warrior",
                Mastery.Ranger => "Ranger",
                Mastery.Mage => "Mage",
                Mastery.Assassin => "Assassin",
                _ => string.Empty
            };

        }
    }
}