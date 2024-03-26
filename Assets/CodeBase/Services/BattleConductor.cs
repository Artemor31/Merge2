using System.Collections.Generic;
using CodeBase.Gameplay;
using CodeBase.Gameplay.Units;
using CodeBase.Models;
using CodeBase.Services.StateMachine;

namespace CodeBase.Services
{
    public class BattleConductor : IService
    {
        private GameplayModel Model => _progressService.GameplayModel;
        private readonly ProgressService _progressService;
        private readonly WaveBuilder _waveBuilder;
        private bool _battling;

        public BattleConductor(ProgressService progressService, WaveBuilder waveBuilder)
        {
            _progressService = progressService;
            _waveBuilder = waveBuilder;
        }

        public void SetState(GameState state)
        {
            switch (state)
            {
                case GameState.Waiting:
                    _waveBuilder.BuildEnemyWave(_progressService.StaticData, _progressService.GameplayModel,0);
                    return;
                case GameState.Processing:
                    StartBattle();
                    return;
            }
        }

        private void StartBattle()
        {
        }

        private void SetTargets(IEnumerable<Actor> units, List<Actor> candidates)
        {
            foreach (Actor unit in units)
                unit.Unleash(candidates);
        }
    }
}