using CodeBase.LevelData;
using CodeBase.Models;
using CodeBase.Services.SaveService;
using UnityEngine;

namespace CodeBase.Services
{
    public class ProgressService : IService
    {
        public PlayerProgress Progress { get; set; }
        public GameplayModel GameplayModel { get; set; } = new();
        public PlayerModel PlayerModel { get; set; } = new();
        public LevelStaticData StaticData { get; set; }

        public ProgressService()
        {
            int wave = PlayerPrefs.GetInt("level", 0);
            Progress = new PlayerProgress
            {
                Wave = wave
            };
        }
    }
}