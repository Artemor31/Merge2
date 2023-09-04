using CodeBase.NaughtyAttributes_2._1._4.Scripts.Core.DrawerAttributes;
using UnityEngine;

namespace CodeBase.NaughtyAttributes_2._1._4.Scripts.Test
{
    //[CreateAssetMenu(fileName = "TestScriptableObjectB", menuName = "NaughtyAttributes/TestScriptableObjectB")]
    public class _TestScriptableObjectB : ScriptableObject
    {
        [MinMaxSlider(0, 10)]
        public Vector2Int slider;
    }
}