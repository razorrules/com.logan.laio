using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Laio;
using Laio.Tools;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Text;

public static class UMLNode
{
    public static BindingFlags METHOD_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;

    private const bool GenerateOnCompile = true;

    [ContextMenu("Test")]
    public static void dTest()
    {

    }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void GenerateUMLMap()
    {
        return;

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Debug.Log("Regenerating UML");

        List<string> namespaces = new List<string>();

        List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name.Contains("Laio")).ToList();

        foreach (Assembly a in assemblies)
        {
            foreach (Type definedType in a.GetTypes())
            {

                StringBuilder sb = new StringBuilder();

                foreach (MethodInfo method in definedType.GetMethods(METHOD_FLAGS))
                    sb.Append(method.Name + "(" + method.Attributes + ")\n");

                Debug.Log(definedType + "\n" + sb.ToString());
            }

        }

        stopwatch.Stop();

        Debug.Log("Time: " + stopwatch.ElapsedMilliseconds + "ms");
    }

}