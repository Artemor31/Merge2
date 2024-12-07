using System;
using UnityEngine;

namespace Infrastructure
{
    [Serializable]
    public class SerializableMonoScript
    {
        [SerializeField] private string _typeName;
        private Type _mType;

        public Type Type
        {
            get
            {
                if (_mType != null) return _mType;
                if (string.IsNullOrEmpty(_typeName)) return null;
                
                _mType = Type.GetType(_typeName);
                return _mType;
            }
            set
            {
                _mType = value;
                _typeName = _mType == null ? "" : _mType.AssemblyQualifiedName;
            }
        }
    }

    [Serializable]
    public class SerializableMonoScript<T> : SerializableMonoScript where T : class
    {
        public T CreateSoInstance()
        {
            var type = Type;
            if (type != null)
                return (T)(object)ScriptableObject.CreateInstance(type);
            return default;
        }

        public T AddToGameObject(GameObject aGo)
        {
            var type = Type;
            return type != null ? (T)(object)aGo.AddComponent(type) : default;
        }
    }
}