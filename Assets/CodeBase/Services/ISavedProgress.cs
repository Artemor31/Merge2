namespace CodeBase.Services
{
    public interface ISavedProgress : ISavedProgressReader
    {
        void UpdateProgress(PlayerProgress playerProgress);
    }
}