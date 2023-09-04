using System.Collections.Generic;
using CodeBase.NaughtyAttributes_2._1._4.Scripts.Core.DrawerAttributes;
using UnityEngine;

namespace CodeBase.NaughtyAttributes_2._1._4.Scripts.Test
{
    //[CreateAssetMenu(fileName = "TestScriptableObjectA", menuName = "NaughtyAttributes/TestScriptableObjectA")]
    public class _TestScriptableObjectA : ScriptableObject
    {
        [Expandable]
        public List<_TestScriptableObjectB> listB;
    }
}