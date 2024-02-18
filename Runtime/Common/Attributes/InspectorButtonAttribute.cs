using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class InspectorButtonAttribute : Attribute { }

//TODO: Implement
#if UNITY_EDITOR

/// <summary>
/// Drawer for readonly attribute
/// </summary>
[UnityEditor.CustomPropertyDrawer(typeof(InspectorButtonAttribute))]
public class InspectorButtonDrawer : UnityEditor.PropertyDrawer
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
        Debug.Log("Hello World");
        //Disable GUI, draw the property and enable it again.
        GUI.enabled = false;
        //UnityEditor.EditorGUI.PropertyField(position, property, label, true);
        if (GUI.Button(position, new GUIContent("Button")))
        {

        }
        GUI.enabled = true;
    }
}

#endif