using System;
using Databases.Data;
using Gameplay.Units;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Database/ActorConfig", fileName = "ActorConfig", order = 0)]
    public class ActorConfig : ScriptableObject
    {
        public int Cost;
        public string Name;
        public ActorData Data;
        public ActorStats Stats;
        public ActorViewData ViewData;
    }

    [Serializable]
    public class ActorViewData
    {
        public Sprite Icon;
        public Actor BaseView;
        public ActorSkin Skin;
    }

    [Serializable]
    public struct ActorStats
    {
        public float Health;
        public float Damage;
        public float CritChance;
        public float CritValue;
        public float Vampirism;
        public float Defence;
        public float Range;
        public float ActCooldown;
        public float MoveSpeed;
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
        Assassin = 4
    }
}