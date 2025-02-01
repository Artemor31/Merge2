using System;
using System.Collections.Generic;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Database/TutorTexts", fileName = "TutorTexts", order = 0)]
    public class TutorTexts : Database
    {
        [SerializeField] private List<TextData> _datas;


        public TextData GetData(int id)
        {
            if (_datas.Count > id)
            {
                return _datas[id];
            }

            Debug.LogError("Id not found");
            return new TextData();
        }
    }
    
    [Serializable]
    public struct TextData
    {
        public string RusText;
    }
}