using Gameplay.Units.Healths;
using Databases.Data;
using Infrastructure;
using UnityEngine;
using Databases;
using System;
using Services.GridService;

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

        protected bool CooldownUp => _actTimer <= 0;
        protected Actor Target;
        protected SearchTargetService SearchTarget;
        protected ActorSkin View;
        private float _actTimer;
        private Health _health;

        public virtual void Initialize(ActorSkin view, ActorData data, ActorStats stats)
        {
            Data = data;
            Stats = stats;
            View = view;
            SearchTarget = ServiceLocator.Resolve<SearchTargetService>();

            _health = new Health(stats.Health, stats.Defence);
            _mover.Init(view, stats);
            _actTimer = 0;
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
                _actTimer -= Time.deltaTime;
        }
        
        protected bool CanFindTarget()
        {
            if (NeedNewTarget())
            {
                SearchNewTarget();
            }

            return Target != null;
        }
            
        public void OnMouseDown() => ServiceLocator.Resolve<GridViewService>().OnClicked(this);
        public void OnMouseUp() => ServiceLocator.Resolve<GridViewService>().OnReleased(this);

        public void Dispose() => View.Dispose();
        protected abstract bool NeedNewTarget();
        protected abstract void SearchNewTarget();
        protected void TickActTimer() => _actTimer -= Time.deltaTime;
        protected bool InRange() => DistanceTo(Target) <= Stats.Range;
        protected void ResetCooldown() => _actTimer = Stats.ActCooldown;
        protected void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(transform.position, Stats.Range);
        private float DistanceTo(Actor actor) => Vector3.Distance(transform.position, actor.transform.position);
    }
}