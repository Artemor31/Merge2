using System;
using System.Collections.Generic;
using Services.Infrastructure;
using UnityEngine;

namespace Services
{
    public class GameplayContainer : IService
    {
        private readonly Dictionary<Type, object> _objects = new();
        public void Add<TMono>(TMono mono) where TMono : MonoBehaviour
        {
            if (_objects.ContainsKey(typeof(TMono)))
            {
                _objects[typeof(TMono)] = mono;
            }
            else
            {
                _objects.Add(mono.GetType(), mono);
            }
        }

        public TMono Get<TMono>() where TMono : MonoBehaviour => _objects[typeof(TMono)] as TMono;
    }
}