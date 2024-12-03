using Laio;
using UnityEditor;
using UnityEngine;

namespace LaioEditor
{

    [CustomPropertyDrawer(typeof(Probability))]
    public class ProbabilityDrawer : PropertyDrawer
    {

        private SerializedProperty _probabilty;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (_probabilty == null)
                _probabilty = property.FindPropertyRelative("_probability");
            return EditorGUI.GetPropertyHeight(_probabilty);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.LabelField(property.displayName);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Slider(_probabilty, 0, 100);
            EditorGUILayout.EndHorizontal();
        }
    }
}
