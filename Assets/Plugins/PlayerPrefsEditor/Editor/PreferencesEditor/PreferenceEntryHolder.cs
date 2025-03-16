using System.Collections.Generic;
using UnityEngine;

namespace BgTools.PlayerPrefsEditor
{
    [System.Serializable]
    public class PreferenceEntryHolder : ScriptableObject
    {
        public List<PreferenceEntry> userDefList;
        public List<PreferenceEntry> unityDefList;

        private void OnEnable()
        {
            hideFlags = HideFlags.DontSave;
            userDefList ??= new List<PreferenceEntry>();
            unityDefList ??= new List<PreferenceEntry>();
        }

        public void ClearLists()
        {
            userDefList?.Clear();
            unityDefList?.Clear();
        }
    }
}