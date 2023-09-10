using CodeBase.Services.SaveService;

namespace CodeBase.Services
{
    public interface ISavedProgressReader
    {
        void LoadProgress(PlayerProgress playerProgress);
    }
}