using System;
using CodeBase.Services;

namespace CodeBase.Infrastructure
{
    public interface IUpdateable : IService
    {
        event Action Tick;
    }
}