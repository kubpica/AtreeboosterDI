using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Reflection;
using System;

/// <summary>
/// Loads preload scene containing common components between scenes (singletons etc).
/// </summary>
public class LoadingSceneIntegration : MonoBehaviour
{
    static MethodInfo _clearConsoleMethod;
    static MethodInfo clearConsoleMethod
    {
        get
        {
            if (_clearConsoleMethod == null)
            {
                #if UNITY_EDITOR
                Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
                Type logEntries = assembly.GetType("UnityEditor.LogEntries");
                _clearConsoleMethod = logEntries.GetMethod("Clear");
                #endif
            }
            return _clearConsoleMethod;
        }
    }

    public static void ClearLogConsole()
    {
        clearConsoleMethod.Invoke(new object(), null);
    }

    public static int otherScene = 2;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitLoadingScene()
    {
        //Debug.Log("InitLoadingScene()");
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 0) return;

        //Debug.Log("Loading _preload scene");
        otherScene = sceneIndex;
        //make sure your _preload scene is the first in scene build list
        SceneManager.LoadScene(0);
    }

    private void Awake()
    {
        if (otherScene > 0)
        {
            //Debug.Log("Returning again to the scene: " + otherScene);
            SceneManager.LoadScene(otherScene);
            ClearLogConsole();
        }
    }
}