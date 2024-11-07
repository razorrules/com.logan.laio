using System;
using UnityEditor;
using UnityEngine;
using Laio;

namespace LaioEditor
{
    [CustomPropertyDrawer(typeof(EArray<,>))]
    public class EArrayDrawer : PropertyDrawer
    {

        private const float FOLDOUT_HEIGHT = 16f;

        private SerializedProperty m_content;
        private Type m_type;
        private string[] m_valueNames;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {

            if (m_content == null)
            {
                m_content = property.FindPropertyRelative("_values");
                if (m_content == null)
                    Debug.LogError("Failed to find '_values' property in EArray");
                m_type = fieldInfo.FieldType.GenericTypeArguments[0];
                m_valueNames = Enum.GetNames(m_type);
            }

            float height = FOLDOUT_HEIGHT;

            if (property.isExpanded)
            {
                if (m_content.arraySize != m_valueNames.Length)
                {
                    m_content.arraySize = m_valueNames.Length;
                }
                for (int i = 0; i < m_content.arraySize; i++)
                {
                    height += EditorGUI.GetPropertyHeight(m_content.GetArrayElementAtIndex(i));
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
                if (m_content == null)
                {
                    m_content.arraySize = m_valueNames.Length;
                }
                for (int i = 0; i < m_content.arraySize; i++)
                {
                    Rect rect = new Rect(position.x, position.y + addY, position.width, EditorGUI.GetPropertyHeight(m_content.GetArrayElementAtIndex(i)));
                    addY += rect.height;

                    EditorGUI.PropertyField(rect,
                        m_content.GetArrayElementAtIndex(i), GetEnumName(i), true);

                }
            }

            EditorGUI.EndProperty();

        }

        private GUIContent GetEnumName(int index)
        {
            Debug.Log("GetEnumName()");
            return new GUIContent(m_valueNames[index]);
        }

    }
}
