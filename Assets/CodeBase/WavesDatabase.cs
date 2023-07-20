using System.Collections.Generic;
using UnityEngine;

namespace CodeBase
{
    [CreateAssetMenu(menuName = "Create WavesDatabase", fileName = "WavesDatabase", order = 0)]
    public class WavesDatabase : ScriptableObject
    {
        public List<WaveData> Datas;
    }
}