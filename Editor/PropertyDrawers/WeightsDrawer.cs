using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Laio
{
    [CustomPropertyDrawer(typeof(Weights<>))]
    public class WeightsDrawer : PropertyDrawer
    {

        private const float FOLDOUT_HEIGHT = 16f;

        private SerializedProperty _weightCount;
        private SerializedProperty _content;
        private Type _type;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (_content == null)
            {
                _content = property.FindPropertyRelative("values");
                _type = fieldInfo.FieldType.GenericTypeArguments[0];
                _weightCount = property.FindPropertyRelative("WeightCount");
            }


            float height = FOLDOUT_HEIGHT;

            if (property.isExpanded)
            {
                if (_content.arraySize != _weightCount.intValue)
                {
                    _content.arraySize = _weightCount.intValue;
                }

                height += EditorGUI.GetPropertyHeight(_weightCount);

                for (int i = 0; i < _content.arraySize; i++)
                {
                    height += EditorGUI.GetPropertyHeight(_content.GetArrayElementAtIndex(i));
                }

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
                for (int i = 0; i < _content.arraySize + 1; i++)
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

                    rect = new Rect(position.x, position.y + addY, position.width, EditorGUI.GetPropertyHeight(_content.GetArrayElementAtIndex(i - 1)));
                    addY += rect.height;

                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.IntSlider(0, 0, 100);

                    EditorGUI.PropertyField(rect,
                        _content.GetArrayElementAtIndex(i - 1), null, true);

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUI.EndProperty();

        }

    }

}
