using CodeBase.LevelData;
using CodeBase.Models;
using CodeBase.Services.SaveService;

namespace CodeBase.Services
{
    public class ProgressService : IService
    {
        public PlayerProgress Progress { get; set; }
        public GameplayModel GameplayModel { get; set; } = new();
        public PlayerModel PlayerModel { get; set; } = new();
        public LevelStaticData StaticData { get; set; }
    }
}