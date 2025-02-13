using System.Collections;
using Gameplay.Units;
using Gameplay.Units.Healths;
using UnityEngine;

namespace Databases.BuffConfigs
{
    [CreateAssetMenu(menuName = "Create StatBuffConfig", fileName = "BuffConfig", order = 0)]
    public class StatBuffConfig : BuffConfig
    {
        public Stat BuffStat;
        public float BuffValue;

        public override void ApplyTo(Actor actor, int level)
        {
            ActorStats stats = actor.Stats;
            float value = BuffValue * level;
            switch (BuffStat)
            {
                case Stat.HealthMaxAdd:
                    float bonus = stats.Health * value;
                    stats.Health += bonus;
                    actor.Stats = stats;
                    actor.ChangeHealth(bonus, HealthContext.Heal);
                    break;
                case Stat.DamageAdd:
                    stats.Damage *= value;
                    break;
                case Stat.CritChanceAdd:
                    stats.CritChance *= value;
                    break;
                case Stat.VampirismAdd:
                    stats.Vampirism += value;
                    break;
                case Stat.DefenceAdd:
                    stats.Defence += value;
                    break;
                case Stat.RangeAdd:
                    if (stats.Range >= 2)
                        stats.Range += value;
                    break;
                case Stat.DefenceRemove:
                    stats.Defence -= value;
                    break;
                case Stat.SlowEnemies:
                    stats.MoveSpeed -= value;
                    break;
                case Stat.HealthRegenAdd:
                    float regen = actor.Stats.Health * value / 2;
                    actor.StartCoroutine(HealthRegen(actor, regen));
                    break;
            }

            actor.Stats = stats;
        }

        private IEnumerator HealthRegen(Actor actor, float regen)
        {
            while (!actor.IsDead)
            {
                actor.ChangeHealth(regen, HealthContext.Heal);
                yield return new WaitForSeconds(0.5f);
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