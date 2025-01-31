using System;
using System.Collections.Generic;
using System.Linq;
using Services;
using Services.GridService;
using Services.Infrastructure;
using UnityEngine;

namespace Gameplay.Units
{
    public class SearchTargetService : IService
    {
        private readonly GridDataService _dataService;
        private readonly WaveBuilder _waveBuilder;
        private Actor _actor;

        public SearchTargetService(GridDataService dataService, WaveBuilder waveBuilder)
        {
            _dataService = dataService;
            _waveBuilder = waveBuilder;
        }

        public SearchTargetService For(Actor actor)
        {
            _actor = actor;
            return this;
        }

        public ActorCollection SelectTargets(Side side)
        {
            bool isPlayers = _dataService.PlayerUnits.Contains(_actor);
            
            return side switch
            {
                Side.Enemy => new ActorCollection(_actor, isPlayers ? _waveBuilder.EnemyUnits : _dataService.PlayerUnits),
                Side.Ally => new ActorCollection(_actor, isPlayers ? _dataService.PlayerUnits : _waveBuilder.EnemyUnits),
                Side.All => new ActorCollection(_actor, _dataService.PlayerUnits.Concat(_waveBuilder.EnemyUnits).ToList()),
                Side.None => throw new ArgumentOutOfRangeException(nameof(side), side, null),
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };
        }

        public struct ActorCollection
        {
            private readonly Actor _owner;
            private List<Actor> _actors;

            public ActorCollection(Actor owner, List<Actor> actors)
            {
                _owner = owner;
                _actors = actors == null ? new List<Actor>() : actors.ToList();
                _actors.Remove(_owner);
            }

            public List<Actor> FilterBy(Strategy strategy)
            {
                if (_actors.Count <= 0) return _actors;
                
                _actors = strategy switch
                {
                    Strategy.OnSameLine => OnSameLine(_owner.transform, _actors),
                    Strategy.Closest => Closest(_owner.transform, _actors),
                    Strategy.MostDamaged => MostDamaged(_actors),
                    _ => _actors
                };

                return _actors;
            }
            
            private List<Actor> OnSameLine(Transform owner, List<Actor> enemies)
            {
                int tries = 1;
                float positionX = owner.position.x;
                var actualTargets = new List<Actor>();
                while (actualTargets.Count < 1 && tries < 9)
                {
                    foreach (var candidate in enemies)
                    {
                        if (!candidate.IsDead && Mathf.Abs(candidate.transform.position.x - positionX) < tries)
                        {
                            actualTargets.Add(candidate);
                        }
                    }

                    if (actualTargets.Count > 0) break;
                    tries *= 2;
                }

                return actualTargets;
            }

            private List<Actor> Closest(Transform owner, List<Actor> actors)
            {
                Actor target = null;
                float currentDistance = float.MaxValue;
                foreach (Actor actor in actors)
                {
                    if (actor.IsDead) continue;
                    
                    float distance = DistanceTo(owner, actor);
                    if (distance < currentDistance)
                    {
                        target = actor;
                        currentDistance = distance;
                    }
                }
                
                return target == null ? new List<Actor>() : new List<Actor>(){target};
            }

            private List<Actor> MostDamaged(List<Actor> actors)
            {
                Actor target = actors.First();
                foreach (Actor actor in actors)
                {
                    if (!actor.IsDead && actor.Health < target.Health)
                    {
                        target = actor;
                    }
                }

                return new List<Actor> {target};
            }

            private float DistanceTo(Transform owner,Actor actor) => 
                Vector3.Distance(owner.position, actor.transform.position);
        }
    }
    
    public enum Strategy
    {
        None = 0,
        OnSameLine = 1,
        Closest = 2,
        MostDamaged = 3,
    }

    public enum Side
    {
        None = 0,
        Ally = 1,
        Enemy = 2,
        All = 3
    }
}