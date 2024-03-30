﻿using UnityEngine;

namespace CodeBase.Services
{
    public class AssetsProvider : IService
    {
        public T Load<T>(string path) where T : Object => 
            Resources.Load<T>(path);

        public T[] LoadAll<T>(string path) where T : Object => 
            Resources.LoadAll<T>(path);
    }
}