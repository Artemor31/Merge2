using System;

namespace CodeBase.Services
{
    public interface IUpdateable : IService
    {
        event Action Tick;
    }
}