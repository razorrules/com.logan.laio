using UnityEngine;
using UnityEditor;


/// <summary>
/// A readonly attribute, this simply shows the value in the inspector
/// but does not allow you to edit it.
/// </summary>
public class ReadOnlyAttribute : PropertyAttribute { }

/// <summary>
/// Drawer for readonly attribute
/// </summary>
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                            GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {
        //Disable GUI, draw the property and enable it again.
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}