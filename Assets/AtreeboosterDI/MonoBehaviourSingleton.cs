using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Makes the derived class a singleton MonoBehaviour.
/// </summary>
/// <typeparam name="T"> Deriving class.</typeparam>
/// <example>
/// <c>public class SoundManager : MonoBehaviourSingleton<SoundManager> {}</c>
/// </example>
/// <remarks>
/// If you want to use Awake() or OnDestroy() in your singleton, hide them (with new keyword) and call the base methods.
/// </remarks>
public class MonoBehaviourSingleton<T> : MonoBehaviourExtended where T : MonoBehaviour
{
    private static readonly Dictionary<Type, object> instances = new Dictionary<Type, object>();

    /// <summary>
    /// Instance of the (singleton) class. (Get-only)
    /// </summary>
    public static T Instance
    {
        get
        {
            var type = typeof(T);
            bool found = instances.TryGetValue(type, out object instanceObject);

            if (!found)
            {
                // Maybe it's not Awake yet, try to find it in active scenes.
                bool stillLoading = false;
                var method = type.GetMethod("GetComponentInChildren", new Type[0]).MakeGenericMethod(type);
                for (int i = 0; i < SceneManager.sceneCount; i++) {
                    var scene = SceneManager.GetSceneAt(i);
                    var roots = scene.GetRootGameObjects();
                    foreach (var r in roots)
                    {
                        var singleton = method.Invoke(r.transform, new Type[0]);

                        if (singleton != null)
                            return singleton as T;
                    }
                    if (!scene.isLoaded)
                        stillLoading = true;
                }

                if (stillLoading)
                {
                    return null;
                }

                // If it's sure there is no instance, creates one dynamically
                LogWarning("Not found " + type.Name + " in the scene!");
                Log("Creating new instance of " + type.Name + ". Start() will be delayed one frame!");
                GameObject gm = new GameObject(type.Name, type);
                DontDestroyOnLoad(gm);
                return gm.GetComponent<T>();
            }

            return instanceObject as T;
        }

        private set
        {
            instances.Add(typeof(T), value);
        }
    }

    /// <summary>
    /// Saves first instance.
    /// </summary>
    /// <remarks>
    /// If you want to use Awake() in your singleton, hide this (with new keyword) and call <c>base.Awake();</c>
    /// </remarks>
    protected new void Awake()
    {
        if (!instances.ContainsKey(typeof(T)))
        {
            Instance = this as T;
        }
        else
        {
            LogWarning("There are multiple " + typeof(T).Name + " in the scene!");
        }

        base.Awake();
    }

    /// <summary>
    /// Removes the <see cref="Instance"/>.
    /// </summary>
    /// <remarks>
    /// If you want to use OnDestroy() in your singleton, hide this (with new keyword) and call <c>base.OnDestroy();</c>
    /// </remarks>
    protected void OnDestroy()
    {
        instances.Remove(typeof(T));
    }
}