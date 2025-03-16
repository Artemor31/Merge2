namespace Infrastructure
{
    public interface IPoolable
    {
        void Disable();
        void Enable();
    }
}