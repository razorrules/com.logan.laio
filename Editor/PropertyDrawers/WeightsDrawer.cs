using System;
using UnityEditor;
using UnityEngine;
using Laio;

namespace LaioEditor
{
    [CustomPropertyDrawer(typeof(Weights<>))]
    public class WeightsDrawer : PropertyDrawer
    {

        private const float FOLDOUT_HEIGHT = 16f;

        private SerializedProperty _weightCount;
        private SerializedProperty _values;
        private SerializedProperty _valuesWeight;
        private Type _type;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (_values == null)
            {
                _values = property.FindPropertyRelative("values");
                _valuesWeight = property.FindPropertyRelative("valuesWeight");
                _type = fieldInfo.FieldType.GenericTypeArguments[0];
                _weightCount = property.FindPropertyRelative("WeightCount");
            }

            float height = FOLDOUT_HEIGHT;

            if (property.isExpanded)
            {
                if (_values.arraySize != _weightCount.intValue ||
                    _valuesWeight.arraySize != _weightCount.intValue)
                {
                    _values.arraySize = _weightCount.intValue;
                    _valuesWeight.arraySize = _weightCount.intValue;
                }

                height += EditorGUI.GetPropertyHeight(_weightCount);
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect foldoutRect = new Rect(position.x, position.y, position.width, FOLDOUT_HEIGHT);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label);

            if (property.isExpanded)
            {
                float addY = FOLDOUT_HEIGHT;
                for (int i = 0; i < _values.arraySize + 1; i++)
                {
                    Rect rect;
                    if (i == 0)
                    {
                        rect = new Rect(position.x, position.y + addY, position.width, EditorGUI.GetPropertyHeight(_weightCount));
                        addY += rect.height;

                        EditorGUI.PropertyField(rect,
                        _weightCount, null, true);
                        continue;
                    }

                    rect = new Rect(position.x, position.y + addY, position.width, EditorGUI.GetPropertyHeight(_values.GetArrayElementAtIndex(i - 1)));
                    addY += rect.height;

                    EditorGUILayout.BeginHorizontal();

                    var val = _valuesWeight.GetArrayElementAtIndex(i - 1);

                    EditorGUILayout.PropertyField(_values.GetArrayElementAtIndex(i - 1), GUIContent.none, false);

                    val.intValue = EditorGUILayout.IntSlider(val.intValue, 0, 1000);

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
