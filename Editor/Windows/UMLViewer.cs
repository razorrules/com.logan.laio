using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
//https://forum.unity.com/threads/ui-toolkit-at-package-level.1252326/

public class UMLViewer : EditorWindow
{
    [SerializeField] private StyleSheet styleSheet;
    [SerializeField] private VisualTreeAsset visualTreeAsset;

    //[MenuItem("Laio/UMLViewer")]
    public static void ShowExample()
    {
        UMLViewer wnd = GetWindow<UMLViewer>();
        wnd.Create();
        wnd.titleContent = new GUIContent("UMLViewer");
    }

    public void Create()
    {

    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        //visualTreeAsset.CloneTree(root);

        root.styleSheets.Add(styleSheet);
    }
}