namespace Data
{
    public interface IRepository<TData> : IRepository
    {
        public void Save(TData data);
        public TData Restore();
    }

    public interface IRepository
    {
        
    }
}