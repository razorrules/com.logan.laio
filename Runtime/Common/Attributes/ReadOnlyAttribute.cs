using UnityEngine;
/// <summary>
/// A readonly attribute, this simply shows the value in the inspector
/// but does not allow you to edit it.
/// </summary>
public class ReadOnlyAttribute : PropertyAttribute { }

#if UNITY_EDITOR

/// <summary>
/// Drawer for readonly attribute
/// </summary>
[UnityEditor.CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : UnityEditor.PropertyDrawer
{
    public override float GetPropertyHeight(UnityEditor.SerializedProperty property,
                                            GUIContent label)
    {
        return UnityEditor.EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position,
                               UnityEditor.SerializedProperty property,
                               GUIContent label)
    {
        //Disable GUI, draw the property and enable it again.
        GUI.enabled = false;
        UnityEditor.EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}

#endif