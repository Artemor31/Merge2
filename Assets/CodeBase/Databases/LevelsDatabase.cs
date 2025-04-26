using System;
using System.Collections.Generic;
using Databases.BuffConfigs;
using UnityEngine;
using YG;

namespace Databases
{
    [CreateAssetMenu(menuName = "Database/LevelsDatabase", fileName = "LevelsDatabase", order = 0)]
    public class LevelsDatabase : Database
    {
        [SerializeField] private List<LevelData> _levels;
        public List<LevelData> Levels => _levels;
    }

    [Serializable]
    public class LevelData
    {
        [SerializeField] private string NameRu;
        [SerializeField] private string NameEn;
        [SerializeField] private string NameTr;

        public Sprite Image;
        public WavesDatabase Levels;
        public List<BuffConfig> DropList;

        public string GetName() => YG2.lang switch
        {
            "ru" => NameRu,
            "en" => NameEn,
            "tr" => NameTr,
            _ => throw new Exception()
        };
    }
}