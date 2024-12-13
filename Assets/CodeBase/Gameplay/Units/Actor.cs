using System.Collections.Generic;
using Services.Infrastructure;
using Gameplay.Units.Healths;
using Gameplay.Units.Moving;
using Databases.Data;
using Infrastructure;
using System.Linq;
using UnityEngine;
using Databases;
using System;
using System.Collections;
using Gameplay.Units.Fighting;

namespace Gameplay.Units
{
    public class RangerActor : Actor
    {
        [SerializeField] private Projectile _projectilePrefab;
        private Vector3 Center => transform.position + Vector3.up;
        private Pool<Projectile> _pool;
        private List<Projectile> _projectiles;
        private Actor _target;

        private void OnEnable()
        {
            
            _projectiles = new List<Projectile>();
            _pool = new Pool<Projectile>(5, 3, _projectilePrefab);
        }

        protected override void Tick()
        {
            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                Projectile projectile = _projectiles[i];
                projectile.Tick();
            }
            
            //Animator.PerformAct();
            ResetCooldown();
            transform.LookAt(_target.transform);
            StartCoroutine(DoDamage(_target.transform));
        }
        
        private IEnumerator DoDamage(Transform target)
        {
            yield return new WaitForSeconds(2.5f);
            Projectile projectile = _pool.Get();
            projectile.transform.position = Center;
            projectile.Init(target, Stats.Damage, ProjectileOnHited);
            _projectiles.Add(projectile);
        }

        private void ProjectileOnHited(Projectile hited)
        {
            _projectiles.Remove(hited);
            _pool.Collect(hited);
        }
    }
    
    public class HealerActor : Actor
    {
        [SerializeField] private ParticleSystem _healVfxPrefab;
        private Actor _target;
        private SearchTargetService _targetSearch;
        
        protected override void Tick()
        {
            if (IsDead || _state == UnitState.Idle) return;
            
            if (_target == null)
            {
                _target = _targetSearch.For(this).SelectTargets(Side.Ally).FilterBy(Strategy.OnSameLine).First();
            }

            transform.LookAt(_target.transform);
            SpawnVFX(_target.transform.position);
            _target.ChangeHealth(Stats.Damage, HealthContext.Heal);
            ActTimer = Stats.ActCooldown;
        }
        
        private void SpawnVFX(Vector3 point) => 
            Instantiate(_healVfxPrefab, point, Quaternion.identity).Play();
        
    }
    
    public class WarriorActor : Actor
    {
        private Actor _target;
        private SearchTargetService _searchTarget;

        private void OnEnable()
        {
            _searchTarget = ServiceLocator.Resolve<SearchTargetService>();
        }

        protected override void Tick()
        {
            if (IsDead || _state == UnitState.Idle) return;

            if (_target == null)
            {
                _target = _searchTarget.For(this).SelectTargets(Side.Enemy).FilterBy(Strategy.OnSameLine).First();
            }
        
            if (!CanPerformOn(_target)) return;

            transform.LookAt(_target.transform);
            //Animator.PerformAct();
            _target.ChangeHealth(Stats.Damage, HealthContext.Damage);
            ResetCooldown();
        }
    }

    public class Actor : MonoBehaviour
    {
        public event Action Died;
        public float Health => _health.CurrentRatio;
        public bool IsDead => _health.IsDead;
        public ActorData Data { get; private set; }
        public ActorStats Stats { get; set; }

        [SerializeField] protected Mover _mover;
        [SerializeField] private ActorSkin _view;

        protected UnitState _state = UnitState.Idle;
        private Health _health;
        protected float ActTimer;
        private bool CooldownUp => ActTimer <= 0;
        
        public virtual void Initialize(ActorSkin view, ActorData data, ActorStats stats)
        {
            Data = data;
            Stats = stats;
            _state = UnitState.Idle;
            _view = view;

            ActTimer = 0;
            _health = new Health(stats.Health);
            _mover.Init(view, stats);
            enabled = false;
        }

        private void Update() => Tick();

        public void Unleash()
        {
            enabled = true;
        }

        public void ChangeHealth(float value, HealthContext context)
        {
            _health.ChangeHealth(value, context);
            if (IsDead)
            {
                Died?.Invoke();
            }
        }

        protected virtual void Tick()
        {
            if (CooldownUp == false)
                ActTimer -= Time.deltaTime;
        }
        

        protected bool CanPerformOn(Actor actor) => InRange(actor) && CooldownUp;
        protected float DistanceTo(Actor actor) => Vector3.Distance(transform.position, actor.transform.position);
        protected bool InRange(Actor actor) => DistanceTo(actor) <= Stats.Range;
        protected void ResetCooldown() => ActTimer = Stats.ActCooldown;
        protected void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(transform.position, Stats.Range);
    }
}