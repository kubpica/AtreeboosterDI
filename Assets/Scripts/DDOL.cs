using UnityEngine;

/// <summary>
/// GameObject with this script attached to it won't be destroyed on load.
/// </summary>
public class DDOL : MonoBehaviour
{
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //Debug.Log("DDOL "+gameObject.name);
    }
}