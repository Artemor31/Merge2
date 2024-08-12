using System;

namespace Services
{
    public interface IUpdateable : IService
    {
        event Action Tick;
    }
}