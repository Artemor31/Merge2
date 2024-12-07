﻿using System;
using System.Collections.Generic;
using Infrastructure;
using Services.BuffService.Components;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Create BuffsDatabase", fileName = "BuffsDatabase", order = 0)]
    public class BuffsDatabase : Database
    {
        public List<BuffConfig> BuffConfigs;
    }
    
    [Serializable]
    public class BuffConfig
    {
        public Race Race;
        public Mastery Mastery;
        public string Description;
        public SerializableMonoScript<BuffComponent> Behaviour;
    }
}