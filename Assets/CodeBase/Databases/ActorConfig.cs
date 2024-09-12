using Data;
using Gameplay.Units;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Create ActorConfig", fileName = "ActorConfig", order = 0)]
    public class ActorConfig : ScriptableObject
    {
        public ActorData Data;
        public Sprite Icon;
        public Actor BaseView;
        public GameObject Prefab;
        public string Name;
        public int Cost;
    }
    
    public enum Race
    {
        None = 0,
        Human = 1,
        Orc = 2,
        Undead = 3,
        Demon = 4
    }

    public enum Mastery
    {
        None = 0,
        Warrior = 1,
        Ranger = 2,
        Mage = 3,
        Assasin = 4
    }
}