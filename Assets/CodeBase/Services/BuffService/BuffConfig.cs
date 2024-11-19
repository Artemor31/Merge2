using System;
using Databases;
using UnityEngine;

namespace Services.BuffService
{
    [Serializable]
    public class BuffConfig
    {
        public Race Race;
        public Mastery Mastery;
        public SerializableMonoScript<BuffBehaviour> Behaviour;
    }
}