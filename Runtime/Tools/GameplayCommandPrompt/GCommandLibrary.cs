using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Laio;
using System.Linq;
using System.Reflection;

namespace Laio.Tools
{
    public class GCommandLibrary : MonoBehaviour
    {

        internal static MethodInfo[] Methods { get; private set; }

        private static void CacheMethods()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Methods = new MethodInfo[0];

            foreach (Assembly assembly in assemblies)
                Methods.Concat(GetTypes(assembly));
        }

        //[UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            CacheMethods();
        }

        public static MethodInfo[] GetTypes(Assembly assembly)
        {
            var methods = assembly.GetTypes()
                      .SelectMany(t => t.GetMethods())
                      .Where(m => m.GetCustomAttributes(typeof(GCommand), false).Length > 0)
                      .ToArray();
            return methods;
        }

        public static IEnumerable<MethodInfo> GetMethodsWithAttribute(Type classType, Type attributeType)
        {
            return classType.GetMethods().Where(methodInfo => methodInfo.GetCustomAttributes(attributeType, true).Length > 0);
        }
    }
}