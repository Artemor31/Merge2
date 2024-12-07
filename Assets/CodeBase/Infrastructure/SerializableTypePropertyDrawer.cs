using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Infrastructure
{
    
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SerializableMonoScript), true)]
    public class SerializableTypePropertyDrawer : PropertyDrawer
    {
        private static readonly Dictionary<Type, MonoScript> MonoScriptCache = new();
        private Type _filterType;

        private static MonoScript GetMonoScript(Type aType)
        {
            if (aType == null) return null;
            if (MonoScriptCache.TryGetValue(aType, out MonoScript script) && script != null)
            {
                return script;
            }

            var scripts = Resources.FindObjectsOfTypeAll<MonoScript>();
            foreach (var s in scripts)
            {
                var type = s.GetClass();
                if (type != null)
                    MonoScriptCache[type] = s;
                if (type == aType)
                    script = s;
            }

            return script;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_filterType == null)
            {
                var fieldType = fieldInfo.FieldType;
                if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    // when used in a List<>, grab the actual type from the generic argument of the List
                    fieldType = fieldInfo.FieldType.GetGenericArguments()[0];
                }
                else if (fieldType.IsArray)
                {
                    // when used in an array, grab the actual type from the element type.
                    fieldType = fieldType.GetElementType();
                }

                if (fieldType.IsGenericType)
                {
                    var types = fieldType.GetGenericArguments();
                    if (types is { Length: 1 })
                        _filterType = types[0];
                }
                else
                    _filterType = typeof(UnityEngine.Object);
            }

            var typeName = property.FindPropertyRelative("_typeName");
            var type = Type.GetType(typeName.stringValue);
            var monoScript = GetMonoScript(type);
            EditorGUI.BeginChangeCheck();
            monoScript = (MonoScript)EditorGUI.ObjectField(position, label, monoScript, typeof(MonoScript), true);
            if (EditorGUI.EndChangeCheck())
            {
                if (monoScript == null)
                    typeName.stringValue = "";
                else
                {
                    var newType = monoScript.GetClass();
                    if (newType != null && _filterType.IsAssignableFrom(newType))
                        typeName.stringValue = newType.AssemblyQualifiedName;
                    else
                        Debug.LogWarning("Dropped type does not derive or implement " + _filterType?.Name);
                }
            }
        }
    }
    
#endif
}