using System;

namespace Services.Infrastructure
{
    public interface IUpdateable : IService
    {
        event Action Tick;
    }
}