﻿using System.Collections;
using Gameplay.Units;
using UnityEngine;

namespace Databases.BuffConfigs
{
    [CreateAssetMenu(menuName = "Create StatBuffConfig", fileName = "BuffConfig", order = 0)]
    public class StatBuffConfig : BuffConfig
    {
        public Stat BuffStat;
        public float BuffValue;
        private WaitForSeconds _waitForSeconds;

        public override void ApplyTo(Actor actor, int buffLevel)
        {
            ActorStats stats = actor.Stats;
            float value = BuffValue * buffLevel;
            switch (BuffStat)
            {
                case Stat.HealthMaxAdd:
                    float bonus = stats.Health * value;
                    stats.Health += bonus;
                    actor.Stats = stats;
                    actor.ChangeHealth(bonus, HealthContext.Heal);
                    break;
                case Stat.DamageAdd:
                    stats.Damage += stats.Damage * value;
                    actor.Stats = stats;
                    break;
                case Stat.CritChanceAdd:
                    stats.CritChance += value;
                    actor.Stats = stats;
                    break;
                case Stat.VampirismAdd:
                    stats.Vampirism += value;
                    actor.Stats = stats;
                    break;
                case Stat.DefenceAdd:
                    stats.Defence += value;
                    actor.Stats = stats;
                    break;
                case Stat.RangeAdd:
                    if (stats.Range >= 4)
                    {
                        stats.Range += value;
                        actor.Stats = stats;
                    }
                    break;
                case Stat.DefenceRemove:
                    stats.Defence -= value;
                    actor.Stats = stats;
                    break;
                case Stat.SlowEnemies:
                    stats.MoveSpeed *= 1 - value;
                    actor.Stats = stats;
                    break;
                case Stat.HealthRegenAdd:
                    float regen = actor.Stats.Health * value;
                    actor.StartCoroutine(HealthRegen(actor, regen));
                    break;
            }
        }

        private IEnumerator HealthRegen(Actor actor, float regen)
        {
            _waitForSeconds = new WaitForSeconds(0.5f);
            var regenHalf = regen / 2;
            while (!actor.IsDead)
            {
                actor.ChangeHealth(regenHalf, HealthContext.Heal);
                yield return _waitForSeconds;
            }
        }

        public enum Stat
        {
            HealthMaxAdd = 0,
            HealthRegenAdd = 1,
            DamageAdd = 2,
            CritChanceAdd = 3,
            VampirismAdd = 4,
            DefenceAdd = 5,
            DefenceRemove = 6,
            RangeAdd = 7,
            SlowEnemies = 8
        }
    }
}