namespace Infrastructure
{
    public interface IPoolable
    {
        void Collect();
        void Release();
    }
}