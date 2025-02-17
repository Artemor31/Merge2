using Gameplay.Units.Healths;
using Databases.Data;
using Infrastructure;
using UnityEngine;
using Databases;
using System;
using Services.GridService;
using UnityEngine.AI;

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
        [SerializeField] protected NavMeshAgent _agent;
        [SerializeField] protected BoxCollider _collider;

        protected bool CooldownUp => _actTimer <= 0;
        protected Actor Target;
        protected SearchTargetService SearchTarget;
        protected GridViewService GridViewService;
        protected ActorSkin View;
        private float _actTimer;
        private Health _health;

        public virtual void Initialize(ActorSkin view, ActorData data, ActorStats stats)
        {
            Data = data;
            Stats = stats;
            View = view;
            SearchTarget = ServiceLocator.Resolve<SearchTargetService>();
            GridViewService = ServiceLocator.Resolve<GridViewService>();

            _health = new Health(stats.Health, stats.Defence);
            _mover.Init(view, stats);
            _actTimer = 0;
            enabled = false;
        }

        public void ChangeHealth(float value, HealthContext context)
        {
            _health.ChangeHealth(value, context);
            View.ChangeHealth(_health.CurrentRatio, context);
            if (IsDead)
            {
                View.Die();
                Died?.Invoke();
            }
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
        protected abstract void SearchNewTarget();

        public void Enable() => _agent.enabled = _collider.enabled = true;
        public void Disable() => _agent.enabled = _collider.enabled = false;
        public void DisableCollider() => _collider.enabled = false;
        public void Unleash() => enabled = true;
        //public void OnMouseDown() => GridViewService.OnClicked(this);
        //public void OnMouseUp() => GridViewService.OnReleased(this);
        //public void OnMouseEnter() => GridViewService.OnHovered(this);
        //public void Dispose() => View.Dispose();
        private float DistanceTo(Actor actor) => Vector3.Distance(transform.position, actor.transform.position);
        protected void TickActTimer() => _actTimer -= Time.deltaTime;
        protected bool InRange() => DistanceTo(Target) <= Stats.Range;
        protected void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(transform.position, Stats.Range);

        protected void LookAtTarget()
        {
            
            Quaternion targetRotation = Quaternion.LookRotation(Target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
        }
        protected void ResetCooldown()
        {
            float part = Stats.ActCooldown * 0.2f;
            _actTimer = Stats.ActCooldown + UnityEngine.Random.Range(-part, part);
        }

        public void Dispose()=> View.Dispose();
    }
}