using CodeBase.NaughtyAttributes_2._1._4.Scripts.Core.DrawerAttributes;
using UnityEngine;

namespace CodeBase.NaughtyAttributes_2._1._4.Scripts.Test
{
    public class ExpandableTest : MonoBehaviour
    {
        // See #294
        public int precedingField = 5;

        [Expandable]
        public ScriptableObject obj0;

        public ExpandableScriptableObjectNest1 nest1;
    }

    [System.Serializable]
    public class ExpandableScriptableObjectNest1
    {
        [Expandable]
        public ScriptableObject obj1;

        public ExpandableScriptableObjectNest2 nest2;
    }

    [System.Serializable]
    public class ExpandableScriptableObjectNest2
    {
        [Expandable]
        public ScriptableObject obj2;
    }
}
