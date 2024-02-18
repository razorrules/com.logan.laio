using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Laio;

namespace Laio.Saving
{

    public static class SceneSaving
    {
        //TODO: Save entire scene

        public static void ParseScene(Scene Scene)
        {
            Debug.Log("Root: " + Scene.rootCount);
            Debug.Log("ToString: " + Scene.ToString());

        }

    }

}