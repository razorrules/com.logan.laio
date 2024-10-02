//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
//public class ConditionalHideAttribute : PropertyAttribute
//{
//    public string conditionalSourceField { get; private set; }
//    public bool HideInInspector { get; private set; } = true;
//    public object equals { get; private set; }

//    public ConditionalHideAttribute(string conditionalSourceField)
//    {
//        this.conditionalSourceField = conditionalSourceField;
//        equals = null;
//        HideInInspector = true;
//    }


//    public ConditionalHideAttribute(string conditionalSourceField, bool hide)
//    {
//        this.conditionalSourceField = conditionalSourceField;
//        equals = null;
//        HideInInspector = hide;
//    }

//    public ConditionalHideAttribute(string conditionalSourceField, object value)
//    {
//        this.conditionalSourceField = conditionalSourceField;
//        this.equals = value;
//        HideInInspector = true;
//    }

//    public ConditionalHideAttribute(string conditionalSourceField, object value, bool hide)
//    {
//        this.conditionalSourceField = conditionalSourceField;
//        this.equals = value;
//        HideInInspector = hide;
//    }
//}

//#if UNITY_EDITOR
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
//public class ConditionalHideAttributeDrawer : PropertyDrawer
//{
//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
//        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

//        if (!condHAtt.HideInInspector || enabled)
//        {
//            return EditorGUI.GetPropertyHeight(property, label);
//        }
//        else
//        {
//            //The property is not being drawn
//            //We want to undo the spacing added before and after the property
//            return -EditorGUIUtility.standardVerticalSpacing;
//        }
//    }

//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        //get the attribute data
//        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
//        //check if the propery we want to draw should be enabled
//        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

//        //Enable/disable the property
//        bool wasEnabled = GUI.enabled;
//        GUI.enabled = enabled;

//        //Check if we should draw the property
//        if (!condHAtt.HideInInspector || enabled)
//        {
//            EditorGUI.PropertyField(position, property, label, true);
//        }

//        //Ensure that the next property that is being drawn uses the correct settings
//        GUI.enabled = wasEnabled;
//    }

//    private object GetPropertyValue(SerializedProperty sourcePropertyValue)
//    {
//        switch (sourcePropertyValue.propertyType)
//        {
//            case SerializedPropertyType.Integer:
//                return sourcePropertyValue.intValue;
//            case SerializedPropertyType.Boolean:
//                return sourcePropertyValue.boolValue;
//            case SerializedPropertyType.Float:
//                return sourcePropertyValue.floatValue;
//            case SerializedPropertyType.String:
//                return sourcePropertyValue.stringValue;
//            case SerializedPropertyType.Color:
//                return sourcePropertyValue.colorValue;
//            case SerializedPropertyType.ObjectReference:
//                return sourcePropertyValue.objectReferenceValue;
//            case SerializedPropertyType.Enum:
//                return sourcePropertyValue.enumNames[sourcePropertyValue.enumValueIndex];
//            case SerializedPropertyType.Vector2:
//                return sourcePropertyValue.vector2Value;
//            case SerializedPropertyType.Vector3:
//                return sourcePropertyValue.vector3Value;
//            case SerializedPropertyType.Vector4:
//                return sourcePropertyValue.vector4Value;
//            case SerializedPropertyType.Rect:
//                return sourcePropertyValue.rectValue;
//            case SerializedPropertyType.ArraySize:
//                return sourcePropertyValue.arraySize;
//            case SerializedPropertyType.AnimationCurve:
//                return sourcePropertyValue.animationCurveValue;
//            case SerializedPropertyType.Bounds:
//                return sourcePropertyValue.boundsValue;
//            case SerializedPropertyType.Gradient:
//                return sourcePropertyValue.gradientValue;
//            case SerializedPropertyType.Quaternion:
//                return sourcePropertyValue.quaternionValue;
//            case SerializedPropertyType.ExposedReference:
//                return sourcePropertyValue.exposedReferenceValue;
//            case SerializedPropertyType.FixedBufferSize:
//                return sourcePropertyValue.fixedBufferSize;
//            case SerializedPropertyType.Vector2Int:
//                return sourcePropertyValue.vector2IntValue;
//            case SerializedPropertyType.Vector3Int:
//                return sourcePropertyValue.vector3IntValue;
//            case SerializedPropertyType.RectInt:
//                return sourcePropertyValue.rectIntValue;
//            case SerializedPropertyType.BoundsInt:
//                return sourcePropertyValue.boundsIntValue;
//            case SerializedPropertyType.ManagedReference:
//                return sourcePropertyValue.managedReferenceValue;
//            default:
//                Debug.LogError("Cannot handle conditional hide comparision of type: " + sourcePropertyValue.propertyType);
//                return null;
//        }
//    }

//    private bool GetConditionalHideAttributeResult(ConditionalHideAttribute condHAtt, SerializedProperty property)
//    {
//        bool enabled = true;
//        //Look for the sourcefield within the object that the property belongs to
//        string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
//        string conditionPath = propertyPath.Replace(property.name, condHAtt.conditionalSourceField); //changes the path to the conditionalsource property path
//        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

//        if (condHAtt.equals == null)
//        {
//            if (sourcePropertyValue != null)
//            {
//                enabled = sourcePropertyValue.boolValue;
//            }
//            else
//            {
//                Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.conditionalSourceField);
//            }
//        }
//        else if (GetPropertyValue(sourcePropertyValue).Equals(condHAtt.equals.ToString()))
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }

//        return enabled;
//    }

//}
//#endif
