using System.Collections.Generic;
using CodeBase.NaughtyAttributes_2._1._4.Scripts.Core.DrawerAttributes;
using UnityEngine;

namespace CodeBase.NaughtyAttributes_2._1._4.Scripts.Test
{
    //[CreateAssetMenu(fileName = "NaughtyScriptableObject", menuName = "NaughtyAttributes/_NaughtyScriptableObject")]
    public class _NaughtyScriptableObject : ScriptableObject
    {
        [Expandable]
        public List<_TestScriptableObjectA> listA;
    }
}
