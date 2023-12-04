using UnityEditor;
using UnityEngine;

namespace MyCustomEditor
{
    [CustomPropertyDrawer(typeof(ReadOnlyInspectorAttribute))]
    
    public class ReadOnlyInspectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = true;
        }
    }
}
