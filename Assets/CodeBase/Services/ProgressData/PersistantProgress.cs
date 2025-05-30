﻿using System;
using System.Collections.Generic;
using Databases;

namespace Services.ProgressData
{
    [Serializable]
    public class PersistantProgress : SaveData
    {
        public int Coins = 100;
        public int Gems;
        public int MaxWave;
        public int[] Races;
        public int[] Masteries;
        public List<(Race, Mastery)> Opened = new() {(Race.Human, Mastery.Warrior)};
        public int Keys;

        public override void Serialize()
        {
            var uniqOpened = new List<(Race, Mastery)>();
            foreach (var opened in Opened)
            {
                if (uniqOpened.Contains(opened)) continue;
                uniqOpened.Add(opened);
            }
            
            Races = new int[uniqOpened.Count];
            Masteries = new int[uniqOpened.Count];

            for (int i = 0; i < uniqOpened.Count; i++)
            {
                Races[i] = (int)uniqOpened[i].Item1;
                Masteries[i] = (int)uniqOpened[i].Item2;
            }
        }

        public override void Deserialize()
        {
            Opened = new();
            int min = Math.Min(Races.Length, Masteries.Length);
            for (int i = 0; i < min; i++)
            {
                int race = Races[i];
                int mastery = Masteries[i];
                
                if (Opened.Contains(((Race)race, (Mastery)mastery)))
                {
                    continue;
                }
                
                Opened.Add(((Race)race, (Mastery)mastery));
            }
        }
    }
}