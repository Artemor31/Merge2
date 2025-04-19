using Services.Infrastructure;
using UnityEngine;

namespace Databases
{
    public class Database : ScriptableObject, IService
    {
        public virtual void Cache(){}
    }
}