using System;
using Databases;
using Services.BuffService.Components;
using UnityEngine;

namespace Services.BuffService
{
    [Serializable]
    public class BuffConfig
    {
        public Race Race;
        public Mastery Mastery;
        public string Description;
        public SerializableMonoScript<BuffComponent> Behaviour;
    }
}