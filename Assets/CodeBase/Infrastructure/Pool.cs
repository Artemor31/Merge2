﻿using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure
{
    public class Pool<T> where T : MonoBehaviour, IPoolable
    {
        private readonly Stack<T> _data;
        private readonly T _prefab;

        public Pool(int capacity, int initAmount, T prefab)
        {
            _prefab = prefab;
            _data = new Stack<T>(capacity);

            for (int i = 0; i < initAmount; i++)
            {
                T instance = CreateInstance();
                _data.Push(instance);
            }
        }
        
        public T Get(Vector3 at)
        {
            if (!_data.TryPop(out T instance))
            {
                instance = CreateInstance();
            }

            instance.transform.position = at;
            instance.Enable();
            return instance;
        }

        public void ToPool(T instance)
        {
            instance.Disable();
            _data.Push(instance);
        }

        private T CreateInstance()
        {
            var instance = Object.Instantiate(_prefab, new Vector3(0, 1000, 0), Quaternion.identity);
            instance.Disable();
            instance.name += Random.Range(int.MinValue, int.MaxValue);
            Object.DontDestroyOnLoad(instance);
            return instance;
        }
    }
}