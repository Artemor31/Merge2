using UnityEngine;

namespace Services.Infrastructure
{
    public class AssetsProvider : IService
    {
        public T Load<T>(string path) where T : Object => 
            UnityEngine.Resources.Load<T>(path);

        public T[] LoadAll<T>(string path) where T : Object => 
            UnityEngine.Resources.LoadAll<T>(path);
    }
}