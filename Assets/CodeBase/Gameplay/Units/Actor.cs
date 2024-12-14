using Gameplay.Units.Healths;
using Gameplay.Units.Moving;
using Databases.Data;
using Infrastructure;
using UnityEngine;
using Databases;
using System;

namespace Gameplay.Units
{
    public abstract class Actor : MonoBehaviour
    {
        public event Action Died;
        public float Health => _health.CurrentRatio;
        public bool IsDead => _health.IsDead;
        public ActorData Data { get; private set; }
        public ActorStats Stats { get; set; }

        [SerializeField] protected Mover _mover;

        protected Actor Target;
        protected SearchTargetService SearchTarget;
        protected ActorSkin View;
        protected float ActTimer;
        protected bool CooldownUp => ActTimer <= 0;
        private Health _health;

        public virtual void Initialize(ActorSkin view, ActorData data, ActorStats stats)
        {
            ActTimer = 0;
            Data = data;
            Stats = stats;
            View = view;
            SearchTarget = ServiceLocator.Resolve<SearchTargetService>();

            _health = new Health(stats.Health);
            _mover.Init(view, stats);
            enabled = false;
        }

        private void Update() => Tick();

        public void Unleash() => enabled = true;

        public void ChangeHealth(float value, HealthContext context)
        {
            _health.ChangeHealth(value, context);
            View.ChangeHealth(_health.CurrentRatio);
            if (IsDead)
            {
                View.Die();
                Died?.Invoke();
            }
        }

        protected virtual void Tick()
        {
            if (CooldownUp == false)
                ActTimer -= Time.deltaTime;
        }
        
        protected bool CanFindTarget()
        {
            if (NeedNewTarget())
            {
                SearchNewTarget();
            }

            return Target != null;
        }

        protected abstract bool NeedNewTarget();
        protected virtual void SearchNewTarget(){}
        protected void TickActTimer() => ActTimer -= Time.deltaTime;
        protected float DistanceTo(Actor actor) => Vector3.Distance(transform.position, actor.transform.position);
        protected bool InRange() => DistanceTo(Target) <= Stats.Range;
        protected void ResetCooldown() => ActTimer = Stats.ActCooldown;
        protected void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(transform.position, Stats.Range);
        public void Dispose() => View.Dispose();
    }
}